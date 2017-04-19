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
    public class AppDetailsController : Controller
    {
        //
        // GET: /AppDetails/

        public ActionResult Index(int MachineDetailId = 0, String AppName = null)
        {

            //Check Authorization
            if (!cls_Authorization.isAllowedURL("Report_ApplicationDetails")) return RedirectToAction("login", "account");
            List<AppDetail> AppDetail = new List<AppDetail>();
            LiveMonitoringAPIController oAPI = new LiveMonitoringAPIController();
            int customerId = Utility.GetCustomerId();
            try
            {
                DateTime dFrom = Utility.GetStartDate();
                DateTime dTo = Utility.GetEndDate();
                AppDetail = oAPI._Apps(dFrom, dTo, customerId, MachineDetailId, AppName);
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return View(AppDetail);
           
        }

        public JsonResult JsonAppDetail(int MachineDetailId = 0, String AppName = null)
        {
            List<AppDetail> AppDetail = new List<AppDetail>();
            LiveMonitoringAPIController oAPI = new LiveMonitoringAPIController();
            int customerId = Utility.GetCustomerId();
            try
            {
                DateTime dFrom = Utility.GetStartDate();
                DateTime dTo = Utility.GetEndDate();
                AppDetail = oAPI._Apps(dFrom, dTo, customerId, MachineDetailId, AppName);
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return Json(AppDetail, JsonRequestBehavior.AllowGet);
           
        }
    }
}
