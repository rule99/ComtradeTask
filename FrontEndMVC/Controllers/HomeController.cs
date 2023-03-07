using FrontEndMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FrontEndMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

    

        public ActionResult Login() 
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserBO user)
        {
            HttpContext.Session.Timeout = 30;
            HttpContext.Session["Username"] = user.Username;
            
            HttpContext.Session["Password"] = user.Password;
           
            HttpCookie Autentifikacija;
            Autentifikacija = FormsAuthentication.GetAuthCookie(user.Username, true);
            Autentifikacija.Expires = DateTime.Now.AddHours(1);
            Response.Cookies.Add(Autentifikacija);
           
            return RedirectToAction("MainMenu");
        }

        public ActionResult MainMenu() 
        {
            return View();
        }
    }
}