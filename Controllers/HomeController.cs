using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Web4.Models;
using Web4.Models.ViewModels;

namespace Web4.Controllers
{
    public class HomeController : Controller
    { 
        public ActionResult Index()
        {
            return View(); 
        }

        public JsonResult BuscarPorNombre(string serie, int cantidad)
        {
            try
            {
                Prueba1Entities db = new Prueba1Entities();
                var lista = db.buscarSerie(serie, cantidad);
                return Json(lista, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
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