using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ECourses.Models;
using System.Web.Mvc;

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


            dbef.Courses.Add(obj);
            dbef.SaveChanges();


            return true;





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
