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
    public class ProductController : Controller
    {
        private WigrosEntities db = new WigrosEntities();

        public ActionResult Index()
        {
            if (Session["CurrentUser"] != null)
            {
                var product = db.Product.ToList();
                var productPrice = db.Price.ToList();

                ProductPrice vm = new ProductPrice()
                {
                    Product = product,
                    Price = productPrice
                };
                return View(vm);
            }
            return RedirectToAction("Login", "Login");
        }

        public ActionResult Create()
        {
            if (Session["CurrentUser"] != null)
            {
                ViewBag.BrandId = new SelectList(db.Brand.Where(x=>x.IsActive==true), "Id", "Name");
                ViewBag.CategoryId = new SelectList(db.Category.Where(x => x.IsActive == true), "Id", "Name");
                ViewBag.UserId = new SelectList(db.User, "Id", "Fullname");
                return View();
            }
            return RedirectToAction("Login", "Login");

        }

        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase productImage)
        {
            if (ModelState.IsValid)
            {
                if (productImage != null)
                {
                    string filename = $"{product.Name}.{productImage.ContentType.Split('/')[1]}";
                    productImage.SaveAs(Server.MapPath($"~/assets/img/product/{filename}"));
                    product.ImagePath = filename;
                }

                User user = Session["CurrentUser"] as User;
                product.IsDelete = false;
                product.IsActive = true;
                product.CreatedDate = DateTime.Now;
                product.UserId = user.Id;
                db.Product.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BrandId = new SelectList(db.Brand.Where(x => x.IsActive == true), "Id", "Name", product.BrandId);
            ViewBag.CategoryId = new SelectList(db.Category.Where(x => x.IsActive == true), "Id", "Name", product.CategoryId);
            ViewBag.UserId = new SelectList(db.User, "Id", "Fullname", product.UserId);
            return View(product);
        }

        public ActionResult Edit(int? id)
        {
            if (Session["CurrentUser"] != null)
            {
                Product product = db.Product.Find(id);
                ViewBag.BrandId = new SelectList(db.Brand.Where(x => x.IsActive == true), "Id", "Name", product.BrandId);
                ViewBag.CategoryId = new SelectList(db.Category.Where(x => x.IsActive == true), "Id", "Name", product.CategoryId);
                ViewBag.UserId = new SelectList(db.User, "Id", "Fullname", product.UserId);
                return View(product);
            }
            return RedirectToAction("Login", "Login");
        }


        [HttpPost]
        public ActionResult Edit(Product product, HttpPostedFileBase productImage)
        {
            if (ModelState.IsValid)
            {
                User user = Session["CurrentUser"] as User;
                if (productImage != null)
                {
                    string filename = $"{product.Id}_{product.Name}.{productImage.ContentType.Split('/')[1]}";
                    productImage.SaveAs(Server.MapPath($"~/assets/img/product/{filename}"));
                    product.ImagePath = filename;
                }

                product.UserId = user.Id;
                product.IsActive = true;
                product.IsDelete = false;

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BrandId = new SelectList(db.Brand.Where(x => x.IsActive == true), "Id", "Name", product.BrandId);
            ViewBag.CategoryId = new SelectList(db.Category.Where(x => x.IsActive == true), "Id", "Name", product.CategoryId);
            ViewBag.UserId = new SelectList(db.User, "Id", "Fullname", product.UserId);
            return View(product);
        }

        public ActionResult Delete(int? id)
        {
            if (Session["CurrentUser"] != null)
            {
                Product product = db.Product.Find(id);
                return View(product);
            }
            return RedirectToAction("Login", "Login");

        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            User user = Session["CurrentUser"] as User;
            Product product = db.Product.Find(id);
            product.IsDelete = true;
            product.UserId = user.Id;
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
