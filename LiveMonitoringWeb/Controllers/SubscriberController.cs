using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LiveMonitoringWeb.Models;
using LiveMonitoringWeb.Classes;

namespace LiveMonitoringWeb.Controllers
{
   [Authorize(Roles = "Admin,Customer")]
    public class SubscriberController : Controller
    {
        private DBContext db = new DBContext();

        //
        // GET: /Subscriber/

        public ActionResult Index()
        {          
            var subscribers = db.Subscribers.Include(s => s.customer);
            return View(subscribers.ToList());
        }

        //
        // GET: /Subscriber/Details/5

        public ActionResult Details(int id = 0)
        {
            Subscriber subscriber = db.Subscribers.Find(id);
            if (subscriber == null)
            {
                return HttpNotFound();
            }
            return View(subscriber);
        }

        //
        // GET: /Subscriber/Create

        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "Email");
            return View();
        }

        //
        // POST: /Subscriber/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Subscriber subscriber)
        {
            if (ModelState.IsValid)
            {
                db.Subscribers.Add(subscriber);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "Email", subscriber.CustomerId);
            return View(subscriber);
        }

        //
        // GET: /Subscriber/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Subscriber subscriber = db.Subscribers.Find(id);
            if (subscriber == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "Email", subscriber.CustomerId);
            return View(subscriber);
        }

        //
        // POST: /Subscriber/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Subscriber subscriber)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subscriber).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "Email", subscriber.CustomerId);
            return View(subscriber);
        }

        //
        // GET: /Subscriber/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Subscriber subscriber = db.Subscribers.Find(id);
            if (subscriber == null)
            {
                return HttpNotFound();
            }
            return View(subscriber);
        }

        //
        // POST: /Subscriber/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Subscriber subscriber = db.Subscribers.Find(id);
            db.Subscribers.Remove(subscriber);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // GET: /SubscriberList/

        public ActionResult SubscriberList()
        {
            List<Subscriber> Subscriberlst = new List<Subscriber>();
            Subscriberlst = db.Subscribers.Include(s => s.customer).ToList();
            return View(Subscriberlst);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}