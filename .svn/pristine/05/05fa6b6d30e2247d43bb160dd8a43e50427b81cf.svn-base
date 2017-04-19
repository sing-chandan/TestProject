using CommonUtility;
using LiveMonitoringWeb.Models;
using System;
using System.Data;
using System.Linq;
using System.Web;
using WebMatrix.WebData;

namespace LiveMonitoringWeb.Classes
{
    public static class cls_Authorization
    {
        public static bool isAllowedURL(Uri requestedURL)
        {
            return isAllowedURL(requestedURL.ToString());
        }

        public static bool isAllowedURL(string requestedURL)
        {
            bool allow = false;
            try
            {                           
                string roleName= System.Web.Security.Roles.GetRolesForUser().Single();
                int loginUserId = WebSecurity.CurrentUserId;
                using (var db = new DBContext())
                {
                    if(roleName.ToUpper()=="USER")
                    {
                        var ScreenId = (from s in db.Screens where s.ScreenInternalName.ToUpper() == requestedURL.ToUpper() select s.ScreenId).ToList().FirstOrDefault();
                        int count = (from u in db.UserScreenPermissions where u.MembershipId == loginUserId && u.ScreenId == ScreenId select u.ScreenId).ToList().Count();
                        if(count>0)
                        {
                            allow = true;
                        }
                        else
                        {
                            allow = false;
                        }

                    }
                    else
                    {
                        allow=true;
                    }
                   
                }
               
              
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return allow;
        }
    }
}