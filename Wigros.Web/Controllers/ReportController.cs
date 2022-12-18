using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Wigros.Web.Models;

namespace Wigros.Web.Controllers
{
    public class ReportController : Controller
    {
        private WigrosEntities db = new WigrosEntities();

        // GET: Report

        public ActionResult ColumnChart()
        {
            return View();
        }

        public ActionResult PieChart()
        {
            return View();
        }

        public ActionResult LineChart()
        {
            return View();
        }

        public class StudentResult
        {
            public string stdName { get; set; }
            public decimal? marksObtained { get; set; }
        }

        public ActionResult VisualizeStudentResult()
        {
            return Json(Result(), JsonRequestBehavior.AllowGet);
        }

        public List<StudentResult> Result()
        {
            List<StudentResult> stdResult = new List<StudentResult>();
            using (var context = new WigrosEntities())
            {
                stdResult = context.Price.Where(x=>x.Id<30).Select(a => new StudentResult { stdName = a.Product.Name, marksObtained = a.NewPrice }).ToList();
            }
            return stdResult;
        }

      

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(int id)
        {
            var data = db.Product.Where(x => x.Id == id).SingleOrDefault();
            var price = db.Price.Where(x => x.ProductId == data.Id).ToList();
            return View(price);
        }


        public ActionResult ExamPerformanceChart()
        {
            var getResults = (from d in db.Price
                              select d).ToList();
            List<ViewResult> result = new List<ViewResult>();
            foreach (var item in getResults)
            {
                result.Add(new ViewResult()
                {
                    Name = item.Product.Name,
                    Price = Convert.ToInt32(item.NewPrice),
                });
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public class ViewResult
        {
            public string Name { get; set; }
            public int Price { get; set; }
        }


        public ActionResult Index2()
        {
            DateTime prevMonthDate = DateTime.Today.AddMonths(-1);
            DateTime currentDate = DateTime.Now;
            ArrayList xvalue = new ArrayList();
            ArrayList yvalue = new ArrayList();
            var price = db.Price.Where(x => x.CreatedDate < currentDate && x.CreatedDate > prevMonthDate).ToList();
            price.ToList().ForEach(x => xvalue.Add(x.Product.Name));
            price.ToList().ForEach(y => yvalue.Add(y.NewPrice));
            var graphic = new Chart(width: 1500, height: 1200).AddTitle
                ("Rapor").AddSeries(chartType: "Column", name: "Rapor",
                xValue: xvalue, yValues: yvalue);

            return File(graphic.ToWebImage().GetBytes(), "image/jpeg");
        }

        //    // GET: Report/Details/5
        //    public ActionResult Details(int? id)
        //    {
        //        if (id == null)
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        }
        //        Price price = db.Price.Find(id);
        //        if (price == null)
        //        {
        //            return HttpNotFound();
        //        }
        //        return View(price);
        //    }

        //    // GET: Report/Create
        //    public ActionResult Create()
        //    {
        //        ViewBag.ProductId = new SelectList(db.Product, "Id", "Name");
        //        ViewBag.UserId = new SelectList(db.User, "Id", "Fullname");
        //        return View();
        //    }

        //    // POST: Report/Create
        //    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public ActionResult Create([Bind(Include = "Id,ProductId,OldPrice,NewPrice,IsActive,CreatedDate,UserId")] Price price)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            db.Price.Add(price);
        //            db.SaveChanges();
        //            return RedirectToAction("Index");
        //        }

        //        ViewBag.ProductId = new SelectList(db.Product, "Id", "Name", price.ProductId);
        //        ViewBag.UserId = new SelectList(db.User, "Id", "Fullname", price.UserId);
        //        return View(price);
        //    }

        //    // GET: Report/Edit/5
        //    public ActionResult Edit(int? id)
        //    {
        //        if (id == null)
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        }
        //        Price price = db.Price.Find(id);
        //        if (price == null)
        //        {
        //            return HttpNotFound();
        //        }
        //        ViewBag.ProductId = new SelectList(db.Product, "Id", "Name", price.ProductId);
        //        ViewBag.UserId = new SelectList(db.User, "Id", "Fullname", price.UserId);
        //        return View(price);
        //    }

        //    // POST: Report/Edit/5
        //    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public ActionResult Edit([Bind(Include = "Id,ProductId,OldPrice,NewPrice,IsActive,CreatedDate,UserId")] Price price)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            db.Entry(price).State = EntityState.Modified;
        //            db.SaveChanges();
        //            return RedirectToAction("Index");
        //        }
        //        ViewBag.ProductId = new SelectList(db.Product, "Id", "Name", price.ProductId);
        //        ViewBag.UserId = new SelectList(db.User, "Id", "Fullname", price.UserId);
        //        return View(price);
        //    }

        //    // GET: Report/Delete/5
        //    public ActionResult Delete(int? id)
        //    {
        //        if (id == null)
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        }
        //        Price price = db.Price.Find(id);
        //        if (price == null)
        //        {
        //            return HttpNotFound();
        //        }
        //        return View(price);
        //    }

        //    // POST: Report/Delete/5
        //    [HttpPost, ActionName("Delete")]
        //    [ValidateAntiForgeryToken]
        //    public ActionResult DeleteConfirmed(int id)
        //    {
        //        Price price = db.Price.Find(id);
        //        db.Price.Remove(price);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    protected override void Dispose(bool disposing)
        //    {
        //        if (disposing)
        //        {
        //            db.Dispose();
        //        }
        //        base.Dispose(disposing);
        //    }
        //}
    }
}
