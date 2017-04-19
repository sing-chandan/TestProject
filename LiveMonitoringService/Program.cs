using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;

namespace InteractiveProcessFromService
{
    class Program
    {
        static void Main(string[] args)
        {
            InteractiveProcess service = new InteractiveProcess();

            if (args.Length > 0 && args[0] == "/i")
            {
                //Launched interactively.
                service.LaunchService(null);
            }
            else
            {
                //Standard service entry point.
                ServiceBase.Run(service);

                //Process[] liveMonitorProcess = Process.GetProcessesByName("svcrhost");
                //if (liveMonitorProcess.Length == 0)
                //{
                //    ThreadPool.QueueUserWorkItem(service.LaunchService);
                //}

                // service.Process();
            }
        }
    }
}
