using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LiveMonitoringWeb.Models;
using System.Data;
using System.Data.Entity;
using CommonUtility;
using LiveMonitoringWeb.Classes;

namespace LiveMonitoringWeb.Controllers
{
    [Authorize(Roles = "Customer, User")]
    public class ReportController : Controller
    {
        //
        // GET: /Reports/

        public ActionResult ProjectSummary()
        {

            //Check Authorization
            if (!cls_Authorization.isAllowedURL("Report_ProjectSummary")) return RedirectToAction("login", "account");
            List<ProjectStatics> objProjectStatisticslst = new List<ProjectStatics>();
            LiveMonitoringAPIController oAPI = new LiveMonitoringAPIController();
           
            int customerId = Utility.GetCustomerId();
            try
            {
                DateTime dFrom = Utility.GetStartDate();
                DateTime dTo = Utility.GetEndDate();
                objProjectStatisticslst = oAPI._ProjectSummary(dFrom, dTo, customerId);
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return View(objProjectStatisticslst);

        }

        public JsonResult JsonProjectSummary()
        {

            //Check Authorization

            List<ProjectStatics> objProjectStatisticslst = new List<ProjectStatics>();
            LiveMonitoringAPIController oAPI = new LiveMonitoringAPIController();

            int customerId = Utility.GetCustomerId();
            try
            {
                DateTime dFrom = Utility.GetStartDate();
                DateTime dTo = Utility.GetEndDate();
                objProjectStatisticslst = oAPI._ProjectSummary(dFrom, dTo, customerId);
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return Json(objProjectStatisticslst, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ProductiveReport()
        {
            //Check Authorization
            if (!cls_Authorization.isAllowedURL("Report_ProductivityReportGraph")) return RedirectToAction("login", "account");

            return View();
        }

        public ActionResult SitesReport()
        {
            //Check Authorization
            if (!cls_Authorization.isAllowedURL("Report_SitesReportGraph")) return RedirectToAction("login", "account");

            return View();
        }

        public ActionResult IdleReport()
        {
            //Check Authorization
            if (!cls_Authorization.isAllowedURL("Report_UserIdleReportGraph")) return RedirectToAction("login", "account");

            return View();
        }
              

        #region Json Data for Graph/Chart

        public JsonResult JsonIdleReport()
        {
            try
            {
                using (var db = new DBContext())
                {
                    DateTime dFrom = Utility.GetStartDate();
                    DateTime dTo = Utility.GetEndDate();
                    int customerId = Utility.GetCustomerId();
                    var data = db.MachineIdles.Include(a => a.machine_detail)
                        .Where(x => x.CreatedDate >= dFrom && x.CreatedDate <= dTo && x.machine_detail.CustomerId == customerId)
                        .GroupBy(g => new { g.machine_detail.UserName, g.machine_detail.MachineName })
                        .Select(g => new
                        {
                            User = g.Key.MachineName + "/" + g.Key.UserName,
                            IdleMinute = (g.Sum(x => x.IdleTime) / 60)
                        }).OrderByDescending(a => a.IdleMinute).ToList();

                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
                return Json(string.Empty);
            }
        }

        public JsonResult JsonSitesReport()
        {
            try
            {
                using (var db = new DBContext())
                {
                    DateTime dFrom = Utility.GetStartDate();
                    DateTime dTo = Utility.GetEndDate();
                    int customerId = Utility.GetCustomerId();
                    var data = (from rs in db.BrowserDetails.Include(a => a.machine_detail).Where(x => x.CreatedDate >= dFrom && x.CreatedDate <= dTo && x.machine_detail.CustomerId == customerId).ToList()
                                select new
                                {
                                    domain = Utility.GetDomainPart(rs.URL)
                                })
                                  .GroupBy(g => new { g.domain })
                                  .Select(g => new
                                  {
                                      Domain = g.Key.domain,
                                      Count = g.Count()
                                  }).OrderByDescending(a => a.Count).ToList();

                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
                return Json(string.Empty);
            }
        }

        public JsonResult JsonProductiveReport()
        {
            try
            {
                using (var db = new DBContext())
                {
                    DateTime dFrom = Utility.GetStartDate();
                    DateTime dTo = Utility.GetEndDate();
                    int customerId = Utility.GetCustomerId();
                    var midData = (from rs in db.BrowserDetails.Include(a => a.machine_detail)
                                .Where(x => x.CreatedDate >= dFrom && x.CreatedDate <= dTo && x.machine_detail.CustomerId == customerId).ToList()
                                   select new
                                   {
                                       domain = Utility.GetDomainPart(rs.URL),
                                       UserName = rs.machine_detail.UserName,
                                       MachineName = rs.machine_detail.MachineName
                                   })
                                 .GroupBy(g => new { g.domain, g.UserName, g.MachineName })
                                 .Select(g => new
                                 {
                                     Domain = g.Key.domain,
                                     User = g.Key.MachineName + "/" + g.Key.UserName,
                                     Count = g.Count()
                                 }).OrderByDescending(a => a.Count).ToList();

                    var midAppData = (from rs in db.AppDetails.Include(a => a.machine_detail)
                                        .Where(x => x.CreatedDate >= dFrom && x.CreatedDate <= dTo && x.machine_detail.CustomerId == customerId).ToList()
                                      select new
                                      {
                                          App = rs.AppName,
                                          UserName = rs.machine_detail.UserName,
                                          MachineName = rs.machine_detail.MachineName
                                      })
                                       .GroupBy(g => new { g.App, g.UserName, g.MachineName })
                                 .Select(g => new
                                 {
                                     Domain = g.Key.App,
                                     User = g.Key.MachineName + "/" + g.Key.UserName,
                                     Count = g.Count()
                                 }).OrderByDescending(a => a.Count).ToList();

                    midData.AddRange(midAppData);
                    var data = (from rs in midData
                                join subCat in db.SubCategory on rs.Domain equals subCat.SubCategoryName
                                select new
                                {
                                    User = rs.User,
                                    Productive = subCat.IsProductive == true ? rs.Count : 0,
                                    NonProductive = subCat.IsProductive == false ? rs.Count : 0
                                })
                                .GroupBy(g => g.User)
                                .Select(g => new
                                {
                                    User = g.Key,
                                    Productive = g.Sum(x => x.Productive),
                                    NonProductive = g.Sum(x => x.NonProductive)
                                })
                                .Select(a => new
                                {
                                    User = a.User.ToLower(),
                                    Productive = Math.Round((a.Productive * 100M) / (a.Productive + a.NonProductive), 2),
                                    NonProductive = Math.Round((a.NonProductive * 100M) / (a.Productive + a.NonProductive), 2)
                                })
                                .ToList();

                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
                return Json(string.Empty);
            }
        }

        #endregion
    }
}
