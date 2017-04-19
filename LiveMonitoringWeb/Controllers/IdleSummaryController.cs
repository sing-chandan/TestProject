using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using CommonUtility;
using LiveMonitoringWeb.Models;
using System.Collections.Generic;
using System.IO;
using LiveMonitoringWeb.Classes;

namespace LiveMonitoringWeb.Controllers
{
    [Authorize(Roles = "Customer, User")]
    public class IdleSummaryController : Controller
    {
        //
        // GET: /IdleSummary/

        public ActionResult Index()
        {
            //Check Authorization
            if (!cls_Authorization.isAllowedURL("Dashboard_TopIdleMachines")) return RedirectToAction("login", "account");

            List<MachineIdle> objMachineIdlelst = new List<MachineIdle>();
            try
            {
                using (var db = new DBContext())
                {
                    DateTime dFrom = Utility.GetStartDate();
                    DateTime dTo = Utility.GetEndDate();
                    int customerId = Utility.GetCustomerId();
                    var data = db.MachineIdles.Include(a => a.machine_detail)
                      .Where(x => x.CreatedDate >= dFrom && x.CreatedDate <= dTo && x.machine_detail.CustomerId == customerId)
                      .GroupBy(g => new { g.machine_detail.MachineName, g.machine_detail.UserName })
                      .Select(g => new
                      {
                          Machine = g.Key.MachineName,
                          User = g.Key.UserName,
                          IdleMinute = (g.Sum(x => x.IdleTime) / 60)
                      })
                      .OrderByDescending(a => a.IdleMinute).ToList();

                    int id = 0;
                    foreach (var item in data)
                    {
                        id = id + 1;
                        var machineData = db.MachineDetails.Where(p => p.MachineName == item.Machine && p.UserName == item.User).FirstOrDefault();
                        objMachineIdlelst.Add(new MachineIdle { IdleTimeId = id, machine_detail = machineData, IdleTime = item.IdleMinute });
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }

            return View(objMachineIdlelst);
        }

    }
}
