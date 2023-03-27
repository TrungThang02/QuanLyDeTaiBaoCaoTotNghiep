using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using QuanLyDeTaiBaoCaoTotNghiep.Models;

namespace QuanLyDeTaiBaoCaoTotNghiep.Areas.Admin.Controllers
{
    public class FacultyController : Controller
    {
        private QuanLyDeTaiBCTNSVEntities db = new QuanLyDeTaiBCTNSVEntities();

        // GET: Admin/Faculty
        public ActionResult Index(int ? page)
        {
            int iSize = 7;
            int iPageNum = (page ?? 1);
            return View(db.Faculty.OrderBy(x => x.FacultyID).ToList().ToPagedList(iPageNum, iSize));
        }

        // GET: Admin/Faculty/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Faculty faculty = db.Faculty.Find(id);
            if (faculty == null)
            {
                return HttpNotFound();
            }
            return View(faculty);
        }

        // GET: Admin/Faculty/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Faculty/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FacultyID,FacultyName")] Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                db.Faculty.Add(faculty);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(faculty);
        }

        // GET: Admin/Faculty/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Faculty faculty = db.Faculty.Find(id);
            if (faculty == null)
            {
                return HttpNotFound();
            }
            return View(faculty);
        }

        // POST: Admin/Faculty/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FacultyID,FacultyName")] Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                db.Entry(faculty).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(faculty);
        }

        public ActionResult Delete(int id)
        {
            Faculty faculty = db.Faculty.Find(id);
            db.Faculty.Remove(faculty);
            db.SaveChanges();
            return RedirectToAction("Index", "Admin/Faculty", "Areas");
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
