using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web4.Models;
using Web4.Models.ViewModels;

namespace Web4.Controllers
{
    public class ResponsableController : Controller
    {
        // GET: Responsable
        public ActionResult Index()
        {
            List<TableResponsableViewModel> model = new List<TableResponsableViewModel>();
            
            //conexion base datos
            using (Prueba1Entities db = new Prueba1Entities())
            {
                model = (from d in db.Responsable
                       select new TableResponsableViewModel
                       {
                           Clave_R = d.Clave_R,
                           Nombre = d.Nombre,
                           Cargo = d.Cargo
                       }).ToList();

                //recordsTotal = lst.Count();
                //lst = lst.Skip(skip).Take(pageSize).ToList();

            }

            return View(model);
        }

        public ActionResult Movimientos(int Clave_R)
        {
            List<TablaMovimientoViewModels> lista = new List<TablaMovimientoViewModels>();
            /*
            var jobs =
               from h in _context.PostThrs
               join e in _context.PostEigs on h.ThrZero equals e.EigZero
               where h.ThrDate > today && h.ThrText == "SERVICE DATE"
                  && e.EigAgen == "OPEN"
               select new AgentClientIndexVM
               {
                   Zero = h.ThrZero,
                   ThrDate = h.ThrDate,
                   ThrTime = h.ThrTime,
                   ThrText = h.ThrText,
                   EigAgen = e.EigAgen,
                   EigRole = e.EigRole,
                   EigLoad = e.EigLoad,
                   EigNote = e.EigNote
               };
            */

            using ( Prueba1Entities db = new Prueba1Entities() )
            {
                lista = (
                    from r in db.Responsable
                    join f in db.Ficha on r.Clave_R equals f.Responsable_Clave_R
                    select new TablaMovimientoViewModels
                    {

                    }).ToList();
            }

            return View();
        }
         

    }
}