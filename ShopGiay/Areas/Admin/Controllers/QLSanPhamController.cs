using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopGiay.Models;
using PagedList;

namespace ShopGiay.Areas.Admin.Controllers
{
    public class QLSanPhamController : Controller
    {
        ShopGiayEntities db = new ShopGiayEntities();
        // GET: Admin/QLSanPham
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DanhSachSanPham(string search, int? page, int? size)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "5", Value = "5" });
            items.Add(new SelectListItem { Text = "10", Value = "10" });

            foreach (var item in items)
            {
                if (item.Value == size.ToString())
                   item.Selected = true;
            }

            ViewBag.Size = items;
            ViewBag.CurrentSize = size;

            page = (page ?? 1);
            int pageNumber = (page ?? 1);
            int pageSize = (size ?? 5);
            var listSP = from sp in db.SANPHAMs select sp;
            listSP = listSP.OrderBy(x => x.MaSP);

            if (search != null)
            {
                listSP = listSP.Where(x => x.TenSP.Contains(search));
            }
            return View(listSP.ToPagedList(pageNumber, pageSize));
        }

       
    }
}