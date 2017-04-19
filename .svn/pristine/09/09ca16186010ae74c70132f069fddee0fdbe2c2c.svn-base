using LiveMonitoringWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiveMonitoringWeb.Controllers
{
    public class WPController : Controller
    {
        //
        // GET: /WP/

        public ActionResult Index(int MachineDetailId)
        {
            return View(WorkingPerformanceData(MachineDetailId));
        }


        public ActionResult Search()
        {
            return View();
        }


        public List<WorkingPerformancetest> WorkingPerformanceData(int MAchineID)
        {
            List<WorkingPerformancetest> lstWorkingPerformance = new List<WorkingPerformancetest>();
            try
            {

                using (var db = new DBContext())
                {
                    var data = (from w in db.MachineSessions
                                where w.MachineDetailId == MAchineID
                                select new
                                {
                                    w.MachineSessionId,
                                    w.CreatedDate,
                                    w.MachineDetailId,
                                    w.SessionStart,
                                    w.SessionEnd

                                }).ToList();

                    lstWorkingPerformance = data.ToList().Select(r => new WorkingPerformancetest
                    {
                        ID = r.MachineSessionId,
                        MachineID = r.MachineDetailId,
                        EndDate = r.SessionEnd.ToString("MM-dd-yyy HH:mm"),
                        EntryDate = r.CreatedDate.Value.ToString("MM-dd-yyy HH:mm"),
                        StartDate = r.SessionStart.ToString("MM-dd-yyy HH:mm"),

                    }).ToList();
                }


            }
            catch (Exception ex)
            {

            }
            return lstWorkingPerformance;
        }
    }
}
