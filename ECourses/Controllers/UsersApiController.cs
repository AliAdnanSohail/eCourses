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

            User check = dbef.Users.Where(x => x.Email == obj.Email).FirstOrDefault();

            if (check != null)
            {
                var ret = new { Status = "Email Already Exist", User_id = check.Id };
                return new JsonResult() { Data = ret , JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {

                dbef.Users.Add(obj);

                dbef.SaveChanges();

                var ret = new { Status = "success", User_id = obj.Id };

                return new JsonResult() { Data = ret, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
         }


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/UsersApiController/SignIn")]

        public ActionResult SignIn(HttpRequestMessage value)
        {

            string v = value.Content.ReadAsStringAsync().Result;
            User obj = JsonConvert.DeserializeObject<User>(v);
            var ret = dbef.Users.Where(x => x.Email.Equals(obj.Email) && x.Password.Equals(obj.Password)).Select(s=>new {s.Id,s.Name,s.Gender,s.Email,s.Address,s.Password }).FirstOrDefault();
            

            if (ret != null)
            {
                
                return new JsonResult() { Data = ret, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            else
            {

                return new JsonResult() { Data = "Failed", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/UsersApiController/SetToken")]

        public ActionResult SetToken(HttpRequestMessage value)

        {
            string v = value.Content.ReadAsStringAsync().Result;
            User obj = JsonConvert.DeserializeObject<User>(v);

            User user = dbef.Users.Find(obj.Id);
            user.Device_id = obj.Device_id;
            dbef.SaveChanges();



            return new JsonResult() { Data = "Success", JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }


        //[System.Web.Http.HttpGet]
        //[System.Web.Http.Route("api/UsersApiController/ChangePassword/{email}")]

        //public ActionResult ChangePassword(User model)

        //{

        //    string emailAddress = WebSecurity.GetEmail(model.Email);
        //    if (!string.IsNullOrEmpty(emailAddress))
        //    {
        //        string confirmationToken =
        //            WebSecurity.GeneratePasswordResetToken(model.UserName);
        //        dynamic email = new Email("ChngPasswordEmail");
        //        email.To = emailAddress;
        //        email.UserName = model.UserName;
        //        email.ConfirmationToken = confirmationToken;
        //        email.Send();

        //        return RedirectToAction("ResetPwStepTwo");
        //    }

        //    return RedirectToAction("InvalidUserName");
        //}

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/UsersApiController/ForgetPassword")]

        public ActionResult ForgetPassword(User obj2)
        {
            User obj = dbef.Users.Where(x => x.Email.Equals(obj2.Email)).FirstOrDefault();
            ImageUpload iu = new ImageUpload();
            
            
            if (obj != null)
            {
                iu.sendEmail(obj.Email, obj.Name,obj.Password);
                return new JsonResult() { Data = "Success", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            else
            {
                return new JsonResult() { Data = "Email Not registered", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

           
        }






    }
}
