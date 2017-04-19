using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LiveMonitoringWeb.Models;
using CommonUtility;
using LiveMonitoringWeb.Classes;

namespace LiveMonitoringWeb.Controllers
{
   [Authorize(Roles = "Customer, User")]
    public class CategoryController : Controller
    {
        //
        // GET: /Category/

        public ActionResult Index()
        {
            //Check Authorization
            if (!cls_Authorization.isAllowedURL("Admin_Category")) return RedirectToAction("login", "account");

            using (var db = new DBContext())
            {
                return View(db.Category.OrderBy(a => a.CategoryName).ToList());
            }
        }

        public JsonResult JsonCategory()
        {
            using (var db = new DBContext())
            {
                return Json(db.Category.Select(a => new {a.CategoryId,a.CategoryName,a.IsActive,a.IsBlocked,a.IsDeleted}).OrderBy(a => a.CategoryName).ToList(), JsonRequestBehavior.AllowGet);
            }

        }

        //
        // GET: /Category/Details/5

        public ActionResult Details(int id = 0)
        {
            //Check Authorization
            if (!cls_Authorization.isAllowedURL("Admin_Category")) return RedirectToAction("login", "account");

            Category category = new Category();
            try
            {
                using (var db = new DBContext())
                {
                    category = db.Category.Find(id);
                    if (category == null)
                    {
                        return HttpNotFound();
                    }
                    return View(category);
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }

            return View(category);
        }

        //
        // GET: /Category/Create

        public ActionResult Create()
        {
            //Check Authorization
            if (!cls_Authorization.isAllowedURL("Admin_Category")) return RedirectToAction("login", "account");

            return View();
        }

        //
        // POST: /Category/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category model)
        {
            try
            {
                //Check Authorization
                if (!cls_Authorization.isAllowedURL("Admin_Category")) return RedirectToAction("login", "account");

                if (ModelState.IsValid)
                {
                    using (var db = new DBContext())
                    {
                        var category = db.Category.FirstOrDefault(c => c.CategoryName == model.CategoryName);
                        if (category == null)
                        {
                            model.IsActive = true;
                            model.CreatedDate = DateTime.Now;
                            model.CreatedBy = WebMatrix.WebData.WebSecurity.CurrentUserId;
                            db.Category.Add(model);
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ViewBag.Message = "Category already exist";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult Save(Category model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new DBContext())
                {
                    if (model.CategoryId == 0)
                    {
                        var category = db.Category.FirstOrDefault(c => c.CategoryName == model.CategoryName);
                        if (category == null)
                        {
                            model.IsActive = true;
                            model.CreatedDate = DateTime.Now;
                            model.CreatedBy = WebMatrix.WebData.WebSecurity.CurrentUserId;
                            db.Category.Add(model);
                            db.SaveChanges();
                            //return RedirectToAction("Index");
                        }
                        else
                        {
                            //  ViewBag.Message = "Category already exist";
                        }
                    }
                    else
                    {
                        var catg = db.Category.FirstOrDefault(c => c.CategoryName == model.CategoryName && c.CategoryId != model.CategoryId);
                        if (catg == null)
                        {
                            var category = db.Category.Find(model.CategoryId);
                            if (category == null)
                            {
                              //  return HttpNotFound();
                            }

                            category.CategoryName = model.CategoryName;
                            category.IsActive = model.IsActive;
                            category.IsBlocked = model.IsBlocked;
                            if (category.IsActive)
                            {
                                category.IsDeleted = false;
                            }
                            category.ModifiedDate = DateTime.Now;
                            category.ModifiedBy = WebMatrix.WebData.WebSecurity.CurrentUserId;
                            db.Entry(category).State = EntityState.Modified;
                            db.SaveChanges();
                          //  return RedirectToAction("Index");
                        }
                        else
                        {
                          //  ViewBag.Message = "Category already exist";
                        }
                    }
                }
            }
            return Json("successfully", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Category/Edit/5

        public ActionResult Edit(int id = 0)
        {
            //Check Authorization
            if (!cls_Authorization.isAllowedURL("Admin_Category")) return RedirectToAction("login", "account");

            Category category = new Category();
            try
            {
                using (var db = new DBContext())
                {
                    category = db.Category.Find(id);
                    if (category == null)
                    {
                        return HttpNotFound();
                    }
                    return View(category);
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return View(category);
        }

        //
        // POST: /Category/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category model)
        {
            try
            {
                //Check Authorization
                if (!cls_Authorization.isAllowedURL("Admin_Category")) return RedirectToAction("login", "account");

                if (ModelState.IsValid)
                {
                    using (var db = new DBContext())
                    {
                        var catg = db.Category.FirstOrDefault(c => c.CategoryName == model.CategoryName && c.CategoryId != model.CategoryId);
                        if (catg == null)
                        {
                            var category = db.Category.Find(model.CategoryId);
                            if (category == null)
                            {
                                return HttpNotFound();
                            }

                            category.CategoryName = model.CategoryName;
                            category.IsActive = model.IsActive;
                            category.IsBlocked = model.IsBlocked;
                            if (category.IsActive)
                            {
                                category.IsDeleted = false;
                            }
                            category.ModifiedDate = DateTime.Now;
                            category.ModifiedBy = WebMatrix.WebData.WebSecurity.CurrentUserId;
                            db.Entry(category).State = EntityState.Modified;
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ViewBag.Message = "Category already exist";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return View(model);
        }

        //
        // GET: /Category/Delete/5

        public ActionResult Delete(int id = 0)
        {
            //Check Authorization
            if (!cls_Authorization.isAllowedURL("Admin_Category")) return RedirectToAction("login", "account");

            Category category = new Category();
            try
            {
                using (var db = new DBContext())
                {
                    category = db.Category.Find(id);
                    if (category == null)
                    {
                        return HttpNotFound();
                    }
                    return View(category);
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }

            return View(category);
        }

        //
        // POST: /Category/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                //Check Authorization
                if (!cls_Authorization.isAllowedURL("Admin_Category")) return RedirectToAction("login", "account");

                using (var db = new DBContext())
                {
                    Category category = db.Category.Find(id);
                    category.IsActive = false;
                    category.IsDeleted = true;
                    category.DeletedDate = DateTime.Now;
                    category.DeletedBy = WebMatrix.WebData.WebSecurity.CurrentUserId;
                    db.Entry(category).State = EntityState.Modified;
                    db.SaveChanges();
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