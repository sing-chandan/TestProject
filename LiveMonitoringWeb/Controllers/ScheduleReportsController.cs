﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LiveMonitoringWeb.Models;
using WebMatrix.WebData;
using System.Data.Common;
using CommonUtility;
using Newtonsoft.Json;

namespace LiveMonitoringWeb.Controllers
{
    [Authorize(Roles = "Customer")]
    public class ScheduleReportsController : Controller
    {

        //
        // GET: /SheduleReports/

        public ActionResult Index()
        {
            try
            {
                //Check Authorization
                string roleName = System.Web.Security.Roles.GetRolesForUser().Single();
                if (roleName.ToUpper() == "USER")
                {
                    return RedirectToAction("login", "account");
                }

                using (var db = new DBContext())
                {
                    List<Screens> objScreenslst = new List<Screens>();
                    //return View(db.ScheduleReports.ToList());
                    ViewBag.ScheduleType = new SelectList(new[] { new { value = "D", text = "Daily" }, new { value = "W", text = "Weekly" }, new { value = "M", text = "Monthly" }, new { value = "Q", text = "Quaterly" }, new { value = "HY", text = "Half-Yearly" }, new { value = "Y", text = "Yearly" } }, "value", "text");


                    objScreenslst = (from s in db.Screens
                                     join sr in db.ScheduleReports.Where(a => a.MembershipId == WebSecurity.CurrentUserId)
                                     on s.ScreenId equals sr.ScreenId into g
                                     from sr in g.DefaultIfEmpty()
                                     where s.ScreenType == "R" && s.IsActive == true
                                     select new
                                     {
                                         ScreenId = s.ScreenId,
                                         ScreenDisplayName = s.ScreenDisplayName,
                                         ScreenType = s.ScreenType,
                                         ScheduleReportId = sr != null ? (int)sr.ScheduleReportId : 0,
                                         Selected = sr.ScreenId != null ? (bool)true : false,
                                         ScheduleType = sr != null ? (string)sr.ScheduleType : "D",
                                         countRecord = (from u in db.Screens where u.IsActive==true && u.ScreenType=="R" select u.ScreenId ).AsEnumerable().Distinct().Count(),

                                     }).ToList().Select(r => new Screens
                                     {
                                         ScreenId = r.ScreenId,
                                         ScreenDisplayName = r.ScreenDisplayName,
                                         ScreenType = r.ScreenType,
                                         ScheduleReportId = r.ScheduleReportId,
                                         Selected = r.Selected,
                                         ScheduleType = r.ScheduleType,
                                         countRecord=r.countRecord,
                                     }).ToList();

                    return View(objScreenslst);

                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return null;
        }

        public JsonResult JsonScheduleReport()
        {
            try
            {
                //Check Authorization
                string roleName = System.Web.Security.Roles.GetRolesForUser().Single();
                if (roleName.ToUpper() == "USER")
                {
                   // return RedirectToAction("login", "account");
                }

                using (var db = new DBContext())
                {
                    ReturnSchedule ReturnData = new ReturnSchedule();
                    List<ReturnSchedule> retData = new List<ReturnSchedule>();

                    List<Screens> objScreenslst = new List<Screens>();
                    //return View(db.ScheduleReports.ToList());
                    ViewBag.ScheduleType = new SelectList(new[] { new { value = "D", text = "Daily" }, new { value = "W", text = "Weekly" }, new { value = "M", text = "Monthly" }, new { value = "Q", text = "Quaterly" }, new { value = "HY", text = "Half-Yearly" }, new { value = "Y", text = "Yearly" } }, "value", "text");

                    List<Selectlist> ScheduleType = new List<Selectlist>();
                    
                    ScheduleType.Add(new Selectlist() { value = "D", text = "Daily" });
                    ScheduleType.Add(new Selectlist() { value = "W", text = "Weekly" });
                    ScheduleType.Add(new Selectlist(){ value = "M", text = "Monthly" });
                    ScheduleType.Add(new Selectlist() { value = "Q", text = "Quaterly" });
                    ScheduleType.Add(new Selectlist() { value = "HY", text = "Half-Yearly" });
                    ScheduleType.Add(new Selectlist() { value = "Y", text = "Yearly" } );

                    
                    objScreenslst = (from s in db.Screens
                                     join sr in db.ScheduleReports.Where(a => a.MembershipId == WebSecurity.CurrentUserId)
                                     on s.ScreenId equals sr.ScreenId into g
                                     from sr in g.DefaultIfEmpty()
                                     where s.ScreenType == "R" && s.IsActive == true
                                     select new
                                     {
                                         ScreenId = s.ScreenId,
                                         ScreenDisplayName = s.ScreenDisplayName,
                                         ScreenType = s.ScreenType,
                                         ScheduleReportId = sr != null ? (int)sr.ScheduleReportId : 0,
                                         Selected = sr.ScreenId != null ? (bool)true : false,
                                         ScheduleType = sr != null ? (string)sr.ScheduleType : "D",
                                         countRecord = (from u in db.Screens where u.IsActive == true && u.ScreenType == "R" select u.ScreenId).AsEnumerable().Distinct().Count(),

                                     }).ToList().Select(r => new Screens
                                     {
                                         ScreenId = r.ScreenId,
                                         ScreenDisplayName = r.ScreenDisplayName,
                                         ScreenType = r.ScreenType,
                                         ScheduleReportId = r.ScheduleReportId,
                                         Selected = r.Selected,
                                         ScheduleType = r.ScheduleType,
                                         countRecord = r.countRecord,
                                     }).ToList();

                    ReturnData.ScheduleType = ScheduleType;
                    ReturnData.Screenlist= objScreenslst;
                    
                    retData.Add(ReturnData);
                    return Json(retData, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return null;
        }

        public ActionResult ResetScheduleReportPermission()
        {
            try
            {
                //Check Authorization
                string roleName = System.Web.Security.Roles.GetRolesForUser().Single();
                if (roleName.ToUpper() == "USER")
                {
                    return RedirectToAction("login", "account");
                }

                using (var db = new DBContext())
                { 
                    var model = (from u in db.ScheduleReports where u.MembershipId == WebSecurity.CurrentUserId select u).ToList();
                    foreach (var itm in model)
                    {
                        itm.SendDate = DateTime.Now;
                        db.Entry(itm).State = EntityState.Modified;
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


        public ActionResult SubmitSehedule(string modellist)
        {
            try
            {
                List<ScheduleReports> ModelData = JsonConvert.DeserializeObject<List<ScheduleReports>>(modellist);

////                Check Authorization
//                string roleName = System.Web.Security.Roles.GetRolesForUser().Single();
//                if (roleName.ToUpper() == "USER")
//                {
//                    return RedirectToAction("login", "account");
//                }

                using (var db = new DBContext())
                {
                    var schedulereports = (from u in db.ScheduleReports where u.MembershipId == WebSecurity.CurrentUserId select u).ToList();
                    foreach (var itm in schedulereports)
                    {
                        db.Entry(itm).State = EntityState.Deleted;
                        db.SaveChanges();
                    }

                    foreach(var item in ModelData)
                    {
                            ScheduleReports model = new ScheduleReports();
                            model.ScreenId = item.ScreenId;
                            model.MembershipId = WebSecurity.CurrentUserId;
                            model.ScheduleType = item.ScheduleType;
                            model.CreatedDate = DateTime.Now;
                            model.SendDate = DateTime.Now;
                            model.IsSend = false;
                            model.CreatedBy = WebMatrix.WebData.WebSecurity.CurrentUserId;
                            db.ScheduleReports.Add(model);
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

        public ActionResult SaveScheduleReportPermission(string ScreensScheduleTypes)
        {
            try
            {
                //Check Authorization
                string roleName = System.Web.Security.Roles.GetRolesForUser().Single();
                if (roleName.ToUpper() == "USER")
                {
                    return RedirectToAction("login", "account");
                }

                using (var db = new DBContext())
                {
                    var schedulereports = (from u in db.ScheduleReports where u.MembershipId == WebSecurity.CurrentUserId select u).ToList();
                    foreach (var itm in schedulereports)
                    {
                        db.Entry(itm).State = EntityState.Deleted;
                        db.SaveChanges();
                    }
                    if (ScreensScheduleTypes != "")
                    {
                        string[] arrScreensSheduleTypes = ScreensScheduleTypes.Split('|');
                        foreach (var item in arrScreensSheduleTypes)
                        {
                            string[] arrScreensSheduleTypesNew = item.Split(',');
                            int ScreenId = Convert.ToInt32(arrScreensSheduleTypesNew[0]);
                            string ScheduleType = arrScreensSheduleTypesNew[1].ToString();
                            ScheduleReports model = new ScheduleReports();
                            model.ScreenId = ScreenId;
                            model.MembershipId = WebSecurity.CurrentUserId;
                            model.ScheduleType = ScheduleType;
                            model.CreatedDate = DateTime.Now;
                            model.SendDate = DateTime.Now;
                            model.IsSend = false;
                            model.CreatedBy = WebMatrix.WebData.WebSecurity.CurrentUserId;
                            db.ScheduleReports.Add(model);
                            db.SaveChanges();
                        }

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

        protected override void Dispose(bool disposing)
        {
            using (var db = new DBContext())
            {
                db.Dispose();
                base.Dispose(disposing);
            }

        }
    }
}