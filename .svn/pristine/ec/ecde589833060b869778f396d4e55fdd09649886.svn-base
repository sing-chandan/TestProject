using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using LiveMonitoringWeb.Models;
using CommonUtility;
using LiveMonitoringWeb.Classes;

namespace LiveMonitoringWeb.Controllers
{
    [Authorize(Roles = "Customer, User")]
    public class AppSummaryController : Controller
    {
        //
        // GET: /AppSummary/

        public ActionResult Index()
        {
            //Check Authorization
            if (!cls_Authorization.isAllowedURL("Dashboard_TopApplications")) return RedirectToAction("login", "account");

            List<AppDetail> objAppDetaillst = new List<AppDetail>();
            try
            {
                using (var db = new DBContext())
                {
                    DateTime dFrom = Utility.GetStartDate();
                    DateTime dTo = Utility.GetEndDate();
                    int customerId = Utility.GetCustomerId();
                    var objAppDetail = db.AppDetails.Include(a=> a.machine_detail).Where(x => x.CreatedDate >= dFrom && x.CreatedDate <= dTo && x.machine_detail.CustomerId == customerId).OrderByDescending(a => a.CreatedDate).GroupBy(g => new { g.AppName }).Select(g => new
                    {
                        AppName = g.Key.AppName,
                        Count = g.Count()
                    }).ToList().OrderByDescending(a => a.Count).ToList();

                    int id = 0;
                    foreach (var item in objAppDetail)
                    {
                        id = id + 1;
                        objAppDetaillst.Add(new AppDetail { AppId = id, AppName = item.AppName, count = item.Count });
                    }
                    return View(objAppDetaillst);
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return View(objAppDetaillst);
        }
    }

}

