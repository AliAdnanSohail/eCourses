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
        ImageUpload imageUpload = new ImageUpload();
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
            if(Session["admin"]==null)
            {
                return RedirectToAction("Login", "Home");
            }
            ECoursesDBEntities dbef = new ECoursesDBEntities();
            Admin obj = dbef.Admins.FirstOrDefault();
            return View(obj);
        }

        [HttpPost]
        public new ActionResult Profile(Admin obj)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            ECoursesDBEntities dbef = new ECoursesDBEntities();

            Admin obj1 = dbef.Admins.Find(1);
            obj1.User_Name = obj.User_Name;
            obj1.Password = obj.Password;

            dbef.SaveChanges();

            return RedirectToAction("Index", "Teachers");


        }

        [HttpGet]
        public ActionResult About_Us()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            ECoursesDBEntities dbef = new ECoursesDBEntities();
            About_Us obj = dbef.About_Us.FirstOrDefault();

            if (obj == null)
            {
                About_Us obj2 = new Models.About_Us();
                obj2.Title = "Enter Title";
               
                obj2.Description = "Enter Description";
                return View(obj2);

            }
            return View(obj);
        }

        [HttpPost]
        public ActionResult About_Us(About_Us obj, HttpPostedFileBase file)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            ECoursesDBEntities dbef = new ECoursesDBEntities();

            About_Us obj1 = dbef.About_Us.Find(1);

            string image = "";

            if (file != null)
            {
              

                byte[] fileBytes = new byte[file.ContentLength];
                file.InputStream.Read(fileBytes, 0, fileBytes.Length);
                file.InputStream.Close();
                string fileContent = Convert.ToBase64String(fileBytes);
                image = imageUpload.Upload(fileContent);


            }
            if (obj1 == null)
            {
                About_Us abc = new Models.About_Us();
                abc.Title = obj.Title;
                abc.Description = obj.Description;
                abc.Image = image;
                

                dbef.About_Us.Add(abc);

            }
            else
            {
                obj1.Title = obj.Title;
                obj1.Description = obj.Description;
                obj1.Image = image;
            }

            dbef.SaveChanges();

            return RedirectToAction("Index", "Teachers");


        }

        [HttpGet]
        public ActionResult Advertisement()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            ECoursesDBEntities dbef = new ECoursesDBEntities();
            Advertisement obj = dbef.Advertisements.FirstOrDefault();
            if(obj==null)
            {
                Advertisement obj2 = new Models.Advertisement();
                obj2.Title = "Enter Title";
                obj2.Phone_Num = "Enter Phone number";
                obj2.Description = "Enter Description";
                obj2.Image = "http://www.ccjk.com/wp-content/uploads/2013/02/13884066-pixeled-word-advertisement-on-digital-screen-3d-render.jpg";
                return View(obj2);

            }
            return View(obj);
        }

        [HttpPost]
        public ActionResult Advertisement(Advertisement obj, HttpPostedFileBase file)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            ECoursesDBEntities dbef = new ECoursesDBEntities();

            Advertisement obj1 = dbef.Advertisements.Find(1);

            string image = "";

            if (file != null)
            {


                byte[] fileBytes = new byte[file.ContentLength];
                file.InputStream.Read(fileBytes, 0, fileBytes.Length);
                file.InputStream.Close();
                string fileContent = Convert.ToBase64String(fileBytes);
                image = imageUpload.Upload(fileContent);


            }
            if (obj1 == null)
            {
                Advertisement abc = new Models.Advertisement();
                abc.Title = obj.Title;
                abc.Description = obj.Description;
                abc.Image = image;
                abc.Phone_Num = obj.Phone_Num;

                dbef.Advertisements.Add(abc);

            }
            else
            {
                obj1.Title = obj.Title;
                obj1.Description = obj.Description;
                obj1.Image = image;
                obj1.Phone_Num = obj.Phone_Num;
            }

            dbef.SaveChanges();

            return RedirectToAction("Index", "Teachers");


        }





        public ActionResult SignOut()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            Session["admin"] = null;
            return View("Login");
        }

        public ActionResult Privacy()
        {
          
    
            return View();
        }


    }


}