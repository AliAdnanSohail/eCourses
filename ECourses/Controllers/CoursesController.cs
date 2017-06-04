using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ECourses.Models;

namespace ECourses.Controllers
{
    public class CoursesController : Controller
    {
        private ECoursesDBEntities db = new ECoursesDBEntities();
        ImageUpload obj = new ImageUpload();

        // GET: Courses
        public ActionResult Index()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View(db.Courses.ToList());
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Course course, HttpPostedFileBase file)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            string image = "";

            if (file != null)
            {

                byte[] fileBytes = new byte[file.ContentLength];
                file.InputStream.Read(fileBytes, 0, fileBytes.Length);
                file.InputStream.Close();
                string fileContent = Convert.ToBase64String(fileBytes);
                image = obj.Upload(fileContent);
            }

                if (ModelState.IsValid)
                {
                   course.Image = image;
                    course.Created_At = DateTime.UtcNow.ToLocalTime();
                    db.Courses.Add(course);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            

            return View(course);
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Course course, HttpPostedFileBase file)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            string image = "";

            if (file != null)
            {

                byte[] fileBytes = new byte[file.ContentLength];
                file.InputStream.Read(fileBytes, 0, fileBytes.Length);
                file.InputStream.Close();
                string fileContent = Convert.ToBase64String(fileBytes);
                image = obj.Upload(fileContent);

                course.Image = image;
            }

            if (ModelState.IsValid)
            {
                course.Updated_At = DateTime.UtcNow.ToLocalTime();
                course.Created_At = course.Created_At;
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {

            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Search(string SearchString)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            List<Course> list = db.Courses.Where(x => x.Title.Contains(SearchString)).ToList();

            return View("Index", list);
        }
    }
}
