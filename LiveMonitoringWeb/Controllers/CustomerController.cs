using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LiveMonitoringWeb.Models;
using WebMatrix.WebData;
using LiveMonitoringWeb.Classes;
using CommonUtility;

namespace LiveMonitoringWeb.Controllers
{
    [Authorize(Roles = "Admin,Customer")]
    
    public class CustomerController : Controller
    {
        private DBContext db = new DBContext();

        //
        // GET: /Customer/

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
                var customer = db.Customers.Where(a => a.MembershipId == WebSecurity.CurrentUserId).FirstOrDefault();
                return View(customer);

            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return null;
        }

        public JsonResult JsonCustomer(){
            try
            {
                //Check Authorization
                string roleName = System.Web.Security.Roles.GetRolesForUser().Single();
                if (roleName.ToUpper() == "USER")
                {
                    //return RedirectToAction("login", "account");
                }
                var customer = db.Customers.Where(a => a.MembershipId == WebSecurity.CurrentUserId).FirstOrDefault();
                return Json(customer,JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return null;
        }

        //
        // GET: /Customer/Details/5

        public ActionResult Details(int id = 0)
        {
            try
            {
                //Check Authorization
                string roleName = System.Web.Security.Roles.GetRolesForUser().Single();
                if (roleName.ToUpper() == "USER")
                {
                    return RedirectToAction("login", "account");
                }
                Customer customer = db.Customers.Find(id);
                if (customer == null)
                {
                    return HttpNotFound();
                }
                return View(customer);
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return null;
        }

        //
        // GET: /Customer/Create

        public ActionResult Create()
        {
            try
            {
                 //Check Authorization
                string roleName = System.Web.Security.Roles.GetRolesForUser().Single();
                if (roleName.ToUpper() == "USER")
                {
                    return RedirectToAction("login", "account");
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return View();
        }

        //
        // POST: /Customer/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer)
        {
            try
            { 
                //Check Authorization
                string roleName = System.Web.Security.Roles.GetRolesForUser().Single();
                if (roleName.ToUpper() == "USER")
                {
                    return RedirectToAction("login", "account");
                }
                if (ModelState.IsValid)
                {
                    db.Customers.Add(customer);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }           
            return View(customer);
        }

        //
        // GET: /Customer/Edit/5

        public ActionResult Edit(int id = 0)
        {
            try
            {
                //Check Authorization
                string roleName = System.Web.Security.Roles.GetRolesForUser().Single();
                if (roleName.ToUpper() == "USER")
                {
                    return RedirectToAction("login", "account");
                }
                Customer customer = db.Customers.Find(id);
                if (customer == null)
                {
                    return HttpNotFound();
                }
                return View(customer);
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return null;
        }

        //
        // POST: /Customer/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer model)
        {
            try
            {
                //Check Authorization
                string roleName = System.Web.Security.Roles.GetRolesForUser().Single();
                if (roleName.ToUpper() == "USER")
                {
                    return RedirectToAction("login", "account");
                }
                if (ModelState.IsValid)
                {
                    var customer = db.Customers.Find(model.CustomerId);
                    if (customer == null)
                    {
                        return HttpNotFound();
                    }

                    customer.FirstName = string.IsNullOrEmpty(model.FirstName) == true ? string.Empty : model.FirstName;
                    customer.LastName = string.IsNullOrEmpty(model.LastName) == true ? string.Empty : model.LastName;
                    customer.OrganizationName = string.IsNullOrEmpty(model.OrganizationName) == true ? string.Empty : model.OrganizationName;
                    db.Entry(customer).State = EntityState.Modified;
                    db.SaveChanges();
                    return View("Index", customer);
                }
                return View(model);
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return null;
            
        }

        //
        // GET: /Customer/Delete/5

        public ActionResult Delete(int id = 0)
        {
            try
            {
                //Check Authorization
                string roleName = System.Web.Security.Roles.GetRolesForUser().Single();
                if (roleName.ToUpper() == "USER")
                {
                    return RedirectToAction("login", "account");
                }
                Customer customer = db.Customers.Find(id);
                if (customer == null)
                {
                    return HttpNotFound();
                }
                return View(customer);
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return null;
           
        }

        //
        // POST: /Customer/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            { 
                //Check Authorization
                string roleName = System.Web.Security.Roles.GetRolesForUser().Single();
                if (roleName.ToUpper() == "USER")
                {
                    return RedirectToAction("login", "account");
                }
                Customer customer = db.Customers.Find(id);
                db.Customers.Remove(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return null;
        }


        //
        // GET: /CustomerList/

        public ActionResult CustomerList()
        {
            try
            { 
                //Check Authorization
                string roleName = System.Web.Security.Roles.GetRolesForUser().Single();
                if (roleName.ToUpper() == "USER")
                {
                    return RedirectToAction("login", "account");
                }
                List<Customer> customerlst = new List<Customer>();
                customerlst = db.Customers.ToList();
                return View(customerlst);
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return null;
            
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}