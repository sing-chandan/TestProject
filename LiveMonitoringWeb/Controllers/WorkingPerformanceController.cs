using CommonUtility;
using LiveMonitoringWeb.Classes;
using LiveMonitoringWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Data.Objects.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace LiveMonitoringWeb.Controllers
{
    [Authorize(Roles = "Customer, User")]
    public class WorkingPerformanceController : Controller
    {
        //
        // GET: /WorkingPerformance/

        public ActionResult Index(string sMachineDetailId)
        {
            Session["sMachineDetailId"] = sMachineDetailId;
            return View(WorkingPerformanceData());
        }

        public ActionResult treePartialView()
        {
            //Check Authorization
            if (!cls_Authorization.isAllowedURL("Report_ScreenShots")) return RedirectToAction("login", "account");

            var model = new treeWorkingPerformance();
            //Populate Tree View
            List<Tree> objTree = new List<Tree>();

            using (var db = new DBContext())
            {
                int CustomerId = Utility.GetCustomerId();
                ViewBag.CustomerId = CustomerId;
                try
                {
                    string UserName = string.Empty;
                    string MachineName = string.Empty;
                    if (Convert.ToString(Session["sMachineDetailId"]).Length>0)
                    {
                        Int32 MachineDetailId = Convert.ToInt32(Session["sMachineDetailId"]);
                        MachineName = db.MachineDetails.Where(a => a.MachineDetailId == MachineDetailId).Select(b => b.MachineName).FirstOrDefault();
                        UserName = db.MachineDetails.Where(a => a.MachineDetailId == MachineDetailId).Select(b => b.UserName).FirstOrDefault();
                    }
                    ViewBag.MachineName = MachineName;
                    ViewBag.UserName = UserName;

                    DataTable dt = new DataTable();
                    DataRow dtrow;
                    dt.Columns.Add("Id", typeof(Int32));
                    dt.Columns.Add("Name", typeof(string));
                    dt.Columns.Add("ParentId", typeof(Int32));
                    dt.Columns.Add("MacAddress", typeof(string));

                    var machinedetaillst = db.MachineDetails.Where(a => a.CustomerId == CustomerId).Select(a => a.MachineName).Distinct().ToList();
                    int Id = 0, PId = 0;
                    foreach (var machinename in machinedetaillst)
                    {
                        PId = Id;
                        var userdetaillst = db.MachineDetails.Where(a => a.MachineName == machinename && a.CustomerId == CustomerId).ToList();
                        dtrow = dt.NewRow();
                        dtrow["Id"] = Id + 1;
                        dtrow["Name"] = machinename;
                        dtrow["ParentId"] = 0;
                        dt.Rows.Add(dtrow);
                        Id = Id + 1;
                        foreach (var userdetail in userdetaillst)
                        {
                            dtrow = dt.NewRow();
                            dtrow["Id"] = Id + 1;
                            dtrow["Name"] = userdetail.UserName;
                            dtrow["ParentId"] = PId + 1;
                            dtrow["MacAddress"] = userdetail.MachineMacAddress;
                            dt.Rows.Add(dtrow);
                            Id = Id + 1;
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ViewBag.MachineCount = dt.Rows.Count.ToString();
                    }


                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        objTree.Add(new Tree { Id = Convert.ToInt32(dt.Rows[i]["Id"]), Name = Convert.ToString(dt.Rows[i]["Name"]), ParentId = Convert.ToInt32(dt.Rows[i]["ParentId"]), MachineMacAddress = Convert.ToString(dt.Rows[i]["MacAddress"]) });
                    }
                    objTree = objTree.OrderBy(a => a.ParentId).ToList();



                }
                catch (Exception e)
                {
                    ExceptionHandler handler = new ExceptionHandler();
                    handler.HandleException(e);
                }
                model.tree = objTree;
              
                return View(model);

            }
        }

        public JsonResult JsonWorkingPerformanceGraph()
        {
            try
            {                
                using (var db = new DBContext())
                {    
                   DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                   DateTime endDate = startDate.AddMonths(1).AddSeconds(-1);                  
                   int nMachineDetailId=(Session["sMachineDetailId"]==null || Session["sMachineDetailId"]=="" ) ? 0 : Convert.ToInt32(Session["sMachineDetailId"]);
                   int customerId = Utility.GetCustomerId();
                   var dates = new List<DateTime>();
                   dates= Enumerable.Range(1, DateTime.DaysInMonth(startDate.Year, startDate.Month))  // Days: 1, 2 ... 31 etc.
                                    .Select(day => new DateTime(startDate.Year, startDate.Month, day)) // Map each day to a date
                                    .ToList(); // Load dates into a list


                    TimeSpan lunchStartTime=TimeSpan.Zero;
                    TimeSpan lunchEndTime = TimeSpan.Zero;
                    TimeSpan shiftStartTime = TimeSpan.Zero;
                    TimeSpan shiftEndTime = TimeSpan.Zero;

                   var groupTime = (from grp in db.Groups
                                    join mg in db.MachineGroupings on grp.GroupId equals mg.GroupId
                                    where mg.MachineDetailId == nMachineDetailId
                                    select new
                                    {
                                        grp.ShiftStartTime,
                                        grp.ShiftEndTime,
                                        grp.LunchStartTime,
                                        grp.LunchEndTime
                                    }).ToList();
                   if (groupTime!=null)
                   {
                       foreach (var grptime in groupTime)
                       {                          
                           lunchStartTime = DateTime.ParseExact(grptime.LunchStartTime, "HH:mm", CultureInfo.InvariantCulture).TimeOfDay;
                           lunchEndTime = DateTime.ParseExact(grptime.LunchEndTime, "HH:mm", CultureInfo.InvariantCulture).TimeOfDay;
                           shiftStartTime = DateTime.ParseExact(grptime.ShiftStartTime, "HH:mm", CultureInfo.InvariantCulture).TimeOfDay;
                           shiftEndTime = DateTime.ParseExact(grptime.ShiftEndTime, "HH:mm", CultureInfo.InvariantCulture).TimeOfDay;
                       }
                   }
                    
                    TimeSpan t=(shiftEndTime - shiftStartTime)-(lunchEndTime-lunchStartTime);


                   var data = (from d in dates
                               select new
                               {
                                   dateOfMonth=d.Day.ToString(),

                                   workingTime = ((from ms in db.MachineSessions
                                                   where ms.MachineDetailId == nMachineDetailId && EntityFunctions.TruncateTime(ms.SessionStart) == EntityFunctions.TruncateTime(d.Date)
                                                   select EntityFunctions.DiffMinutes(ms.SessionStart, ms.SessionEnd)).ToList().Sum()
                                                      - (db.MachineIdles.Where(x => EntityFunctions.TruncateTime(x.CreatedDate) == EntityFunctions.TruncateTime(d.Date) && x.MachineDetailId == nMachineDetailId).Select(k => k.IdleTime).ToList().FirstOrDefault()) / 60)<0 ? 0 :
                                                      ((from ms in db.MachineSessions
                                                        where ms.MachineDetailId == nMachineDetailId && EntityFunctions.TruncateTime(ms.SessionStart) == EntityFunctions.TruncateTime(d.Date)
                                                        select EntityFunctions.DiffMinutes(ms.SessionStart, ms.SessionEnd)).ToList().Sum()
                                                      - (db.MachineIdles.Where(x => EntityFunctions.TruncateTime(x.CreatedDate) == EntityFunctions.TruncateTime(d.Date) && x.MachineDetailId == nMachineDetailId).Select(k => k.IdleTime).ToList().Sum()) / 60),


                                   idleTime = (db.MachineIdles.Where(x => EntityFunctions.TruncateTime(x.CreatedDate) == EntityFunctions.TruncateTime(d.Date) && x.MachineDetailId == nMachineDetailId).Select(
                                                  k => k.IdleTime).ToList().Sum()) / 60,


                                   groupTime = t.TotalMinutes,

                               }).ToList();
                    Session["sMachineDetailId"] = null;
                  
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

        public List<WorkingPerformance> WorkingPerformanceData()
        {
            try                
            {
                List<WorkingPerformance> lstWorkingPerformance = new List<WorkingPerformance>();
                using (var db = new DBContext())
                {
                    DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    DateTime endDate = startDate.AddMonths(1).AddSeconds(-1);
                    int nMachineDetailId = (Session["sMachineDetailId"] == null || Session["sMachineDetailId"].ToString() == "") ? 0 : Convert.ToInt32(Session["sMachineDetailId"]);
                    if(nMachineDetailId > 0)
                    {
                        ViewBag.macName = (db.MachineDetails.Where(x => x.MachineDetailId == nMachineDetailId).Select(x => x.MachineName)).ToList().FirstOrDefault();
                        ViewBag.UserName = (db.MachineDetails.Where(x => x.MachineDetailId == nMachineDetailId).Select(x => x.UserName)).ToList().FirstOrDefault();
                    }
                    else
                    {
                        ViewBag.macName = "";
                    }
                    
                    int customerId = Utility.GetCustomerId();
                    var dates = new List<DateTime>();
                    dates = Enumerable.Range(1, DateTime.DaysInMonth(startDate.Year, startDate.Month))  // Days: 1, 2 ... 31 etc.
                                     .Select(day => new DateTime(startDate.Year, startDate.Month, day)) // Map each day to a date
                                     .ToList(); // Load dates into a list


                    TimeSpan lunchStartTime = TimeSpan.Zero;
                    TimeSpan lunchEndTime = TimeSpan.Zero;
                    TimeSpan shiftStartTime = TimeSpan.Zero;
                    TimeSpan shiftEndTime = TimeSpan.Zero;

                    var groupTime = (from grp in db.Groups
                                     join mg in db.MachineGroupings on grp.GroupId equals mg.GroupId
                                     where mg.MachineDetailId == nMachineDetailId
                                     select new
                                     {
                                         grp.ShiftStartTime,
                                         grp.ShiftEndTime,
                                         grp.LunchStartTime,
                                         grp.LunchEndTime
                                     }).ToList();
                    if (groupTime != null)
                    {
                        foreach (var grptime in groupTime)
                        {
                            lunchStartTime = DateTime.ParseExact(grptime.LunchStartTime, "HH:mm", CultureInfo.InvariantCulture).TimeOfDay;
                            lunchEndTime = DateTime.ParseExact(grptime.LunchEndTime, "HH:mm", CultureInfo.InvariantCulture).TimeOfDay;
                            shiftStartTime = DateTime.ParseExact(grptime.ShiftStartTime, "HH:mm", CultureInfo.InvariantCulture).TimeOfDay;
                            shiftEndTime = DateTime.ParseExact(grptime.ShiftEndTime, "HH:mm", CultureInfo.InvariantCulture).TimeOfDay;
                        }
                    }

                    TimeSpan t = (shiftEndTime - shiftStartTime) - (lunchEndTime - lunchStartTime);


                    var data = (from d in dates
                                select new
                                {
                                    dateOfMonth = d.Day.ToString(),

                                    workingTime = ((from ms in db.MachineSessions
                                                    where ms.MachineDetailId == nMachineDetailId && EntityFunctions.TruncateTime(ms.SessionStart) == EntityFunctions.TruncateTime(d.Date)
                                                    select EntityFunctions.DiffMinutes(ms.SessionStart, ms.SessionEnd)).ToList().Sum()
                                                      - (db.MachineIdles.Where(x => EntityFunctions.TruncateTime(x.CreatedDate) == EntityFunctions.TruncateTime(d.Date) && x.MachineDetailId == nMachineDetailId).Select(k => k.IdleTime).ToList().FirstOrDefault()) / 60) < 0 ? 0 :
                                                      ((from ms in db.MachineSessions
                                                        where ms.MachineDetailId == nMachineDetailId && EntityFunctions.TruncateTime(ms.SessionStart) == EntityFunctions.TruncateTime(d.Date)
                                                        select EntityFunctions.DiffMinutes(ms.SessionStart, ms.SessionEnd)).ToList().Sum()
                                                      - (db.MachineIdles.Where(x => EntityFunctions.TruncateTime(x.CreatedDate) == EntityFunctions.TruncateTime(d.Date) && x.MachineDetailId == nMachineDetailId).Select(k => k.IdleTime).ToList().Sum()) / 60),


                                    idleTime = (db.MachineIdles.Where(x => EntityFunctions.TruncateTime(x.CreatedDate) == EntityFunctions.TruncateTime(d.Date) &&                       x.MachineDetailId == nMachineDetailId).Select(
                                                   k => k.IdleTime).ToList().Sum())/60 ,


                                    groupTime = t.TotalMinutes,
                                   
                                }).ToList();
                  

                    lstWorkingPerformance = data.ToList().Select(r => new WorkingPerformance
                    {
                        dateOfMonth=r.dateOfMonth,
                        workingTime=Convert.ToString(r.workingTime),
                        idleTime=Convert.ToString(r.idleTime),
                        groupTime=Convert.ToString(r.groupTime),
                       
                    }).ToList();
                }
                return lstWorkingPerformance;
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return null;
        }
    }
}
