﻿using System;
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
    public class KeyLoggerController : Controller
    {
        //
        // GET: /KeyLogger/

        public ActionResult Index(int KeyLoggerId = 0, string Type = null, int MachineDetailId = 0)
        {

            //Check Authorization
            if (!cls_Authorization.isAllowedURL("Report_KeyLoggerDetails")) return RedirectToAction("login", "account");
            List<KeyLogging> KeyLogger = new List<KeyLogging>();
            LiveMonitoringAPIController oAPI = new LiveMonitoringAPIController();
            int customerId = Utility.GetCustomerId();
            try
            {
                DateTime dFrom = Utility.GetStartDate();
                DateTime dTo = Utility.GetEndDate();
                KeyLogger = oAPI._KeyLogger(dFrom, dTo, customerId, MachineDetailId, KeyLoggerId,Type);
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return View(KeyLogger);
       }

        public JsonResult JsonKeyLogger(int KeyLoggerId = 0, string Type = null, int MachineDetailId = 0)
        {
            List<KeyLogging> KeyLogger = new List<KeyLogging>();
            LiveMonitoringAPIController oAPI = new LiveMonitoringAPIController();
            int customerId = Utility.GetCustomerId();
            try
            {
                DateTime dFrom = Utility.GetStartDate();
                DateTime dTo = Utility.GetEndDate();
                KeyLogger = oAPI._KeyLogger(dFrom, dTo, customerId, MachineDetailId, KeyLoggerId, Type);
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return Json(KeyLogger, JsonRequestBehavior.AllowGet);
        }
    }
}
