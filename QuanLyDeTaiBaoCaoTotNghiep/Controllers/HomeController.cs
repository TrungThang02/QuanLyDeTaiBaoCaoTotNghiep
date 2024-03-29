﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyDeTaiBaoCaoTotNghiep.Models;
using System.Data.Entity;
using System.Net.Mail;
using System.Net;

namespace QuanLyDeTaiBaoCaoTotNghiep.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        QuanLyDeTaiBCTNSVEntities db = new QuanLyDeTaiBCTNSVEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Index2()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Project()
        {
            var detai = from dt in db.Faculty select dt;
            ViewBag.detai = detai;
            return PartialView();
        }
        public ActionResult DanhMucBaoCao()
        {
            var detai = from dt in db.Faculty select dt;
            ViewBag.detai = detai;
            return PartialView();
        }

        public ActionResult Khoa()
        {
            var detai = from dt in db.Faculty.Where(d => d.FacultyName.Contains(@"Khoa")) select dt;
            ViewBag.detai = detai;
            return PartialView();
        }
        public ActionResult Vien()
        {
            var detai = from dt in db.Faculty.Where(d => d.FacultyName.Contains(@"Viện")) select dt;
            ViewBag.detai = detai;
            return PartialView();
        }


        public ActionResult Faculty(int? id)
        {
            var fa = from s in db.GraduationReport
                       join c in db.Faculty on s.FacultyID equals c.FacultyID
                       where s.FacultyID == id
                       select s;

            var sl = fa.Count();

            ViewBag.sl = sl;
            return View(fa.ToList().Where(r => r.Status == true));
        }

        public ActionResult NienKhoa(int ?id)
        {
            var detai = from dt in db.AcademicYear select dt;
            ViewBag.detai = detai;
            return PartialView();
        }

        public ActionResult Year(int ?id)
        {

            var year = from s in db.GraduationReport
                       join c in db.AcademicYear on s.YearID equals c.YearID
                       where s.YearID == id
                       select s;
            var sl = year.Count();

            ViewBag.sl = sl;
            return View(year.ToList().Where(r => r.Status == true));
        }

        public ActionResult KhoaVien()
        {
            var detai = from dt in db.Faculty select dt;
            ViewBag.detai = detai;
            return PartialView();
        }
        public ActionResult TrangDanhMucBaoCao(int? id)
        {
            var dt = from c in db.Class
                     join r in db.GraduationReport on c.ClassID equals r.ClassID
                     join f in db.Faculty on c.FacultyID equals f.FacultyID
                     join y in db.AcademicYear on r.YearID equals y.YearID
                     where r.GraduationReportID == id
                     select r;



            //var dt =  from s in db.GraduationReport
            //          join p in db.Faculty
            //          on s.FacultyID equals p.FacultyID
            //          join n in db.AcademicYear on s.YearID equals n.YearID
            //          where s.FacultyID == id
            //          select s;
            return View(dt.Where(r => r.Status == true));
        }
        public ActionResult Search(String search = "")
        {
            //var tk = from d in db.GraduationReport select d;
            List<GraduationReport> products = db.GraduationReport.Where(p => p.Keyword.Contains(search)).ToList();
            ViewBag.search = search;
            var sl = products.Count();
            ViewBag.sl = sl;
            return View(products.Where(r => r.Status == true));
        }
        public ActionResult SearchCategory(string searchString, int categoryID = 0, int year = 0)
        {
            // 1. Lưu tư khóa tìm kiếm
            ViewBag.tukhoa = searchString;
            //2.Tạo câu truy vấn kết 3 bảng Book, Author, Category
            //var dt = db.GraduationReport.Include(b => b.Faculty).Include(b => b.Class);
            var tk = from d in db.GraduationReport select d;
            //3. Tìm kiếm theo searchStrin
            if (!String.IsNullOrEmpty(searchString))
                tk = tk.Where(b => b.Keyword.Contains(searchString));
  

            //4. Tìm kiếm theo CategoryID
            if (categoryID != 0)
            {
                tk = tk.Where(c => c.FacultyID == categoryID);
            }
           
            //}
                //5. Tạo danh sách danh mục để hiển thị ở giao diện View thông qua DropDownList
            ViewBag.CategoryID = new SelectList(db.Faculty, "FacultyID", "FacultyName");
            ViewBag.YearID = new SelectList(db.AcademicYear, "YearID", "Name");// danh sách Category
            var sl = tk.Count();
            ViewBag.sl = sl;
            return View(tk.ToList().Where(r => r.Status == true));
        }

        public ActionResult K()
        {
            ViewBag.YearID = new SelectList(db.AcademicYear, "YearID", "Name");// danh sách Category     
            ViewBag.CategoryID = new SelectList(db.Faculty, "FacultyID", "FacultyName"); // dan
            return PartialView();
        }

        public ActionResult BaoCaoGanDay()
        {
            var d = from t in db.GraduationReport select t;
            return PartialView(d.ToList().Take(4));
        }

        public ActionResult Khongcodulieu()
        {
            return View();
        }

       

    }
}