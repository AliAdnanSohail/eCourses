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
    public class Course_TypeController : Controller
    {
        private ECoursesDBEntities db = new ECoursesDBEntities();

        // GET: Course_Type
        public ActionResult Index()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View(db.Course_Type.ToList());
        }

        // GET: Course_Type/Details/5
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
            Course_Type course_Type = db.Course_Type.Find(id);
            if (course_Type == null)
            {
                return HttpNotFound();
            }
            return View(course_Type);
        }

        // GET: Course_Type/Create
        public ActionResult Create()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        // POST: Course_Type/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Course_Type1")] Course_Type course_Type)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                db.Course_Type.Add(course_Type);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(course_Type);
        }

        // GET: Course_Type/Edit/5
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
            Course_Type course_Type = db.Course_Type.Find(id);
            if (course_Type == null)
            {
                return HttpNotFound();
            }
            return View(course_Type);
        }

        // POST: Course_Type/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Course_Type1")] Course_Type course_Type)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                db.Entry(course_Type).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course_Type);
        }

        // GET: Course_Type/Delete/5
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
            Course_Type course_Type = db.Course_Type.Find(id);
            if (course_Type == null)
            {
                return HttpNotFound();
            }
            return View(course_Type);
        }

        // POST: Course_Type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            Course_Type course_Type = db.Course_Type.Find(id);
            db.Course_Type.Remove(course_Type);
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
    }
}
