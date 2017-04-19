using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LiveMonitoring
{
    class MachineIdleTracker
    {
        [DllImport("user32.dll")]
        static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        public static uint OldIdleTime { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        struct LASTINPUTINFO
        {
            public static readonly int SizeOf = Marshal.SizeOf(typeof(LASTINPUTINFO));

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 cbSize;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dwTime;
        }

        static uint GetLastInputTime()
        {
            uint idleTime = 0;
            try
            {
                LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
                lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
                lastInputInfo.dwTime = 0;

                uint envTicks = (uint)Environment.TickCount;

                if (GetLastInputInfo(ref lastInputInfo))
                {
                    uint lastInputTick = lastInputInfo.dwTime;

                    idleTime = envTicks - lastInputTick;
                }
            }
            catch (Exception ex)
            {
            }
            return ((idleTime > 0) ? (idleTime / 1000) : 0);
        }

        public static void fncMachineIdleTracker()
        {
            try
            {
                uint IdleTime = GetLastInputTime();
                Console.WriteLine(IdleTime);

                if (IdleTime >= Program.Idle_MinTime)
                {
                    OldIdleTime = IdleTime;
                }
                int Interval = Program.Idle_Interval / 1000;
                if (IdleTime <= Interval && OldIdleTime >= Program.Idle_MinTime)
                {
                    int Idletime = (int)OldIdleTime;
                    Console.WriteLine(Idletime);
                    OldIdleTime = 0;
                    WriteMachineIdleTime(Idletime);
                }       
            }
            catch (Exception ex)
            {
                
                
            }
            
        }

        static void WriteMachineIdleTime(int IdleTimeSec)
        {
            try
            {
                using (HttpClient client = MachineInfoTracker.GetHttpClient())
                {
                    MachineIdle model = new MachineIdle { MachineDetailId = Program.MachineId, IdleTime = IdleTimeSec, CreatedDate = DateTime.Now };
                    HttpResponseMessage Response = client.PostAsync<MachineIdle>("api/LiveMonitoringAPI/AddMachineIdleTime", model, new JsonMediaTypeFormatter()).Result;

                    if (Response.IsSuccessStatusCode == true)
                    {
                        var result = Response.Content.ReadAsStringAsync().Result;
                        if (result.Contains("OK"))
                        {
                            Console.WriteLine("Record saved successfully.");
                        }
                        else if (result.Contains("Invalid"))
                        {
                            Program.AddMachineInfo();
                        }
                    }
                    else if (Response != null && Response.IsSuccessStatusCode == false)
                    {
                        var result = Response.Content.ReadAsStringAsync().Result;
                        Console.Write("Error Message - " + result);
                    }
                }
            }
            catch (Exception ex)
            {
                
                
            }
            
        }
    }

    
}
