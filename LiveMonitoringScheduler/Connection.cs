using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;
using System.Diagnostics;
using Microsoft.Win32;
using System.Net.Http;
using System.Net.Http.Headers;
using CommonUtility;

namespace LiveMonitoringScheduler
{
    class Connection
    {
        public static HttpClient GetHttpClient()
        {
            AppSettingReader asr = new AppSettingReader();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(asr.ReadKey("RemoteUrl"));
            return client;
        }
    }
}
