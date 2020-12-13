using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ShopGiay.Models;

namespace ShopGiay.Controllers
{
    public class StoreController : Controller
    {
        ShopGiayEntities db = new ShopGiayEntities();
         public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        public ActionResult Register()
        {
            return View();
        }

        //POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(KHACHHANG user)
        {
            if (ModelState.IsValid)
            {
                var check = db.KHACHHANGs.FirstOrDefault(s => s.Email == user.Email);
                if (check == null)
                {
                    user.MatKhau = GetMD5(user.MatKhau);
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.KHACHHANGs.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View();
                }
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(FormCollection f)
        {
            string sEmail = Request.Form["Email"].ToString();
            string sMatKhau = Request.Form["MatKhau"].ToString();
            var f_password = GetMD5(sMatKhau);
            var kh = db.KHACHHANGs.Where(s => s.Email.Equals(sEmail) && s.MatKhau.Equals(f_password)).ToList();
            if (kh != null)
            {
                ViewBag.ThongBao = "Đăng nhập thành công";
                Session["UserID"] = kh.FirstOrDefault().MaKH;
                Session["Email"] = kh.FirstOrDefault().Email;
                Session["UserName"] = kh.FirstOrDefault().TenKH;
                return RedirectToAction("Index");
            }
            ViewBag.ThongBao = "Tên tài khoản hoặc mật khẩu không đúng!!!";
            return View();
        }


        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }



        //create a string MD5
        public static string GetMD5(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }


        [HttpGet]
        public ActionResult UpdateInfoCaNhan(string email)
        {
            email = Session["Email"].ToString();
            if (email == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(x => x.Email == email);
                if (kh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(kh);
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateInfoCaNhan([Bind(Include ="TenKH, Email, Sdt")]KHACHHANG kh)
        {
            if (ModelState.Count == 6)
            {
                db.Entry(kh).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["ThongBao"] = "Chỉnh sửa thông tin khách hàng thành công!";
            }
            else
            {
                TempData["ThongBao"] = "Chỉnh sửa thông tin khách hàng không thành công!";
            }
            return View(kh);
        }
        public ActionResult Shop()
        {
            return View();
        }
        [ChildActionOnly]
        public ActionResult SiteNavBar()
        {
            if (Session["UserID"] != null)
            {
                ViewBag.KiemTra = 0;
            }    
            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult SiteBlocksCover()
        {
            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult SiteSection()
        {
            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult SiteBlocks2()
        {
            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult SiteBlocks3()
        {
            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult SiteBlocks8()
        {
            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult SiteFooter()
        {
            return PartialView();
        }
    }

}

