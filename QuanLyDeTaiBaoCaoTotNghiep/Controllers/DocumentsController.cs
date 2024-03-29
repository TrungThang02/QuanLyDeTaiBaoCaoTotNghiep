﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyDeTaiBaoCaoTotNghiep.Models;
using System.Data.Entity;
using PagedList;
using System.IO;
using System.Data;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace QuanLyDeTaiBaoCaoTotNghiep.Controllers
{
    [HandleError]
    public class DocumentsController : Controller
    {

        UserController Users = new UserController();
       

        
        QuanLyDeTaiBCTNSVEntities db = new QuanLyDeTaiBCTNSVEntities();
        // GET: Documentse
        public ActionResult Index(int ?page)
        {
           

            int iSize = 12;
            int iPageNum = (page ?? 1);

            var d = (from t in db.GraduationReport select t).OrderBy(t => t.UploadDate).Where(r => r.Status == true);
   

            return View(d.OrderBy(s=> s.GraduationReportID).ToPagedList(iPageNum, iSize));
        }
        [HandleError]


        public ActionResult CheckTypeFile(int? id)
        {
            string myString = db.GraduationReport.FirstOrDefault(f => f.GraduationReportID == id)?.UrlFile;
            if (myString.Contains("pdf"))
            {
                return Redirect("/Documents/XemBaoCao/" + id);
            }
            else if (myString.Contains("docx"))
            {
                return Redirect("/Documents/View/" + id);
               
            }
            else
            {
                return View();
            }
        }

        public ActionResult XemBaoCao(int? id)
        {
            //if (ModelState.IsValid)
            //{

            var document = db.GraduationReport.Find(id);
            document.ViewCount++;
            db.SaveChanges();

            string currentUrl = Request.Url.AbsoluteUri;
            ViewBag.CurrentUrl = currentUrl;

            var dt = from s in db.GraduationReport where s.GraduationReportID == id select s;

            return View(dt.Single());


        }



        public ActionResult View(int ?id)
        {

            var document = db.GraduationReport.Find(id);
            document.ViewCount++;
            db.SaveChanges();
            string currentUrl = Request.Url.AbsoluteUri;
            ViewBag.CurrentUrl = currentUrl;
            var dt = from s in db.GraduationReport where s.GraduationReportID == id select s;
            return View(dt.Single());
        }
        public ActionResult TheoTacGia()
        {

         
            var detai = (from s in db.GraduationReport select s).OrderByDescending(s => s.UploadDate).Take(10).Where(r => r.Status == true);
            
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
        public ActionResult Create(GraduationReport baocao, HttpPostedFileBase image, HttpPostedFileBase file, FormCollection f, string email)
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
                    baocao.Author = f["tentacgia"];

                    baocao.ClassID = Convert.ToInt32(f["ClassID"]);
                    baocao.FacultyID = Convert.ToInt32(f["FacultyID"]);
                    baocao.YearID = Convert.ToInt32(f["YearID"]);
                    db.GraduationReport.Add(baocao);
                    db.SaveChanges();







                    ////gỬI MAIL 
                    //string to = "trantrungthang01699516993@gmail.com";
                    ////string from = Session["Email"].ToString();
                    //string from = "thangpy2k2@gmail.com";

                    //string subject = "THÔNG BÁO !!!"; // chủ đề email
                    //string mes = "Có báo cáo mới vừa được tải lên hệ thống !"; // nội dung email

                    //// Khởi tạo đối tượng MailMessage
                    //// Khởi tạo đối tượng SmtpClient và gửi email
                    //SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587); // SMTP server và cổng của email
                    //smtpClient.Credentials = new NetworkCredential("trantrungthang01699516993@gmail.com", "iauopfthcsrhnunj"); // địa chỉ email và mật khẩu
                    //smtpClient.EnableSsl = true; // kích hoạt SSL cho kết nối SMTP



                    //using (var message = new MailMessage(from, to)
                    //{
                    //    Subject = subject,
                    //    Body = mes,
                    //    IsBodyHtml = true
                    //})
                    //    smtpClient.Send(message);



                    var fromEmail = new MailAddress("trantrungthang01699516993@gmail.com", "Hệ thống quản lí đề tài báo cáo tốt nghiệp -  TDMU");
                    string toEmail = "2024801030146@student.tdmu.edu.vn";

                    string link = "http://trungthangnckh-001-site1.dtempurl.com/";

                    string bc = baocao.GraduationReportName;

                    string  subject = "THÔNG BÁO !!!";
                    string body = "Người dùng vừa tải báo cáo lên hệ thống <br /> Tên báo cáo :" + bc + "  <br /> Bạn có thể quản lí <a href='" + link + "'> tại đây </a>";



                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = true,
                        Credentials = new NetworkCredential(fromEmail.Address, "iauopfthcsrhnunj")
                    };
                    using (var message = new MailMessage(fromEmail.ToString(), toEmail)
                    {
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    })
                        smtp.Send(message);




                    return Redirect("/User/Info/" + Session["TaiKhoan3"]);



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
                        if(file != null && file.ContentLength > 0)
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

        public ActionResult Delete(int ? id)
        {
            GraduationReport graduationReport = db.GraduationReport.Find(id);
            db.GraduationReport.Remove(graduationReport);
            db.SaveChanges();
            return Redirect("/User/Info/" + Session["TaiKhoan3"]);
        }
        //public ActionResult ShareOnFacebook(int id)
        //{
        //    // Lấy thông tin tài liệu từ database
        //    var document = db.GraduationReport.FirstOrDefault(d => d.GraduationReportID == id);

        //    string url = "https://ttt.com/Documents/XemBaoCao/" + id;
        //    // Tạo đường dẫn chia sẻ trên Facebook
        //    var shareUrl = "https://www.facebook.com/sharer/sharer.php?u=" + url;

        //    // Redirect đến đường dẫn chia sẻ
        //    return Redirect(shareUrl);
        //}
        public ActionResult Share(int id)
        {
            var post = db.GraduationReport.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }


        public ActionResult DownloadFile(string filePath)
        {
            var document = db.GraduationReport.FirstOrDefault(x => x.UrlFile == filePath);

            document.DownloadCount++;
            db.SaveChanges();

            string fullName = Server.MapPath("~/File/" + filePath);

            int layduoifile = filePath.LastIndexOf('.');
            byte[] fileBytes = GetFile(fullName);
            return File(
                fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, document.GraduationReportName + "." + filePath.Substring(layduoifile + 1));
        }

        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }


        public ActionResult BaoCaoXemNhieu()
        {
            var bc = (from s in db.GraduationReport select s).Where(r => r.Status == true);
            return PartialView(bc.OrderByDescending(n => n.ViewCount).ToList().Take(8));
        }

        public ActionResult BaoCaoTaiNhieu()
        {
            var bc = (from s in db.GraduationReport select s).Where(r => r.Status == true);
            return PartialView(bc.OrderByDescending(n => n.DownloadCount).ToList().Take(8));
        }
    }
}