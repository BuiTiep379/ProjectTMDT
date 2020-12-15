﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopGiay.Models;
using System.Data.Entity;


namespace ShopGiay.Controllers
{
    public class GioHangController : Controller
    {
        ShopGiayEntities db = new ShopGiayEntities();
        // GET: GioHang
        public List<GIOHANG> LayGioHang()
        {
            List<GIOHANG> listGioHang = Session["GIOHANG"] as List<GIOHANG>;
            // Nếu giỏ hàng chưa tồn tại 
            if (listGioHang == null)
            {
                // khởi tạo giỏ hang
                listGioHang = new List<GIOHANG>();
                Session["GIOHANG"] = listGioHang;
            }
            return listGioHang;
        }
        public ActionResult GioHang()
        {
            int maKH = int.Parse(Session["UserID"].ToString());
            KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(x => x.MaKH == maKH);
            ViewBag.MaKH = maKH;
            ViewBag.Email = kh.Email;
            ViewBag.DiaChi = kh.DiaChi;
            ViewBag.Sdt = kh.Sdt;
            if (Session["GIOHANG"] == null)
            {
                TempData["KhongCo"] = "Cart is empty";
                return View();
            }
            List<GIOHANG> listGioHang = LayGioHang();
            TempData["TongTien"] = TongTien();
            return View(listGioHang);
        }
        // Tính tổng số lượng sản phẩm trong kho
        private int TongSoLuong()
        {
            int tongsoluong = 0;
            List<GIOHANG> listGioHang = Session["GIOHANG"] as List<GIOHANG>;
            if (listGioHang != null)
            {
                tongsoluong = listGioHang.Sum(n => n.SoLuong);
            }
            return tongsoluong;
        }
        private int TongSoLuongSP(int maSP)
        {
            int tongsoluong = 0;
            List<GIOHANG> listGioHang = Session["GIOHANG"] as List<GIOHANG>;
            if (listGioHang != null)
            {
                tongsoluong = listGioHang.Where(n => n.MaSP == maSP).Sum(n => n.SoLuong);
            }
            return tongsoluong;
        }
        private decimal TongTien()
        {
            decimal tongtien = 0;
            List<GIOHANG> listGioHang = Session["GIOHANG"] as List<GIOHANG>;
            if (listGioHang != null)
            {
                tongtien = listGioHang.Sum(n => n.ThanhTien);
            }
            Session["TongTien"] = tongtien;
            return tongtien;
        }
        public ActionResult ThemSanPham(int maSP, string url)
        {
            //lấy mã màu mã  size từ dropdownlist
            int maMau = int.Parse(Request.Form["MaMau"]);
            int maSize = int.Parse(Request.Form["MaSize"]);
            // kiểm tra sự tồn tại của sản phẩm
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(x => x.MaSP == maSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            // kiểm tra sự tồn tại màu sắc và size của đôi giày trong kho
            CHITIETSP ct = db.CHITIETSPs.SingleOrDefault(x => x.MaSP == maSP && x.MaSize == maSize && x.MaMau == maMau);
            if (ct == null)
            {
                TempData["KhongThanhCong"] = "Products are out of stock. You can try different colors or sizes!";
                return RedirectToAction("XemChiTiet", "SanPham", new { @maSP = sp.MaSP, @maNhanHieu = sp.MaNhanHieu, @maLoai = sp.MaLoai });
            }
            //Lấy ra session giỏ hàng
            List<GIOHANG> listGioHang = LayGioHang();
            //Kiểm tra sản phẩm đã tồn tại trong giỏ hàng chưa?
            GIOHANG sanpham = listGioHang.Find(x => x.MaSP == maSP && x.MaMau == maMau && x.MaSize == maSize);
            var slsp = db.CHITIETSPs.SingleOrDefault(n => n.MaSP == maSP && n.MaMau == maMau && n.MaSize == maSize).SoLuong;
            TempData["TongSoLuong"] = TongSoLuong();
            if (TongSoLuongSP(maSP) >= slsp && sanpham != null)
            {
                TempData["LoiSL"] = "The product does not have the required quantity. Please choose less!";
                ModelState.AddModelError("LoiSP", "");
                return RedirectToAction("XemChiTiet", "SanPham", new { @maSP = sp.MaSP, @maNhanHieu = sp.MaNhanHieu, @maLoai = sp.MaLoai });
            }
            if (sanpham == null)
            {
                sanpham = new GIOHANG(maSP, maMau, maSize);
                listGioHang.Add(sanpham);
                TempData["ThanhCong"] = "Add to cart successfully";
                return RedirectToAction("XemChiTiet", "SanPham", new { @maSP = sp.MaSP, @maNhanHieu = sp.MaNhanHieu, @maLoai = sp.MaLoai });
            }
            else
            {
                sanpham.SoLuong++;
                TempData["ThanhCong"] = "Add to cart successfully";
                return RedirectToAction("XemChiTiet", "SanPham", new { @maSP = sp.MaSP, @maNhanHieu = sp.MaNhanHieu, @maLoai = sp.MaLoai });
            }
        }

        public ActionResult CapNhatGioHang(int maSP, int maMau, int maSize, FormCollection f)
        {
            int check = Int32.Parse(Request.Form["txtSoLuong"]);// số lượng giày cần mua
            var soLuongGiay = db.CHITIETSPs.SingleOrDefault(x => x.MaSP == maSP && x.MaMau == maMau && x.MaSize == maSize).SoLuong;// số lượng giày trong kho
            if (check > soLuongGiay)
            {
                TempData["LoiSL"] = "Sản phẩm hiện không đủ số lượng. Vui lòng chọn ít hơn!";
                ModelState.AddModelError("LoiSP", " ");
                return RedirectToAction("GioHang", "GioHang");
            }
            if (check < 1)
            {
                TempData["LoiSL"] = "Không thể mua với số lượng nhỏ hơn 1. Vui lòng chọn nhiều hơn!";
                ModelState.AddModelError("LoiSL", " ");
                return RedirectToAction("GioHang", "GioHang");
            }
            else
            {
                SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == maSP);
                if (sp == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                List<GIOHANG> listGioHang = LayGioHang();
                GIOHANG sanPham = listGioHang.SingleOrDefault(n => n.MaSP == maSP && n.MaMau == maMau && n.MaSize == maSize);
                if (sanPham != null)
                {
                    sanPham.SoLuong = int.Parse(f["txtSoLuong"].ToString());
                }
                return RedirectToAction("GioHang", "GioHang");
            }
        }
        public ActionResult XoaGioHang(int maSP, int maMau, int maSize)
        {
            // kiểm tra sản phẩm 
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == maSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<GIOHANG> listGioHang = LayGioHang();
            GIOHANG sanpham = listGioHang.SingleOrDefault(n => n.MaSP == maSP && n.MaMau == maMau && n.MaSize == maSize);
            if (sanpham != null)
            {
                listGioHang.RemoveAll(n => n.MaSP == maSP && n.MaMau == maMau && n.MaSize == maSize);

            }
            if (listGioHang.Count == 0)
            {
                return RedirectToAction("Shop", "Store");
            }
            return RedirectToAction("GioHang");
        }
        [ChildActionOnly]
        public ActionResult GioHangPartial()
        {
            if (TongSoLuong() == 0)
            {
                return PartialView();
            }
            
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }
        
        public ActionResult ThanhToan(int maKH,string email, string diaChiGiao, string sdt)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Store");
            }
            else
            {
                if (Session["GioHang"] == null)
                {
                    TempData["ThongBao"] = "Giỏ hàng của bạn trống";
                    return RedirectToAction("Shop", "Store");
                }
                else
                {
                    List<GIOHANG> listGioHang = LayGioHang();
                    TempData["TongTien"] = TongTien();
                    maKH = int.Parse(Session["UserID"].ToString());
                    KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(x => x.MaKH == maKH);
                    ViewBag.TenKH = kh.TenKH;
                    ViewBag.Email = kh.Email;
                    ViewBag.DiaChi = kh.DiaChi;
                    ViewBag.Sdt = kh.Sdt;
                    return View(listGioHang);
                }
            }
            
        }
        [HttpPost, ActionName("ThanhToan")]
        [ValidateAntiForgeryToken]
        public ActionResult XacNhanThanhToan(int maKH,string email, string diaChiGiao, string sdt)
        {
            KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(x => x.MaKH == maKH);
            DONHANG dh = new DONHANG()
            {
                MaKH = int.Parse(Session["UserID"].ToString()),
                NgayDatHang = DateTime.Now,
                NgayGiaoHang = DateTime.Now,
                DiaChiGiao = diaChiGiao,
                TongTien = TongTien(),
                ThanhToan = "Cash",
                TinhTrang = "Chưa xác nhận",
                HoTen = kh.TenKH,
                Email = email,
                Sdt = sdt
            };
            db.DONHANGs.Add(dh);
            db.SaveChanges();
            List<GIOHANG> listGioHang = LayGioHang();
            foreach (var item in listGioHang)
            {
                int maSP = item.MaSP;
                int maSize = item.MaSize;
                int maMau = item.MaMau;
                int soLuong = item.SoLuong;
                CHITIETSP ct = db.CHITIETSPs.SingleOrDefault(x => x.MaSP == maSP && x.MaMau == maMau && x.MaSize == maSize);
                ct.SoLuong -= soLuong;
                db.Entry(ct).State = EntityState.Modified;
                db.SaveChanges();
            }
            Session.Remove("GIOHANG");
            return RedirectToAction("ThanksYou");
        }
        public ActionResult ThanksYou()
        {
            int maKH = int.Parse(Session["UserID"].ToString());
            KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(x => x.MaKH == maKH);
            ViewBag.TenKhachHang = kh.TenKH;
            return View();
        }
    }
}