using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using QuanLyDeTaiBaoCaoTotNghiep.Models;

namespace QuanLyDeTaiBaoCaoTotNghiep.Controllers
{
    public class UserController : Controller
    {
        QuanLyDeTaiBCTNSVEntities db = new QuanLyDeTaiBCTNSVEntities();
        // GET: User
        public static string Encrypt(string password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] encrypt;
            UTF8Encoding encode = new UTF8Encoding();
            encrypt = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder encryptdata = new StringBuilder();
            for (int i = 0; i < encrypt.Length; i++)
            {
                encryptdata.Append(encrypt[i].ToString());

            }
            return encryptdata.ToString();

        }


        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var response = Request["g-recaptcha-response"];
            string secretKey = "6Let3L0jAAAAAPx3bZRlTYo3gt-0rfbbrmCFIsvk";
            var client = new WebClient();

            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));

            var obj = JObject.Parse(result);
            var status = (bool)obj.SelectToken("success");
            ViewBag.Message = status ? "Xác thực Google reCaptcha thành công !" : "Bạn chưa xác thực Google reCaptcha";


            if (ModelState.IsValid && status)
            {
                int state = Convert.ToInt32(Request.QueryString["id"]);
                var sTenDN = collection["TenDN"];
                var sMatKhau = Encrypt(collection["MatKhau"].ToString());

                //var sMatKhau = collection["MatKhau"].ToString();
                if (String.IsNullOrEmpty(sTenDN))
                {
                    ViewData["err"] = "Tài khoản tên không được để trống";
                }
                else if (String.IsNullOrEmpty(sMatKhau))
                {
                    ViewData["err"] = "Mật khẩu không được để trống";
                }
                else
                {
                    Users nguoidung = db.Users.SingleOrDefault(n => n.UserName == sTenDN && n.PassWord == sMatKhau);


                    Users ADMIN = db.Users.SingleOrDefault(n => n.UserName == sTenDN && n.PassWord == sMatKhau && n.Role == 0);

                    if (nguoidung != null)
                    {
                        ViewBag.ThongBao = "Chúc mừng bạn đăng nhập thành công";

                        Session["TaiKhoan"] = nguoidung;

                        Session["TaiKhoan2"] = nguoidung.Name;

                        if (nguoidung == ADMIN)
                        {
                            Session.Clear();

                            Session["ADMIN"] = ADMIN;
                            return RedirectToAction("Index2", "Home");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                          
                        }

                      
                    }
                    else
                    {
                        ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                        ViewBag.Message = status ? "Xác thực Google reCaptcha thành công !" : "Bạn chưa xác thực Google reCaptcha";

                    }
                }
            }


            return View();
        }
       
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(FormCollection collection, Users nguoidung)
        {
            var sTen = collection["Ten"];
            var sTenDN = collection["TenDN"];
            var sEmail = collection["Email"];
            var sMatKhau = Encrypt(collection["MatKhau"].ToString());
            var sNhapLaiMatKhau = collection["MatKhauNL"];
                  
            if (String.IsNullOrEmpty(sTenDN))
            {
                ViewData["err2"] = "Tên đăng nhập không được để trống";
            }
            else if (String.IsNullOrEmpty(sEmail))
            {
                ViewData["err6"] = "Email không được để trống";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["err3"] = "Mật khẩu không được để trống";
            }
            else if (String.IsNullOrEmpty(sNhapLaiMatKhau))
            {
                ViewData["err4"] = "Phải nhập lại mật khẩu";
            }
            
        
            else if (db.Users.SingleOrDefault(n => n.UserName == sTenDN) != null)
            {
                ViewData["err8"] = "Tên đăng nhâp đã tồn tại";
            }
            else if (db.Users.SingleOrDefault(n => n.Email == sEmail) != null)
            {
                ViewData["err9"] = "Email đã được sử dụng";
            }
            else
            {
                try
                {
                    nguoidung.Name = sTen;
                    nguoidung.UserName = sTenDN;
                    nguoidung.PassWord = sMatKhau;
                    nguoidung.Email = sEmail;
        
                    db.Users.Add(nguoidung);
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    Console.WriteLine(e);
                }
                return RedirectToAction("Login", "User");
            }
            return this.SignUp();
        }
        public ActionResult LoginLogOutPartial()
        {

            return PartialView();
        }
    }
}