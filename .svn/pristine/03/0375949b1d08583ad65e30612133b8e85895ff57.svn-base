using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommonUtility;

namespace LiveMonitoringScheduler
{
    public static class Program
    {
        
        static void Main(string[] args)
        {   
            GetSheduleReportsSetting();
            _RecentCustomers();
        }

        #region SheduleReports
        public static void GetSheduleReportsSetting()
        {
            try
            {
                using (HttpClient client = Connection.GetHttpClient())
                {
                    HttpResponseMessage Response = client.GetAsync("api/LiveMonitoringAPI/GetSheduleReportsSettings").Result;
                    if (Response.IsSuccessStatusCode == true)
                    {
                        List<ScheduleReportsSettings> lstScheduleReportsSettings = new List<ScheduleReportsSettings>();
                        lstScheduleReportsSettings = Response.Content.ReadAsAsync<List<ScheduleReportsSettings>>().Result;
                        if (lstScheduleReportsSettings != null && lstScheduleReportsSettings.Count>0)
                        {
                            foreach (var item in lstScheduleReportsSettings)
                            {
                                int ScreenId = item.ScreenId;
                                string sReportName = item.ScreenInternalName;
                                string sEmail = item.Email;
                                string sSheduleType = item.ScheduleType;
                                int CustomerId = item.CustomerId;
                                int MembershipId = item.MembershipId;
                                DateTime dSendDate = item.SendDate;
                                bool bIsSend = item.IsSend;
                                string CustomerName = item.CustomerName;
                                int countCustomer=item.countCustomer;
                                Console.Write("\n\n-------------------------------------------");
                                Console.Write("\n\n Fetching SheduleReportsSettings Data");
                                Console.Write("\n Total Customer(s) : "+ countCustomer);
                                Console.Write("\n Customer :"+ CustomerName + "  Email ID: " + sEmail);
                                Console.Write("\n Report : " + sReportName + " Previous SendDate :" + dSendDate);
                              
                                ReportScheduler(sReportName, dSendDate, bIsSend, sSheduleType, MembershipId, CustomerId, sEmail, ScreenId);
                            }
                        }
                        else
                        {
                            Console.Write("\n No Data fetched by function <GetSheduleReportsSetting>");
                        }
                    }
                    else if (Response != null && Response.IsSuccessStatusCode == false)
                    {
                        var result = Response.Content.ReadAsStringAsync().Result;
                        Console.Write("\n Error Message at function <GetSheduleReportsSetting> - " + result);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write("\n Error Message at function <GetSheduleReportsSetting>  - " + e.Message);
            }
        }

        private static void ReportScheduler(string sReportName, DateTime dSendDate, bool bIsSend, string sSheduleType, int MembershipId, int CustomerId, string CustomerEmailID, int ScreenId)
        {
            try
            {   
                Console.Write("\n\n Jumped into ReportScheduler Function");
                Console.Write("\n Report :" + sReportName + " SheduleType : " + sSheduleType);
                Console.Write("\n Current Date : " + DateTime.Now.ToString("MM/dd/yyyy") + " SendDate : " + dSendDate.ToString("MM/dd/yyyy") );
                if (sSheduleType.Trim().ToUpper() == ("D").Trim().ToUpper())
                {
                    Console.Write("\n Checked SheduleType : " + sSheduleType);
                    if (DateTime.Now.ToString("MM/dd/yyyy") == dSendDate.ToString("MM/dd/yyyy"))
                    {
                        Console.Write("\n Checked Current Date equal to Send Date ");
                        getReportData(dSendDate, dSendDate, sReportName, sSheduleType, MembershipId, CustomerId, CustomerEmailID, ScreenId);
                    }
                }

                else if (sSheduleType.Trim().ToUpper() == ("W").Trim().ToUpper())
                {
                    Console.Write("\n Checked SheduleType : " + sSheduleType);
                    if (DateTime.Now.ToString("MM/dd/yyyy") == dSendDate.ToString("MM/dd/yyyy"))
                    {
                        Console.Write("\n Checked Current Date equal to Send Date ");
                        getReportData(dSendDate.AddDays(-6), dSendDate, sReportName, sSheduleType, MembershipId, CustomerId, CustomerEmailID, ScreenId);
                    }
                }
                else if (sSheduleType.Trim().ToUpper() == ("M").Trim().ToUpper())
                {
                    Console.Write("\n Checked SheduleType : " + sSheduleType);
                    if (DateTime.Now.ToString("MM/dd/yyyy") == dSendDate.ToString("MM/dd/yyyy"))
                    {
                        Console.Write("\n Checked Current Date equal to Send Date ");
                        getReportData(dSendDate.AddMonths(-1), dSendDate, sReportName, sSheduleType, MembershipId, CustomerId, CustomerEmailID, ScreenId);
                    }
                }
                else if (sSheduleType.Trim().ToUpper() == ("Q").Trim().ToUpper())
                {
                    Console.Write("\n Checked SheduleType : " + sSheduleType);
                    if (DateTime.Now.ToString("MM/dd/yyyy") == dSendDate.ToString("MM/dd/yyyy"))
                    {
                        Console.Write("\n Checked Current Date equal to Send Date ");
                        getReportData(dSendDate.AddMonths(-3), dSendDate, sReportName, sSheduleType, MembershipId, CustomerId, CustomerEmailID, ScreenId);
                    }
                }
                else if (sSheduleType.Trim().ToUpper() == ("HY").Trim().ToUpper())
                {
                    Console.Write("\n Checked SheduleType : " + sSheduleType);
                    if (DateTime.Now.ToString("MM/dd/yyyy") == dSendDate.ToString("MM/dd/yyyy"))
                    {
                        Console.Write("\n Checked Current Date equal to Send Date ");
                        getReportData(dSendDate.AddMonths(-6), dSendDate, sReportName, sSheduleType, MembershipId, CustomerId, CustomerEmailID, ScreenId);
                    }
                }
                else if (sSheduleType.Trim().ToUpper() == ("Y").Trim().ToUpper())
                {
                    Console.Write("\n Checked SheduleType : " + sSheduleType);
                    if (DateTime.Now.ToString("MM/dd/yyyy") == dSendDate.ToString("MM/dd/yyyy"))
                    {
                        Console.Write("\n Checked Current Date equal to Send Date ");
                        getReportData(dSendDate.AddYears(-1), dSendDate, sReportName, sSheduleType, MembershipId, CustomerId, CustomerEmailID, ScreenId);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write("\n Error Message at function <ReportScheduler>- " + e.Message);
            }
        }

        private static void getReportData(DateTime dFrom, DateTime dTo, string sReportName, string sSheduleType, int MembershipId, int CustomerId, string CustomerEmailID, int ScreenId)
        {
            #region Reports  
            try
            {
                using (HttpClient client = Connection.GetHttpClient())
                {
                    Console.Write("\n\n Jumped into getReportData Function");
                    if (sReportName.Trim().ToUpper() == ("Report_MachineDetails").Trim().ToUpper())
                    {
                        Console.Write("\n Checked ReportName : " + sReportName);
                        Console.Write("\n Calling _Users API function ");
                        HttpResponseMessage Response = client.GetAsync("api/LiveMonitoringAPI/_Users?customerId=" + CustomerId).Result;
                        Console.Write("\n Response IsSuccessStatusCode : " + Response.IsSuccessStatusCode);
                        if (Response.IsSuccessStatusCode == true)
                        {
                            List<MachineDetail> lstMachineDetail = new List<MachineDetail>();
                            lstMachineDetail = Response.Content.ReadAsAsync<List<MachineDetail>>().Result;
                            if (lstMachineDetail != null && lstMachineDetail.Count > 0)
                            {
                                Console.Write("\n Fetched Data of Report : " + sReportName + " Total Records : " + lstMachineDetail.Count);
                                string HtmlBody = convertMachineDetailsDataToHTML(lstMachineDetail);
                                if (sendSchedulerEMail(sReportName, HtmlBody, CustomerEmailID))
                                {
                                    Console.Write("\n Send Report MachineDetails to Email ID:" + CustomerEmailID);
                                    Console.Write("\n Successfully Send Email...");
                                    UpdateScheduleReportsStatus(ScreenId, MembershipId, sSheduleType, sReportName);
                                }
                            }
                            else
                            {
                                Console.Write("\n No Data fetched of Report" + sReportName);
                            }
                        }
                        else if (Response != null && Response.IsSuccessStatusCode == false)
                        {
                            var result = Response.Content.ReadAsStringAsync().Result;
                            Console.Write("\n Error Message at function <getReportData of Report_MachineDetails>  - " + result);
                        }
                    }
                    else if (sReportName.Trim().ToUpper() == ("Report_KeyLoggerDetails").Trim().ToUpper())
                    {
                        Console.Write("\n Checked ReportName : " + sReportName);
                        Console.Write("\n Calling _KeyLogger API function ");
                        HttpResponseMessage Response = client.GetAsync("api/LiveMonitoringAPI/_KeyLogger?dFrom=" + dFrom + "&dTo=" + dTo + "&customerId=" + CustomerId).Result;
                        Console.Write("\n Response IsSuccessStatusCode : " + Response.IsSuccessStatusCode);
                        if (Response.IsSuccessStatusCode == true)
                        {
                            List<KeyLogging> lstKeyLogger = new List<KeyLogging>();
                            lstKeyLogger = Response.Content.ReadAsAsync<List<KeyLogging>>().Result;
                            if (lstKeyLogger != null && lstKeyLogger.Count > 0)
                            {
                                Console.Write("\n Data fetched of Report : " + sReportName + " Total Records : " + lstKeyLogger.Count);
                                string HtmlBody = convertKeyLoggerDataToHTML(lstKeyLogger);
                                if (sendSchedulerEMail(sReportName, HtmlBody, CustomerEmailID))
                                {
                                    Console.Write("\n Send Report MachineDetails to Email ID:" + CustomerEmailID);
                                    Console.Write("\n Successfully Send Email...");
                                    UpdateScheduleReportsStatus(ScreenId, MembershipId, sSheduleType, sReportName);
                                }
                            }
                            else
                            {
                                Console.Write("\n No Data fetched of Report" + sReportName);
                            }
                        }
                        else if (Response != null && Response.IsSuccessStatusCode == false)
                        {
                            var result = Response.Content.ReadAsStringAsync().Result;
                            Console.Write("\n Error Message at function <getReportData of Report_KeyLoggerDetails>   - " + result);
                        }
                    }
                    else if (sReportName.Trim().ToUpper() == ("Report_BrowserDetails").Trim().ToUpper())
                    {
                        Console.Write("\n Checked ReportName : " + sReportName);
                        Console.Write("\n Calling _Sites API function ");
                        HttpResponseMessage Response = client.GetAsync("api/LiveMonitoringAPI/_Sites?dFrom=" + dFrom + "&dTo=" + dTo + "&customerId=" + CustomerId).Result;
                        Console.Write("\n Response IsSuccessStatusCode : " + Response.IsSuccessStatusCode);
                        if (Response.IsSuccessStatusCode == true)
                        {
                            List<BrowserDetail> lstBrowserDetail = new List<BrowserDetail>();
                            lstBrowserDetail = Response.Content.ReadAsAsync<List<BrowserDetail>>().Result;
                            if (lstBrowserDetail != null && lstBrowserDetail.Count > 0)
                            {
                                Console.Write("\n Data fetched of Report : " + sReportName + " Total Records : " + lstBrowserDetail.Count);
                                string HtmlBody = convertBrowserDetailDataToHTML(lstBrowserDetail);
                                if (sendSchedulerEMail(sReportName, HtmlBody, CustomerEmailID))
                                {
                                    Console.Write("\n Send Report MachineDetails to Email ID:" + CustomerEmailID);
                                    Console.Write("\n Successfully Send Email...");
                                    UpdateScheduleReportsStatus(ScreenId, MembershipId, sSheduleType, sReportName);
                                }
                            }
                            else
                            {
                                Console.Write("\n No Data fetched of Report" + sReportName);
                            }
                        }
                        else if (Response != null && Response.IsSuccessStatusCode == false)
                        {
                            var result = Response.Content.ReadAsStringAsync().Result;
                            Console.Write("\n Error Message at function <getReportData of Report_BrowserDetails>   - " + result);
                        }
                    }
                    else if (sReportName.Trim().ToUpper() == ("Report_MachineIdleDetails").Trim().ToUpper())
                    {
                        Console.Write("\n Checked ReportName : " + sReportName);
                        Console.Write("\n Calling _IdleMachines API function ");
                        HttpResponseMessage Response = client.GetAsync("api/LiveMonitoringAPI/_IdleMachines?dFrom=" + dFrom + "&dTo=" + dTo + "&customerId=" + CustomerId).Result;
                        Console.Write("\n Response IsSuccessStatusCode : " + Response.IsSuccessStatusCode);
                        if (Response.IsSuccessStatusCode == true)
                        {
                            List<MachineIdle> lstMachineIdle = new List<MachineIdle>();
                            lstMachineIdle = Response.Content.ReadAsAsync<List<MachineIdle>>().Result;
                            if (lstMachineIdle != null && lstMachineIdle.Count > 0)
                            {
                                Console.Write("\n Data fetched of Report : " + sReportName + " Total Records : " + lstMachineIdle.Count);
                                string HtmlBody = convertMachineIdleDataToHTML(lstMachineIdle);
                                if (sendSchedulerEMail(sReportName, HtmlBody, CustomerEmailID))
                                {
                                    Console.Write("\n Send Report MachineDetails to Email ID:" + CustomerEmailID);
                                    Console.Write("\n Successfully Send Email...");
                                    UpdateScheduleReportsStatus(ScreenId, MembershipId, sSheduleType, sReportName);
                                }
                            }
                            else
                            {
                                Console.Write("\n No Data fetched of Report" + sReportName);
                            }
                        }
                        else if (Response != null && Response.IsSuccessStatusCode == false)
                        {
                            var result = Response.Content.ReadAsStringAsync().Result;
                            Console.Write("\n Error Message at function <getReportData of Report_MachineIdleDetails>   - " + result);
                        }
                    }
                    else if (sReportName.Trim().ToUpper() == ("Report_ApplicationDetails").Trim().ToUpper())
                    {
                        Console.Write("\n Checked ReportName : " + sReportName);
                        Console.Write("\n Calling _Apps API function ");
                        HttpResponseMessage Response = client.GetAsync("api/LiveMonitoringAPI/_Apps?dFrom=" + dFrom + "&dTo=" + dTo + "&customerId=" + CustomerId).Result;
                        Console.Write("\n Response IsSuccessStatusCode : " + Response.IsSuccessStatusCode);
                        if (Response.IsSuccessStatusCode == true)
                        {
                            List<AppDetail> lstAppDetail = new List<AppDetail>();
                            lstAppDetail = Response.Content.ReadAsAsync<List<AppDetail>>().Result;
                            if (lstAppDetail != null && lstAppDetail.Count > 0)
                            {
                                Console.Write("\n Data fetched of Report : " + sReportName + " Total Records : " + lstAppDetail.Count);
                                string HtmlBody = convertAppDetailDataToHTML(lstAppDetail);
                                if (sendSchedulerEMail(sReportName, HtmlBody, CustomerEmailID))
                                {
                                    Console.Write("\n Send Report MachineDetails to Email ID:" + CustomerEmailID);
                                    Console.Write("\n Successfully Send Email...");
                                    UpdateScheduleReportsStatus(ScreenId, MembershipId, sSheduleType, sReportName);
                                }
                            }
                            else
                            {
                                Console.Write("\n No Data fetched of Report" + sReportName);
                            }
                        }
                        else if (Response != null && Response.IsSuccessStatusCode == false)
                        {
                            var result = Response.Content.ReadAsStringAsync().Result;
                            Console.Write("\n Error Message at function <getReportData of Report_ApplicationDetails>   - " + result);
                        }
                    }
                    else if (sReportName.Trim().ToUpper() == ("Report_ProjectSummary").Trim().ToUpper())
                    {
                        Console.Write("\n Checked ReportName : " + sReportName);
                        Console.Write("\n Calling _ProjectSummary API function ");
                        HttpResponseMessage Response = client.GetAsync("api/LiveMonitoringAPI/_ProjectSummary?dFrom=" + dFrom + "&dTo=" + dTo + "&customerId=" + CustomerId).Result;
                        Console.Write("\n Response IsSuccessStatusCode : " + Response.IsSuccessStatusCode);
                        if (Response.IsSuccessStatusCode == true)
                        {
                            List<ProjectSummary> lstProjectSummary = new List<ProjectSummary>();
                            lstProjectSummary = Response.Content.ReadAsAsync<List<ProjectSummary>>().Result;
                            if (lstProjectSummary != null && lstProjectSummary.Count > 0)
                            {
                                Console.Write("\n Data fetched of Report : " + sReportName + " Total Records : " + lstProjectSummary.Count);
                                string HtmlBody = convertProjectSummaryDataToHTML(lstProjectSummary);
                                if (sendSchedulerEMail(sReportName, HtmlBody, CustomerEmailID))
                                {
                                    Console.Write("\n Send Report MachineDetails to Email ID:" + CustomerEmailID);
                                    Console.Write("\n Successfully Send Email...");
                                    UpdateScheduleReportsStatus(ScreenId, MembershipId, sSheduleType, sReportName);
                                }
                            }
                            else
                            {
                                Console.Write("\n No Data fetched of Report" + sReportName);
                            }
                        }
                        else if (Response != null && Response.IsSuccessStatusCode == false)
                        {
                            var result = Response.Content.ReadAsStringAsync().Result;
                            Console.Write("\n Error Message at function <getReportData of Report_ProjectSummary>   - " + result);
                        }
                    }
                }               
            }
            catch (Exception e)
            {
                Console.Write("\n Error Message at function <getReportData>- " + e.Message);
            }

            #endregion
        }

        #endregion

        #region UpdateScheduleReportsStatus
        private static void UpdateScheduleReportsStatus(int ScreenId, int MembershipId, string sSheduleType,string sReportName)
        {
            try
            {
                using (HttpClient client = Connection.GetHttpClient())
                {
                    Console.Write("\n Jumped into UpdateScheduleReportsStatus API function ");
                    ScheduleReportsPOSTparam postData = new ScheduleReportsPOSTparam { ScreenId = ScreenId, MembershipId = MembershipId, sSheduleType = sSheduleType };
                    HttpContent content = new ObjectContent<ScheduleReportsPOSTparam>(postData, new JsonMediaTypeFormatter());
                    var Response = client.PostAsync("api/LiveMonitoringAPI/UpdateScheduleReportsStatus", content).Result;
                    Console.Write("\n Response IsSuccessStatusCode : " + Response.IsSuccessStatusCode);
                    if (Response.IsSuccessStatusCode == true)
                    {
                        Console.Write("\n Successfully Updated Schedule Reports Status...  ");
                    }
                    else if (Response != null && Response.IsSuccessStatusCode == false)
                    {
                        var result = Response.Content.ReadAsStringAsync().Result;
                        Console.Write("\n Error Message at function <UpdateScheduleReportsStatus of Report> +  - sReportName" + " : " + result);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write("\n Error Message at function <UpdateScheduleReportsStatus of Report> +  - sReportName" + " : " +e.Message);
            }
        }
        #endregion

        #region Set Table Style
        private static string setTableStyle()
        {            
            return "<table border='1px' cellpadding='1' cellspacing='1' bgcolor='lightYellow' style='font-family:Arial; font-size:smaller'>";           
        }
        #endregion

        #region ConvertDateToHTML
        public static string convertMachineDetailsDataToHTML(List<MachineDetail> lstMachineDetail)
        {
            try
            {
                StringBuilder strHTMLBuilder = new StringBuilder();
                strHTMLBuilder.Append("<html >");
                strHTMLBuilder.Append("<head>");
                strHTMLBuilder.Append("</head>");
                strHTMLBuilder.Append("<body>");
                strHTMLBuilder.Append(setTableStyle());
                
                //Column Name
                strHTMLBuilder.Append("<tr >");

                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("Customer Name");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("Machine Name");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("User Name");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("MachineMacAddress");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("MachineIP");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("CreatedDate");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("Blocked Status");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("</tr>");

                //Row Data
                foreach (var myColumn in lstMachineDetail)
                {
                    strHTMLBuilder.Append("<tr >");
                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.customer.FirstName + " " + myColumn.customer.LastName);
                    strHTMLBuilder.Append("</td>");

                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.MachineName);
                    strHTMLBuilder.Append("</td>");

                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.UserName);
                    strHTMLBuilder.Append("</td>");

                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.MachineMacAddress);
                    strHTMLBuilder.Append("</td>");

                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.MachineIP);
                    strHTMLBuilder.Append("</td>");

                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.CreatedDate);
                    strHTMLBuilder.Append("</td>");

                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.IsBlocked);
                    strHTMLBuilder.Append("</td>");
                }
                //Close tags.  
                strHTMLBuilder.Append("</table>");
                strHTMLBuilder.Append("</body>");
                strHTMLBuilder.Append("</html>");

                string Htmltext = strHTMLBuilder.ToString();
                Console.Write("\n Converted Data to HTML Format of Report MachineDetail" );
                return Htmltext;
            }
             
