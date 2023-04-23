using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLyDeTaiBaoCaoTotNghiep.Models;
using PagedList;
using System.Text.RegularExpressions;
using System.Text;

namespace QuanLyDeTaiBaoCaoTotNghiep.Areas.Admin.Controllers
{

    public class GraduationReportController : Controller
    {
        private QuanLyDeTaiBCTNSVEntities db = new QuanLyDeTaiBCTNSVEntities();

        // GET: Admin/GraduationReport

        public ActionResult Index(int ? page)
        {
            //if (Session["ADMIN"] != null)
            //{
            int iSize = 3;
            int iPageNum = (page ?? 1);
            var graduationReport = db.GraduationReport.Include(g => g.Class).Include(g => g.Faculty).ToList();
            if (Session["ADMIN"] != null)
            {
                return View(graduationReport.OrderBy(x => x.GraduationReportID).ToPagedList(iPageNum, iSize));
            }
            else
            {
                return RedirectToAction("Erorr", "GraduationReport");
            }


           


           // }
           //else
           // {
           //    return RedirectToAction("Erorr", "GraduationReport");
           //}



        }

        // GET: Admin/GraduationReports

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GraduationReport graduationReport = db.GraduationReport.Find(id);
            if (graduationReport == null)
            {
                return HttpNotFound();
            }
            return View(graduationReport);
        }

        // GET: Admin/GraduationReports/Create
        public ActionResult Create()
        {
            ViewBag.ClassID = new SelectList(db.Class, "ClassID", "ClassName");
            ViewBag.FacultyID = new SelectList(db.Faculty, "FacultyID", "FacultyName");
            ViewBag.YearID = new SelectList(db.AcademicYear, "YearID", "Name");
            return View();
        }

        // POST: Admin/GraduationReports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(GraduationReport baocao, HttpPostedFileBase image, HttpPostedFileBase file, FormCollection f)
        {
            ViewBag.ClassID = new SelectList(db.Class, "ClassID", "ClassName");
            ViewBag.FacultyID = new SelectList(db.Faculty, "FacultyID", "FacultyName");
            ViewBag.YearID = new SelectList(db.AcademicYear, "YearID", "Name");


            if (image == null && file == null)
            {


             
                baocao.GraduationReportName = f["sTenSach"];
                baocao.Description = f["sMoTa"].Replace("<p>", "").Replace("<p>", "\n");
                baocao.Keyword = f["sKeyword"];
                baocao.UploadDate = Convert.ToDateTime(f["dNgayCapNhat"]);
                baocao.Author = f["tentacgia"];

                ViewBag.ClassID = new SelectList(db.Class, "ClassID", "ClassName");
                ViewBag.FacultyID = new SelectList(db.Faculty, "FacultyID", "FacultyName");
                ViewBag.YearID = new SelectList(db.AcademicYear, "YearID", "Name");

                return View();

            }
            else
            {
                if (ModelState.IsValid)
                {
                    var newFile = Guid.NewGuid();
                    var renamefile = Path.GetExtension(file.FileName);
                    string newName = newFile + renamefile;
                    //lấy tên file, khai báo thư viện(System IO)
       
                    string sFileName2 = Path.GetFileName(newName);

                    var newImage = Guid.NewGuid();
                    var renameImage = Path.GetExtension(image.FileName);
                    string newImageName = newImage + renameImage;
                    //lấy tên file, khai báo thư viện(System IO)

                    var sFileName = Path.GetFileName(newImageName);



                    //Lấy đường dẫn lưu file
                    var path = Path.Combine(Server.MapPath("~/Content/images/AnhBaoCao"), sFileName);
                    string path2 = Path.Combine(Server.MapPath("~/File"),sFileName2);
                    file.SaveAs(path2);
                    image.SaveAs(path);

                    //Kiểm tra ảnh đã được tải lê
                    //n chưa
                    if (!System.IO.File.Exists(path))
                    {
                        image.SaveAs(path);
                        file.SaveAs(path2);
                    }


                   
                baocao.GraduationReportName = f["sTenSach"];
                baocao.Description = f["sMoTa"].Replace("<p>", "").Replace("<p>", "\n");
                baocao.Image = sFileName;
                baocao.UrlFile = sFileName2;
                baocao.Keyword = f["sKeyword"];
                baocao.UploadDate = Convert.ToDateTime(f["dNgayCapNhat"]);
                   baocao.Author = f["tentacgia"];
                    //sach.NgayCapNhat = Convert.ToDateTime(f["dNgayCapNhat"]);


                    baocao.ClassID = Convert.ToInt32(f["ClassID"]);
                baocao.FacultyID = Convert.ToInt32(f["FacultyID"]);
                baocao.YearID = Convert.ToInt32(f["YearID"]);
                db.GraduationReport.Add(baocao);
                db.SaveChanges();
                return RedirectToAction("Index");
                }

                return View();
            }
                
           
        }
       

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var baocao = db.GraduationReport.SingleOrDefault(n => n.GraduationReportID == id);
            if (baocao == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            else
            {
                ViewBag.YearID = new SelectList(db.AcademicYear, "YearID", "Name", baocao.YearID);
                ViewBag.ClassID = new SelectList(db.Class, "ClassID", "ClassName", baocao.ClassID);
                ViewBag.FacultyID = new SelectList(db.Faculty, "FacultyID", "FacultyName", baocao.FacultyID);

                return View(baocao);
            }
        }



