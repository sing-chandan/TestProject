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
    public class SiteSummaryController : Controller
    {
        //
        // GET: /SiteSummary/

        public ActionResult Index()
        {
            //Check Authorization
            if (!cls_Authorization.isAllowedURL("Dashboard_TopSites")) return RedirectToAction("login", "account");

            List<BrowserDetail> objBrowserDetaillst = new List<BrowserDetail>();
            try
            {
                using (var db = new DBContext())
                {

                    DateTime dFrom = Utility.GetStartDate();
                    DateTime dTo = Utility.GetEndDate();
                    int customerId = Utility.GetCustomerId();
                    var data = (from rs in db.BrowserDetails.Include(a=> a.machine_detail).Where(x => x.CreatedDate >= dFrom && x.CreatedDate <= dTo && x.machine_detail.CustomerId == customerId).ToList()
                                select new
                                {
                                    domain = Utility.GetDomainPart(rs.URL)
                                })
                                     .GroupBy(g => new { g.domain })
                                     .Select(g => new
                                     {
                                         Domain = g.Key.domain.StartsWith("localhost") ? "localhost" : g.Key.domain,
                                         Count = g.Count()
                                     }).OrderByDescending(a => a.Count).ToList();

                    int id = 0;
                    foreach (var item in data)
                    {
                        id = id + 1;
                        objBrowserDetaillst.Add(new BrowserDetail { BrowserDetailId = id, URL = item.Domain, count = item.Count });
                    }
                    return PartialView(objBrowserDetaillst);
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return PartialView(objBrowserDetaillst);
        }
    }
}
