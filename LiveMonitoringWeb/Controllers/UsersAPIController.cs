using CommonUtility;
using LiveMonitoringWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace LiveMonitoringWeb.Controllers
{
    public class UsersAPIController : ApiController
    {
        #region HttpGet

        [System.Web.Http.HttpGet]
        public List<UserProfile> GetUsers()
        {
            List<UserProfile> ListUserProfile = new List<UserProfile>();
            try
            {
                using (var db = new DBContext())
                {
                    var obj = (from u in db.tbl_UserProfile
                               join m in db.tbl_webpages_Membership on u.UserId equals m.UserId
                               join ur in db.tbl_webpages_UsersInRoles on m.UserId equals ur.UserId
                               join wr in db.tbl_webpages_Roles on ur.RoleId equals wr.RoleId
                               select new
                               {
                                   UserId = u.UserId,
                                   UserName = u.UserName,
                                   Password = m.Password,
                                   CreateDate = m.CreateDate,
                                   RoleName = wr.RoleName
                               });
                    ListUserProfile = obj.ToList().Select(r => new UserProfile
                    {
                        UserId = r.UserId,
                        UserName = r.UserName,
                        Password = r.Password,
                        CreateDate = r.CreateDate,
                        RoleName = r.RoleName
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return ListUserProfile;
        }

        [System.Web.Http.HttpGet]
        public IList<SelectListItem> GetActiveUser(string userName)
        {
            List<SelectListItem> ActiveUserList = new List<SelectListItem>();
            try
            {
                using (var db = new DBContext())
                {
                    if (System.Web.Security.Roles.GetRolesForUser(userName).Single() == "Admin")
                    {
                        var comp = (from c in db.tbl_UserProfile
                                    orderby c.UserName ascending
                                    select
                                        new
                                        {
                                            c.UserId,
                                            c.UserName,
                                        }).ToList();

                        if (comp != null)
                        {
                            foreach (var obj in comp)
                            {
                                ActiveUserList.Add(new SelectListItem { Text = obj.UserName, Value = Convert.ToString(obj.UserId) });
                            }
                        }
                    }
                    else
                    {
                        int userID = WebSecurity.GetUserId(userName);
                        var comp = (from c in db.tbl_UserProfile
                                    where c.UserId == userID
                                    orderby c.UserName ascending
                                    select
                                        new
                                        {
                                            c.UserId,
                                            c.UserName,
                                        }).ToList();

                        if (comp != null)
                        {
                            foreach (var obj in comp)
                            {
                                ActiveUserList.Add(new SelectListItem { Text = obj.UserName, Value = Convert.ToString(obj.UserId) });
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return ActiveUserList;
        }

        [System.Web.Http.HttpGet]
        public IList<SelectListItem> ListRoles()
        {
            List<SelectListItem> roles = new List<SelectListItem>();
            roles.Add(new SelectListItem { Text = "User", Value = "2" });
            roles.Add(new SelectListItem { Text = "Admin", Value = "1" });

            return roles;
        }

        [System.Web.Http.HttpGet]
        public webpages_UsersInRoles GetUsersInRolesByID(int? UserId)
        {
            webpages_UsersInRoles model = new webpages_UsersInRoles();
            try
            {
                using (var db = new DBContext())
                {
                    model = (from u in db.tbl_webpages_UsersInRoles
                             where u.UserId == UserId.Value
                             select u).ToList().FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return model;
        }

        [System.Web.Http.HttpGet]
        public string GetPasswordResetToken(string UserName)
        {
            var token = WebSecurity.GeneratePasswordResetToken(UserName);
            return token;
        }

        [System.Web.Http.HttpGet]
        public void GetResetPassword(string token, string password)
        {
            WebSecurity.ResetPassword(token, password);
        }

        [System.Web.Http.HttpGet]
        public string GetMemberShipPassWordByID(int? UserId)
        {
            string model = string.Empty;
            try
            {
                using (var db = new DBContext())
                {
                    model = (from m in db.tbl_webpages_Membership
                             where m.UserId == UserId.Value
                             select m.Password).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return model;
        }

        [System.Web.Http.HttpGet]
        public UserProfile GetUserProfileByID(int? UserId)
        {
            UserProfile model = new UserProfile();
            try
            {
                using (var db = new DBContext())
                {
                    model = (from u in db.tbl_UserProfile
                             where u.UserId == UserId.Value
                             select u).ToList().FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return model;
        }

        [System.Web.Http.HttpGet]
        public int CntDuplicateUserName(string UserName, int UserId)
        {
            DBContext db = new DBContext();
            var Cnt = 0;
            try
            {
                Cnt = Convert.ToInt16((from u in db.tbl_UserProfile
                                       where (u.UserName).ToUpper().Trim() == (UserName).ToUpper().Trim() && u.UserId != UserId
                                       select u).Count());
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }

            return Cnt;
        }

        [System.Web.Http.HttpGet]
        public int GetUserIDByName(string userName)
        {
            return Convert.ToInt16(WebSecurity.GetUserId(userName));
        }

        #endregion HttpGet

        #region HttpPost

        [System.Web.Mvc.HttpPost]
        public void CreateUserAndAccount(string userName, string password)
        {
            WebSecurity.CreateUserAndAccount(userName, password);
        }

        [System.Web.Mvc.HttpPost]
        public void AddUserToRoles(string userName, string RoleName)
        {
            Roles.AddUserToRoles(userName, new[] { RoleName });
        }

        public void AddToCust_User_Mapping(CustomerUserMapping model)
        {
            using (var db = new DBContext())
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(model).State = EntityState.Added;
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ExceptionHandler handler = new ExceptionHandler();
                    handler.HandleException(e);
                }
            }
        }
        #endregion HttpPost

        #region HttpPut

        [System.Web.Mvc.HttpPut]
        public void PutUserProfile(UserProfile model)
        {
            using (var db = new DBContext())
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(model).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ExceptionHandler handler = new ExceptionHandler();
                    handler.HandleException(e);
                }
            }
        }

        public string PutUserProfileDate(UserProfile model)
        {
            try
            {
                using (var db = new DBContext())
                {
                    if (ModelState.IsValid)
                    {
                        var getUserProfile = GetUserProfileByID(model.UserId);

                        if (Convert.ToString(model.Password).Trim().Length < 6)
                        {
                            return "exist";
                        }

                        getUserProfile.Password = "x";
                        getUserProfile.ComfirmPassword = "x";

                        var getUserRole = GetUsersInRolesByID(model.UserId);
                        if (model.RoleName == "Admin")
                            getUserRole.RoleId = 1;
                        else
                            getUserRole.RoleId = 2;
                        PutUserRoles(getUserRole);
                        PutUserProfile(getUserProfile);

                        if (GetMemberShipPassWordByID(model.UserId) != Convert.ToString(model.Password))
                        {
                            var token = GetPasswordResetToken(getUserProfile.UserName);
                            GetResetPassword(token, Convert.ToString(model.Password));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return null;
        }

        public string PostUserProfileDate(UserProfile model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DBContext db = new DBContext();
                    if (CntDuplicateUserName(model.UserName, model.UserId) > 0)
                    {
                        return "exist";
                    }

                    if (Convert.ToString(model.Password).Trim().Length < 6)
                    {
                        return "password";
                    }
                    else
                    {
                        var userName = Convert.ToString(model.UserName).Trim();
                        var password = Convert.ToString(model.Password).Trim();
                        CreateUserAndAccount(userName, password);
                        string RoleName = string.Empty;

                        AddUserToRoles(userName, model.RoleName);
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return null;
        }

        [System.Web.Mvc.HttpPut]
        public void PutUserRoles(webpages_UsersInRoles model)
        {
            DBContext db = new DBContext();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
        }

        #endregion HttpPut

       
    }

}
