﻿using System;
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
    public class TeachersController : Controller
    {
        private ECoursesDBEntities db = new ECoursesDBEntities();
        ImageUpload obj = new ImageUpload();
        // GET: Teachers
        public ActionResult Index()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View(db.Teachers.ToList());
        }

        // GET: Teachers/Details/5
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
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // GET: Teachers/Create
        public ActionResult Create()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       
        public ActionResult Create( Teacher teacher, HttpPostedFileBase file)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            string image="";
           
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
                teacher.Image = image;
                db.Teachers.Add(teacher);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(teacher);
        }

        // GET: Teachers/Edit/5
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
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Teacher teacher, HttpPostedFileBase file)
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

                teacher.Image = image;
            }
            if (ModelState.IsValid)
            {
                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(teacher);
        }

        // GET: Teachers/Delete/5
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
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            Teacher teacher = db.Teachers.Find(id);
            db.Teachers.Remove(teacher);
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

            List<Teacher> list = db.Teachers.Where(x => x.First_Name.Contains(SearchString)).ToList();

            return View("Index",list);
        }
    }
}
