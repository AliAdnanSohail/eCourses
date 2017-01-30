using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ECourses.Models;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace ECourses.Controllers
{
    public class UsersApiController : ApiController
    {
        ECoursesDBEntities dbef;

        public UsersApiController()
        {
            dbef = new ECoursesDBEntities();
        }


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/UsersApiController/SignUp")]

        public ActionResult SignUp(HttpRequestMessage value)
        {

            string v = value.Content.ReadAsStringAsync().Result;
            User obj = JsonConvert.DeserializeObject<User>(v);
            dbef.Users.Add(obj);
 
            dbef.SaveChanges();

            return new JsonResult() { Data = "success", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
         }


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/UsersApiController/SignIn")]

        public ActionResult SignIn(HttpRequestMessage value)
        {

            string v = value.Content.ReadAsStringAsync().Result;
            User obj = JsonConvert.DeserializeObject<User>(v);
            User ret = dbef.Users.Where(x => x.Email.Equals(obj.Email) && x.Password.Equals(obj.Password)).FirstOrDefault();

            if (ret != null)
            {
                return new JsonResult() { Data = ret, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            else
            {

                return new JsonResult() { Data = "Failed", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }






    }
}
