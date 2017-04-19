using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using CommonUtility;
using LiveMonitoringWeb.Models;

namespace LiveMonitoringWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _RecentCustomers()
        {

            List<Customer> objCustomerlst = new List<Customer>();
            try
            {
                using (var db = new DBContext())
                {
                    var objCustomer = db.Customers.OrderByDescending(a => a.CreateDate).ToList().Take(5).ToList();

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
                            Downloads = Downloads
                        });
                    }
                    return PartialView(objCustomerlst);
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return PartialView(objCustomerlst);
        }

        public ActionResult CustomerSummary()
        {
            List<Customer> objCustomerlst = new List<Customer>();
            try
            {
                using (var db = new DBContext())
                {
                    var objCustomer = db.Customers.OrderByDescending(a => a.CreateDate).ToList();

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
                            Downloads = Downloads
                        });
                    }
                    return PartialView(objCustomerlst);
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return View();
        }

        public ActionResult CustomerDownloadHistory(int CustomerId = 0)
        {
            List<CustomerDownloadHistory> objCustomerDownloadHistorylst = new List<CustomerDownloadHistory>();
            try
            {
                using (var db = new DBContext())
                {
                    objCustomerDownloadHistorylst = db.CustomerDownloadHistory.Include(a => a.customer).Where(a => a.CustomerId == CustomerId).OrderByDescending(a => a.DownloadDate).ToList();
                }
                return View(objCustomerDownloadHistorylst);

            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return View();
        }

    }
}
