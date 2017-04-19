using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using CommonUtility;
using LiveMonitoringWeb.Models;
using System.Collections.Generic;
using System.IO;
using System.Web.Security;
using WebMatrix.WebData;
using System.Data;
using LiveMonitoringWeb.Classes;
using System.Diagnostics;

namespace LiveMonitoringWeb.Controllers
{
    [Authorize(Roles = "Customer, User")]
    public class HomeController : Controller
    {
        #region Declaration

        private AppSettingReader appKeyReader = new AppSettingReader();
        private const int _pageSize = 10;

        #endregion Declaration

        public ActionResult Index()
        {
            using (var db = new DBContext())
            {
                int customerId = Utility.GetCustomerId();
                var customer = db.Customers.Where(a => a.CustomerId == customerId).FirstOrDefault();
                if (customer != null)
                {
                    if (customer.LastLoginDate == null)
                    {
                        customer.LastLoginDate = DateTime.Now;
                        db.Entry(customer).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Edit", "Customer", new { id = customer.CustomerId });
                    }
                }

            }

            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            return View();
        }

        public ActionResult _RecentIdleMachines()
        {
            //Check Authorization
            if (!cls_Authorization.isAllowedURL("Dashboard_TopIdleMachines")) return null;

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
                      .OrderByDescending(a => a.IdleMinute).Take(5).ToList();

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
            return PartialView(objMachineIdlelst);
        }

