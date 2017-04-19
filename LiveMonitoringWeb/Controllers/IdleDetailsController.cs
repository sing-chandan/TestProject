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
    public class IdleDetailsController : Controller
    {
        //
        // GET: /IdleDetails/

        public ActionResult Index(int MachineDetailId = 0)
        {
            //Check Authorization
            if (!cls_Authorization.isAllowedURL("Report_MachineIdleDetails")) return RedirectToAction("login", "account");
            List<MachineIdle> MachineIdle = new List<MachineIdle>();
            LiveMonitoringAPIController oAPI = new LiveMonitoringAPIController();
             int customerId = Utility.GetCustomerId();
            try
            {
                DateTime dFrom = Utility.GetStartDate();
                DateTime dTo = Utility.GetEndDate();
                MachineIdle = oAPI._IdleMachines(dFrom, dTo, customerId, MachineDetailId);
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return View(MachineIdle);
        }

        public JsonResult JsonIdleDetail(int MachineDetailId = 0)
        {
            //Check Authorization

            

            List<MachineIdle> MachineIdle = new List<MachineIdle>();
            LiveMonitoringAPIController oAPI = new LiveMonitoringAPIController();
            int customerId = Utility.GetCustomerId();
            try
            {
                DateTime dFrom = Utility.GetStartDate();
                DateTime dTo = Utility.GetEndDate();
                MachineIdle = oAPI._IdleMachines(dFrom, dTo, customerId, MachineDetailId);
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return Json(MachineIdle, JsonRequestBehavior.AllowGet);
        }
    }
}
