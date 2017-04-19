using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LiveMonitoringWeb.Models;
using CommonUtility;
using WebMatrix.WebData;
using System.Text.RegularExpressions;
using System.Data;
using LiveMonitoringWeb.Classes;

namespace LiveMonitoringWeb.Controllers
{
    [Authorize(Roles = "Customer")]
    public class UserPermissionController : Controller
    {
        //
        // GET: /UserPermission/       
        public ActionResult Index(int userId = 0)
        {
            string roleName = System.Web.Security.Roles.GetRolesForUser().Single();
            if (roleName.ToUpper() == "USER")
            {
                return RedirectToAction("login", "account");
            }
            List<Screens> ScreenUserPermission = new List<Screens>();
            try
            {
                using (var db = new DBContext())
                {
                    ViewBag.UserList = GetUserList();
                    if (userId == 0)
                    {
                        userId = Session["UserId"] != null ? Convert.ToInt32(Session["UserId"]) : Convert.ToInt32(Session["storeUserId"]);
                        ViewBag.selecteduser = userId;
                    }

                    else
                    {
                        ViewBag.selecteduser = userId;
                        Session["UserId"] = userId;
                    }

                    var query = (from scr in db.Screens
                                 select new
                                 {
                                     ScreenId = scr.ScreenId,
                                     ScreenDisplayName = scr.ScreenDisplayName,
                                     ScreenType = scr.ScreenType,
                                     Selected = ((from u in db.UserScreenPermissions
                                                  where scr.ScreenId == u.ScreenId && u.MembershipId == userId
                                                  select u.ScreenId).AsEnumerable().Count()) > 0 ? true : false,

                                     countRecord = (from u in db.Screens select u.ScreenId).AsEnumerable().Distinct().Count(),
                                 }).ToList();


                    ScreenUserPermission = query.ToList().Select(r => new Screens
                    {
                        ScreenId = r.ScreenId,
                        ScreenDisplayName = r.ScreenDisplayName,
                        ScreenType = r.ScreenType,
                        Selected = r.Selected,
                        countRecord = r.countRecord,
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return View(ScreenUserPermission);

        }

        public JsonResult JsonUserPermission(int userId = 0)
        {
            ReturnUserPermission returnUserPermission = new ReturnUserPermission();

            string roleName = System.Web.Security.Roles.GetRolesForUser().Single();
            if (roleName.ToUpper() == "USER")
            {
               // return RedirectToAction("login", "account");
            }
            List<Screens> ScreenUserPermission = new List<Screens>();

            try
            {
                using (var db = new DBContext())
                {
                    returnUserPermission.Userlist = GetUserList();
                    if (userId == 0)
                    {
                        userId = Session["UserId"] != null ? Convert.ToInt32(Session["UserId"]) : Convert.ToInt32(Session["storeUserId"]);
                        returnUserPermission.selectId = userId;
                    }

                    else
                    {
                        returnUserPermission.selectId = userId;
                        Session["UserId"] = userId;
                    }

                    var query = (from scr in db.Screens
                                 select new
                                 {
                                     ScreenId = scr.ScreenId,
                                     ScreenDisplayName = scr.ScreenDisplayName,
                                     ScreenType = scr.ScreenType,
                                     Selected = ((from u in db.UserScreenPermissions
                                                  where scr.ScreenId == u.ScreenId && u.MembershipId == userId
                                                  select u.ScreenId).AsEnumerable().Count()) > 0 ? true : false,

                                     countRecord = (from u in db.Screens select u.ScreenId).AsEnumerable().Distinct().Count(),
                                 }).ToList();


                    ScreenUserPermission = query.ToList().Select(r => new Screens
                    {
                        ScreenId = r.ScreenId,
                        ScreenDisplayName = r.ScreenDisplayName,
                        ScreenType = r.ScreenType,
                        Selected = r.Selected,
                        countRecord = r.countRecord,
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }

            returnUserPermission.ScreenList = ScreenUserPermission;
            
            return Json(returnUserPermission,JsonRequestBehavior.AllowGet);

        }

        public IList<SelectListItem> GetUserList()
        {
            List<SelectListItem> UserList = new List<SelectListItem>();
            try
            {
                int loginUserId = WebSecurity.CurrentUserId;
                int CustomerId = 0;
                string roleName = System.Web.Security.Roles.GetRolesForUser().Single();


                using (var db = new DBContext())
                {
                    if (roleName.ToUpper() == "CUSTOMER")
                    {
                        CustomerId = Utility.GetCustomerId();
                    }

                    var query = (from u in db.tbl_UserProfile
                                 join cum in db.CustomerUserMapping on u.UserId equals cum.MembershipId
                                 where cum.CustomerId == CustomerId
                                 orderby u.UserName
                                 select
                                     new
                                     {
                                         u.UserId,
                                         u.UserName
                                     }).ToList();

                    if (query != null)
                    {
                        foreach (var obj in query)
                        {
                            UserList.Add(new SelectListItem { Text = obj.UserName, Value = Convert.ToString(obj.UserId) });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            Session["storeUserId"] = UserList.FirstOrDefault().Value;
            return UserList;
        }

        public void SaveUserPermission(string screenId, int userId)
        {
            UserScreenPermissions addModel = new UserScreenPermissions();
            string[] arrScreen = screenId.Split('\n');

            using (var db = new DBContext())
            {
                var model = (from u in db.UserScreenPermissions where u.MembershipId == userId select u).ToList();
                foreach (var itm in model)
                {
                    db.Entry(itm).State = EntityState.Deleted;
                    db.SaveChanges();
                }

                foreach (var screenName in arrScreen)
                {
                    if (screenName.Trim().Length > 0)
                    {
                        int Id = Convert.ToInt32(screenName.Trim());
                        //string Id = (from s in db.Screens where s.ScreenDisplayName == screenName.Trim() select s.ScreenId).ToList().FirstOrDefault();
                        addModel.ScreenId = Convert.ToInt32(Id);
                        addModel.MembershipId = userId;
                        addModel.CreatedDate = DateTime.UtcNow;

                        db.Entry(addModel).State = EntityState.Added;
                        db.SaveChanges();
                    }

                }

            }
        }
    }
}