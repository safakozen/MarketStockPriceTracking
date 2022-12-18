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
    public class BrandController : Controller
    {
        private WigrosEntities db = new WigrosEntities();

        public ActionResult Index()
        {
            if (Session["CurrentUser"] != null)
            {
                var brand = db.Brand.ToList();
                return View(brand);
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
        public ActionResult Create(Brand brand)
        {
            if (ModelState.IsValid)
            {
                User user = Session["CurrentUser"] as User;
                brand.CreatedDate = DateTime.Now;
                brand.UserId = user.Id;
                db.Brand.Add(brand);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.User, "Id", "Fullname", brand.UserId);
            return View(brand);
        }

        public ActionResult Edit(int? id)
        {
            if (Session["CurrentUser"] != null)
            {
                Brand brand = db.Brand.Find(id);
                ViewBag.UserId = new SelectList(db.User, "Id", "Fullname", brand.UserId);
                return View(brand);
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult Edit(Brand brand)
        {
            if (ModelState.IsValid)
            {
                User user = Session["CurrentUser"] as User;
                brand.UserId = user.Id;
                db.Entry(brand).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(brand);
        }

        public ActionResult Delete(int? id)
        {
            if (Session["CurrentUser"] != null)
            {
                Brand brand = db.Brand.Find(id);
                return View(brand);
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            User user = Session["CurrentUser"] as User;
            Brand brand = db.Brand.Find(id);
            brand.IsActive = false;
            brand.UserId = user.Id;
            db.Entry(brand).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
