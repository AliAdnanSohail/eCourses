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
using System.Web.Script.Serialization;
using System.Text;
using System.IO;

namespace ECourses.Controllers
{
    public class CoursesApiController : ApiController
    {
        ECoursesDBEntities dbef;
        ImageUpload fcm = new ImageUpload();

        public CoursesApiController()
        {
            dbef = new ECoursesDBEntities();
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/CoursesApiController/Recent/{user_id}")]

        public ActionResult Recent(int user_id)
        {

         var ret = dbef.Courses.Select(s => new { s.Id, s.Title, s.Image, s.Likes, s.Views, s.Teacher.First_Name, s.Address ,s.Created_At}).OrderByDescending(x => x.Created_At).ToList();
            List<LikedCourse> list = ret.Select(t => new LikedCourse(t.Id, t.Title, t.Likes, t.Views, t.First_Name, t.Image, t.Address, t.Created_At, false)).ToList();



            foreach (LikedCourse x in list)
            {
                Like_Courses obj = dbef.Like_Courses.Where(q => q.Course_Id == x.Id && q.User_Id == user_id).FirstOrDefault();

                if (obj != null)

                {
                    x.isLiked = true;

                }
            }

            return new JsonResult() { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/CoursesApiController/MostViewed/{user_id}")]

        public ActionResult MostViewed(int user_id)
        {


            var ret = dbef.Courses.Select(s => new { s.Id, s.Title, s.Image, s.Likes, s.Views, s.Teacher.First_Name, s.Address }).OrderByDescending(x => x.Views).ToList();

            List<LikedCourse> list = ret.Select(t => new LikedCourse(t.Id, t.Title, t.Likes, t.Views, t.First_Name, t.Image, t.Address, false)).ToList();



            foreach (LikedCourse x in list)
            {
                Like_Courses obj = dbef.Like_Courses.Where(q => q.Course_Id == x.Id && q.User_Id == user_id).FirstOrDefault();

                if (obj != null)

                {
                    x.isLiked = true;

                }
            }

            return new JsonResult() { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/CoursesApiController/TopCourses/{user_id}")]

        public ActionResult TopCourses(int user_id)
        {


            var ret = dbef.Courses.Select(s => new { s.Id, s.Title, s.Image, s.Likes, s.Views, s.Teacher.First_Name, s.Address }).OrderByDescending(x => x.Likes).ToList();

            List<LikedCourse> list = ret.Select(t => new LikedCourse(t.Id, t.Title, t.Likes, t.Views, t.First_Name, t.Image, t.Address, false)).ToList();



            foreach (LikedCourse x in list)
            {
                Like_Courses obj = dbef.Like_Courses.Where(q => q.Course_Id == x.Id && q.User_Id == user_id).FirstOrDefault();

                if (obj != null)

                {
                    x.isLiked = true;

                }
            }

            return new JsonResult() { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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

            var ret = list.Select(s => new { s.Id, s.Title, s.Image, s.Likes, s.Views, s.Teacher.First_Name, s.Address });

            List<LikedCourse> list2 = ret.Select(t => new LikedCourse(t.Id, t.Title, t.Likes, t.Views, t.First_Name, t.Image, t.Address, false)).ToList();



            foreach (LikedCourse x in list2)
            {
                Like_Courses obj = dbef.Like_Courses.Where(q => q.Course_Id == x.Id && q.User_Id == id).FirstOrDefault();

                if (obj != null)

                {
                    x.isLiked = true;

                }
            }



            return new JsonResult() { Data = list2, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/CoursesApiController/Search")]

        public ActionResult Search(CourseAndCity obj)
        {
    
            List<Course> list = dbef.Courses.ToList();
            City city = dbef.Cities.Where(x => x.Name.Equals(obj.city)).FirstOrDefault();
            Course_Type ct = dbef.Course_Type.Where(x => x.Course_Type1.Equals(obj.course_type)).FirstOrDefault();
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

            var ret = list.Select(s => new { s.Id, s.Title, s.Image, s.Likes, s.Views, s.Teacher.First_Name, s.Address });


            return new JsonResult() { Data = ret, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/CoursesApiController/SearchByType")]

        public ActionResult SearchByType(CourseAndCity obj)
        {
            List<Course> list = dbef.Courses.ToList();
      
            Course_Type ct = dbef.Course_Type.Where(x => x.Course_Type1.Equals(obj.course_type)).FirstOrDefault();
          
            if (ct != null )
            {
                list = dbef.Courses.Where(x => x.Course_Type_id == ct.Id).ToList();
               var ret = list.Select(s => new { s.Id, s.Title, s.Image, s.Likes, s.Views, s.Teacher.First_Name, s.Address });
                return new JsonResult() { Data = ret, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            else
            {

                return new JsonResult() { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

          


           
        }
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/CoursesApiController/SearchByCity")]

        public ActionResult SearchByCity(CourseAndCity obj)
        {
            List<Course> list = dbef.Courses.ToList();

            City cityret = dbef.Cities.Where(x => x.Name.Equals(obj.city)).FirstOrDefault();

            if (cityret != null)
            {
                list = dbef.Courses.Where(x => x.City_id == cityret.Id).ToList();
                var ret = list.Select(s => new { s.Id, s.Title, s.Image, s.Likes, s.Views, s.Teacher.First_Name, s.Address });
                return new JsonResult() { Data = ret, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            else
            {

                return new JsonResult() { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }





        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/CoursesApiController/Show/{id}/{user_id}")]

        public ActionResult Show(int id,int user_id)
        {
            bool isSubcribe = false;
            bool isFavourite = false;
            bool isLiked = false;
            Subscription_Courses obj = dbef.Subscription_Courses.Where(x => x.Course_Id == id && x.User_Id == user_id).FirstOrDefault();
            if(obj!=null)
            {
                isSubcribe = true;
            }
            Favourite_Courses obj1 = dbef.Favourite_Courses.Where(x => x.Course_Id == id && x.User_Id == user_id).FirstOrDefault();
            if (obj1 != null)
            {
                isFavourite = true;
            }
            Like_Courses obj2= dbef.Like_Courses.Where(x => x.Course_Id == id && x.User_Id == user_id).FirstOrDefault();
            if (obj2 != null)
            {
                isLiked = true;
            }

            var ret = dbef.Courses.Where(x => x.Id == id).Select(s => new {s.Id, s.Title, s.Image, s.Likes, s.Address, s.Duration, s.Description, s.Gender, s.Price, s.Start_Date, s.Teacher.First_Name, s.Views,s.Course_Type.Course_Type1 ,s.Teacher.Phone});

            var retur = new { course = ret, IsFavourite = isFavourite, IsSubscribed = isSubcribe,IsLiked=isLiked };

            return new JsonResult() { Data = retur, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/CoursesApiController/RemoveFavourite")]

        public ActionResult RemoveFavourite(HttpRequestMessage value)
        {

            string v = value.Content.ReadAsStringAsync().Result;
            Favourite_Courses obj = JsonConvert.DeserializeObject<Favourite_Courses>(v);

            Favourite_Courses y =  dbef.Favourite_Courses.Where(x=>x.User_Id==obj.User_Id && x.Course_Id==obj.Course_Id).FirstOrDefault();


            if (y == null)
            {
                return new JsonResult() { Data = "Failed , Not Favourite", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                dbef.Favourite_Courses.Remove(y);
                dbef.SaveChanges();
                return new JsonResult() { Data = "Success", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                
            }



        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/CoursesApiController/CheckFavourite")]

        public ActionResult CheckFavourite(HttpRequestMessage value)
        {

            string v = value.Content.ReadAsStringAsync().Result;
            Favourite_Courses obj = JsonConvert.DeserializeObject<Favourite_Courses>(v);

            Favourite_Courses y = dbef.Favourite_Courses.Where(x => x.Course_Id == obj.Course_Id && x.User_Id == obj.User_Id).FirstOrDefault();

            if (y == null)
            {
                return new JsonResult() { Data = "false", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {          
                return new JsonResult() { Data = "true", JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }



        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/CoursesApiController/LikeCourse")]

        public ActionResult LikeCourse(HttpRequestMessage value)
        {

            string v = value.Content.ReadAsStringAsync().Result;
            Like_Courses obj = JsonConvert.DeserializeObject<Like_Courses>(v);
           
           
            dbef.Like_Courses.Add(obj);
            dbef.SaveChanges();

            Course course_obj = dbef.Courses.Where(x => x.Id == obj.Course_Id).FirstOrDefault();
            if (obj != null)
            {
                course_obj.Likes = course_obj.Likes + 1;
                dbef.SaveChanges();
                return new JsonResult() { Data = "Success", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            else
            {

                return new JsonResult() { Data = "Failed", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/CoursesApiController/UnlikeCourse")]

        public ActionResult UnlikeCourse(HttpRequestMessage value)
        {

            string v = value.Content.ReadAsStringAsync().Result;
            Like_Courses obj = JsonConvert.DeserializeObject<Like_Courses>(v);

            Like_Courses toremove = dbef.Like_Courses.Where(x => x.Course_Id == obj.Course_Id && x.User_Id == obj.User_Id).FirstOrDefault();
            if(toremove!=null)
            {
                dbef.Like_Courses.Remove(toremove);
                dbef.SaveChanges();
            }

            Course course_obj = dbef.Courses.Where(x => x.Id == obj.Course_Id).FirstOrDefault();
            if (obj != null)
            {
                course_obj.Likes = course_obj.Likes - 1;
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
                obj.Views = obj.Views + 1;
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

            Course c = dbef.Courses.Where(x => x.Id == obj.Course_Id).FirstOrDefault();
            Teacher t = dbef.Teachers.Where(x => x.Id == c.Teacher_Id).FirstOrDefault();
            User u = dbef.Users.Where(x => x.Id == obj.User_Id).FirstOrDefault();

            fcm.sendFCMtoTeacher(t.Device_id, c.Title, u.Name);

            if (obj != null)
            {
                return new JsonResult() { Data = "Success", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                return new JsonResult() { Data = "Failed", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }



        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/CoursesApiController/UnsubscribeCourse")]

        public ActionResult UnsubscribeCourse(HttpRequestMessage value)
        {

            string v = value.Content.ReadAsStringAsync().Result;
            Subscription_Courses obj = JsonConvert.DeserializeObject<Subscription_Courses>(v);

            Subscription_Courses y = dbef.Subscription_Courses.Where(x => x.User_Id == obj.User_Id && x.Course_Id == obj.Course_Id).FirstOrDefault();


            if (y == null)
            {
                return new JsonResult() { Data = "Failed , Not subscribed", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                dbef.Subscription_Courses.Remove(y);
                dbef.SaveChanges();
                return new JsonResult() { Data = "Success", JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }



        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/CoursesApiController/CheckSubscription")]

        public ActionResult CheckSubscription(HttpRequestMessage value)
        {

            string v = value.Content.ReadAsStringAsync().Result;
            Subscription_Courses obj = JsonConvert.DeserializeObject<Subscription_Courses>(v);

            Subscription_Courses y = dbef.Subscription_Courses.Where(x => x.User_Id == obj.User_Id && x.Course_Id == obj.Course_Id).FirstOrDefault();


            if (y == null)
            {
                return new JsonResult() { Data = "false", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
               
                return new JsonResult() { Data = "true", JsonRequestBehavior = JsonRequestBehavior.AllowGet };

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
        [System.Web.Http.Route("api/CoursesApiController/GetCitiesAndTypes")]

        public ActionResult GetCitiesAndTypes()
        {
            var city = dbef.Cities.Where(x => x.Id != 0).Select(s => new { s.Id, s.Name }).ToList();
            var type = dbef.Course_Type.Where(x => x.Id != 0).Select(s => new {s.Id, s.Course_Type1 });

            var obj = new { CitySend = city, TypeSend = type };




            return new JsonResult() { Data =obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/CoursesApiController/GetTypes")]

        public ActionResult GetTypes()
        {
           // var city = dbef.Cities.Where(x => x.Id != 0).Select(s => new { s.Id, s.Name }).ToList();
            var type = dbef.Course_Type.Where(x => x.Id != 0).Select(s => new {s.Id, s.Course_Type1 });


            return new JsonResult() { Data = type, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        //[System.Web.Http.HttpGet]
        //[System.Web.Http.Route("api/CoursesApiController/GetCourse")]

        //public ActionResult GetCourse()
        //{
        //    int user_id=1;
         
        //     var ret = dbef.Courses.Select(s => new { s.Id, s.Title, s.Image, s.Likes, s.Views, s.Teacher.First_Name, s.Address, s.Created_At,isLiked=false }).OrderByDescending(x => x.Created_At).ToList();
        //    List<LikedCourse> list = ret.Select(t => new LikedCourse(t.Id, t.Title,t.Likes,t.Views, t.First_Name, t.Image, t.Address,t.Created_At,false)).ToList();

            
           
        //    foreach(LikedCourse x in list)
        //    {
        //        Like_Courses obj = dbef.Like_Courses.Where(q => q.Course_Id == x.Id && q.User_Id == user_id).FirstOrDefault();

        //        if(obj!=null)

        //        {
        //            x.isLiked = true;
     
        //        }
        //    }
        //    sendFCMtoTeacher("1","2","English","Ali");
            

        //    return new JsonResult() { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        //}

     

    }

    }
