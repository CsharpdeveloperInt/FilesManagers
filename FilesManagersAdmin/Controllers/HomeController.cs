using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FilesManagersDbContext.Models;

namespace FilesManagersAdmin.Controllers
{
    [AllowAnonymous]
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

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public JsonResult UserLogin(users u)
        {
            var user = FilesManagers.users.FirstOrDefault(a => a.login.Equals(u.login) && a.pass.Equals(u.pass));

            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);
               
                Session["Autorize"] = user.Id;
               
                Redirect(Request.UrlReferrer.ToString());
            }

            return Json(user, JsonRequestBehavior.AllowGet);
        }
    }
}