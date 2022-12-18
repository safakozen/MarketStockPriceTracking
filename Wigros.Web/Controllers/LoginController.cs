using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wigros.Web.Models;

namespace Wigros.Web.Controllers
{
    public class LoginController : Controller
    {
        WigrosEntities db = new WigrosEntities();
        public ActionResult Login()
        {
            if (Session["CurrentUser"] == null)
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            var data = db.User.Where(x => x.Username == user.Username && x.Password == user.Password && x.IsActive == true).SingleOrDefault(); ;
            if (data != null)
            {
                Session["CurrentUser"] = data;
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

     

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login","Login");
        }
    }

}