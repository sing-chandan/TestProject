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
    public class BrowserDetailsController : Controller
    {
        //
        // GET: /BrowserDetails/

        public ActionResult Index(int MachineDetailId = 0, String URL = null)
        {
            //Check Authorization
          
            if (!cls_Authorization.isAllowedURL("Report_BrowserDetails")) return RedirectToAction("login", "account");
            List<BrowserDetail> BrowserDetail = new List<BrowserDetail>();
            LiveMonitoringAPIController oAPI = new LiveMonitoringAPIController();
            int customerId = Utility.GetCustomerId();
            try
            {
                DateTime dFrom = Utility.GetStartDate();
                DateTime dTo = Utility.GetEndDate();
                BrowserDetail = oAPI._Sites(dFrom, dTo, customerId, MachineDetailId, URL);
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return View(BrowserDetail);
        }

        public JsonResult JsonBrowserDetail(int MachineDetailId = 0, String URL = null)
       {
            List<BrowserDetail> BrowserDetail = new List<BrowserDetail>();
            LiveMonitoringAPIController oAPI = new LiveMonitoringAPIController();
            int customerId = Utility.GetCustomerId();
            try
            {
                DateTime dFrom = Utility.GetStartDate();
                DateTime dTo = Utility.GetEndDate();
                BrowserDetail = oAPI._Sites(dFrom, dTo, customerId, MachineDetailId, URL);
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return Json(BrowserDetail, JsonRequestBehavior.AllowGet);
        }
    }
}
