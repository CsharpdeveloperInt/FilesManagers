using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using FilesManagersDbContext.Models;

namespace FilesManagersDbContext.Controllers
{
    public class HomeController : Controller
    {
        protected FilesManagersMicroservicesEntities FilesManagers { get; set; }

        public HomeController()
        {
            //ConfigurationApi = WebConfigurationManager.AppSettings["ApiKey"];
            FilesManagers = new FilesManagersMicroservicesEntities();
        }

        public ActionResult Index()
        {
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

        public JsonResult UserLogin(users u)
        {
            var user = FilesManagers.users.FirstOrDefault(a => a.login.Equals(u.login) && a.pass.Equals(u.pass));
            return Json(user, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Login()
        {
            return View();
        }
    }
}