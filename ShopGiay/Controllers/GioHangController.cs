using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopGiay.Models;


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
            if (Session["GIOHANG"] == null)
            {
                return RedirectToAction("Index", "Store");
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
                TempData["KhongThanhCong"] = "Sản phẩm hiện hết hàng. Bạn thử lựa chọn màu sắc hoặc size khác!";
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
                TempData["LoiSL"] = "Sản phẩm hiện không đủ số lượng yêu cầu. Vui lòng chọn ít hơn";
                ModelState.AddModelError("LoiSP", "");
                return RedirectToAction("XemChiTiet", "SanPham", new { @maSP = sp.MaSP, @maNhanHieu = sp.MaNhanHieu, @maLoai = sp.MaLoai });
            }
            if (sanpham == null)
            {
                sanpham = new GIOHANG(maSP, maMau, maSize);
                listGioHang.Add(sanpham);
                TempData["ThanhCong"] = "Thêm vào giỏ hàng thành công";
                return RedirectToAction("GioHang", "GioHang");
            }    
            else
            {
                sanpham.SoLuong++;
                TempData["ThanhCong"] = "Thêm vào giỏ hàng thành công";
                return RedirectToAction("XemChiTiet", "SanPham", new { @maSP = sp.MaSP, @maNhanHieu = sp.MaNhanHieu, @maLoai = sp.MaLoai });
            }    
        }
    }
}