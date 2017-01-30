using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECourses.Models;

namespace ECourses.Controllers
{
    public class HomeController : Controller
    {
        ECoursesDBEntities dbef = new ECoursesDBEntities();
        // GET: Home
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Teachers");
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string name, string password)
        {

            Admin obj = dbef.Admins.FirstOrDefault();

            if (name.Equals(obj.User_Name) && password.Equals(obj.Password))
            {
                Session["admin"] = name;

                return RedirectToAction("Index","Teachers");
            }

            else
            {
                return RedirectToAction("Login");
            }



        }
        [HttpGet]
        public new ActionResult Profile()
        {
            ECoursesDBEntities dbef = new ECoursesDBEntities();
            Admin obj = dbef.Admins.FirstOrDefault();
            return View(obj);
        }

        [HttpPost]
        public new ActionResult Profile(Admin obj)
        {

            ECoursesDBEntities dbef = new ECoursesDBEntities();

            Admin obj1 = dbef.Admins.Find(1);
            obj1.User_Name = obj.User_Name;
            obj1.Password = obj.Password;

            dbef.SaveChanges();

            return RedirectToAction("Index", "Teachers");


        }




        public ActionResult SignOut()
        {
            Session["admin"] = null;
            return View("Login");
        }

    }


}