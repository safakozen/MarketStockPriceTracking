using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Wigros.Web.Models;

namespace Wigros.Web.Controllers
{
    public class UserController : Controller
    {
        private WigrosEntities db = new WigrosEntities();

        public ActionResult MyProfile()
        {
            if (Session["CurrentUser"] != null)
            {
                //var user = db.User;
                var user = Session["CurrentUser"] as User;
                return View(user);
            }
            return RedirectToAction("Login", "Login");

        }

        [HttpPost]
        public ActionResult MyProfile(User model, HttpPostedFileBase ProfileImage)
        {
            var currentuser = Session["CurrentUser"] as User;
            User user = db.User.Where(x => x.Id == currentuser.Id).SingleOrDefault();
            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    if (ProfileImage != null)
                    {
                        user.ProfileImage = new byte[ProfileImage.ContentLength];
                        ProfileImage.InputStream.Read(user.ProfileImage, 0, ProfileImage.ContentLength);
                    }

                    user.Username = model.Username;
                    user.Fullname = model.Fullname;
                    user.Email = model.Email;
                    user.PhoneNumber = model.PhoneNumber;
                    user.Password = model.Password;

                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    Session["CurrentUser"] = user;
                    return RedirectToAction("MyProfile");
                }

            }
            return View(model);
        }

        [HttpPost]
        public ActionResult MyProf2ile(User model, HttpPostedFileBase profileImage)
        {
            var currentuser = Session["CurrentUser"] as User;
            User user = db.User.Where(x => x.Id == currentuser.Id).SingleOrDefault();
            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    if (profileImage != null)
                    {
                        model.ProfileImage = new byte[profileImage.ContentLength];
                        profileImage.InputStream.Read(model.ProfileImage, 0, profileImage.ContentLength);
                    }

                    user.Username = model.Username;
                    user.Fullname = model.Fullname;
                    user.Email = model.Email;
                    user.PhoneNumber = model.PhoneNumber;
                    user.Password = model.Password;

                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        public ActionResult Index()
        {
            if (Session["CurrentUser"] != null)
            {
                User CurrentUser = Session["CurrentUser"] as User;
                if (CurrentUser.IsAdmin==true)
                {
                    return View(db.User.ToList());
                }
            }
            return RedirectToAction("Login", "Login");
        }

        public ActionResult Create()
        {
            if (Session["CurrentUser"] != null)
            {
                User CurrentUser = Session["CurrentUser"] as User;
                if (CurrentUser.IsAdmin == true)
                {
                    return View();
                }
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                db.User.Add(user);
                db.SaveChanges();
                MailMessage message = new MailMessage();
                message.From = new MailAddress(user.Email);
                message.To.Add(user.Email);
                message.Subject = "Wigros'a Hoşgeldiniz";
                message.Body = "Sayın: " + user.Fullname+ " Sistemimize Erişim Sağlayabilirsiniz";
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("*****@gmail.com", "*****");
                smtp.EnableSsl = true;
                smtp.Send(message);
                return RedirectToAction("Index");
            }

            return View(user);
        }

        public ActionResult Edit(int? id)
        {
            if (Session["CurrentUser"] != null)
            {
                User CurrentUser = Session["CurrentUser"] as User;
                if (CurrentUser.IsAdmin == true)
                {
                    User user = db.User.Find(id);
                    return View(user);
                }
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        public ActionResult Delete(int? id)
        {
            if (Session["CurrentUser"] != null)
            {
                User CurrentUser = Session["CurrentUser"] as User;
                if (CurrentUser.IsAdmin == true)
                {
                    User user = db.User.Find(id);
                    return View(user);
                }
            }
            return RedirectToAction("Login", "Login");
            
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            User user = db.User.Find(id);
            db.User.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
