using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopGiay.Models;
using PagedList;
using System.Security.Cryptography;
using System.Text;

namespace ShopGiay.Controllers
{
    public class KhachHangController : Controller
    {
        ShopGiayEntities db = new ShopGiayEntities();
        // GET: KhachHang
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(KHACHHANG khachHang)
        {
            if (ModelState.IsValid)
            {
                var check = EmaiExist(khachHang.Email);
                if (check)
                {
                    ModelState.AddModelError("EmailExist", "Email đã tồn tại!");
                    return View(khachHang);
                }
                else
                {
                    khachHang.MatKhau = GetMD5(khachHang.MatKhau);
                    db.KHACHHANGs.Add(khachHang);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string matKhau)
        {
            if (ModelState.IsValid)
            {
                var f_matKhau = GetMD5(matKhau);
                var data = db.KHACHHANGs.Where(s => s.Email.Equals(email) && s.MatKhau.Equals(f_matKhau)).ToList();
                if (data.Count > 0)
                {
                    // them session 
                    Session["TenKH"] = data.FirstOrDefault().TenKH;
                    Session["Email"] = data.FirstOrDefault().Email;
                    Session["MaKH"] = data.FirstOrDefault().MaKH;
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }
        [NonAction]
        public bool EmaiExist(string email)
        {
            using (ShopGiayEntities db = new ShopGiayEntities())
            {
                var check = db.KHACHHANGs.Where(x => x.Email == email).FirstOrDefault();
                return check != null;
            }
        }
    }
}