            catch (Exception e)
            {
                Console.Write("\n Error Message at function <convertMachineDetailsDataToHTML> - " + e.Message);
            }
            return null;
        }

        private static string convertKeyLoggerDataToHTML(List<KeyLogging> lstKeyLogger)
        {
            try
            {
                StringBuilder strHTMLBuilder = new StringBuilder();
                strHTMLBuilder.Append("<html >");
                strHTMLBuilder.Append("<head>");
                strHTMLBuilder.Append("</head>");
                strHTMLBuilder.Append("<body>");               
                strHTMLBuilder.Append(setTableStyle());

                //Column Name
                strHTMLBuilder.Append("<tr >");

                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("Machine Name");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("User Name");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("Text Type");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("Text");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("CreatedDate");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("</tr>");

                //Row Data
                foreach (var myColumn in lstKeyLogger)
                {
                    strHTMLBuilder.Append("<tr >");
                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.machine_detail.MachineName);
                    strHTMLBuilder.Append("</td>");

                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.machine_detail.UserName);
                    strHTMLBuilder.Append("</td>");

                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.TextType);
                    strHTMLBuilder.Append("</td>");

                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.Text);
                    strHTMLBuilder.Append("</td>");

                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.CreatedDate);
                    strHTMLBuilder.Append("</td>");
                }
                //Close tags.  
                strHTMLBuilder.Append("</table>");
                strHTMLBuilder.Append("</body>");
                strHTMLBuilder.Append("</html>");

                string Htmltext = strHTMLBuilder.ToString();
                Console.Write("\n Converted Data to HTML Format of Report : KeyLogger");
                return Htmltext;
            }
            catch (Exception e)
            {
                Console.Write("\n Error Message at function <convertKeyLoggerDataToHTML>  - " + e.Message);
            }
            return null;
        }

        private static string convertBrowserDetailDataToHTML(List<BrowserDetail> lstBrowserDetail)
        {
            try
            {
                StringBuilder strHTMLBuilder = new StringBuilder();
                strHTMLBuilder.Append("<html >");
                strHTMLBuilder.Append("<head>");
                strHTMLBuilder.Append("</head>");
                strHTMLBuilder.Append("<body>");
                strHTMLBuilder.Append("<table border='1px' cellpadding='1' cellspacing='1' bgcolor='lightyellow' style='font-family:Garamond; font-size:smaller'>");

                strHTMLBuilder.Append("<tr >");

                //Column Name
                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("Machine Name");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("User Name");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("Browser Name");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("Browser Version");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("Title");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("URL");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("CreatedDate");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("</tr>");

                //Row Data
                foreach (var myColumn in lstBrowserDetail)
                {
                    strHTMLBuilder.Append("<tr >");
                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.machine_detail.MachineName);
                    strHTMLBuilder.Append("</td>");

                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.machine_detail.UserName);
                    strHTMLBuilder.Append("</td>");

                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.BrowserName);
                    strHTMLBuilder.Append("</td>");

                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.BrowserVersion);
                    strHTMLBuilder.Append("</td>");

                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.Title);
                    strHTMLBuilder.Append("</td>");

                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.URL);
                    strHTMLBuilder.Append("</td>");

                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.CreatedDate);
                    strHTMLBuilder.Append("</td>");
                }
                //Close tags.  
                strHTMLBuilder.Append("</table>");
                strHTMLBuilder.Append("</body>");
                strHTMLBuilder.Append("</html>");

                string Htmltext = strHTMLBuilder.ToString();
                Console.Write("\n Converted Data to HTML Format of Report : BrowserDetail");
                return Htmltext;
            }
            catch (Exception e)
            {
                Console.Write("\n Error Message at function <convertBrowserDetailDataToHTML>  - " + e.Message);
            }
            return null;
        }

        private static string convertMachineIdleDataToHTML(List<MachineIdle> lstMachineIdle)
        {
            try
            {
                StringBuilder strHTMLBuilder = new StringBuilder();
                strHTMLBuilder.Append("<html >");
                strHTMLBuilder.Append("<head>");
                strHTMLBuilder.Append("</head>");
                strHTMLBuilder.Append("<body>");
                strHTMLBuilder.Append(setTableStyle());

                strHTMLBuilder.Append("<tr >");

                //Column Name
                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("Machine Name");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("User Name");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("Idle Time(Sec.)");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("CreatedDate");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("</tr>");

                //Row Data
                foreach (var myColumn in lstMachineIdle)
                {
                    strHTMLBuilder.Append("<tr >");
                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.MachineName);
                    strHTMLBuilder.Append("</td>");

                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.UserName);
                    strHTMLBuilder.Append("</td>");

                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.IdleTime);
                    strHTMLBuilder.Append("</td>");

                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.CreatedDate);
                    strHTMLBuilder.Append("</td>");

                }
                //Close tags.  
                strHTMLBuilder.Append("</table>");
                strHTMLBuilder.Append("</body>");
                strHTMLBuilder.Append("</html>");

                string Htmltext = strHTMLBuilder.ToString();
                Console.Write("\n Converted Data to HTML Format of Report: MachineIdle");
                return Htmltext;
            }
            catch (Exception e)
            {
                Console.Write("\n Error Message at function <convertMachineIdleDataToHTML>  - " + e.Message);
            }
            return null;
        }

        private static string convertAppDetailDataToHTML(List<AppDetail> lstAppDetail)
        {
            try
            {
                StringBuilder strHTMLBuilder = new StringBuilder();
                strHTMLBuilder.Append("<html >");
                strHTMLBuilder.Append("<head>");
                strHTMLBuilder.Append("</head>");
                strHTMLBuilder.Append("<body>");
                strHTMLBuilder.Append(setTableStyle());

                strHTMLBuilder.Append("<tr >");

                //Column Name
                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("Machine Name");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("User Name");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("Title");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("App. Name");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("CreatedDate");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("</tr>");

                //Row Data
                foreach (var myColumn in lstAppDetail)
                {
                    strHTMLBuilder.Append("<tr >");
                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.machine_detail.MachineName);
                    strHTMLBuilder.Append("</td>");

                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.machine_detail.UserName);
                    strHTMLBuilder.Append("</td>");

                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.Title);
                    strHTMLBuilder.Append("</td>");

                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.AppName);
                    strHTMLBuilder.Append("</td>");

                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.CreatedDate);
                    strHTMLBuilder.Append("</td>");

                }
                //Close tags.  
                strHTMLBuilder.Append("</table>");
                strHTMLBuilder.Append("</body>");
                strHTMLBuilder.Append("</html>");

                string Htmltext = strHTMLBuilder.ToString();
                Console.Write("\n Converted Data to HTML Format of Report: ApplicationDetail");
                return Htmltext;
            }
            catch (Exception e)
            {
                Console.Write("\n Error Message at function <convertAppDetailDataToHTML>  - " + e.Message);
            }
            return null;
        }
     
        private static string convertProjectSummaryDataToHTML(List<ProjectSummary> lstProjectSummary)
        {
            try
            {
                StringBuilder strHTMLBuilder = new StringBuilder();
                strHTMLBuilder.Append("<html >");
                strHTMLBuilder.Append("<head>");
                strHTMLBuilder.Append("</head>");
                strHTMLBuilder.Append("<body>");      
                strHTMLBuilder.Append(setTableStyle());

                strHTMLBuilder.Append("<tr >");

                //Column Name
                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("Machine Name");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("User Name");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("Key Log");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("Browser Detail");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("App Detail");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("Idle Time");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("</tr>");

                //Row Data
                foreach (var myColumn in lstProjectSummary)
                {
                    strHTMLBuilder.Append("<tr >");
                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.MachineName);
                    strHTMLBuilder.Append("</td>");

                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.UserName);
                    strHTMLBuilder.Append("</td>");

                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.KeyLoggerCount);
                    strHTMLBuilder.Append("</td>");

                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.BrowserDetailCount);
                    strHTMLBuilder.Append("</td>");

                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.AppDetailCount);
                    strHTMLBuilder.Append("</td>");

                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.IdleTimeCount);
                    strHTMLBuilder.Append("</td>");

                }
                //Close tags.  
                strHTMLBuilder.Append("</table>");
                strHTMLBuilder.Append("</body>");
                strHTMLBuilder.Append("</html>");

                string Htmltext = strHTMLBuilder.ToString();
                Console.Write("\n Converted Data to HTML Format of Report: ProjectSummary");
                return Htmltext;
            }
            catch (Exception e)
            {
                Console.Write("\n Error Message at function <convertProjectSummaryDataToHTML>  - " + e.Message);
            }
            return null;
        }

        private static string convertRecentCustomersDataToHTML(List<Customer> lstRecentCustomers)
        {
            try
            {
                StringBuilder strHTMLBuilder = new StringBuilder();
                strHTMLBuilder.Append("<html >");
                strHTMLBuilder.Append("<head>");
                strHTMLBuilder.Append("</head>");
                strHTMLBuilder.Append("<body>");
                strHTMLBuilder.Append(setTableStyle());

                strHTMLBuilder.Append("<tr >");

                //Column Name
                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("Customer Name");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("Organisation Name");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("Email");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("Counts");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("Downloads");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");
                
                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append("<b>");
                strHTMLBuilder.Append("Created Date");
                strHTMLBuilder.Append("</b>");
                strHTMLBuilder.Append("</td>");

                strHTMLBuilder.Append("</tr>");

                //Row Data
                foreach (var myColumn in lstRecentCustomers)
                {
                    strHTMLBuilder.Append("<tr >");
                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.FirstName + " " + myColumn.LastName);
                    strHTMLBuilder.Append("</td>");

                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.OrganizationName);
                    strHTMLBuilder.Append("</td>");

                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.Email);
                    strHTMLBuilder.Append("</td>");

                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.count);
                    strHTMLBuilder.Append("</td>");

                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.Downloads);
                    strHTMLBuilder.Append("</td>");
                    
                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myColumn.CreateDate);
                    strHTMLBuilder.Append("</td>");

                }
                //Close tags.  
                strHTMLBuilder.Append("</table>");
                strHTMLBuilder.Append("</body>");
                strHTMLBuilder.Append("</html>");

                string Htmltext = strHTMLBuilder.ToString();
                Console.Write("\n Converted Data to HTML Format of Report: RecentCustomers");
                return Htmltext;
            }
            catch (Exception e)
            {
                Console.Write("\n Error Message at function <convertRecentCustomersDataToHTML>  - " + e.Message);
            }
            return null;
        }
        
        #endregion

        #region Send Email To Customer
        public static bool sendSchedulerEMail(string ReportName, string HtmlBody, string CustomerEmailID)
        {   
            try
            {
                Console.Write("\n Sending Email In Progress...");              
                var sToAddress = CustomerEmailID;
                var sSubject = "Live Monitoring Scheduler Report : [" + ReportName + "]";
                var sBody = HtmlBody;

                EmailUtils oEmailUtils = new EmailUtils(sToAddress, sSubject, sBody);
                return oEmailUtils.SendMail();   
            }
            catch (Exception e)
            {
                Console.Write("\n Error Message at function <sendSchedulerEMail> - " + e.Message);
            }
            Console.Write("\n FAILED Sending Email of Report :" + ReportName + " to Customer Email ID:" + CustomerEmailID);
            return false;
        }
        #endregion

        #region send Report to Admin Only
        public static void _RecentCustomers()
        {
            try
            {
                using (HttpClient client = Connection.GetHttpClient())
                {

                    Console.Write("\n\n Jumped into _RecentCustomers Function");
                    Console.Write("\n Calling _RecentCustomers API function ");
                     AppSettingReader asr = new AppSettingReader();
                     string adminEmailIDs = asr.ReadKey("AdminEmailIDs");


                    HttpResponseMessage Response = client.GetAsync("api/LiveMonitoringAPI/_RecentCustomers").Result;
                    Console.Write("\n Response IsSuccessStatusCode : " + Response.IsSuccessStatusCode);
                    if (Response.IsSuccessStatusCode == true)
                    {
                        List<Customer> objCustomerlst = new List<Customer>();
                        objCustomerlst = Response.Content.ReadAsAsync<List<Customer>>().Result;
                        if (objCustomerlst != null && objCustomerlst.Count > 0)
                        {
                            Console.Write("\n Fetched Data of Report : RecentCustomers  Total Records : " + objCustomerlst.Count);
                            string HtmlBody = convertRecentCustomersDataToHTML(objCustomerlst);
                            if (sendSchedulerEMail("RecentCustomers", HtmlBody, adminEmailIDs))
                            {
                                Console.Write("\n Send Report RecentCustomers to Email ID:" + adminEmailIDs);
                                Console.Write("\n Successfully Send Email...");
                                UpdateMachineSendDateStatus();
                            }
                        }
                        else
                        {
                            Console.Write("\n No Data fetched of Report RecentCustomers");
                        }
                    }
                }                
            }
            catch (Exception e)
            {
                Console.Write("\n Error Message at function <_RecentCustomers>  - " + e.Message);
            }
        }

        private static void UpdateMachineSendDateStatus()
        {
 	        try
            {
                using (HttpClient client = Connection.GetHttpClient())
                {
                    Console.Write("\n Jumped into UpdateMachineSendDateStatus API function ");
                    var Response = client.PostAsync("api/LiveMonitoringAPI/UpdateMachineSendDateStatus", null).Result;
                    Console.Write("\n Response IsSuccessStatusCode : " + Response.IsSuccessStatusCode);
                    if (Response.IsSuccessStatusCode == true)
                    {
                        Console.Write("\n Successfully Updated Schedule Reports Status...  ");
                    }
                    else if (Response != null && Response.IsSuccessStatusCode == false)
                    {
                        var result = Response.Content.ReadAsStringAsync().Result;
                        Console.Write("\n Error Message at function<UpdateMachineSendDateStatus of Report RecentCustomer>" + " : " + result);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write("\n Error Message at function <UpdateMachineSendDateStatus of Report RecentCustomer>" + " : " + e.Message);
            }
        }

        #endregion


        #region Models
        public class ScheduleReportsSettings
        {
            public int ScheduleReportId { get; set; }
            public int ScreenId { get; set; }
            public string ScreenInternalName { get; set; }
            public int MembershipId { get; set; }
            public string ScheduleType { get; set; }
            public string Email { get; set; }
            public int CustomerId { get; set; }
            public DateTime SendDate { get; set; }
            public bool IsSend { get; set; }
            public string CustomerName { get; set; }
            public int countCustomer { get; set; }
        }
        public class MachineDetail
        {
            public string MachineName { get; set; }
            public string UserName { get; set; }
            public string MachineMacAddress { get; set; }
            public string MachineIP { get; set; }
            public DateTime? CreatedDate { get; set; }
            public bool IsBlocked { get; set; }
            public int CustomerId { get; set; }
            public Customer customer { get; set; }
        }
        public class MachineIdle
        {
            public string MachineName { get; set; }
            public string UserName { get; set; }
            public int IdleTime { get; set; }
            public DateTime? CreatedDate { get; set; }
        }
        public class KeyLogging
        {
            public string MachineName { get; set; }
            public string UserName { get; set; }
            public string TextType { get; set; }
            public string Text { get; set; }
            public DateTime? CreatedDate { get; set; }
            public MachineDetail machine_detail { get; set; }
        }
        public class AppDetail
        {
            public int AppId { get; set; }
            public string Title { get; set; }
            public string AppName { get; set; }
            public int MachineDetailId { get; set; }
            public DateTime? CreatedDate { get; set; }
            public int count { get; set; }
            public MachineDetail machine_detail { get; set; }
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
            public int count { get; set; }
            public MachineDetail machine_detail { get; set; }
        }
        public class ProjectSummary
        {
            public int MachineDetailId { get; set; }
            public string MachineName { get; set; }
            public string UserName { get; set; }
            public string KeyLoggerCount { get; set; }
            public string BrowserDetailCount { get; set; }
            public string AppDetailCount { get; set; }
            public string IdleTimeCount { get; set; }
            public string IdleTimeSum { get; set; }
        }
        public class Customer
        {
            public int CustomerId { get; set; }
            public int MembershipId { get; set; }
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string OrganizationName { get; set; }
            public DateTime? CreateDate { get; set; }
            public DateTime? LastLoginDate { get; set; }
            public int count { get; set; }
            public int Downloads { get; set; }           
        }
               
        #endregion

        #region ----Web API parameters For POST-----
        public class ScheduleReportsPOSTparam
        {
            public int ScreenId { get; set; }
            public int MembershipId { get; set; }
            public string sSheduleType { get; set; }
        }
        #endregion
    }
}