        // POST: Admin/GraduationReporjfgkjhjdkts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HttpPostedFileBase image, HttpPostedFileBase file, FormCollection f)
        {
            var baocao = db.GraduationReport.AsEnumerable().SingleOrDefault(n => n.GraduationReportID == int.Parse(f["iMaSach"]));
            ViewBag.ClassID = new SelectList(db.Class, "ClassID", "ClassName");
            ViewBag.FacultyID = new SelectList(db.Faculty, "FacultyID", "FacultyName");
            ViewBag.YearID = new SelectList(db.AcademicYear, "YearID", "Name");

            if (ModelState.IsValid)
            {
                try
                {
                    if (image != null && image.ContentLength > 0)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            var newFile = Guid.NewGuid();
                            var renamefile = Path.GetExtension(file.FileName);
                            string newName = newFile + renamefile;
                            //lấy tên file, khai báo thư viện(System IO)

                            string sFileName2 = Path.GetFileName(newName);

                            var newImage = Guid.NewGuid();
                            var renameImage = Path.GetExtension(image.FileName);
                            string newImageName = newImage + renameImage;
                            //lấy tên file, khai báo thư viện(System IO)

                            var sFileName = Path.GetFileName(newImageName);



                            //Lấy đường dẫn lưu file
                            var path = Path.Combine(Server.MapPath("~/Content/images/AnhBaoCao"), sFileName);
                            string path2 = Path.Combine(Server.MapPath("~/File"), sFileName2);
                            file.SaveAs(path2);
                            image.SaveAs(path);
                            //Kiểm tra ảnh đã được tải lên chưa
                            //Kiểm tra ảnh đã được tải lên chưa
                            if (!System.IO.File.Exists(path) && !System.IO.File.Exists(path2))
                            {
                                System.IO.File.Delete(path);
                                System.IO.File.Delete(path2);

                            }
                            else
                            {
                                image.SaveAs(path);
                                file.SaveAs(path2);
                                baocao.Image = sFileName;
                                baocao.UrlFile = sFileName2;
                            }

                        }


                    }
                    else
                    {
                        ViewBag.Message = "Please select a file to upload";
                    }

                    baocao.GraduationReportName = f["sTenSach"];
                    baocao.Description = f["sMoTa"].Replace("<p>", "").Replace("<p>", "\n");
                    baocao.Author = f["tentacgia"];

                    baocao.Keyword = f["sKeyword"];
                    baocao.UploadDate = Convert.ToDateTime(f["dNgayCapNhat"]);
                    //sach.NgayCapNhat = Convert.ToDateTime(f["dNgayCapNhat"]);


                    baocao.ClassID = Convert.ToInt32(f["ClassID"]);
                    baocao.FacultyID = Convert.ToInt32(f["FacultyID"]);
                    baocao.YearID = Convert.ToInt32(f["YearID"]);

                    db.SaveChanges();


                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    ViewBag.Thongbao = "Chưa chọn file";
                }



            }
            return View();
        }
        

        // GET: Admin/GraduationReporjfgkjhjdkts/Delete/5
   
        // POST: Admin/GraduationReporjfgkjhjdkts/Delete/5
  
        public ActionResult Delete(int id)
        {
            GraduationReport graduationReport = db.GraduationReport.Find(id);
            db.GraduationReport.Remove(graduationReport);
            db.SaveChanges();
            return RedirectToAction("Index", "Admin/GraduationReport", "Areas");
        }


        public ActionResult Erorr()
        {
            return View();
        }
        public ActionResult UnapprovedReports()
        {
            var reports = db.GraduationReport.Where(r => r.Status == false).ToList();
            return View(reports);
        }

        public ActionResult ApproveDocument(int id)
        {
            var report = db.GraduationReport.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            report.Status = true;
            db.Entry(report).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("UnapprovedReports");
        }


    }
}
