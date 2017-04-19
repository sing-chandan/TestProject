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

namespace LiveMonitoring
{
    class MachineInfoTracker
    {

        public static string GetMachineIP()
        {
            string hostName = Dns.GetHostName(); // Retrive the Name of HOST
            string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
            return myIP;
        }

        public static string GetUserName()
        {
            string strUserName = Environment.UserName;
            return strUserName;
        }

        public static string GetMachineName()
        {
            string strMachineName = Environment.MachineName;
            return strMachineName;
        }


        public static AdapterList GetPrimaryAdapterDetails()
        {
            AdapterList ReturnAdapterList = new AdapterList();
            List<AdapterList> list = new List<AdapterList>();

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                try
                {
                    if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    {
                        int AdpIndex = nic.GetIPProperties().GetIPv4Properties().Index;
                        string macAdd = nic.GetPhysicalAddress().ToString().Trim();
                        string ipAddress = string.Empty;
                        foreach (UnicastIPAddressInformation ip in nic.GetIPProperties().UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                ipAddress = ip.Address.ToString();
                            }
                        }

                        if (AdpIndex > 0 && !string.IsNullOrEmpty(macAdd))
                        {
                            list.Add(new AdapterList { AdapterMacAddress = macAdd, AdapterIndex = AdpIndex, AdapterIP = ipAddress });
                        }
                    }
                }
                catch (Exception)
                {
                }
            }

            if (list.Count > 0)
            {
                try
                {
                    ReturnAdapterList.AdapterMacAddress = list.Where(z => z.AdapterIndex == list.Min(x => x.AdapterIndex)).Select(x => x.AdapterMacAddress).FirstOrDefault();
                    ReturnAdapterList.AdapterIP = list.Where(z => z.AdapterIndex == list.Min(x => x.AdapterIndex)).Select(x => x.AdapterIP).FirstOrDefault();
                    ReturnAdapterList.AdapterIndex = list.Where(z => z.AdapterIndex == list.Min(x => x.AdapterIndex)).Select(x => x.AdapterIndex).FirstOrDefault();
                }
                catch (Exception)
                {
                }
            }
            Console.WriteLine("Primary MacAddress is    :- " + ReturnAdapterList.AdapterMacAddress);
            Console.WriteLine("Primary Adapter IP is    :- " + ReturnAdapterList.AdapterIP);
            Console.WriteLine("Primary Adapter Index is :- " + ReturnAdapterList.AdapterIndex);
            return ReturnAdapterList;
        }

        //public static string GetMACAddress()
        //{
        //    String sMacAddress = string.Empty;
        //    try
        //    {
        //        NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

        //        foreach (NetworkInterface adapter in nics)
        //        {
        //            if (sMacAddress == String.Empty)// only return MAC Address from first card  
        //            {
        //                IPInterfaceProperties properties = adapter.GetIPProperties();
        //                sMacAddress = adapter.GetPhysicalAddress().ToString();
        //            }
        //        }

        //        sMacAddress = sMacAddress + "," + Convert.ToString(GetMACAddressEthernet());
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return sMacAddress;
        //}

        public static HttpClient GetHttpClient()
        {
            HttpClient client = new HttpClient();
            try
            {

                client.BaseAddress = new Uri(Constants.RemoteUrl);
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", Convert.ToBase64String(Encoding.ASCII.GetBytes(String.Format("{0}:{1}", Convert.ToString(asr.ReadKey("userName")), Convert.ToString(asr.ReadKey("password"))))));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            }
            catch (Exception ex)
            {

            }
            return client;

        }
    }

    class AdapterList
    {
        public string AdapterMacAddress { get; set; }
        public int AdapterIndex { get; set; }
        public string AdapterIP { get; set; }
    }
}
