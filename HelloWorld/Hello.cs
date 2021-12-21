using System.Runtime.InteropServices;
using System;

namespace HelloWorld
{
    public unsafe class Hello
    {
        // static readonly uint* PALETTE = ((uint*)0x04);
        // static readonly uint* DRAW_COLORS ((uint16_t*)0x14)
        static readonly byte* GAMEPAD1 = (byte*)0x16;
        //static readonly uint* GAMEPAD2((const uint8_t*)0x17)
        //static readonly uint* GAMEPAD3((const uint8_t*)0x18)
        //static readonly uint* GAMEPAD4((const uint8_t*)0x19)
        //static readonly uint* MOUSE_X((const int16_t*)0x1a)
        //#define MOUSE_Y ((const int16_t*)0x1c)
        //#define MOUSE_BUTTONS ((const uint8_t*)0x1e)
        //#define SYSTEM_FLAGS ((uint8_t*)0x1f)
        //#define FRAMEBUFFER ((uint8_t*)0xa0)

        const byte BUTTON_1 = 1;
        const byte BUTTON_2 = 2;
        const byte BUTTON_LEFT = 16;
        //#define BUTTON_RIGHT 32
        const byte BUTTON_UP = 64;
        //#define BUTTON_DOWN 128

        //#define MOUSE_LEFT 1
        //#define MOUSE_RIGHT 2
        //#define MOUSE_MIDDLE 4

        //#define SYSTEM_PRESERVE_FRAMEBUFFER 1
        //#define SYSTEM_HIDE_GAMEPAD_OVERLAY 2

        const uint BLIT_2BPP = 1;
        const uint BLIT_1BPP = 0;
//#define BLIT_FLIP_X 2
//#define BLIT_FLIP_Y 4
//#define BLIT_ROTATE


        [DllImport("*")]
        static extern void text(byte* data, int x, int y);

        [DllImport("*")]
        static extern void blit(byte* data, int x, int y, uint width, uint height, uint flags);

        struct msg_struct
        {
            internal fixed byte msg[10];

            public void SetMsg()
            {
                msg[0] = 72;
                msg[1] = 0;
            }
        }

        struct player
        {
            internal fixed byte Smiley[8];

            public void SetPlayer()
            {
                
            //}
            //internal void Init()
            //{
                Smiley[0] = 0b11000011;
                Smiley[1] = 0b10000001;
                Smiley[2] = 0b00100100;
                Smiley[3] = 0b00100100;
                Smiley[4] = 0b00000000;
                Smiley[5] = 0b00100100;
                Smiley[6] = 0b10011001;
                Smiley[7] = 0b11000011;
            }
                // = { 0b11000011, 0b10000001, 0b00100100, 0b00100100, 0b00000000, 0b00100100, 0b10011001, 0b11000011 };
        }


        static int x = 80;
        static int y = 80;

        [System.Runtime.InteropServices.UnmanagedCallersOnly(EntryPoint = "update")]
        public static void update()
        {

            //*DRAW_COLORS = 2;
            var m = new msg_struct();
            m.SetMsg();

            text(m.msg, 10, 10);

            if (((*GAMEPAD1) & BUTTON_LEFT) != 0)
            {
                x--;
            }

            if (((*GAMEPAD1) & BUTTON_UP) != 0)
            {
                y--;
            }

            var p = new player();
            p.SetPlayer();

            blit(p.Smiley, x, y, 8, 8, BLIT_1BPP);
            //uint8_t gamepad = *GAMEPAD1;
            //if (gamepad & BUTTON_1)
            //{
            //    *DRAW_COLORS = 4;
            //}

            //blit(smiley, 76, 76, 8, 8, BLIT_1BPP);
            //text("Press X to blink", 16, 90);
        }
    }


}

namespace System.Runtime.InteropServices
{
    public class UnmanagedCallersOnlyAttribute : Attribute
    {
        public string EntryPoint { get; set; }
    }
}

namespace Internal.Runtime.CompilerHelpers
{
    public class ThrowHelpers
    {
        public void ThrowNullReferenceException()
        {
        }
    }

}
