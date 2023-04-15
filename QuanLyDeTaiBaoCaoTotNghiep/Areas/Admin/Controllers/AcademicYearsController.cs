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
    public class AcademicYearsController : Controller
    {
        private QuanLyDeTaiBCTNSVEntities db = new QuanLyDeTaiBCTNSVEntities();

        // GET: Admin/AcademicYears
        public ActionResult Index(int ? page)
        {
            int iSize = 7;
            int iPageNum = (page ?? 1);
            return View(db.AcademicYear.OrderBy(x=> x.YearID).ToList().ToPagedList(iPageNum, iSize));
        }

        // GET: Admin/AcademicYears/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AcademicYear academicYear = db.AcademicYear.Find(id);
            if (academicYear == null)
            {
                return HttpNotFound();
            }
            return View(academicYear);
        }

        // GET: Admin/AcademicYears/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/AcademicYears/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "YearID,Name")] AcademicYear academicYear)
        {
            if (ModelState.IsValid)
            {
                db.AcademicYear.Add(academicYear);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(academicYear);
        }

        // GET: Admin/AcademicYears/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AcademicYear academicYear = db.AcademicYear.Find(id);
            if (academicYear == null)
            {
                return HttpNotFound();
            }
            return View(academicYear);
        }

        // POST: Admin/AcademicYears/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "YearID,Name")] AcademicYear academicYear)
        {
            if (ModelState.IsValid)
            {
                db.Entry(academicYear).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(academicYear);
        }

        // GET: Admin/AcademicYears/Delete/5

        public ActionResult Delete(int ? id)
        {
            AcademicYear academicYear = db.AcademicYear.Find(id);
            db.AcademicYear.Remove(academicYear);
            db.SaveChanges();
            return RedirectToAction("Index", "Admin/AcademicYears", "Areas");
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
