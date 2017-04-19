using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace LiveMonitoringWeb.Models
{
    public static class Utility
    {
        private static DBContext db = new DBContext();
        public static String EllipsisString(String source, Int32 length)
        {
            if (source == null)
            {
                return source = "";
            }
            else
            {
                if (source.Length >= length)
                {
                    return source.Substring(0, length) + "...";
                }
            }
            return source;
        }

        public static string GetDomainPart(string url)
        {
            var doubleSlashesIndex = url.IndexOf("://");
            var start = doubleSlashesIndex != -1 ? doubleSlashesIndex + "://".Length : 0;
            var end = url.IndexOf("/", start);
            if (end == -1)
                end = url.Length;

            string trimmed = url.Substring(start, end - start);
            if (trimmed.StartsWith("www."))
                trimmed = trimmed.Substring("www.".Length);

            if (trimmed.IndexOf(":") > 0)
            {
                var colonIndex = trimmed.IndexOf(":");
                trimmed = trimmed.Substring(0, colonIndex);
            }

            return trimmed;
        }

        public static void SetDates(string dateFrom, string dateTo)
        {
            HttpContext.Current.Session["DateFrom"] = dateFrom.ToString();
            HttpContext.Current.Session["DateTo"] = dateTo.ToString();
        }

        public static DateTime GetStartDate()
        {
            if (HttpContext.Current.Session["DateFrom"] == null)
            {
                HttpContext.Current.Session["DateFrom"] = DateTime.Now.Date.ToString("MM-dd-yyyy") + " 00:00:00"; ;
            }


            string timeString = HttpContext.Current.Session["DateFrom"].ToString();

            try
            {
                return DateTime.ParseExact(timeString, "MM-dd-yyyy HH:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None);
            }
            catch (Exception)
            {
                return DateTime.ParseExact(timeString, "MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None);
            }
           

            //return Convert.ToDateTime(HttpContext.Current.Session["DateFrom"].ToString(), "MM-dd-yyyy HH:mm:ss");           
        }

        public static DateTime GetEndDate()
        {
            if (HttpContext.Current.Session["DateTo"] == null)
            {
                HttpContext.Current.Session["DateTo"] = DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss");
            }

            string timeString = HttpContext.Current.Session["DateTo"].ToString();
            try
            {
                 return DateTime.ParseExact(timeString, "MM-dd-yyyy HH:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None);
            }
            catch (Exception)
            {
                return DateTime.ParseExact(timeString, "MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None);
            }
           

            //return Convert.ToDateTime(HttpContext.Current.Session["DateTo"].ToString());
        }

        public static int GetCustomerId()
        {
            int userId = WebMatrix.WebData.WebSecurity.CurrentUserId;
            using (var db = new DBContext())
            {      
                int customerId = db.Customers.Where(a => a.MembershipId == userId).Select(a => a.CustomerId).FirstOrDefault();
                if (customerId == 0)
                {
                    customerId = db.CustomerUserMapping.Where(a => a.MembershipId == userId).Select(a => a.CustomerId).FirstOrDefault();
                }
                return customerId;
            }
        }

    }
}