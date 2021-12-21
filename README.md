# csharp-wasm4

Dumping space for notes will this is WIP

```
emcc MiniRuntime.wasm.c -c -o MiniRuntime.bc -s WASM=1 -Os

csc.exe /debug /O /noconfig /nostdlib /runtimemetadataversion:v4.0.30319 MiniRuntime.cs MiniBCL.cs HelloWorld\Hello.cs /out:Hello.ilexe /langversion:latest /unsafe /target:library

E:\GitHub\runtimelab\artifacts\bin\coreclr\windows.x64.Debug\ilc\ilc --targetos=wasm --targetarch=wasm hello.ilexe -o hello.bc --systemmodule:hello --Os -g

## needs PR to build libraries, or ctrl-f5 from vs
E:\GitHub\runtimelab\artifacts\bin\coreclr\windows.x64.Debug\ilc\ilc --targetarch=wasm --targetos:wasm e:\github\w4\hello.dll -o e:\github\w4\hello.bc --systemmodule:hello --O --nativelib --codegenopt:Target=wasm32-unknown-unknown

build LLVM ld and run from VS with opts

-flavor wasm -o e:\GitHub\w4\hello-wasm.wasm E:\GitHub\w4\hello.bc E:\GitHub\w4\MiniRuntime.bc E:\GitHub\w4\helloclrjit.bc -mllvm -combiner-global-alias-analysis=false -mllvm -disable-lsr --import-undefined --strip-debug --export-if-defined=__start_em_asm --export-if-defined=__stop_em_asm --export-if-defined=fflush  --export-table -z stack-size=1024 --import-memory --initial-memory=65536 --max-memory=65536 --global-base=6560 --export=update --no-entry
```
