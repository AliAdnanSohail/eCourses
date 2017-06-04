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
        ImageUpload fcm = new ImageUpload();

        public TeachersApiController()
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

        public ActionResult GetCoursesOfTeacher(string name, string pass)
        {

            Teacher obj = dbef.Teachers.Where(x => x.User_Name.Equals(name) && x.Password.Equals(pass)).FirstOrDefault();

            if (obj != null)
            {
                List<Course> list = obj.Courses.ToList();



                return new JsonResult() { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else

                return new JsonResult() { Data = "Not Found", JsonRequestBehavior = JsonRequestBehavior.AllowGet };



        }


        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/TeachersApiController/GetCoursesOfTeacherById/{id}")]

        public ActionResult GetCoursesOfTeacherById(int id)
        {

            Teacher obj = dbef.Teachers.Where(x => x.Id == id).FirstOrDefault();

            if (obj != null)
            {
                var list = obj.Courses.Where(x=>x.Teacher_Id==id).Select(s=> new { s.Id,s.Title});


                return new JsonResult() { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else

                return new JsonResult() { Data = "No Courses", JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/TeachersApiController/AboutUs")]

        public ActionResult AboutUs()
        {

            About_Us obj = dbef.About_Us.FirstOrDefault();


                return new JsonResult() { Data = obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
           


        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/TeachersApiController/Advertisement")]

        public ActionResult Advertisement()
        {

            var obj = dbef.Advertisements.Select(s=>new { s.Title,s.Description,s.Phone_Num,s.Image}).FirstOrDefault();
            


            return new JsonResult() { Data = obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet };



        }



        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/TeachersApiController/InsertCourse")]

        public ActionResult InsertCourse(HttpRequestMessage value)
        {

            string v = value.Content.ReadAsStringAsync().Result;
            Course obj = JsonConvert.DeserializeObject<Course>(v);


            if (obj.Id == 0)
            {

                dbef.Courses.Add(obj);

            }
            else
            {
                Course temp = dbef.Courses.Where(x => x.Id == obj.Id).FirstOrDefault();
                temp.Title = obj.Title;
                temp.Price = obj.Price;
                temp.Image = obj.Image;
                temp.Address = obj.Address;
               
                
                temp.Description = obj.Description;
                temp.Duration = obj.Duration;
                if (obj.End_Date != null)
                {
                    temp.End_Date = obj.End_Date;
                }
                temp.Gender = obj.Gender;
                if (obj.Start_Date != null)
                {
                    temp.Start_Date = obj.Start_Date;
                }
                temp.Updated_At = DateTime.Now;


                var list = dbef.Subscription_Courses.Where(x => x.Course_Id == obj.Id).ToList();
                foreach(var x in list)
                {
                    fcm.sendFCMtoStudent(x.User.Device_id, x.Course.Title, x.Course.Teacher.First_Name);

                }
                
                


            }
            dbef.SaveChanges();

            return new JsonResult() { Data = true, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }




        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/TeachersApiController/LoginPost")]

        public ActionResult LoginPost(HttpRequestMessage value)

        {
            string v = value.Content.ReadAsStringAsync().Result;
            Teacher obj = JsonConvert.DeserializeObject<Teacher>(v);
            string name = obj.User_Name;
            string pass = obj.Password;
            Teacher obj1 = dbef.Teachers.Where(x => x.User_Name.Equals(name) && x.Password.Equals(pass)).FirstOrDefault();
            var ret= dbef.Teachers.Where(x => x.User_Name.Equals(name) && x.Password.Equals(pass)).Select(s=>new {s.Id,s.Image,s.First_Name }).FirstOrDefault();
            if (obj1 != null)
            {
                var list = obj1.Courses.Select(s => new { s.Id, s.Title, s.Teacher.First_Name, s.Teacher_Id, s.Teacher.Image });
                Course c = obj1.Courses.FirstOrDefault();
                if (c==null)
                {
                    return new JsonResult() { Data = ret, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {

                    //   return new JsonResult() { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    return new JsonResult() { Data = ret, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                }
            }
            else
            {
                return new JsonResult() { Data = "Failed", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }


        //[System.Web.Http.HttpPost]
        //[System.Web.Http.Route("api/TeachersApiController/GetCourseDetails")]

        //public ActionResult GetCourseDetails(HttpRequestMessage value)

        //{
        //    string v = value.Content.ReadAsStringAsync().Result;
        //    Course obj = JsonConvert.DeserializeObject<Course>(v);
        //    var ret = dbef.Courses.Where(x => x.Id == obj.Id).Select(s => new { s.Id, s.Title, s.Address, s.Description,s.Created_At, s.Duration, s.Image, s.Gender, s.Price, s.Likes, s.Views, s.Updated_At,s.Start_Date,s.End_Date }).FirstOrDefault();

        //    if (ret != null)

        //    {
        //        return new JsonResult() { Data = ret, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //    }

        //    else
        //    {
        //        return new JsonResult() { Data = "Not found", JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        //    }

        //}

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/TeachersApiController/GetCourseDetails/{id}")]

        public ActionResult GetCourseDetails(int id)

        {
            
            
            var ret = dbef.Courses.Where(x => x.Id == id).Select(s => new { s.Id, s.Title, s.Address,s.Course_Type.Course_Type1, s.Description, s.Created_At, s.Duration, s.Image, s.Gender, s.Price, s.Likes, s.Views, s.Updated_At, s.Start_Date, s.End_Date }).FirstOrDefault();

            if (ret != null)

            {
                return new JsonResult() { Data = ret, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            else
            {
                return new JsonResult() { Data = "Not found", JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }

        }







        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/TeachersApiController/getAllTeachers")]

        public ActionResult getAllTeachers()

        {

            List<Teacher> obj = dbef.Teachers.Where(x => x.Id != 0).ToList();


            return new JsonResult() { Data = obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }



        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/TeachersApiController/Login/{name}/{pass}")]

        public ActionResult Login(string name, string pass)

        {
            Teacher obj = dbef.Teachers.Where(x => x.User_Name.Equals(name) && x.Password.Equals(pass)).FirstOrDefault();



            return new JsonResult() { Data = obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }




        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/TeachersApiController/GetCoursesNameofTeacher")]

        public ActionResult GetCoursesNameofTeacher(HttpRequestMessage value)

        {
            string v = value.Content.ReadAsStringAsync().Result;
            Teacher obj = JsonConvert.DeserializeObject<Teacher>(v);
            var ret = dbef.Courses.Where(x => x.Teacher_Id == obj.Id).Select(s => new { s.Id, s.Title }).ToList();

            if (ret != null)

            {
                return new JsonResult() { Data = ret, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            else
            {
                return new JsonResult() { Data = "No Courses", JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }

        }


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/TeachersApiController/SetToken")]

        public ActionResult SetToken(HttpRequestMessage value)

        {
            string v = value.Content.ReadAsStringAsync().Result;
            Teacher obj = JsonConvert.DeserializeObject<Teacher>(v);

            Teacher teacher = dbef.Teachers.Find(obj.Id);
            teacher.Device_id = obj.Device_id;
            dbef.SaveChanges();


            
                return new JsonResult() { Data = "Success", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
           

        }

    }
}
