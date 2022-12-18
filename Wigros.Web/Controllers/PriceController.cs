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
    public class PriceController : Controller
    {
        private WigrosEntities db = new WigrosEntities();

        public ActionResult Index()
        {
            if (Session["CurrentUser"] != null)
            {
                return View(db.Price.Where(x => x.IsActive == true).ToList());
            }
            return RedirectToAction("Login", "Login");
        }

        public ActionResult Create()
        {
            if (Session["CurrentUser"] != null)
            {
                ViewBag.ProductId = new SelectList(db.Product.Where(x => x.IsActive == true), "ID", "Name");
                return View();
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult Create(Price price)
        {
            if (ModelState.IsValid)
            {
                var data = db.Price.Where(x => x.ProductId == price.ProductId && x.IsActive == true).SingleOrDefault();
                if (data != null)
                {
                    data.IsActive = false;
                    data.CreatedDate = DateTime.Now;
                }

                var CurrentUser = Session["CurrentUser"] as User;
                price.UserId = CurrentUser.Id;
                price.IsActive = true;
                price.CreatedDate = DateTime.Now;
                db.Price.Add(price);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(price);
        }

        public ActionResult Edit(int? id)
        {
            if (Session["CurrentUser"] != null)
            {
                Price price = db.Price.Find(id);
                ViewBag.ProductId = new SelectList(db.Product.Where(x => x.IsActive == true), "ID", "Name", price.ProductId);
                return View(price);
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult Edit(Price price)
        {
            if (ModelState.IsValid)
            {
                var CurrentUser = Session["CurrentUser"] as User;
                Price _price = db.Price.Single(x => x.Id == price.Id);
                _price.IsActive = true;
                _price.NewPrice = price.NewPrice;
                _price.OldPrice = price.OldPrice;
                _price.CreatedDate = price.CreatedDate;
                _price.UserId = CurrentUser.Id;
                db.Entry(_price).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductId = new SelectList(db.Product.Where(x => x.IsActive == true), "ID", "Name", price.Id);
            return View(price);
        }

        public ActionResult Delete(int? id)
        {
            if (Session["CurrentUser"] != null)
            {
                Price price = db.Price.Find(id);
                return View(price);
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            User user = Session["CurrentUser"] as User;
            Price price = db.Price.Find(id);
            price.IsActive = false;
            price.UserId = user.Id;
            db.Entry(price).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
