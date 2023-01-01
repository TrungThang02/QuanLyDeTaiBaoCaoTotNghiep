using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyDeTaiBaoCaoTotNghiep.Models;

namespace QuanLyDeTaiBaoCaoTotNghiep.Controllers
{
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


        public ActionResult KhoaVien()
        {
            var detai = from dt in db.Faculty select dt;
            ViewBag.detai = detai;
            return PartialView();
        }
    }
}