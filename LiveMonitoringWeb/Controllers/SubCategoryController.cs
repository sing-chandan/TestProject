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
    public class SubCategoryController : Controller
    {
        private DBContext db = new DBContext();

        //
        // GET: /SubCategory/

        public ActionResult Index()
        {
            //Check Authorization
            if (!cls_Authorization.isAllowedURL("Admin_SubCategory")) return RedirectToAction("login", "account");

            using (var db = new DBContext())
            {
                
                var subcategory = db.SubCategory.Include(s => s.Category).Include(s => s.SubCategoryType);
                return View(subcategory.ToList());
            }
        }

        //
        // GET: /SubCategory/Details/5

        public JsonResult JsonSubCategoryData()
        {
            var subcategory = db.SubCategory.Include(s => s.Category).Include(s => s.SubCategoryType);
            var category = db.Category;
            var subcategoryType = db.SubCategoryType;

            ReturnSubCategory returnSubCatData = new ReturnSubCategory();

            returnSubCatData.SubCategory = subcategory.ToList();
            returnSubCatData.Categorylist = category.ToList();
            returnSubCatData.SubCategoryType = subcategoryType.ToList();

            return Json(returnSubCatData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(int id = 0)
        {
            //Check Authorization
            if (!cls_Authorization.isAllowedURL("Admin_SubCategory")) return RedirectToAction("login", "account");

            SubCategory subcategory = new SubCategory();
            try
            {
                using (var db = new DBContext())
                {
                   
                    subcategory = db.SubCategory.Include(a => a.Category).Include(b => b.SubCategoryType).Where(c => c.SubCategoryId == id).FirstOrDefault();
                    if (subcategory == null)
                    {
                        return HttpNotFound();
                    }
                    return View(subcategory);
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            return View(subcategory);
        }

        //
        // GET: /SubCategory/Create

        public ActionResult Create()
        {
            //Check Authorization
            if (!cls_Authorization.isAllowedURL("Admin_SubCategory")) return RedirectToAction("login", "account");

            var db = new DBContext();
            
            ViewBag.CategoryId = new SelectList(db.Category, "CategoryId", "CategoryName");
            ViewBag.SubCategoryTypeId = new SelectList(db.SubCategoryType, "SubCategoryTypeId", "SubCategoryTypeName");
            return View();

        }

        //
        // POST: /SubCategory/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SubCategory model)
        {
            //Check Authorization
            if (!cls_Authorization.isAllowedURL("Admin_SubCategory")) return RedirectToAction("login", "account");

            var db = new DBContext();
            try
            {
                if (ModelState.IsValid)
                {
                    var subCategory = db.SubCategory.FirstOrDefault(sc => sc.SubCategoryName == (string)model.SubCategoryName && sc.CategoryId == model.CategoryId && sc.SubCategoryTypeId == model.SubCategoryTypeId);
                    if (subCategory == null)
                    {
                        model.IsActive = true;
                        model.CreatedDate = DateTime.Now;
                        model.CreatedBy = WebMatrix.WebData.WebSecurity.CurrentUserId;
                        db.SubCategory.Add(model);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Message = "SubCategory already exist";
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            ViewBag.CategoryId = new SelectList(db.Category, "CategoryId", "CategoryName", model.CategoryId);
            ViewBag.SubCategoryTypeId = new SelectList(db.SubCategoryType, "SubCategoryTypeId", "SubCategoryTypeName", model.SubCategoryTypeId);
            return View(model);
        }

        //
        // GET: /SubCategory/Edit/5

        public ActionResult Edit(int id = 0)
        {
            //Check Authorization
            if (!cls_Authorization.isAllowedURL("Admin_SubCategory")) return RedirectToAction("login", "account");

            var db = new DBContext();
            SubCategory subcategory = db.SubCategory.Find(id);
            if (subcategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Category, "CategoryId", "CategoryName", subcategory.CategoryId);
            ViewBag.SubCategoryTypeId = new SelectList(db.SubCategoryType, "SubCategoryTypeId", "SubCategoryTypeName", subcategory.SubCategoryTypeId);
            return View(subcategory);
        }

        //
        // POST: /SubCategory/Edit/5

        [HttpPost]
        public ActionResult Save(SubCategory model)
        {
            var db = new DBContext();
            try
            {
                if (ModelState.IsValid)
                {
                    if(model.SubCategoryId==0)
                    {
                        var subCategory = db.SubCategory.FirstOrDefault(sc => sc.SubCategoryName == (string)model.SubCategoryName && sc.CategoryId == model.CategoryId && sc.SubCategoryTypeId == model.SubCategoryTypeId);
                         if (subCategory == null)
                         {
                        model.IsActive = true;
                        model.CreatedDate = DateTime.Now;
                        model.CreatedBy = WebMatrix.WebData.WebSecurity.CurrentUserId;
                        db.SubCategory.Add(model);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                        }
                      else
                      {
                        // ViewBag.Message = "SubCategory already exist";
                          return Json(new ReturnData { Status = true, Msg = "SubCategory already exist" }, JsonRequestBehavior.AllowGet);
                       }

                    }
                    else{

                         var subcatg = db.SubCategory.FirstOrDefault(sc => sc.SubCategoryName == (string)model.SubCategoryName && sc.CategoryId == model.CategoryId && sc.SubCategoryTypeId == model.SubCategoryTypeId && sc.SubCategoryId != model.SubCategoryId);
                    if (subcatg == null)
                    {
                        var subcategory = db.SubCategory.Find(model.SubCategoryId);
                        if (subcategory == null)
                        {
                            return HttpNotFound();
                        }

                        subcategory.CategoryId = model.CategoryId;
                        subcategory.SubCategoryTypeId = model.SubCategoryTypeId;
                        subcategory.SubCategoryName = model.SubCategoryName;
                        subcategory.IsProductive = model.IsProductive;
                        subcategory.IsBlocked = model.IsBlocked;
                        subcategory.IsActive = model.IsActive;
                        if (model.IsActive)
                        {
                            subcategory.IsDeleted = false;
                        }
                        subcategory.ModifiedDate = DateTime.Now;
                        subcategory.ModifiedBy = WebMatrix.WebData.WebSecurity.CurrentUserId;
                        db.Entry(subcategory).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        //ViewBag.Message = "SubCategory already exist";
                        return Json(new ReturnData { Status = true, Msg = "SubCategory already exist" }, JsonRequestBehavior.AllowGet);
                    }
                    }

                }
                else
                {
                    return Json("Data inproper", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
                return Json("Exception Occurs",JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SubCategory model)
        {
            //Check Authorization
            if (!cls_Authorization.isAllowedURL("Admin_SubCategory")) return RedirectToAction("login", "account");

            var db = new DBContext();
            try
            {
                if (ModelState.IsValid)
                {
                    var subcatg = db.SubCategory.FirstOrDefault(sc => sc.SubCategoryName == (string)model.SubCategoryName && sc.CategoryId == model.CategoryId && sc.SubCategoryTypeId == model.SubCategoryTypeId && sc.SubCategoryId != model.SubCategoryId);
                    if (subcatg == null)
                    {
                        var subcategory = db.SubCategory.Find(model.SubCategoryId);
                        if (subcategory == null)
                        {
                            return HttpNotFound();
                        }

                        subcategory.CategoryId = model.CategoryId;
                        subcategory.SubCategoryTypeId = model.SubCategoryTypeId;
                        subcategory.SubCategoryName = model.SubCategoryName;
                        subcategory.IsProductive = model.IsProductive;
                        subcategory.IsBlocked = model.IsBlocked;
                        subcategory.IsActive = model.IsActive;
                        if (model.IsActive)
                        {
                            subcategory.IsDeleted = false;
                        }
                        subcategory.ModifiedDate = DateTime.Now;
                        subcategory.ModifiedBy = WebMatrix.WebData.WebSecurity.CurrentUserId;
                        db.Entry(subcategory).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Message = "SubCategory already exist";
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionHandler handler = new ExceptionHandler();
                handler.HandleException(e);
            }
            ViewBag.CategoryId = new SelectList(db.Category, "CategoryId", "CategoryName", model.CategoryId);
            ViewBag.SubCategoryTypeId = new SelectList(db.SubCategoryType, "SubCategoryTypeId", "SubCategoryTypeName", model.SubCategoryTypeId);
            return View(model);
        }

        //
        // GET: /SubCategory/Delete/5

        public ActionResult Delete(int id = 0)
        {
            //Check Authorization
            if (!cls_Authorization.isAllowedURL("Admin_SubCategory")) return RedirectToAction("login", "account");

            var db = new DBContext();
            SubCategory subcategory = db.SubCategory.Include(a => a.Category).Include(b => b.SubCategoryType).Where(c => c.SubCategoryId == id).FirstOrDefault();
            if (subcategory == null)
            {
                return HttpNotFound();
            }
            return View(subcategory);
        }

        //
        // POST: /SubCategory/Delete/5

        [HttpPost, ActionName("Delete")]
      //  [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Check Authorization
            if (!cls_Authorization.isAllowedURL("Admin_SubCategory")) return RedirectToAction("login", "account");

            using (var db = new DBContext())
            {
                SubCategory subcategory = db.SubCategory.Find(id);
                subcategory.IsActive = false;
                subcategory.IsDeleted = true;
                subcategory.DeletedDate = DateTime.Now;
                subcategory.DeletedBy = WebMatrix.WebData.WebSecurity.CurrentUserId;
                db.Entry(subcategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
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