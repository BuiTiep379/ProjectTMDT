using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopGiay.Models;
using PagedList;

namespace ShopGiay.Controllers
{
    public class SanPhamController : Controller
    {
        ShopGiayEntities db = new ShopGiayEntities();
        // GET: SanPham
        public ActionResult Index()
        {
            return View();
        }
        // danh sách sản phẩm ở header
        [ChildActionOnly]
        public ActionResult SanPhamNoiBat()
        {
            var listSP = db.SANPHAMs.Take(5);
            return PartialView(listSP);
        }
        //Danh sách sản phẩm ở body store
        public ActionResult DanhSachSanPhamNoiBat()
        {
            var ListSP = db.SANPHAMs.OrderBy(x => x.MaSP).Take(9);
            return PartialView(ListSP);
        }
        public ActionResult DanhSachSanPham(string search, int? page, int? size)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "3", Value = "3" });
            items.Add(new SelectListItem { Text = "6", Value = "6" });

            foreach (var item in items)
            {
                if (item.Value == size.ToString())
                    item.Selected = true;
            }

            ViewBag.Size = items;
            ViewBag.CurrentSize = size;

            page = (page ?? 1);
            int pageNumber = (page ?? 1);
            int pageSize = (size ?? 3);
            var listSP = from sp in db.SANPHAMs select sp;
            listSP = listSP.OrderBy(x => x.MaSP);
            if (search != null)
            {
                listSP = listSP.Where(x => x.TenSP.Contains(search));
                return PartialView(listSP.ToPagedList(pageNumber, pageSize));
            }
            return PartialView(listSP.ToPagedList(pageNumber, pageSize));
        }
        
    }
}