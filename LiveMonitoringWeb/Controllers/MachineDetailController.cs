﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using LiveMonitoringWeb.Models;
using CommonUtility;
using System.Data;
using LiveMonitoringWeb.Classes;


namespace LiveMonitoringWeb.Controllers
{
    [Authorize(Roles = "Admin,User,Customer")]

    public class MachineDetailController : Controller
    {
        //
        // GET: /MachineDetail/

        public ActionResult Index(int MachineId = 0)
        {
            //Check Authorization
            if (!cls_Authorization.isAllowedURL("Report_MachineDetails")) return RedirectToAction("login", "account");
            List<MachineDetail> MachineDetail = new List<MachineDetail>();
            LiveMonitoringAPIController oAPI = new LiveMonitoringAPIController();

            try
            {
                int customerId = Utility.GetCustomerId();
                DateTime dFrom = Utility.GetStartDate();
                DateTime dTo = Utility.GetEndDate();
                MachineDetail = oAPI._Users(customerId, MachineId);
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return View(MachineDetail);
        }

        //
        // GET: /MachineDetail/

        public ActionResult CustomerMachineDetail(int CustomerId = 0)
        {
            List<MachineDetail> MachineDetail = new List<MachineDetail>();
            try
            {
                using (var db = new DBContext())
                {

                    MachineDetail = db.MachineDetails.Include(s => s.customer).Where(p => p.CustomerId == CustomerId).ToList();
                    return View(MachineDetail);
                }

            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return View(MachineDetail);
        }

        public ActionResult SetBlockUser(int MachineDetailId = 0, string Status = null)
        {
            MachineDetail MachineDetail = new MachineDetail();
            try
            {
                if (ModelState.IsValid)
                {
                    using (var db = new DBContext())
                    {
                        MachineDetail = db.MachineDetails.Where(a => a.MachineDetailId == MachineDetailId).FirstOrDefault();
                        if (MachineDetail == null)
                        {
                            return HttpNotFound();
                        }
                        if (Status == "block")
                        {
                            MachineDetail.IsBlocked = true;
                        }
                        else
                        {
                            MachineDetail.IsBlocked = false;
                        }
                        db.Entry(MachineDetail).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return RedirectToAction("Index");
        }

        public ActionResult _OpenModelPopup(int MachineId = 0)
        {
            var db = new DBContext();
            try
            {
                ViewBag.GroupId = new SelectList(db.Groups.Where(m => m.IsActive == true), "GroupId", "GroupName");
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return PartialView();
        }

        [HttpPost]
        public ActionResult _OpenModelPopup(FormCollection frm)
        {
            int machineid = Convert.ToInt16(frm["machineid"].ToString());
            int groupid = Convert.ToInt16(frm["GroupId"].ToString());
            MachineGrouping model = new MachineGrouping();
            var db = new DBContext();
            try
            {
                if (ModelState.IsValid)
                {
                    var machinegroup = db.MachineGroupings.FirstOrDefault(s => s.MachineDetailId == machineid);
                    if (machinegroup == null)
                    {
                        model.MachineDetailId = machineid;
                        model.GroupId = groupid;
                        model.CreatedDate = DateTime.Now;
                        model.CreatedBy = WebMatrix.WebData.WebSecurity.CurrentUserId;
                        db.MachineGroupings.Add(model);
                        db.SaveChanges();
                    }
                    else
                    {
                        machinegroup.GroupId = groupid;
                        machinegroup.ModifiedDate = DateTime.Now;
                        machinegroup.ModifiedBy = WebMatrix.WebData.WebSecurity.CurrentUserId;
                        db.Entry(machinegroup).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }

            return RedirectToAction("Index");
        }

        public int CheckGroups()
        {
            var db = new DBContext();
            try
            {
                var groups = db.Groups.Where(m => m.IsActive == true);
                if (groups.Count() == 0)
                {
                    return 1;
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return 0;
        }

        #region Json Data for report_machineData

        public JsonResult JsonMachineDetail(int MachineId = 0)
        {
            List<MachineDetail> MachineDetail = new List<MachineDetail>();
            LiveMonitoringAPIController oAPI = new LiveMonitoringAPIController();

            try
            {
                int customerId = Utility.GetCustomerId();
                DateTime dFrom = Utility.GetStartDate();
                DateTime dTo = Utility.GetEndDate();
                MachineDetail = oAPI._Users(customerId, MachineId);
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return Json(MachineDetail, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
