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
  
    public class UsersController : Controller
    {
        private QuanLyDeTaiBCTNSVEntities db = new QuanLyDeTaiBCTNSVEntities();

        // GET: Admin/Users
        public ActionResult Index(int ? page)
        {
            int iSize = 7;
            int iPageNum = (page ?? 1);
            var users = db.Users.Include(u => u.Class).Include(u => u.Faculty).Include(u => u.AcademicYear).ToList();
            return View(users.OrderBy(x => x.ID).ToPagedList(iPageNum, iSize));
        }

        // GET: Admin/Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // GET: Admin/Users/Create
        public ActionResult Create()
        {
            ViewBag.ClassID = new SelectList(db.Class, "ClassID", "ClassName");
            ViewBag.FacultyID = new SelectList(db.Faculty, "FacultyID", "FacultyName");
            ViewBag.YearID = new SelectList(db.AcademicYear, "YearID", "Name");
            return View();
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Age,Address,UserName,PassWord,Role,Email,Phone,ClassID,FacultyID,YearID,Date,ResetPasswordCode")] Users users)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(users);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClassID = new SelectList(db.Class, "ClassID", "ClassName", users.ClassID);
            ViewBag.FacultyID = new SelectList(db.Faculty, "FacultyID", "FacultyName", users.FacultyID);
            ViewBag.YearID = new SelectList(db.AcademicYear, "YearID", "Name", users.YearID);
            return View(users);
        }

        // GET: Admin/Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassID = new SelectList(db.Class, "ClassID", "ClassName", users.ClassID);
            ViewBag.FacultyID = new SelectList(db.Faculty, "FacultyID", "FacultyName", users.FacultyID);
            ViewBag.YearID = new SelectList(db.AcademicYear, "YearID", "Name", users.YearID);
            return View(users);
        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Age,Address,UserName,PassWord,Role,Email,Phone,ClassID,FacultyID,YearID,Date,ResetPasswordCode")] Users users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassID = new SelectList(db.Class, "ClassID", "ClassName", users.ClassID);
            ViewBag.FacultyID = new SelectList(db.Faculty, "FacultyID", "FacultyName", users.FacultyID);
            ViewBag.YearID = new SelectList(db.AcademicYear, "YearID", "Name", users.YearID);
            return View(users);
        }

        // GET: Admin/Users/Delete/5

        public ActionResult Delete(int id)
        {
            Users users = db.Users.Find(id);
            db.Users.Remove(users);
            db.SaveChanges();
            return RedirectToAction("Index", "Admin/Users", "Areas");
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
