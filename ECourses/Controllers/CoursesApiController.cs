using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ECourses.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Mvc;

namespace ECourses.Controllers
{
    public class CoursesApiController : ApiController
    {
        ECoursesDBEntities dbef;

        public CoursesApiController()
        {
            dbef = new ECoursesDBEntities();
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/CoursesApiController/Recent")]

        public ActionResult Recent()
        {


            var ret = dbef.Courses.Select(s => new { s.Id, s.Title, s.Image, s.Likes, s.Views, s.Teacher.First_Name, s.Address ,s.Created_At}).OrderByDescending(x => x.Created_At);

            return new JsonResult() { Data = ret, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/CoursesApiController/MostViewed")]

        public ActionResult MostViewed()
        {


            var ret = dbef.Courses.Select(s => new { s.Id, s.Title, s.Image, s.Likes, s.Views, s.Teacher.First_Name, s.Address }).OrderByDescending(x => x.Views);

            return new JsonResult() { Data = ret, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/CoursesApiController/TopCourses")]

        public ActionResult TopCourses()
        {


            var ret = dbef.Courses.Select(s => new { s.Id, s.Title, s.Image, s.Likes, s.Views, s.Teacher.First_Name, s.Address }).OrderByDescending(x => x.Likes);

            return new JsonResult() { Data = ret, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/CoursesApiController/Favourite/{id}")]

        public ActionResult Favourite(int id)
        {
            List<Favourite_Courses> r = dbef.Favourite_Courses.Where(x => x.User_Id == id).ToList();

            List<Course> list = new List<Course>();

            foreach (var x in r)
            {
                list.Add(dbef.Courses.Where(a => a.Id == x.Course_Id).FirstOrDefault());
            }

            var ret = list.Select(s => new { s.Id, s.Title });



            return new JsonResult() { Data = ret, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/CoursesApiController/Search/{Type}/{Address}")]

        public ActionResult Search(string type, string address)
        {
            List<Course> list = dbef.Courses.ToList();
            City city = dbef.Cities.Where(x => x.Name.Equals(address)).FirstOrDefault();
            Course_Type ct = dbef.Course_Type.Where(x => x.Course_Type1.Equals(type)).FirstOrDefault();
            if (city != null && ct == null)
            {
                list = dbef.Courses.Where(x => x.Id == -1).ToList();
            }
            if (ct != null && city == null)
            {
                list = dbef.Courses.Where(x => x.Id == -1).ToList();
            }
            if (ct != null && city != null)
            {
                list = dbef.Courses.Where(x => x.Course_Type_id == ct.Id && x.City_id == city.Id).ToList();
            }

            var ret = list.Select(s => new { s.Id, s.Title ,s.Image,s.Likes,s.Views,s.Teacher.First_Name,s.Address});


            return new JsonResult() { Data = ret, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/CoursesApiController/Show/{id}")]

        public ActionResult Show(int id)
        {

            var ret = dbef.Courses.Where(x => x.Id == id).Select(s => new {s.Id, s.Title, s.Image, s.Likes, s.Address, s.Duration, s.Description, s.Gender, s.Price, s.Start_Date, s.Teacher.First_Name, s.Views,s.Course_Type.Course_Type1 });

            return new JsonResult() { Data = ret, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/CoursesApiController/makeFavourite")]

        public ActionResult makeFavourite(HttpRequestMessage value)
        {

            string v = value.Content.ReadAsStringAsync().Result;
            Favourite_Courses obj = JsonConvert.DeserializeObject<Favourite_Courses>(v);

            dbef.Favourite_Courses.Add(obj);
            dbef.SaveChanges();

            if (obj != null)
            {
                return new JsonResult() { Data = "Success", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                return new JsonResult() { Data = "Failed", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }



        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/CoursesApiController/LikeCourse/{id}")]

        public ActionResult LikeCourse(int id)
        {

            Course obj = dbef.Courses.Where(x => x.Id == id).FirstOrDefault();
            if (obj != null)
            {
                obj.Likes = obj.Likes + 1;
                dbef.SaveChanges();
                return new JsonResult() { Data = "Success", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            else
            {

                return new JsonResult() { Data = "Failed", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/CoursesApiController/ViewCourse/{id}")]

        public ActionResult ViewCourse(int id)
        {

            Course obj = dbef.Courses.Where(x => x.Id == id).FirstOrDefault();
            if (obj != null)
            {
                obj.Likes = obj.Views + 1;
                dbef.SaveChanges();
                return new JsonResult() { Data = "Success", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            else
            {

                return new JsonResult() { Data = "Failed", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/CoursesApiController/subscribeCourse")]

        public ActionResult subscribeCourse(HttpRequestMessage value)
        {

            string v = value.Content.ReadAsStringAsync().Result;
            Subscription_Courses obj = JsonConvert.DeserializeObject<Subscription_Courses>(v);

            dbef.Subscription_Courses.Add(obj);
            dbef.SaveChanges();

            if (obj != null)
            {
                return new JsonResult() { Data = "Success", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                return new JsonResult() { Data = "Failed", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }



        }




        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/CoursesApiController/GetCities")]

        public ActionResult GetCities()
        {
            var city = dbef.Cities.Where(x => x.Id != 0).Select(s=>new {s.Id,s.Name }).ToList();
            //var type = dbef.Course_Type.Where(x => x.Id != 0).Select(s => new {s.Id, s.Course_Type1 });

            //var obj = city;
           
            
          
          
            return new JsonResult() { Data = city, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/CoursesApiController/GetTypes")]

        public ActionResult GetTypes()
        {
           // var city = dbef.Cities.Where(x => x.Id != 0).Select(s => new { s.Id, s.Name }).ToList();
            var type = dbef.Course_Type.Where(x => x.Id != 0).Select(s => new {s.Id, s.Course_Type1 });

           



            return new JsonResult() { Data = type, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

    }
}
