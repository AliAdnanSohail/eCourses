using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ECourses.Models;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace ECourses.Controllers
{
    public class TeachersApiController : ApiController
    {
        ECoursesDBEntities dbef;

        TeachersApiController()
        {
            dbef = new ECoursesDBEntities();
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/TeachersApiController/GetTeacherDetails/{id}")]

        public ActionResult GetTeacherDetails(int id)
        {

            Teacher obj = dbef.Teachers.Where(x => x.Id == id).FirstOrDefault();
           

            return new JsonResult() { Data = obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/TeachersApiController/GetCoursesOfTeacher/{name}/{pass}")]

        public ActionResult GetCoursesOfTeacher(string name,string pass)
        {

            Teacher obj = dbef.Teachers.Where(x => x.User_Name.Equals(name) && x.Password.Equals(pass)).FirstOrDefault();

            if (obj != null)
            {
                List<Course> list = obj.Courses.ToList();


                return new JsonResult() { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else

                return new JsonResult() { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };



        }


        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/TeachersApiController/GetCoursesOfTeacherById/{id}")]

        public ActionResult GetCoursesOfTeacherById(int id)
        {

            Teacher obj = dbef.Teachers.Where(x => x.Id==id).FirstOrDefault();

            if (obj != null)
            {
                List<Course> list = obj.Courses.ToList();


                return new JsonResult() { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else

                return new JsonResult() { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }



        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/TeachersApiController/InsertCourse")]

        public bool InsertCourse(Course obj)
        {
            if (obj.Id == 0)
            {

                dbef.Courses.Add(obj);
                
            }
            else
            {
                Course temp = dbef.Courses.Where(x => x.Id == obj.Id).FirstOrDefault();
                temp = obj;
                

            }
            dbef.SaveChanges();

            return true;





        }




        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/TeachersApiController/LoginPost")]

        public ActionResult LoginPost(HttpRequestMessage value)

        {
            string v = value.Content.ReadAsStringAsync().Result;
            Teacher obj= JsonConvert.DeserializeObject<Teacher>(v);
            string name = obj.User_Name;
            string pass = obj.Password;
            Teacher obj1 = dbef.Teachers.Where(x => x.User_Name.Equals(name) && x.Password.Equals(pass)).FirstOrDefault();

            var list = obj1.Courses.Select(s=> new { s.Id,s.Title,s.Teacher.First_Name });


            return new JsonResult() { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
          
        }


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/TeachersApiController/GetCourseDetails")]

        public ActionResult GetCourseDetails(HttpRequestMessage value)

        {
            string v = value.Content.ReadAsStringAsync().Result;
            Course obj = JsonConvert.DeserializeObject<Course>(v);
            var ret = dbef.Courses.Where(x => x.Id == obj.Id || x.Title.Equals(obj.Title)).Select(s=> new { s.Id,s.Title,s.Address,s.Course_Type,s.Description,s.Created_At,s.Duration,s.Image,s.Likes,s.Views,s.Updated_At,s.Start_Date,s.End_Date}).FirstOrDefault();
           


            return new JsonResult() { Data = ret, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }




        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/TeachersApiController/getAllTeachers")]

        public ActionResult getAllTeachers()

        {

            List<Teacher> obj=dbef.Teachers.Where(x=>x.Id!=0).ToList();


            return new JsonResult() { Data = obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }



        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/TeachersApiController/Login/{name}/{pass}")]

        public ActionResult Login(string name, string pass)

        {
            Teacher obj = dbef.Teachers.Where(x => x.User_Name.Equals(name) && x.Password.Equals(pass)).FirstOrDefault();
            


            return new JsonResult() { Data = obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

    }
}
