using SHDocVw;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NDde.Client;
using System.Diagnostics;
using System.Windows.Automation;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;

namespace LiveMonitoring
{
    public class BrowserUrlTracker
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string IconPath { get; set; }
        public string Version { get; set; }

        public static string OldUrlIE { get; set; }
        public static string OldURLChrome { get; set; }
        public static string OldURLOpera { get; set; }
        public static string OldURLFirefox { get; set; }

        static void WriteBrowserData(string Title, string Url, string BrowserName, string Version)
        {
            if (GlobalClass.CheckForInternetConnection())
            {
                try
                {
                    if (!(Title.Contains("New Tab") || Title.Contains("Speed Dial") || Title.Contains("\"")))
                    {
                        using (HttpClient client = MachineInfoTracker.GetHttpClient())
                        {
                            BrowserDetail model = new BrowserDetail { MachineDetailId = Program.MachineId, BrowserName = BrowserName, BrowserVersion = Version, Title = Title, URL = Url, CreatedDate = DateTime.Now };
                            HttpResponseMessage Response = client.PostAsync<BrowserDetail>("api/LiveMonitoringAPI/AddBrowserDetails", model, new JsonMediaTypeFormatter()).Result;
                            if (Response.IsSuccessStatusCode == true)
                            {
                                var result = Response.Content.ReadAsStringAsync().Result;
                                if (result.Contains("OK"))
                                {
                                    Console.WriteLine("Record saved successfully.");
                                    GlobalClass.WriteTolog("Record saved successfully.");
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
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error In BrowserUrlTracker.cs in WriteBrowserData Fuction " + ex.Message);
                    GlobalClass.WriteTolog("Error In BrowserUrlTracker.cs in WriteBrowserData Fuction " + ex.Message);
                }

            }
            else
            {
                GlobalClass.WriteTolog("server not giving any response");
            }
        }

        public static void ReadBrowserUrlforIE()
        {
            try
            {
                if (HttpContext.Current != null)
                {
                    HttpBrowserCapabilities objbrowser = HttpContext.Current.Request.Browser;
                }
                SHDocVw.InternetExplorer browser;
                SHDocVw.ShellWindows shellWindows = new SHDocVw.ShellWindows();
                string filename;
                foreach (SHDocVw.InternetExplorer ie in shellWindows)
                {
                    filename = System.IO.Path.GetFileNameWithoutExtension(ie.FullName).ToLower();
                    if ((filename == "iexplore"))
                    {
                        browser = ie;
                        string Title = AppTracker.GetActiveWindowTitle();
                        GlobalClass.WriteTolog(Title);
                        GlobalClass.WriteTolog(browser.LocationName);
                        if (Title.Contains(browser.LocationName) && Title.Contains("Internet Explorer") && browser.LocationName != "")
                        {
                            if (browser.LocationName != browser.LocationURL && browser.LocationURL != OldUrlIE)
                            {
                                OldUrlIE = browser.LocationURL;
                                string version = GetBrowsers("Internet Explorer");
                                WriteBrowserData(browser.LocationName, browser.LocationURL, "Internet Explorer", version);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }
        }

        public static void ReadBrowserUrlforFirefox()
        {
            try
            {
                string url = string.Empty;
                string title = string.Empty;


                Process[] process = Process.GetProcessesByName("firefox");

                foreach (Process firefox in process)
                {
                    // the chrome process must have a window
                    if (firefox.MainWindowHandle == IntPtr.Zero)
                    {
                        break;
                    }

                    AutomationElement element = AutomationElement.FromHandle(firefox.MainWindowHandle);
                    if (element == null)
                        break;

                    //search for first custom element
                    AutomationElement custom1 = element.FindFirst(TreeScope.Descendants,
                     new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Custom));

                    //search all custom element children
                    AutomationElementCollection custom2 = custom1.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Custom));

                    //for each custom child
                    foreach (AutomationElement item in custom2)
                    {
                        //search for first custom element
                        AutomationElement custom3 = item.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Custom));
                        //search for first document element
                        AutomationElement doc3 = custom3.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Document));



                        if (!doc3.Current.IsOffscreen)
                        {
                            url = ((ValuePattern)doc3.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
                            OldURLOpera = url;
                            title = element.Current.Name;
                            string version = GetBrowsers("Mozilla Firefox");
                            WriteBrowserData(title, url, "Mozilla Firefox", version);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }


        }

        public static void ReadBrowserUrlforOpera()
        {
            try
            {
                string url = string.Empty;
                string title = string.Empty;
                Process[] procsOpera = Process.GetProcessesByName("opera");
                foreach (Process opera in procsOpera)
                {
                    // the chrome process must have a window
                    if (opera.MainWindowHandle == IntPtr.Zero)
                    {
                        continue;
                    }

                    // find the automation element
                    AutomationElement elm = AutomationElement.FromHandle(opera.MainWindowHandle);
                    AutomationElement elmUrlBar = elm.FindFirst(TreeScope.Descendants,
                      new PropertyCondition(AutomationElement.NameProperty, "Address field"));

                    // if it can be found, get the value from the URL bar
                    if (elmUrlBar != null)
                    {
                        AutomationPattern[] patterns = elmUrlBar.GetSupportedPatterns();
                        if (patterns.Length > 0)
                        {
                            ValuePattern val = (ValuePattern)elmUrlBar.GetCurrentPattern(patterns[0]);
                            title = elm.Current.Name;
                            url = val.Current.Value.ToString();
                        }
                        else
                        {
                            url = "";
                        }
                    }
                    else
                    {
                        url = "";
                    }
                }

                if (url != "" && url != OldURLOpera)
                {
                    OldURLOpera = url;
                    string version = GetBrowsers("Opera Stable");
                    WriteBrowserData(title, url, "Opera Stable", version);
                }
            }
            catch
            {
            }
        }

        public static void ReadBrowserUrlforChrome()
      {
            string title = string.Empty;
            string version = GetBrowsers("Google Chrome");
            Process[] procsChrome = Process.GetProcessesByName("chrome");
            foreach (Process chrome in procsChrome)
            {
                // the chrome process must have a window
                if (chrome.MainWindowHandle == IntPtr.Zero)
                {
                    continue;
                }
                //AutomationElement elm = AutomationElement.RootElement.FindFirst(TreeScope.Children,
                //         new PropertyCondition(AutomationElement.ClassNameProperty, "Chrome_WidgetWin_1"));
                // find the automation element
                AutomationElement elm = AutomationElement.FromHandle(chrome.MainWindowHandle);

              
                // manually walk through the tree, searching using TreeScope.Descendants is too slow (even if it's more reliable)
                AutomationElement elmUrlBar = null;
                try
                {
                    var elm1 = elm.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "Google Chrome"));
                    if (elm1 == null) { continue; } // not the right chrome.exe
                    var elm2 = TreeWalker.RawViewWalker.GetLastChild(elm1); // I don't know a Condition for this for finding
                    AutomationElement elm3, elm4, elm5;
                    switch (version)
                    {
                      //  case "35.0.1916.153":
                        //default:
                        //    elm3 = elm2.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, ""));
                        //    elm4 = TreeWalker.RawViewWalker.GetNextSibling(elm3); // I don't know a Condition for this for finding
                        //    elm5 = elm3.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ToolBar));
                        //    elmUrlBar = elm5.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Custom));
                        //    break;
                        default:
                            //elm3 = elm2.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.HelpTextProperty, "TopContainerView"));
                            //elm4 = elm3.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ToolBar));
                            //elm5 = elm4.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.HelpTextProperty, "LocationBarView"));
                            elmUrlBar = elm.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));
                            break;
                    }


                }
                catch (Exception e)
                {
                    // Chrome has probably changed something, and above walking needs to be modified. :(
                    // put an assertion here or something to make sure you don't miss it
                    continue;
                }

                // make sure it's valid
                if (elmUrlBar == null)
                {
                    // it's not..
                    continue;
                }

                // elmUrlBar is now the URL bar element. we have to make sure that it's out of keyboard focus if we want to get a valid URL
                if ((bool)elmUrlBar.GetCurrentPropertyValue(AutomationElement.HasKeyboardFocusProperty))
                {
                    continue;
                }

                // there might not be a valid pattern to use, so we have to make sure we have one
                AutomationPattern[] patterns = elmUrlBar.GetSupportedPatterns();
                if (patterns.Length == 1)
                {
                    string url = string.Empty;
                    try
                    {
                        title = elm.Current.Name;
                        url = ((ValuePattern)elmUrlBar.GetCurrentPattern(patterns[0])).Current.Value;
                    }
                    catch { }
                    if (url != "" && url != OldURLChrome)
                    {
                        OldURLChrome = url;
                        WriteBrowserData(title, url, "Google Chrome", version);
                    }
                    continue;
                }
            }
        }

        public static string GetBrowsers(string strBrowserName)
        {
            RegistryKey browserKeys;
            string strBrowserVersion = string.Empty;
            //on 64bit the browsers are in a different location
            browserKeys = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Clients\StartMenuInternet");
            if (browserKeys == null)
                browserKeys = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet");
            string[] browserNames = browserKeys.GetSubKeyNames();
            var browsers = new List<string>();
            for (int i = 0; i < browserNames.Length; i++)
            {
                BrowserUrlTracker browser = new BrowserUrlTracker();
                RegistryKey browserKey = browserKeys.OpenSubKey(browserNames[i]);
                browser.Name = (string)browserKey.GetValue(null);
                RegistryKey browserKeyPath = browserKey.OpenSubKey(@"shell\open\command");
                browser.Path = (string)browserKeyPath.GetValue(null).ToString().StripQuotes();
                RegistryKey browserIconPath = browserKey.OpenSubKey(@"DefaultIcon");
                browser.IconPath = (string)browserIconPath.GetValue(null).ToString().StripQuotes();
                if (strBrowserName == browser.Name)
                {
                    if (browser.Path != null)
                    {
                        string[] NewPath = browser.Path.Split(new string[] { ".exe" }, StringSplitOptions.None);
                        string FinalPath = NewPath[0] + ".exe";
                        if (FinalPath.Contains("\""))
                        {
                            FinalPath = FinalPath.Substring(1, FinalPath.Length - 1);
                        }
                        browser.Version = FileVersionInfo.GetVersionInfo(FinalPath).FileVersion;
                    }

                    else
                        browser.Version = "unknown";

                    strBrowserVersion = browser.Version;
                    break;
                }

            }
            return strBrowserVersion;
        }

    }

    internal static class Extensions
    {
        ///
        /// if string begins and ends with quotes, they are removed
        ///
        internal static String StripQuotes(this String s)
        {
            if (s.EndsWith("\"") && s.StartsWith("\""))
            {
                return s.Substring(1, s.Length - 2);
            }
            else
            {
                return s;
            }
        }
    }

    public class BrowserDetail
    {
        public int BrowserDetailId { get; set; }

        public int MachineDetailId { get; set; }

        public string BrowserName { get; set; }

        public string BrowserVersion { get; set; }

        public string Title { get; set; }

        public string URL { get; set; }

        public DateTime? CreatedDate { get; set; }
    }

}
