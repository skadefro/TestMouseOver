using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TestMouseOver
{
    public static class NativeMethods
    {
        public const Int32 HC_ACTION = 0;
        public const Int32 WH_KEYBOARD_LL = 13;
        public const Int32 WH_MOUSE_LL = 14;
        public const Int32 WM_MOUSEMOVE = 0x0200;
        public struct POINT
        {
            public int x;
            public int y;
        }
        public delegate IntPtr LLProc(Int32 nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(Int32 idHook, LLProc lpfn, IntPtr hMod, UInt32 dwThreadId);
        [DllImport("user32.dll")]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);
        [DllImport("user32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, Int32 nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern Boolean GetPhysicalCursorPos(ref POINT pt);
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);


        [Serializable]
        public struct MSG
        {
            public IntPtr hwnd;

            public IntPtr lParam;

            public int message;

            public int pt_x;

            public int pt_y;

            public int time;

            public IntPtr wParam;
        }
        [DllImport("user32.dll")]
        public static extern bool GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport("user32.dll")]
        public static extern bool TranslateMessage([In] ref MSG lpMsg);

        [DllImport("user32.dll")]
        public static extern IntPtr DispatchMessage([In] ref MSG lpmsg);

    }
}
