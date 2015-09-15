using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSoftSeo.Migrations;
using WebSoftSeo.Models;

namespace WebSoftSeo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //var configuration = new Configuration();
            //var migrator = new DbMigrator(configuration);
            //migrator.Update();
            //ViewBag.Message = "Your app description page.";
            

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