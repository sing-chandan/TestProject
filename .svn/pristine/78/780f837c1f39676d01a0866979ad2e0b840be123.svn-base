using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Collections.Specialized;

namespace LiveMonitoring
{
    class ScreenShot
    {
        [DllImport("user32.dll")]
        static extern int GetForegroundWindow();

        public static string OldTitle { get; set; }

        [DllImport("user32.dll")]
        static extern int GetWindowText(int hWnd, StringBuilder text, int count);

        private static string GetActiveWindowTitle()
        {
            try
            {
                const int nChars = 256;
                int handle = 0;
                StringBuilder Buff = new StringBuilder(nChars);

                handle = GetForegroundWindow();

                if (GetWindowText(handle, Buff, nChars) > 0)
                {
                    return Buff.ToString();
                }
            }
            catch (Exception ex)
            {

            }
            return string.Empty;
        }

        private static Bitmap screenCapture()
        {
            Bitmap bitmap = null;
            try
            {
                Thread.Sleep(100);
                Rectangle SelectionRectangle = Screen.GetBounds(Screen.GetBounds(Point.Empty));
                bitmap = new Bitmap(SelectionRectangle.Width, SelectionRectangle.Height);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, SelectionRectangle.Size);
                }
            }
            catch (Exception e)
            {
            }

            return bitmap;
        }

        private static byte[] ImageToByte2(Bitmap img)
        {
            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Close();

                byteArray = stream.ToArray();
            }
            return byteArray;
        }

        public static void fncScreenShot()
        {
            try
            {
                if (GlobalClass.CheckForInternetConnection())
                {
                    using (HttpClient client = MachineInfoTracker.GetHttpClient())
                    {
                        using (var content = new MultipartFormDataContent("---------------------------" + DateTime.Now.Ticks.ToString("x")))
                        {
                            //Take Screen Shot
                            string activeWindowTitle = GetActiveWindowTitle();
                            Bitmap img = screenCapture();

                            var imageFile = new ByteArrayContent(ImageToByte2(img));

                            // Add file content  
                            if (activeWindowTitle != OldTitle && !string.IsNullOrEmpty(activeWindowTitle))
                            {
                                OldTitle = activeWindowTitle;

                                imageFile.Headers.ContentType = MediaTypeHeaderValue.Parse("image/png");
                                imageFile.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                                {
                                    FileName = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".png"
                                };
                                content.Add(imageFile);

                                // Add file content 
                                content.Add(new StringContent(Program.MachineId.ToString()), name: "MachineId");
                                content.Add(new StringContent(MachineInfoTracker.GetUserName()), name: "UserName");

                                // Make a call to Web API
                                var result = client.PostAsync("api/LiveMonitoringAPI/UploadFiles", content).Result;
                                if (result.StatusCode.ToString() != "OK")
                                {
                                    Console.WriteLine(result.StatusCode);
                                    GlobalClass.WriteTolog("Save new Screen Shot on server");
                                }
                            }

                        }
                    }
                }
                else
                {
                    GlobalClass.WriteTolog("server not giving any response in  ScreenShot function");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error In ScreenShot.cs in fncScreenShot Fuction " + ex.Message);
                GlobalClass.WriteTolog("Error In ScreenShot.cs in fncScreenShot Fuction " + ex.Message);
            }

        }
    }
}
