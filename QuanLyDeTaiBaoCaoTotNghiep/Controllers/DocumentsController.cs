using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyDeTaiBaoCaoTotNghiep.Models;

namespace QuanLyDeTaiBaoCaoTotNghiep.Controllers
{
    public class DocumentsController : Controller
    {
        QuanLyDeTaiBCTNSVEntities db = new QuanLyDeTaiBCTNSVEntities();
        // GET: Documentse
        public ActionResult Index()
        {
            var d = from t in db.GraduationReport select t;
            return View(d);
        }
    }
}