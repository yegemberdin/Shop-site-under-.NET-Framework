using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Projectnaz.Models;

namespace Projectnaz.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {

        private Database1 db = new Database1();

        public ActionResult Index()
        {
            var featured = db.Products.Where(i => i.Featured == true).ToList();
            var top = db.Products
                .GroupBy(t => t.Type)
                .SelectMany(i => i.OrderByDescending(p => p.Purchases)
                .Take(1))
                .Take(3)
                .ToList();
            var newItems = db.Products
                .OrderByDescending(i => i.DateCreated)
                .Take(3)
                .ToList();

            //for (int i = 0; i < top.Count; i++)
            //{
            //    var item = top[i];
            //    item.Picture = Server.MapPath(item.Picture);
            //}

            ViewData["featured"] = featured;
            ViewData["top"] = top;
            ViewData["new"] = newItems;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}