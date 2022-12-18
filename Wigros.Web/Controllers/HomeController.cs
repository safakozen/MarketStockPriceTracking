using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wigros.Web.Models;

namespace Wigros.Web.Controllers
{
    public class HomeController : Controller
    {
        WigrosEntities db = new WigrosEntities();

        public ActionResult Index()
        {
            if (Session["CurrentUser"] != null)
            {
                ViewBag.ToplamUrun = db.Product.Where(x => x.IsActive == true).Count();
                ViewBag.ToplamKategori = db.Category.Where(x => x.IsActive == true).Count();
                ViewBag.ToplamMarka = db.Brand.Where(x => x.IsActive == true).Count();
                return View();
            }
            return RedirectToAction("Login", "Login");
        }

        public class ProductReport
        {
            public string ProductName { get; set; }
            public decimal? ProductPrice { get; set; }
        }

        public ActionResult ProductReportResult()
        {
            return Json(Result(), JsonRequestBehavior.AllowGet);
        }

        public List<ProductReport> Result()
        {
            List<ProductReport> productResult = new List<ProductReport>();
            using (var context = new WigrosEntities())
            {
                productResult = context.Price.OrderByDescending(x => x.CreatedDate).Select(a => new ProductReport { ProductName = a.Product.Name, ProductPrice = a.NewPrice }).Skip(0).Take(10).ToList();
            }
            return productResult;
        }
       
    }
}