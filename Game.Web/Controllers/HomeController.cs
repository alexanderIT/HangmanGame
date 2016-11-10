using System;
using System.Linq;
using System.Web.Mvc;
using Game.BL;
using Game.Web.Models;

namespace Game.Web.Controllers
{
    public class HomeController : BaseController
    {
        
        public HomeController()
        {
            
        }
        public ActionResult Index()
        {
            var type= Enum.GetValues(typeof(TypeOfWord))
                    .Cast<TypeOfWord>()
                    .Select(x => new SelectListItem { Value = ((int)x).ToString(), Text = x.ToString() })
                    .ToList();

            var model =new HomeViewModel
            {
                TypeOfWord = type
            };
            return View(model);
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