        public ActionResult _RecentUsers()
        {
            //Check Authorization
            if (!cls_Authorization.isAllowedURL("Dashboard_TopUsers")) return null;

            List<MachineDetail> objMachineDetaillst = new List<MachineDetail>();
            try
            {
                using (var db = new DBContext())
                {
                    DateTime dFrom = Utility.GetStartDate();
                    DateTime dTo = Utility.GetEndDate();
                    int customerId = Utility.GetCustomerId();
                    objMachineDetaillst = db.MachineDetails.Where(x => x.CreatedDate >= dFrom && x.CreatedDate <= dTo && x.CustomerId == customerId).OrderByDescending(a => a.CreatedDate).Take(5).ToList();
                    return PartialView(objMachineDetaillst);
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return PartialView(objMachineDetaillst);
        }

        public ActionResult _RecentSites()
        {
            //Check Authorization
            if (!cls_Authorization.isAllowedURL("Dashboard_TopSites")) return null;

            DateTime dFrom = Utility.GetStartDate();
            DateTime dTo = Utility.GetEndDate();
            int customerId = Utility.GetCustomerId();
            List<BrowserDetail> objBrowserDetaillst = new List<BrowserDetail>();
            try
            {
                using (var db = new DBContext())
                {
                    var data = (from rs in db.BrowserDetails.Include(a => a.machine_detail).Where(x => x.CreatedDate >= dFrom && x.CreatedDate <= dTo && x.machine_detail.CustomerId == customerId).ToList()
                                select new
                                {
                                    domain = Utility.GetDomainPart(rs.URL)
                                })
                                     .GroupBy(g => new { g.domain })
                                     .Select(g => new
                                     {
                                         Domain = g.Key.domain,
                                         Count = g.Count()
                                     }).OrderByDescending(a => a.Count).Take(5).ToList();

                    int id = 0;
                    foreach (var item in data)
                    {
                        id = id + 1;
                        objBrowserDetaillst.Add(new BrowserDetail { BrowserDetailId = id, URL = item.Domain, count = item.Count });
                    }
                    return PartialView(objBrowserDetaillst);
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return PartialView(objBrowserDetaillst);
        }

        public ActionResult _RecentApps()
        {
            //Check Authorization
            if (!cls_Authorization.isAllowedURL("Dashboard_TopApplications")) return null;

            List<AppDetail> objAppDetaillst = new List<AppDetail>();
            try
            {
                using (var db = new DBContext())
                {
                    DateTime dFrom = Utility.GetStartDate();
                    DateTime dTo = Utility.GetEndDate();
                    int customerId = Utility.GetCustomerId();
                    var objAppDetail = db.AppDetails.Include(a => a.machine_detail).Where(x => x.CreatedDate >= dFrom && x.CreatedDate <= dTo && x.machine_detail.CustomerId == customerId).GroupBy(g => new { g.AppName }).Select(g => new
                     {
                         AppName = g.Key.AppName,
                         Count = g.Count()
                     }).ToList().OrderByDescending(a => a.Count).ToList().Take(5);

                    int id = 0;
                    foreach (var item in objAppDetail)
                    {
                        id = id + 1;
                        objAppDetaillst.Add(new AppDetail { AppId = id, AppName = item.AppName, count = item.Count });
                    }
                    return PartialView(objAppDetaillst);
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return PartialView(objAppDetaillst);
        }

        public ActionResult _RecentKeyActivity()
        {
            //Check Authorization
            if (!cls_Authorization.isAllowedURL("Dashboard_TopClipboardActivity")) return null;

            List<KeyLogging> objKeyLogginglst = new List<KeyLogging>();
            try
            {
                using (var db = new DBContext())
                {
                    DateTime dFrom = Utility.GetStartDate();
                    DateTime dTo = Utility.GetEndDate();
                    int customerId = Utility.GetCustomerId();
                    objKeyLogginglst = db.KeyLoggings.Include(a => a.machine_detail).Where(x => x.TextType == "KL" && x.CreatedDate >= dFrom && x.CreatedDate <= dTo && x.machine_detail.CustomerId == customerId).OrderByDescending(b => b.CreatedDate).Take(5).ToList();
                    return PartialView(objKeyLogginglst);
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return PartialView(objKeyLogginglst);
        }

        public ActionResult _RecentClipbordActivity()
        {
            //Check Authorization
            if (!cls_Authorization.isAllowedURL("Dashboard_TopClipboardActivity")) return null;

            List<KeyLogging> objKeyLogginglst = new List<KeyLogging>();
            try
            {
                using (var db = new DBContext())
                {
                    DateTime dFrom = Utility.GetStartDate();
                    DateTime dTo = Utility.GetEndDate();
                    int customerId = Utility.GetCustomerId();
                    objKeyLogginglst = db.KeyLoggings.Include(a => a.machine_detail).Where(x => x.TextType == "CB" && x.CreatedDate >= dFrom && x.CreatedDate <= dTo && x.machine_detail.CustomerId == customerId).OrderByDescending(b => b.CreatedDate).Take(5).ToList();
                    return PartialView(objKeyLogginglst);
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return PartialView(objKeyLogginglst);
        }

        public ActionResult _RecentScreenShots()
        {
            //Check Authorization
            if (!cls_Authorization.isAllowedURL("Dashboard_TopScreenShots")) return null;

            List<FileList> objfilelist = new List<FileList>();
            try
            {
                using (var db = new DBContext())
                {
                    string screenShotFolderPath = Server.MapPath(appKeyReader.ReadKey("ScreenShotFolder"));
                    if (Directory.Exists(screenShotFolderPath))
                    {
                        DateTime dFrom = Utility.GetStartDate();
                        DateTime dTo = Utility.GetEndDate();

                        int count = 0;
                        var Folder = new DirectoryInfo(screenShotFolderPath);
                        var Images = Folder.GetFiles("*.png", SearchOption.AllDirectories).OrderByDescending(p => p.LastWriteTime).ToArray();
                        foreach (FileInfo img in Images)
                        {
                            string GetCustomerPath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetFullPath(img.DirectoryName)));
                            int customerId = Convert.ToInt16(Path.GetFileName(GetCustomerPath));

                            if (customerId == Utility.GetCustomerId())
                            {
                                string machineName = new DirectoryInfo(Convert.ToString(Directory.GetParent(img.DirectoryName))).Name;
                                int MachineDetailId = Convert.ToInt16(machineName.Substring(0, machineName.IndexOf('_')));
                                var MachineName = db.MachineDetails.Where(a => a.MachineDetailId == MachineDetailId).Select(b => b.MachineName).FirstOrDefault();
                                var UserName = db.MachineDetails.Where(a => a.MachineDetailId == MachineDetailId).Select(b => b.UserName).FirstOrDefault();
                                String[] FileName = img.ToString().Split('_');
                                //DateTime FileDate = Convert.ToDateTime(FileName[1] + "/" + FileName[2] + "/" + FileName[0]);
                                DateTime FileDate = DateTime.ParseExact((FileName[1] + "-" + FileName[2] + "-" + FileName[0]), "MM-dd-yyyy", new System.Globalization.CultureInfo("en-US"), System.Globalization.DateTimeStyles.None);
                                if (FileDate >= dFrom && FileDate <= dTo && count < 14)
                                {
                                    count++;
                                    objfilelist.Add(new FileList { FileURL = img.FullName.Replace(Server.MapPath("~/"), string.Empty).Replace("\\", "/"), MachineName = MachineName, UserName = UserName });
                                }
                            }
                        }
                    }
                    return PartialView(objfilelist);
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return PartialView(objfilelist);
        }

        public ActionResult _CurrentIdleUsersGraph()
        {
            //Check Authorization
            if (!cls_Authorization.isAllowedURL("Dashboard_TopIdleUserGraph")) return null;

            return PartialView();
        }

        public ActionResult _TopSitesGraph()
        {
            //Check Authorization
            if (!cls_Authorization.isAllowedURL("Dashboard_TopSitesGraph")) return null;

            return PartialView();
        }

        public ActionResult _UserProductivity()
        {
            //Check Authorization
            if (!cls_Authorization.isAllowedURL("Dashboard_TopUserProductivityGraph")) return null;

            return PartialView();
        }

        #region Json Data for Graph/Chart

        public JsonResult JsonCurrentIdleUsers()
        {
            try
            {
                using (var db = new DBContext())
                {
                    DateTime dFrom = Utility.GetStartDate();
                    DateTime dTo = Utility.GetEndDate();
                    int customerId = Utility.GetCustomerId();
                    var data = db.MachineIdles.Include(a => a.machine_detail)
                        .Where(x => x.CreatedDate >= dFrom && x.CreatedDate <= dTo && x.machine_detail.CustomerId == customerId)
                        .GroupBy(g => new { g.machine_detail.UserName, g.machine_detail.MachineName })
                        .Select(g => new
                        {
                            User = g.Key.MachineName + "/" + g.Key.UserName,
                            IdleMinute = (g.Sum(x => x.IdleTime) / 60)
                        })
                        .Select(a => new
                        {
                            User = a.User.Substring(0, a.User.Length >= 20 ? 20 : a.User.Length).ToLower(),
                            IdleMinute = a.IdleMinute
                        })
                        .OrderByDescending(a => a.IdleMinute).Take(10).ToList();

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

        public JsonResult JsonTopSites()
        {
            try
            {
                using (var db = new DBContext())
                {
                    DateTime dFrom = Utility.GetStartDate();
                    DateTime dTo = Utility.GetEndDate();
                    int customerId = Utility.GetCustomerId();
                    var data = (from rs in db.BrowserDetails.Include(a => a.machine_detail).Where(x => x.CreatedDate >= dFrom && x.CreatedDate <= dTo && x.machine_detail.CustomerId == customerId).ToList()
                                select new
                                {
                                    domain = Utility.GetDomainPart(rs.URL)
                                })
                                  .GroupBy(g => new { g.domain })
                                  .Select(g => new
                                  {
                                      Domain = g.Key.domain,
                                      Count = g.Count()
                                  }).OrderByDescending(a => a.Count).Take(5).ToList();

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

        public JsonResult JsonTopApps()
        {
            try
            {
                using (var db = new DBContext())
                {
                    DateTime dFrom = Utility.GetStartDate();
                    DateTime dTo = Utility.GetEndDate();
                    int customerId = Utility.GetCustomerId();
                    var data = ( db.AppDetails.Include(a => a.machine_detail).Where(x => x.CreatedDate >= dFrom && x.CreatedDate <= dTo && x.machine_detail.CustomerId == customerId).GroupBy(g => new { g.AppName }).Select(g => new
                    {
                        AppName = g.Key.AppName,
                        Count = g.Count()
                    }).ToList().OrderByDescending(a => a.Count).ToList().Take(5));

                   
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

        public JsonResult JsonRecentKeyActivity()
        {
            try
            {
                using (var db = new DBContext())
                {
                    DateTime dFrom = Utility.GetStartDate();
                    DateTime dTo = Utility.GetEndDate();
                    int customerId = Utility.GetCustomerId();
                    var data = db.KeyLoggings.Include(a => a.machine_detail).Where(x => x.TextType == "KL" && x.CreatedDate >= dFrom && x.CreatedDate <= dTo && x.machine_detail.CustomerId == customerId).OrderByDescending(b => b.CreatedDate).Take(5).ToList();
                    

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

        public JsonResult JsonClipboardActivity()
        {
            try
            {
                using (var db = new DBContext())
                {
                    DateTime dFrom = Utility.GetStartDate();
                    DateTime dTo = Utility.GetEndDate();
                    int customerId = Utility.GetCustomerId();
                    var data = db.KeyLoggings.Include(a => a.machine_detail).Where(x => x.TextType == "CB" && x.CreatedDate >= dFrom && x.CreatedDate <= dTo && x.machine_detail.CustomerId == customerId).OrderByDescending(b => b.CreatedDate).Take(5).ToList();
                   

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

        public JsonResult Jsonscreenshot()
        {
            List<FileList> objfilelist = new List<FileList>();
            try
            {
                using (var db = new DBContext())
                {

                    string screenShotFolderPath = Server.MapPath(appKeyReader.ReadKey("ScreenShotFolder"));
                    if (Directory.Exists(screenShotFolderPath))
                    {
                        DateTime dFrom = Utility.GetStartDate();
                        DateTime dTo = Utility.GetEndDate();

                        int count = 0;
                        var Folder = new DirectoryInfo(screenShotFolderPath);
                        var Images = Folder.GetFiles("*.png", SearchOption.AllDirectories).OrderByDescending(p => p.LastWriteTime).ToArray();
                        foreach (FileInfo img in Images)
                        {
                            string GetCustomerPath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetFullPath(img.DirectoryName)));
                            int customerId = Convert.ToInt16(Path.GetFileName(GetCustomerPath));

                            if (customerId == Utility.GetCustomerId())
                            {
                                string machineName = new DirectoryInfo(Convert.ToString(Directory.GetParent(img.DirectoryName))).Name;
                                int MachineDetailId = Convert.ToInt16(machineName.Substring(0, machineName.IndexOf('_')));
                                var MachineName = db.MachineDetails.Where(a => a.MachineDetailId == MachineDetailId).Select(b => b.MachineName).FirstOrDefault();
                                var UserName = db.MachineDetails.Where(a => a.MachineDetailId == MachineDetailId).Select(b => b.UserName).FirstOrDefault();
                                String[] FileName = img.ToString().Split('_');
                                string clickDate = FileName[2] + "/" + FileName[1] + "/" + FileName[0] + " " + FileName[3] + ":" + FileName[4];
                                DateTime FileDate = DateTime.ParseExact((FileName[1] + "-" + FileName[2] + "-" + FileName[0]), "MM-dd-yyyy", new System.Globalization.CultureInfo("en-US"), System.Globalization.DateTimeStyles.None);
                                if (FileDate >= dFrom && FileDate <= dTo && count < 14)
                                {
                                    count++;
                                    objfilelist.Add(new FileList { FileURL = img.FullName.Replace(Server.MapPath("~/"), string.Empty).Replace("\\", "/"), MachineName = MachineName, UserName = UserName, CaptureDate = clickDate });
                                }
                            }
                        }
                    }
                

                    return Json(objfilelist, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
                return Json(string.Empty);
            }
        }

        public JsonResult JsonUserProductivity()
        {
            try
            {
                using (var db = new DBContext())
                {
                    DateTime dFrom = Utility.GetStartDate();
                    DateTime dTo = Utility.GetEndDate();
                    int customerId = Utility.GetCustomerId();
                    var midData = (from rs in db.BrowserDetails.Include(a => a.machine_detail)
                                .Where(x => x.CreatedDate >= dFrom && x.CreatedDate <= dTo && x.machine_detail.CustomerId == customerId).ToList()
                                   select new
                                   {
                                       domain = Utility.GetDomainPart(rs.URL),
                                       UserName = rs.machine_detail.UserName,
                                       MachineName = rs.machine_detail.MachineName
                                   })
                                 .GroupBy(g => new { g.domain, g.UserName, g.MachineName })
                                 .Select(g => new
                                 {
                                     Domain = g.Key.domain,
                                     User = g.Key.MachineName + "/" + g.Key.UserName,
                                     Count = g.Count()
                                 }).OrderByDescending(a => a.Count).ToList();

                    var midAppData = (from rs in db.AppDetails.Include(a => a.machine_detail)
                                      .Where(x => x.CreatedDate >= dFrom && x.CreatedDate <= dTo && x.machine_detail.CustomerId == customerId).ToList()
                                      select new
                                      {
                                          App = rs.AppName,
                                          UserName = rs.machine_detail.UserName,
                                          MachineName = rs.machine_detail.MachineName
                                      })
                                     .GroupBy(g => new { g.App, g.UserName, g.MachineName })
                               .Select(g => new
                               {
                                   Domain = g.Key.App,
                                   User = g.Key.MachineName + "/" + g.Key.UserName,
                                   Count = g.Count()
                               }).OrderByDescending(a => a.Count).ToList();

                    midData.AddRange(midAppData);

                    var data = (from rs in midData
                                join subCat in db.SubCategory on rs.Domain equals subCat.SubCategoryName
                                select new
                                {
                                    User = rs.User,
                                    Productive = subCat.IsProductive == true ? rs.Count : 0,
                                    NonProductive = subCat.IsProductive == false ? rs.Count : 0
                                })
                                .GroupBy(g => g.User)
                                .Select(g => new
                                {
                                    User = g.Key,
                                    Productive = g.Sum(x => x.Productive),
                                    NonProductive = g.Sum(x => x.NonProductive)
                                })
                                .Select(a => new
                                {
                                    User = a.User.Substring(0, a.User.Length >= 20 ? 20 : a.User.Length).ToLower(),
                                    Productive = Math.Round((a.Productive * 100M) / (a.Productive + a.NonProductive), 2),
                                    NonProductive = Math.Round((a.NonProductive * 100M) / (a.Productive + a.NonProductive), 2)
                                })
                                .Take(10).ToList();

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



        #endregion
        [HttpPost]
        public ActionResult SetTimeInterval(string DateFrom = null, string DateTo = null)
        {
            if (!String.IsNullOrEmpty(DateFrom) && !String.IsNullOrEmpty(DateTo))
            {
                Utility.SetDates(DateFrom, DateTo);
            }

            return Json(new ReturnData { Status=true,Msg="Reload"},JsonRequestBehavior.AllowGet);
            //var uri = Request.UrlReferrer;
            //string ControllerName = uri.AbsoluteUri;
            //if (!string.IsNullOrEmpty(uri.Query))
            //{
            //    ControllerName = uri.AbsoluteUri.Replace(uri.Query, "");
            //}
            //ControllerName = ControllerName.ToLower().Replace(new CommonUtility.AppSettingReader().ReadKey("WebBaseURL"), "");

            //if (ControllerName.Contains("/"))
            //{
            //    string[] data = ControllerName.Split('/');
            //    return Redirect(Url.Action(data[1], data[0]) + uri.Query);
            //}
            //else
            //{
            //    return RedirectToAction("Index", ControllerName);
            //}
        }

        public ActionResult Download()
        {
            try
            {
                lock (this)
                {
                    int customerId = Utility.GetCustomerId();
                    var batchFile = Server.MapPath("~/App_Data/data/MyCopy.bat");
                    var logfile = Server.MapPath("~/App_Data/data/logs/" + customerId.ToString() + ".log");


                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.CreateNoWindow = false;
                    startInfo.UseShellExecute = false;
                    startInfo.FileName = batchFile;
                    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    startInfo.Arguments = customerId.ToString().Trim();

                    // Start the process with the info we specified.
                    // Call WaitForExit and then the using statement will close.
                    using (Process exeProcess = Process.Start(startInfo))
                    {
                        exeProcess.WaitForExit();
                    }
                    var installerFile = Server.MapPath("~/App_Data/data/LiveMonitoringInstaller/LiveMonitoringSetup.exe");
                    if (!System.IO.File.Exists(installerFile))
                    {
                        TempData["ErrorMessage"] = "Try again later";
                        return RedirectToAction("Index");
                    }

                    CustomerDownloadHistory model = new CustomerDownloadHistory();
                    using (var db = new DBContext())
                    {
                        if (model != null)
                        {
                            model.CustomerId = customerId;
                            model.DownloadDate = DateTime.Now;
                            db.CustomerDownloadHistory.Add(model);
                            db.SaveChanges();
                        }

                    }

                    return File(installerFile, "application/octet-stream", "LiveMonitoringSetup.exe");
                }
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("Index");
        }

    }
}
