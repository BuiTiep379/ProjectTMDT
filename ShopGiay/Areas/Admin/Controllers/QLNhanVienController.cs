using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopGiay.Models;
using PagedList;
using System.Net;
using PagedList.Mvc;

namespace ShopGiay.Areas.Admin.Controllers
{
    public class QLNhanVienController : Controller
    {
        ShopGiayEntities db = new ShopGiayEntities();

        // GET: Admin/QLNhanVien
        public ActionResult Index()
        {
            return View();
        }
        // Hiện thị danh sách nhân viên
        public ActionResult DanhSachNV(string search, int? page)
        {
            ViewBag.Search = search;
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            
            if (search != null)
            {
                List<NHANVIEN> listNV = db.NHANVIENs.Where(m => m.TenNV.Contains(search)).ToList();
                if (listNV.Count == 0)
                {
                    // Lưu message vào TempData
                    TempData["ThongBao"] = "Không có nhân viên nào phù hợp!";
                    return View(db.NHANVIENs.OrderBy(m => m.TenNV).ToPagedList(pageNumber, pageSize));
                }
                // nếu có nhân viên thì hiện thị list nhân viên tìm kiếm được
                return View(db.NHANVIENs.OrderBy(m => m.TenNV).ToPagedList(pageNumber, pageSize));

            }
            // nếu từ khóa null thì trả về list nhân viên ban đầu
            return View(db.NHANVIENs.OrderBy(m => m.TenNV).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult AddNhanVien()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddNhanVien(NHANVIEN nv)
        {
            if (ModelState.IsValid)
            {
                // thêm dữ liệu vào bảng nhân viên
                db.NHANVIENs.Add(nv);
                db.SaveChanges();
                TempData["ThongBao"] = "Thêm nhân viên thành công!";
            }
            else
            {
                TempData["ThongBao"] = "Thêm nhân viên thất bại!";
            }
            return View();
        }
        [HttpGet]
        public ActionResult EditNhanVien(int maNV)
        {
            // Lấy nhân viên theo mã
            NHANVIEN nv = db.NHANVIENs.SingleOrDefault(m => m.MaNV == maNV);
            if (nv == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            // nếu có nhân viên thì trả về view của nhân viên đó
            return View(nv);
        }
        [HttpPost]
        [ValidateInput(false)]// cho phép chèn code HTML xuống database với toàn bộ các field, nội dung truyền lên url false nó không kiểm tra.
        public ActionResult EditNhanVien(NHANVIEN nv)
        {
            // thêm vào csdl 
            if (ModelState.IsValid)
            {
                // thực hiện cập nhật trong model
                db.Entry(nv).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["ThongBao"] = "Chỉnh sửa nhân viên thành công!";
            }
            else
            {
                TempData["ThongBao"] = "Chỉnh sửa nhân viên không thành công!";
            }
            return View(nv);
        }
        public ActionResult DetailNhanVien(int maNV)
        {
            NHANVIEN nv = db.NHANVIENs.SingleOrDefault(m => m.MaNV == maNV);
            if (nv == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nv);
        }
        public ActionResult DeleteNhanvien(int? maNV)
        {
            if (maNV == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHANVIEN nv = db.NHANVIENs.Find(maNV);
            if (nv == null)
            {
                return HttpNotFound();
            }
            return View(nv);
        }
        [HttpPost, ActionName("DeleteNhanVien")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCofirm(int maNV)
        {
            NHANVIEN category = db.NHANVIENs.Find(maNV);
            db.NHANVIENs.Remove(category);
            db.SaveChanges();
            TempData["ThongBao"] = "Xóa nhân viên thành công!";
            return RedirectToAction("DanhSachNV");
        }
    }
}