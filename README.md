# csharp-wasm4

This is an example of using C# to create a demo for wasm4.org.  This is not your traditional C# program, but is based on zerosharp, https://github.com/MichalStrehovsky/zerosharp.  There is no runtime, no GC, no exceptions, no reflection, just the basic value types and C# syntax.  There's no way that in 64KB a "normal" C# program is going to be viable.  As you can see from the zerosharp page, including the normal runtime, even without the classes is 400KB.  However you can do stuff with this if you like the C# syntax, just don't expect to be able to "new" anything, or call any libraries.  Building at the moment is a little tricky due to the lack of prebuilt binaries.  I'll try to improve that and the instructions

Dumping space for notes while this is WIP

```
# I used emscripten to build this file, but there's no need and clang on its own should be fine
# THis is the C  part of the "runtime"
emcc MiniRuntime.wasm.c -c -o MiniRuntime.bc -s WASM=1 -Os

#  This builds a wasm library from the c# inputs
csc.exe /debug /O /noconfig /nostdlib /runtimemetadataversion:v4.0.30319 MiniRuntime.cs MiniBCL.cs HelloWorld\Hello.cs /out:Hello.dll /langversion:latest /unsafe /target:library

## needs PR to build libraries, or ctrl-f5 from vs
# Use NativeAOT-LLVM to compile the library from above to LLVM bitcode
E:\GitHub\runtimelab\artifacts\bin\coreclr\windows.x64.Debug\ilc\ilc --targetarch=wasm --targetos:wasm e:\github\w4\hello.dll -o e:\github\w4\hello.bc --systemmodule:hello --O --nativelib --codegenopt:Target=wasm32-unknown-unknown

# For linking, the easiest thing is to use wasm-ld from the WASI SDK.  Download the  wasi-sdk-14.0-linux.tar.gz 
from https://github.com/WebAssembly/wasi-sdk/releases  .   And extract it.  
Then from WSL, or a real linux installation, (I tested with WSL Ubuntu 20.04) you can run:

./wasm-ld -o /mnt/e/GitHub/w4/hello-wasm.wasm /mnt/e/GitHub/w4/hello.bc /mnt/e/GitHub/w4/MiniRuntime.bc /mnt/e/GitHub/w4/helloclrjit.bc -mllvm -combiner-global-alias-analysis=false -mllvm -disable-lsr --import-undefined --strip-debug  --export-table -z stack-size=1024 --import-memory --initial-memory=65536 --max-memory=65536 --global-base=6560 --export=update --export=start --no-entry

# Alternatively for masochists, you can:
#build LLVM ld and run from VS with opts
# I'm running LLVM built from the src download and from inside VS as the one bundled with the current emscripten tree failed for me.
#
# Configure LLVM
cmake -S . -B build -DLLVM_ENABLE_PROJECTS=clang;lld -DLLVM_TARGETS_TO_BUILD=X86;WebAssembly -Thost=x64

lld -flavor wasm -o e:\GitHub\w4\hello-wasm.wasm E:\GitHub\w4\hello.bc E:\GitHub\w4\MiniRuntime.bc E:\GitHub\w4\helloclrjit.bc -mllvm -combiner-global-alias-analysis=false -mllvm -disable-lsr --import-undefined --strip-debug --export-if-defined=__start_em_asm --export-if-defined=__stop_em_asm --export-if-defined=fflush  --export-table -z stack-size=1024 --import-memory --initial-memory=65536 --max-memory=65536 --global-base=6560 --export=update --no-entry

# to run
w4 run hello-wasm.wasm

## Linux

# Configure LLVM, build release as the linking uses less resources
cmake -G Ninja -DLLVM_ENABLE_PROJECTS=clang;lld -DLLVM_TARGETS_TO_BUILD=WebAssembly -DCMAKE_BUILD_TYPE=Release ../llvm
# Building LLVM
cmake --build .

## NativeAOT-LLVM
./build.sh nativeaot+libs -c Debug -lc Debug -a wasm -os Browser
# if emscripten cache lock errors appear, just try again

# build the compiler
./build.sh nativeaot+libs -c Debug -lc Debug -a wasm -os Browser
./build.sh clr.jit+clr.wasmjit+nativeaot.ilc -c Debug

# to build the packages
./build.sh nativeaot.packages -c Debug -a wasm -os Browser

# Test - doesn't work
./src/tests/build.sh nativeaot Debug wasm -os browser  skipnative /p:SmokeTestsOnly=true /p:LibrariesConfiguration=Debug
./src/tests/run.sh --runnativeaottests Debug wasm

```

