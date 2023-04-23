using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyDeTaiBaoCaoTotNghiep.Models;

namespace QuanLyDeTaiBaoCaoTotNghiep.Areas.Admin.Controllers
{
   
    public class HomesController : Controller
    {
        QuanLyDeTaiBCTNSVEntities db = new QuanLyDeTaiBCTNSVEntities();
        // GET: Admin/Homes

        public ActionResult Index()
        {
            var tsbc = (from s in db.GraduationReport select s).Count();
            ViewBag.tongbaocao = tsbc;


            var tstk = (from s in db.Users select s).Count();
            ViewBag.tongtaikhoan = tstk;

            int totalViews = db.GraduationReport.Sum(r => r.ViewCount);
            ViewBag.totalView = totalViews;

            int totalDowns = db.GraduationReport.Sum(r => r.DownloadCount);
            ViewBag.totalDown = totalDowns;
           if(Session["ADMIN"] != null)
           {
            return View();
            }
            else
            {
                return RedirectToAction("Erorr", "GraduationReport");
            }


        }
    }
}