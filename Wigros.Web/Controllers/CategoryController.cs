using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Wigros.Web.Models;

namespace Wigros.Web.Controllers
{
    public class CategoryController : Controller
    {
        private WigrosEntities db = new WigrosEntities();

        public ActionResult Index()
        {
            if (Session["CurrentUser"] != null)
            {
                var Category = db.Category.ToList();
                return View(Category);
            }
            return RedirectToAction("Login", "Login");

        }

        public ActionResult Create()
        {
            if (Session["CurrentUser"] != null)
            {
                ViewBag.UserId = new SelectList(db.User, "Id", "Fullname");
                return View();
            }
            return RedirectToAction("Login", "Login");
        }
       
        [HttpPost]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                User user = Session["CurrentUser"] as User;
                category.CreatedDate = DateTime.Now;
                category.UserId = user.Id;
                db.Category.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.User, "Id", "Fullname", category.UserId);
            return View(category);
        }

        public ActionResult Edit(int? id)
        {
            if (Session["CurrentUser"] != null)
            {
                Category category = db.Category.Find(id);
                ViewBag.UserId = new SelectList(db.User, "Id", "Fullname", category.UserId);
                return View(category);
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                var CurrentUser = Session["CurrentUser"] as User;
                category.UserId = CurrentUser.Id;
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public ActionResult Delete(int? id)
        {
            if (Session["CurrentUser"] != null)
            {
                Category category = db.Category.Find(id);
                return View(category);
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            User user = Session["CurrentUser"] as User;
            Category category = db.Category.Find(id);
            category.IsActive = false;
            category.UserId = user.Id;
            db.Entry(category).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
