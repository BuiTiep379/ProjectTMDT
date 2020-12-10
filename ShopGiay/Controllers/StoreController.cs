using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopGiay.Controllers
{
    public class StoreController : Controller
    {
        // GET: Store
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Shop()
        {
            return View();
        }
        [ChildActionOnly]
        public ActionResult SiteNavBar()
        {
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