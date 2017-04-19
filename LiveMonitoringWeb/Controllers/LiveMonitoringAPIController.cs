﻿using CommonUtility;
using LiveMonitoringWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace LiveMonitoringWeb.Controllers
{

    public class LiveMonitoringAPIController : ApiController
    {
        #region Declaration

        private AppSettingReader appKeyReader = new AppSettingReader();

        #endregion Declaration

        #region HttpGet

        [System.Web.Http.HttpGet]
        public Configuration GetCofigrationSettings(int customerId)
        {
            using (var db = new DBContext())
            {
                var configuration = db.Configurations.Where(a => a.CustomerId == customerId && a.IsDeleted == false).FirstOrDefault();
                if (configuration != null)
                {
                    return configuration;
                }
                else
                {
                    Configuration model = new Configuration();
                    model.AppTracker_Interval = 60;
                    model.BrowserTacker_Interval = 60;
                    model.KeyLogger_Interval = 60;
                    model.KeyLogger_MinTime = 60;
                    model.MachineIdle_Interval = 60;
                    model.MachineIdle_MinTime = 300;
                    model.ScreenShot_Interval = 60;
                    model.IsSendBlockData = true;
                    return model;
                }
            }
        }

        [System.Web.Http.HttpGet]
        public List<ScheduleReportsSettings> GetSheduleReportsSettings()
        {
            List<ScheduleReportsSettings> oScheduleReportsSettingslst = new List<ScheduleReportsSettings>();
            try
            {
                using (var db = new DBContext())
                {
                    var objScheduleReportsSettings = (from sr in db.ScheduleReports
                                                      join c in db.Customers on sr.MembershipId equals c.MembershipId
                                                      join s in db.Screens on sr.ScreenId equals s.ScreenId
                                                      select new
                                                      {
                                                          sr.ScreenId,
                                                          sr.ScheduleReportId,
                                                          s.ScreenInternalName,
                                                          sr.MembershipId,
                                                          sr.ScheduleType,
                                                          c.Email,
                                                          c.CustomerId,
                                                          sr.SendDate,
                                                          sr.IsSend,
                                                          CustomerName = c.FirstName + " " + c.LastName,
                                                          countCustomer = (from cnt in db.ScheduleReports select new { cnt.MembershipId }).AsEnumerable().Distinct().Count(),
                                                      }).ToList();


                    foreach (var item in objScheduleReportsSettings)
                    {
                        oScheduleReportsSettingslst.Add(new ScheduleReportsSettings
                        {
                            ScheduleReportId = item.ScheduleReportId,
                            ScreenId = item.ScreenId,
                            ScreenInternalName = item.ScreenInternalName,
                            MembershipId = item.MembershipId,
                            ScheduleType = item.ScheduleType,
                            Email = item.Email,
                            CustomerId = item.CustomerId,
                            SendDate = (DateTime)item.SendDate,
                            IsSend = (bool)item.IsSend,
                            CustomerName = item.CustomerName,
                            countCustomer = item.countCustomer,
                        });
                    }

                    return oScheduleReportsSettingslst;
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return null;
        }

        #region Schedule Reports

        [System.Web.Http.HttpGet]
        public List<MachineDetail> _Users(int customerId, int MachineId = 0)
        {
            List<MachineDetail> MachineDetail = new List<MachineDetail>();
            try
            {
                using (var db = new DBContext())
                {
                    if (MachineId != 0)
                    {
                        var data1 = (from md in db.MachineDetails
                                     join c in db.Customers on md.CustomerId equals c.CustomerId into cus
                                     from x in cus.DefaultIfEmpty()
                                     join mg in db.MachineGroupings on md.MachineDetailId equals mg.MachineDetailId into macgrp
                                     from y in macgrp.DefaultIfEmpty()
                                     join g in db.Groups.Where(m => m.IsActive == true) on y.GroupId equals g.GroupId into grp
                                     from z in grp.DefaultIfEmpty()
                                     select new
                                     {
                                         MachineDetailId = md.MachineDetailId,
                                         CustomerId = md.CustomerId,
                                         GroupName = z.GroupName,
                                         MachineName = md.MachineName,
                                         UserName = md.UserName,
                                         MachineMacAddress = md.MachineMacAddress,
                                         MachineIP = md.MachineIP,
                                         CreatedDate = md.CreatedDate,
                                         GroupId = y == null ? 0 : y.GroupId == null ? 0 : y.GroupId,
                                         IsBlocked = md.IsBlocked
                                     }).Where(p => p.MachineDetailId == MachineId && p.CustomerId == customerId);

                       

                        var customerdata = db.Customers.Where(p => p.CustomerId == customerId).FirstOrDefault();

                        foreach (var item in data1)
                        {
                            MachineDetail.Add(new MachineDetail
                            {
                                MachineDetailId = item.MachineDetailId,
                                MachineName = item.MachineName,
                                UserName = item.UserName,
                                MachineIP = item.MachineIP,
                                MachineMacAddress = item.MachineMacAddress,
                                CustomerId = item.CustomerId,
                                customer = customerdata,
                                GroupName = item.GroupName,
                                CreatedDate = item.CreatedDate,
                                GroupId = item.GroupId,
                                IsBlocked = item.IsBlocked
                            });
                        }
                        return MachineDetail;
                    }
                    else
                    {
                        var data2 = (from md in db.MachineDetails
                                     join c in db.Customers on md.CustomerId equals c.CustomerId into cus
                                     from x in cus.DefaultIfEmpty()
                                     join mg in db.MachineGroupings on md.MachineDetailId equals mg.MachineDetailId into macgrp
                                     from y in macgrp.DefaultIfEmpty()
                                     join g in db.Groups.Where(m => m.IsActive == true) on y.GroupId equals g.GroupId into grp
                                     from z in grp.DefaultIfEmpty()
                                     select new
                                     {
                                         MachineDetailId = md.MachineDetailId,
                                         CustomerId = md.CustomerId,
                                         GroupName = z.GroupName,
                                         MachineName = md.MachineName,
                                         UserName = md.UserName,
                                         MachineMacAddress = md.MachineMacAddress,
                                         MachineIP = md.MachineIP,
                                         CreatedDate = md.CreatedDate,
                                         GroupId  = y==null ? 0 : y.GroupId == null ? 0 : y.GroupId,
                                         IsBlocked = md.IsBlocked
                                     }).Where(p => p.CustomerId == customerId).ToList();

                       

                        var customerdata = db.Customers.Where(p => p.CustomerId == customerId).FirstOrDefault();

                        foreach (var item in data2)
                        {
                            MachineDetail.Add(new MachineDetail
                            {
                                MachineDetailId = item.MachineDetailId,
                                MachineName = item.MachineName,
                                UserName = item.UserName,
                                MachineIP = item.MachineIP,
                                MachineMacAddress = item.MachineMacAddress,
                                CustomerId = item.CustomerId,
                                customer = customerdata,
                                GroupName = item.GroupName,
                                CreatedDate = item.CreatedDate,
                                GroupId = item.GroupId,
                                IsBlocked = item.IsBlocked
                            });
                        }
                        return MachineDetail;
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return null;
        }

        [System.Web.Http.HttpGet]
        public List<MachineIdle> _IdleMachines(DateTime dFrom, DateTime dTo, int customerId, int MachineDetailId = 0)
        {
            List<MachineIdle> MachineIdle = new List<MachineIdle>();
            try
            {
                using (var db = new DBContext())
                {
                    if (MachineDetailId != 0)
                    {

                        MachineIdle = db.MachineIdles.Include("machine_detail").Where(x => x.MachineDetailId == MachineDetailId && EntityFunctions.TruncateTime(x.CreatedDate) >= EntityFunctions.TruncateTime(dFrom) && EntityFunctions.TruncateTime(x.CreatedDate) <= EntityFunctions.TruncateTime(dTo) && x.machine_detail.CustomerId == customerId).ToList();
                        return MachineIdle;

                    }
                    else
                    {

                        MachineIdle = db.MachineIdles.Include("machine_detail").Where(x => EntityFunctions.TruncateTime(x.CreatedDate) >= EntityFunctions.TruncateTime(dFrom) && EntityFunctions.TruncateTime(x.CreatedDate) <= EntityFunctions.TruncateTime(dTo) && x.machine_detail.CustomerId == customerId).ToList();
                        return MachineIdle;

                    }

                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return null;
        }

        [System.Web.Http.HttpGet]
        public List<BrowserDetail> _Sites(DateTime dFrom, DateTime dTo, int customerId, int MachineDetailId = 0, string URL = "")
        {
            List<BrowserDetail> BrowserDetail = new List<BrowserDetail>();
            try
            {
                using (var db = new DBContext())
                {

                    if (MachineDetailId != 0)
                    {

                        BrowserDetail = db.BrowserDetails.Include("machine_detail").Where(x => x.MachineDetailId == MachineDetailId && EntityFunctions.TruncateTime(x.CreatedDate) >= EntityFunctions.TruncateTime(dFrom) && EntityFunctions.TruncateTime(x.CreatedDate) <= EntityFunctions.TruncateTime(dTo) && x.machine_detail.CustomerId == customerId).ToList();
                      
                        return BrowserDetail;

                    }
                    else if (!String.IsNullOrEmpty(URL))
                    {

                        BrowserDetail = db.BrowserDetails.Include("machine_detail").Where(x => x.URL.Contains(URL) && EntityFunctions.TruncateTime(x.CreatedDate) >= EntityFunctions.TruncateTime(dFrom) && EntityFunctions.TruncateTime(x.CreatedDate) <= EntityFunctions.TruncateTime(dTo) && x.machine_detail.CustomerId == customerId).ToList();
                        //BrowserDetail = db.BrowserDetails.Where(x => x.URL.Contains(URL) && EntityFunctions.TruncateTime(x.CreatedDate) >= EntityFunctions.TruncateTime(dFrom) && EntityFunctions.TruncateTime(x.CreatedDate) <= EntityFunctions.TruncateTime(dTo) && x.machine_detail.CustomerId == customerId).ToList();
                        return BrowserDetail;

                    }
                    else
                    {

                        BrowserDetail = db.BrowserDetails.Include("machine_detail").Where(x => EntityFunctions.TruncateTime(x.CreatedDate) >= EntityFunctions.TruncateTime(dFrom) && EntityFunctions.TruncateTime(x.CreatedDate) <= EntityFunctions.TruncateTime(dTo) && x.machine_detail.CustomerId == customerId).ToList();
                      //  BrowserDetail = db.BrowserDetails.Where(x => EntityFunctions.TruncateTime(x.CreatedDate) >= EntityFunctions.TruncateTime(dFrom) && EntityFunctions.TruncateTime(x.CreatedDate) <= EntityFunctions.TruncateTime(dTo) && x.machine_detail.CustomerId == customerId).ToList();
                        return BrowserDetail;

                    }


                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return null;
        }

        [System.Web.Http.HttpGet]
        public List<AppDetail> _Apps(DateTime dFrom, DateTime dTo, int customerId, int MachineDetailId = 0, string AppName = "")
        {
            List<AppDetail> AppDetail = new List<AppDetail>();
            try
            {
                using (var db = new DBContext())
                {

                    if (MachineDetailId != 0)
                    {
                        AppDetail = db.AppDetails.Include("machine_detail").Where(x => x.MachineDetailId == MachineDetailId && x.machine_detail.CustomerId == customerId
                           && EntityFunctions.TruncateTime(x.CreatedDate) >= EntityFunctions.TruncateTime(dFrom) && EntityFunctions.TruncateTime(x.CreatedDate) <= EntityFunctions.TruncateTime(dTo)).ToList();
                        return AppDetail;
                    }
                    else if (!String.IsNullOrEmpty(AppName))
                    {
                        AppDetail = db.AppDetails.Include("machine_detail").Where(x => x.AppName == AppName && x.machine_detail.CustomerId == customerId
                            && EntityFunctions.TruncateTime(x.CreatedDate) >= EntityFunctions.TruncateTime(dFrom) && EntityFunctions.TruncateTime(x.CreatedDate) <= EntityFunctions.TruncateTime(dTo)).ToList();
                        return AppDetail;
                    }
                    else
                    {
                        AppDetail = db.AppDetails.Include("machine_detail").Where(x => EntityFunctions.TruncateTime(x.CreatedDate) >= EntityFunctions.TruncateTime(dFrom) && EntityFunctions.TruncateTime(x.CreatedDate) <= EntityFunctions.TruncateTime(dTo) && x.machine_detail.CustomerId == customerId).ToList();
                        return AppDetail;
                    }
                }

            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return null;
        }

        [System.Web.Http.HttpGet]
        public List<KeyLogging> _KeyLogger(DateTime dFrom, DateTime dTo, int customerId, int MachineDetailId = 0, int KeyLoggerId = 0, string Type = "")
        {
            List<KeyLogging> KeyLogger = new List<KeyLogging>();
            try
            {
                using (var db = new DBContext())
                {
                    if (KeyLoggerId != 0)
                    {
                        KeyLogger = db.KeyLoggings.Include("machine_detail").Where(p => p.KeyLoggerId == KeyLoggerId && EntityFunctions.TruncateTime(p.CreatedDate) >= EntityFunctions.TruncateTime(dFrom) && EntityFunctions.TruncateTime(p.CreatedDate) <= EntityFunctions.TruncateTime(dTo) && p.machine_detail.CustomerId == customerId).ToList();
                        return KeyLogger;

                    }
                    else if (MachineDetailId != 0)
                    {

                        KeyLogger = db.KeyLoggings.Include("machine_detail").Where(p => p.MachineDetailId == MachineDetailId && EntityFunctions.TruncateTime(p.CreatedDate) >= EntityFunctions.TruncateTime(dFrom) && EntityFunctions.TruncateTime(p.CreatedDate) <= EntityFunctions.TruncateTime(dTo) && p.machine_detail.CustomerId == customerId).ToList();
                        return KeyLogger;

                    }
                    else
                    {
                        if (Type == "KL")
                        {

                            KeyLogger = db.KeyLoggings.Include("machine_detail").Where(p => p.TextType == "KL" && EntityFunctions.TruncateTime(p.CreatedDate) >= EntityFunctions.TruncateTime(dFrom) && EntityFunctions.TruncateTime(p.CreatedDate) <= EntityFunctions.TruncateTime(dTo) && p.machine_detail.CustomerId == customerId).ToList();
                            return KeyLogger;

                        }
                        else if (Type == "CB")
                        {

                            KeyLogger = db.KeyLoggings.Include("machine_detail").Where(p => p.TextType == "CB" && EntityFunctions.TruncateTime(p.CreatedDate) >= EntityFunctions.TruncateTime(dFrom) && EntityFunctions.TruncateTime(p.CreatedDate) <= EntityFunctions.TruncateTime(dTo) && p.machine_detail.CustomerId == customerId).ToList();
                            return KeyLogger;

                        }
                        else
                        {

                            //KeyLogger = db.KeyLoggings.Include("machine_detail").Where(x => x.CreatedDate >= dFrom && x.CreatedDate <= dTo && x.machine_detail.CustomerId == customerId).ToList();

                            KeyLogger = db.KeyLoggings.Include("machine_detail").Where(x => EntityFunctions.TruncateTime(x.CreatedDate) >= EntityFunctions.TruncateTime(dFrom) && EntityFunctions.TruncateTime(x.CreatedDate) <= EntityFunctions.TruncateTime(dTo) && x.machine_detail.CustomerId == customerId).ToList();

                            return KeyLogger;

                        }
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return null;
        }

        [System.Web.Http.HttpGet]
        public List<ProjectStatics> _ProjectSummary(DateTime dFrom, DateTime dTo, int customerId)
        {
            List<ProjectStatics> objProjectStatisticslst = new List<ProjectStatics>();
            try
            {
                using (var db = new DBContext())
                {
                    var data = db.MachineDetails
                        .Where(p => p.CustomerId == customerId)
                        .GroupBy(g => new { g.MachineDetailId, g.MachineName, g.UserName })
                        .Select(g => new
                        {
                            MachineDetailId = g.Key.MachineDetailId,
                            MachineName = g.Key.MachineName,
                            UserName = g.Key.UserName,
                            KeyLoggerCount = db.KeyLoggings.Where(b => b.MachineDetailId == g.Key.MachineDetailId && EntityFunctions.TruncateTime(b.CreatedDate) >= EntityFunctions.TruncateTime(dFrom) && EntityFunctions.TruncateTime(b.CreatedDate) <= EntityFunctions.TruncateTime(dTo)).Count(),
                            BrowserDetailCount = db.BrowserDetails.Where(b => b.MachineDetailId == g.Key.MachineDetailId && EntityFunctions.TruncateTime(b.CreatedDate) >= EntityFunctions.TruncateTime(dFrom) && EntityFunctions.TruncateTime(b.CreatedDate) <= EntityFunctions.TruncateTime(dTo)).Count(),
                            AppDetailCount = db.AppDetails.Where(b => b.MachineDetailId == g.Key.MachineDetailId && EntityFunctions.TruncateTime(b.CreatedDate) >= EntityFunctions.TruncateTime(dFrom) && EntityFunctions.TruncateTime(b.CreatedDate) <= EntityFunctions.TruncateTime(dTo)).Count(),
                            IdleTimeCount = db.MachineIdles.Where(b => b.MachineDetailId == g.Key.MachineDetailId && EntityFunctions.TruncateTime(b.CreatedDate) >= EntityFunctions.TruncateTime(dFrom) && EntityFunctions.TruncateTime(b.CreatedDate) <= EntityFunctions.TruncateTime(dTo)).Count(),
                            IdleTimeSum = db.MachineIdles.Where(b => b.MachineDetailId == g.Key.MachineDetailId && EntityFunctions.TruncateTime(b.CreatedDate) >= EntityFunctions.TruncateTime(dFrom) && EntityFunctions.TruncateTime(b.CreatedDate) <= EntityFunctions.TruncateTime(dTo)).Count() == 0 ? 0 : db.MachineIdles.Where(b => b.MachineDetailId == g.Key.MachineDetailId && EntityFunctions.TruncateTime(b.CreatedDate) >= EntityFunctions.TruncateTime(dFrom) && EntityFunctions.TruncateTime(b.CreatedDate) <= EntityFunctions.TruncateTime(dTo)).Sum(b => b.IdleTime)
                        }).OrderByDescending(a => a.MachineName).ToList();
                    foreach (var item in data)
                    {
                        objProjectStatisticslst.Add(new ProjectStatics
                        {
                            MachineDetailId = item.MachineDetailId,
                            MachineName = item.MachineName,
                            UserName = item.UserName,
                            KeyLoggerCount = item.KeyLoggerCount.ToString(),
                            BrowserDetailCount = item.BrowserDetailCount.ToString(),
                            AppDetailCount = item.AppDetailCount.ToString(),
                            IdleTimeCount = item.IdleTimeCount.ToString(),
                            IdleTimeSum = item.IdleTimeSum.ToString()
                        });
                    }

                    return objProjectStatisticslst;
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return null;
        }

        [System.Web.Http.HttpGet]
        public List<Customer> _RecentCustomers()
        {
            List<Customer> objCustomerlst = new List<Customer>();
            try
            {
                using (var db = new DBContext())
                {
                    var objCustomer = (from c in db.Customers
                                       join m in db.MachineDetails on c.CustomerId equals m.CustomerId
                                       join am in db.AdminScheduleReport on m.MachineDetailId equals am.MachineDetailId
                                       where EntityFunctions.TruncateTime(m.CreatedDate) == EntityFunctions.TruncateTime(DateTime.Now)
                                       && am.bSend == false
                                       select new
                                       {
                                           c.FirstName,
                                           c.LastName,
                                           c.CustomerId,
                                           c.Email,
                                           c.OrganizationName,
                                           m.CreatedDate,
                                       }).ToList();

                    foreach (var item in objCustomer)
                    {
                        var MachineCount = db.MachineDetails.Where(a => a.CustomerId == item.CustomerId).Count();
                        var Downloads = db.CustomerDownloadHistory.Where(a => a.CustomerId == item.CustomerId).Count();
                        objCustomerlst.Add(new Customer
                        {
                            CustomerId = item.CustomerId,
                            LastName = item.LastName,
                            FirstName = item.FirstName,
                            Email = item.Email,
                            OrganizationName = item.OrganizationName,
                            count = MachineCount,
                            Downloads = Downloads,
                            CreateDate = item.CreatedDate,
                        });
                    }
                    return objCustomerlst;
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return null;
        }
        #endregion

        #endregion

        #region HttpPost

        [System.Web.Mvc.HttpPost]
        public string AddMachineDetails(MachineDetail model)
        {
            string machineId = string.Empty;
            try
            {
                using (var db = new DBContext())
                {
                    if (ModelState.IsValid)
                    {
                        MachineDetail machineDetail = db.MachineDetails.FirstOrDefault(m => m.MachineMacAddress.ToUpper() == model.MachineMacAddress.ToUpper() && m.UserName == model.UserName && m.CustomerId == model.CustomerId);
                        if (machineDetail == null)
                        {
                            db.MachineDetails.Add(model);
                            db.SaveChanges();
                            machineId = model.MachineDetailId.ToString() + "_" + model.IsBlocked;

                            // Adding record in AdminScheduleReport Table for sending Report to <ADMIN> only
                            AdminScheduleReport modl = new AdminScheduleReport();
                            modl.CustomerId = model.CustomerId;
                            modl.MachineDetailId = model.MachineDetailId;
                            modl.bSend = false;
                            db.AdminScheduleReport.Add(modl);
                            db.SaveChanges();
                        }
                        else
                        {
                            machineId = machineDetail.MachineDetailId.ToString() + "_" + machineDetail.IsBlocked;

                            // Adding record in AdminScheduleReport Table for sending Report to <ADMIN> only
                            AdminScheduleReport modl = new AdminScheduleReport();
                            modl.CustomerId = machineDetail.CustomerId;
                            modl.MachineDetailId = machineDetail.MachineDetailId;
                            modl.bSend = false;
                            db.AdminScheduleReport.Add(modl);
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

            return machineId;
        }

        [System.Web.Mvc.HttpPost]
        public string AddBrowserDetails(BrowserDetail model)
        {
            string returnMessage = string.Empty;

            try
            {
                using (var db = new DBContext())
                {
                    if (ModelState.IsValid)
                    {
                        MachineDetail machineDetail = db.MachineDetails.Find(model.MachineDetailId);
                        if (machineDetail == null)
                        {
                            return "Invalid Machine Id";
                        }

                        string domain = Utility.GetDomainPart(model.URL);
                        var subCategory = db.SubCategory.Where(a => a.SubCategoryName == domain && a.SubCategoryTypeId == 1).FirstOrDefault();
                        if (subCategory == null)
                        {
                            var uncaregorizedId = db.Category.Where(a => a.CategoryName.Trim() == "UnCategorized".Trim()).Select(a => a.CategoryId).FirstOrDefault();
                            subCategory = new SubCategory();
                            subCategory.CategoryId = Convert.ToInt32(uncaregorizedId);
                            subCategory.SubCategoryTypeId = 1;
                            subCategory.SubCategoryName = domain;
                            subCategory.IsProductive = false;
                            subCategory.IsBlocked = false;
                            subCategory.IsActive = true;
                            subCategory.CreatedDate = DateTime.Now;
                            subCategory.CreatedBy = 1;
                            db.SubCategory.Add(subCategory);
                            db.SaveChanges();
                        }

                        db.BrowserDetails.Add(model);
                        db.SaveChanges();
                        returnMessage = "OK";
                    }
                }
            }
            catch (Exception e)
            {
                returnMessage = e.Message;
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }

            return returnMessage;
        }

        [System.Web.Mvc.HttpPost]
        public string AddKeyLoggings(KeyLogging model)
        {
            string returnMessage = string.Empty;
            try
            {
                using (var db = new DBContext())
                {
                    if (ModelState.IsValid)
                    {
                        MachineDetail machineDetail = db.MachineDetails.Find(model.MachineDetailId);
                        if (machineDetail == null)
                        {
                            return "Invalid Machine Id";
                        }

                        db.KeyLoggings.Add(model);
                        db.SaveChanges();
                        returnMessage = "OK";
                    }
                }
            }
            catch (Exception e)
            {
                returnMessage = e.Message;
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }

            return returnMessage;
        }

        [System.Web.Mvc.HttpPost]
        public string AddMachineIdleTime(MachineIdle model)
        {
            string returnMessage = string.Empty;

            try
            {
                using (var db = new DBContext())
                {
                    if (ModelState.IsValid)
                    {
                        MachineDetail machineDetail = db.MachineDetails.Find(model.MachineDetailId);
                        if (machineDetail == null)
                        {
                            return "Invalid Machine Id";
                        }

                        db.MachineIdles.Add(model);
                        db.SaveChanges();
                        returnMessage = "OK";
                    }
                }
            }
            catch (Exception e)
            {
                returnMessage = e.Message;
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }

            return returnMessage;
        }

        [System.Web.Mvc.HttpPost]
        public string AddAppDetails(AppDetail model)
        {
            string returnMessage = string.Empty;

            try
            {
                using (var db = new DBContext())
                {
                    if (ModelState.IsValid)
                    {
                        MachineDetail machineDetail = db.MachineDetails.Find(model.MachineDetailId);
                        if (machineDetail == null)
                        {
                            return "Invalid Machine Id";
                        }

                        var subCategory = db.SubCategory.Where(a => a.SubCategoryName == model.AppName && a.SubCategoryTypeId == 2).FirstOrDefault();
                        if (subCategory == null)
                        {
                            var uncaregorizedId = db.Category.Where(a => a.CategoryName.Trim() == "UnCategorized".Trim()).Select(a => a.CategoryId).FirstOrDefault();
                            subCategory = new SubCategory();
                            subCategory.CategoryId = Convert.ToInt32(uncaregorizedId);
                            subCategory.SubCategoryTypeId = 2;
                            subCategory.SubCategoryName = model.AppName;
                            subCategory.IsProductive = false;
                            subCategory.IsBlocked = false;
                            subCategory.IsActive = true;
                            subCategory.CreatedDate = DateTime.Now;
                            subCategory.CreatedBy = 1;
                            db.SubCategory.Add(subCategory);
                            db.SaveChanges();
                        }

                        db.AppDetails.Add(model);
                        db.SaveChanges();
                        returnMessage = "OK";
                    }
                }
            }
            catch (Exception e)
            {
                returnMessage = e.Message;
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }

            return returnMessage;
        }

        [System.Web.Mvc.HttpPost]
        public async Task<HttpResponseMessage> UploadFiles()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string fileSaveLocation = HttpContext.Current.Server.MapPath(appKeyReader.ReadKey("ScreenShotFolder"));

            var provider = new MultipartFormDataStreamProvider(fileSaveLocation);
            string fileName = "";

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                int machineID = Convert.ToInt16(provider.FormData["MachineId"].ToString());
                string userName = provider.FormData["UserName"];
                using (var db = new DBContext())
                {
                    var machineData = db.MachineDetails.Where(x => x.MachineDetailId == machineID).Select((y => new { CustomerId = y.CustomerId, y.MachineName })).FirstOrDefault();
                    string customerId = machineData.CustomerId.ToString();
                    string machineName = machineID.ToString() + "_" + machineData.MachineName;

                    if (!string.IsNullOrEmpty(machineName) && !string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(customerId))
                    {
                        //Create CustomerId Folder if not exists
                        fileSaveLocation = Path.Combine(fileSaveLocation, customerId);
                        if (!Directory.Exists(fileSaveLocation))
                        {
                            Directory.CreateDirectory(fileSaveLocation);
                        }

                        //Create Machine Folder if not exists
                        fileSaveLocation = Path.Combine(fileSaveLocation, machineName);
                        if (!Directory.Exists(fileSaveLocation))
                        {
                            Directory.CreateDirectory(fileSaveLocation);
                        }

                        //Create User Folder if not exists
                        fileSaveLocation = Path.Combine(fileSaveLocation, userName);
                        if (!Directory.Exists(fileSaveLocation))
                        {
                            Directory.CreateDirectory(fileSaveLocation);
                        }

                        foreach (MultipartFileData fileData in provider.FileData)
                        {
                            if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
                            {
                                fileName = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".png";
                            }
                            else
                            {
                                fileName = fileData.Headers.ContentDisposition.FileName;
                                fileName = fileName.Replace("\"", string.Empty);
                            }

                            //Three Attempt if getting error
                            for (int i = 0; i < 3; i++)
                            {
                                try
                                {
                                    //File Move
                                    if (File.Exists(fileData.LocalFileName))
                                    {
                                        File.Move(fileData.LocalFileName, Path.Combine(fileSaveLocation, fileName));
                                        break;
                                    }
                                }
                                catch (Exception)
                                {
                                    System.Threading.Thread.Sleep(3000);
                                }
                            }
                        }
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, fileName);
                }
            }
            catch (System.Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [System.Web.Mvc.HttpPost]
        public void UpdateScheduleReportsStatus(ScheduleReportsPOSTparam postData)
        {
            string returnMessage = string.Empty;
            try
            {
                using (var db = new DBContext())
                {
                    if (ModelState.IsValid)
                    {
                        ScheduleReports model = new ScheduleReports();
                        model = db.ScheduleReports.AsNoTracking().First(p => p.ScreenId == postData.ScreenId && p.MembershipId == postData.MembershipId && p.ScheduleType == postData.sSheduleType);

                        if (postData.sSheduleType == "D")
                        {
                            model.SendDate = model.SendDate.Value.AddDays(1);
                        }
                        else if (postData.sSheduleType == "W")
                        {
                            model.SendDate = model.SendDate.Value.AddDays(7);
                        }
                        else if (postData.sSheduleType == "M")
                        {
                            model.SendDate = model.SendDate.Value.AddMonths(1);
                        }
                        else if (postData.sSheduleType == "Q")
                        {
                            model.SendDate = model.SendDate.Value.AddMonths(3);
                        }
                        else if (postData.sSheduleType == "HY")
                        {
                            model.SendDate = model.SendDate.Value.AddMonths(6);
                        }
                        else if (postData.sSheduleType == "Y")
                        {
                            model.SendDate = model.SendDate.Value.AddYears(1);
                        }
                        db.Entry(model).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                returnMessage = e.Message;
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
        }

        [System.Web.Mvc.HttpPost]
        public void UpdateMachineSendDateStatus()
        {
            try
            {
                using (var db = new DBContext())
                {
                    if (ModelState.IsValid)
                    {
                        List<AdminScheduleReport> lstAdminScheduleReport = new List<AdminScheduleReport>();
                        lstAdminScheduleReport = (from am in db.AdminScheduleReport
                                                  join m in db.MachineDetails on am.MachineDetailId equals m.MachineDetailId
                                                  where EntityFunctions.TruncateTime(m.CreatedDate.Value) == EntityFunctions.TruncateTime(DateTime.Now)
                                                  select am).ToList();

                        foreach (var itm in lstAdminScheduleReport)
                        {
                            itm.bSend = true;
                            db.Entry(itm).State = EntityState.Modified;
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
        }

        [System.Web.Mvc.HttpPost]
        public int AddMachineSession(MachineSession model)
        {
            int machineSessionId = 0;
            try
            {
                using (var db = new DBContext())
                {
                    if (ModelState.IsValid)
                    {
                        model.CreatedDate = DateTime.Now;
                        db.MachineSessions.Add(model);
                        db.SaveChanges();
                        machineSessionId = model.MachineSessionId;
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }

            return machineSessionId;
        }

        [System.Web.Mvc.HttpPost]
        public int UpdateMachineSession(MachineSession model)
        {
            int machineSessionId = 0;
            try
            {
                using (var db = new DBContext())
                {
                    if (ModelState.IsValid)
                    {
                        if (model.MachineSessionId > 0)
                        {
                            MachineSession machineSession = db.MachineSessions.Where(x => x.MachineSessionId == model.MachineSessionId).FirstOrDefault();
                            if (machineSession != null)
                            {
                                machineSession.SessionEnd = model.SessionEnd;
                                machineSession.ModifiedDate = DateTime.Now;
                                db.Entry(machineSession).State = EntityState.Modified;
                                db.SaveChanges();
                                machineSessionId = machineSession.MachineSessionId;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }

            return machineSessionId;
        }

        #endregion
    }
}
