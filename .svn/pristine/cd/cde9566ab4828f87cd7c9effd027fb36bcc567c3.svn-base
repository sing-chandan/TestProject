using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Timers;

namespace InteractiveProcessFromService
{

    partial class InteractiveProcess : ServiceBase
    {
        private System.Timers.Timer serviceTimer = null;
        ServiceController sc = new ServiceController("LiveMonitoringService");
        public InteractiveProcess()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            serviceTimer = new System.Timers.Timer();
            serviceTimer.Interval = 120000;
            serviceTimer.Elapsed += new ElapsedEventHandler(serviceTimer_Elapsed);
            serviceTimer.Enabled = true;
            serviceTimer.Start();
        
        }

        void serviceTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
           Process[] liveMonitorProcess = Process.GetProcessesByName("svcrhost");
            if(liveMonitorProcess.Length == 0)
            {
                ThreadPool.QueueUserWorkItem(LaunchService);
            }
           
        }

        public void LaunchService(object context)
        {
            IntPtr hSessionToken = IntPtr.Zero;
            try
            {
                SessionFinder sf = new SessionFinder();
                //Get the ineractive console session.
                hSessionToken = sf.GetLocalInteractiveSession();

                if (hSessionToken != IntPtr.Zero)
                {
                    
                    InteractiveProcessRunner runner =
                        new InteractiveProcessRunner(AppDomain.CurrentDomain.BaseDirectory + "svcrhost.exe", hSessionToken) { CreateNoWindow = true };
                    runner.Run();
                }
                else
                {
                    EventLog.WriteEntry("Session not found.", EventLogEntryType.Error);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(string.Format("Exception thrown: {0}{0}{1}", Environment.NewLine, ex), EventLogEntryType.Error);
            }
            finally
            {
                if (hSessionToken != IntPtr.Zero)
                {
                    CloseHandle(hSessionToken);
                }
            }
        }


        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObj);

    }


}
