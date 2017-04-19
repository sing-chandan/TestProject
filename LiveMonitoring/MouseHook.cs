using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace LiveMonitoring
{
    internal class MouseHook
    {
        private static LowLevelMouseProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        public static void MouseStart()
        {
            try
            {
                if (!GlobalClass.Ismousehook)
                {
                    _hookID = SetHook(_proc);
                    System.Threading.Thread.Sleep(1000);
                    GlobalClass.Ismousehook = true;
                    GlobalClass.WriteTolog("Mouse Hook Start");
                }
            }
            catch (Exception)
            {
                GlobalClass.Ismousehook = false;
            }

        }

        public static void MouseSTOP()
        {
            try
            {
                UnhookWindowsHookEx(_hookID);
                GlobalClass.Ismousehook = false;
            }
            catch (Exception)
            {
                GlobalClass.Ismousehook = true;
            }

        }

        private static IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                if (GlobalClass.ApplicationNowTime >= GlobalClass.LastIdletime.AddSeconds(Program.Idle_MinTime))
                {
                    GlobalClass.lastIdleMinute = GlobalClass.ApplicationNowTime.Subtract(GlobalClass.LastIdletime).Minutes;
                    Console.WriteLine("Mouse hook Idletime update is :- " + GlobalClass.lastIdleMinute);
                    //send your idle time
                }

                GlobalClass.LastIdletime = GlobalClass.ApplicationNowTime;

                if (nCode >= 0 &&
                    MouseMessages.WM_LBUTTONDOWN == (MouseMessages)wParam)
                {
                    MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                    //Console.WriteLine(hookStruct.pt.x + ", " + hookStruct.pt.y);
                }
            }
            catch (Exception)
            {

            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private const int WH_MOUSE_LL = 14;

        private enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
