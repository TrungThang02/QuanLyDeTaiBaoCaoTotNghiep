using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyDeTaiBaoCaoTotNghiep.Models;
using System.Data.Entity;
using PagedList;
using System.IO;

namespace QuanLyDeTaiBaoCaoTotNghiep.Controllers
{
    [HandleError]
    public class DocumentsController : Controller
    {

        UserController User = new UserController();


        
        QuanLyDeTaiBCTNSVEntities db = new QuanLyDeTaiBCTNSVEntities();
        // GET: Documentse
        public ActionResult Index(int ?page)
        {
            int iSize = 12;
            int iPageNum = (page ?? 1);

            var d = (from t in db.GraduationReport select t).OrderByDescending(t => t.UploadDate);
            return View(d.OrderBy(s=> s.GraduationReportID).ToPagedList(iPageNum, iSize));
        }
        [HandleError]
        public ActionResult XemBaoCao(int ? id)
        {
            //if (ModelState.IsValid)
            //{
          
                var dt = from s in db.GraduationReport where s.GraduationReportID == id select s;
                return View(dt.Single());
              
            //}
            //else
            //{
            //    return RedirectToAction("PageNotFound", "Error");
            //}
            //    //var dt = db.GraduationReport.FirstOrDefault(p => p.GraduationReportID == id);
           

          
           
        }
        public ActionResult TheoTacGia()
        {

         
            var detai = (from s in db.GraduationReport select s).OrderByDescending(s => s.UploadDate).Take(10);
            
            return PartialView(detai);
        }

        public ActionResult GioiThieu()
        {
            return View();
        }
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
                baocao.ID = Convert.ToInt32(f["UserID"]);
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
                    string path2 = Path.Combine(Server.MapPath("~/File"), sFileName2);
                    file.SaveAs(path2);
                    image.SaveAs(path);
                    //Kiểm tra ảnh đã được tải lên chưa
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
                    baocao.ID = Convert.ToInt32(f["UserID"]);
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
                    if (image != null || image.ContentLength > 0)
                    {
                        if(file != null || file.ContentLength > 0)
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
            return RedirectToAction("Index");
        }


    }
}