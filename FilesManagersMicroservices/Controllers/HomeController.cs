using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using FilesManagersMicroservices.Core;
using FilesManagersDbContext.Models;


namespace FilesManagersMicroservices.Controllers
{
    
    public class HomeController : Controller
    {
        public string ConfigurationApi { get; }
        protected FilesManagersMicroservicesEntities FilesManagers { get; set; }

        public HomeController()
        {
            ConfigurationApi = WebConfigurationManager.AppSettings["ApiKey"];
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




        public JsonResult CreateDirectory(string name, string key)
        {
            return ConfigurationApi != key ? Json(500, JsonRequestBehavior.AllowGet) : Json(new BuisnessProcess().CreateDirectory(name), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteDirectory(string name, string key)
        {
            return ConfigurationApi != key ? Json(500, JsonRequestBehavior.AllowGet) : Json(new BuisnessProcess().DeleteDirectory(name), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteMassDirectory(string[] name, string key)
        {
            return ConfigurationApi != key ? Json(500, JsonRequestBehavior.AllowGet) : Json(new BuisnessProcess().DeleteMassDirectory(name), JsonRequestBehavior.AllowGet);
        }

        public JsonResult RenameDirectory(string oldname, string newname, string key)
        {
            return ConfigurationApi != key ? Json(500, JsonRequestBehavior.AllowGet) : Json(new BuisnessProcess().RenameDirectory(oldname,newname), JsonRequestBehavior.AllowGet);
        }

        public JsonResult RenameFile(string oldname, string newname, string key)
        {
            return ConfigurationApi != key ? Json(500, JsonRequestBehavior.AllowGet) : Json(new BuisnessProcess().RenameFile(oldname, newname), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteFile(string name, string key)
        {
            return ConfigurationApi != key ? Json(500, JsonRequestBehavior.AllowGet) : Json(new BuisnessProcess().DeleteFile(name), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteMassFile(string[] name, string key)
        {
            return ConfigurationApi != key ? Json(500, JsonRequestBehavior.AllowGet) : Json(new BuisnessProcess().DeleteMassFile(name), JsonRequestBehavior.AllowGet);
        }

        public JsonResult SendEmail(string[] files,string email, string key)
        {
            return ConfigurationApi != key ? Json(500, JsonRequestBehavior.AllowGet) : Json(new BuisnessProcess().SendEmail(files,email), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUrlForFile(string file, string key)
        {
            return ConfigurationApi != key ? Json(500, JsonRequestBehavior.AllowGet) : Json(new BuisnessProcess().GetUrlForFile(file), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSizeGlobalPath(string key)
        {
            return ConfigurationApi != key ? Json(500, JsonRequestBehavior.AllowGet) : Json(new BuisnessProcess().GetSizeGlobalPath(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UploadFiles(HttpPostedFileBase file, string dirname, string key)
        {
            return ConfigurationApi != key ? Json(500, JsonRequestBehavior.AllowGet) : Json(new BuisnessProcess().UploadFileInDirectory(file, dirname), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveFileInDb(string filename, HttpPostedFileBase file, string key)
        {
            return ConfigurationApi != key ? Json(500, JsonRequestBehavior.AllowGet) : Json(new BuisnessProcess().SaveFileInDb(filename, file), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFile(string name, string key)
        {
            if (ConfigurationApi == key)
            {
                return new BuisnessProcess().GetFile(name);
            }
            return RedirectToAction("None");
        }


        public ActionResult GetZipFolder(string name, string key)
        {
            if (ConfigurationApi == key)
            {
                return new BuisnessProcess().GetZipFolder(name);
            }
            return RedirectToAction("None");
        }


        public ActionResult None()
        {
            return View();
        }

        public ActionResult UploadFiles()
        {
            return View();
        }

        public ActionResult SaveFileInDb()
        {
            return View();
        }

        public ActionResult GetFilesFromDb()
        {
            var model = FilesManagers.Files.ToList();
            return View(model);
        }
    }
}