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


        public ActionResult Movimientos(int id)
        {
            List<Fichas> lista = new List<Fichas>();
            List<TableResponsableViewModel> modelResponsable = new List<TableResponsableViewModel>();
            TablaMovimientoViewModels modelMovimientos = new TablaMovimientoViewModels();


            //conexion base datos
            using (Prueba1Entities db = new Prueba1Entities())
            {
                modelResponsable = (from d in db.Responsable
                                    select new TableResponsableViewModel
                                    {
                                        Clave_R = d.Clave_R,
                                        Nombre = d.Nombre,
                                        Cargo = d.Cargo
                                    }).ToList();

                modelMovimientos.Clave_R = modelResponsable[id].Clave_R;
                modelMovimientos.Nombre = modelResponsable[id].Nombre;
                modelMovimientos.Cargo = modelResponsable[id].Cargo;

                modelMovimientos.ListaFichas = (from f in db.Ficha
                                                where f.Responsable_Clave_R == id
                                                select new Fichas
                                                {
                                                    Clave_F = f.Clave_F,
                                                    Fecha = (DateTime)f.Fecha,
                                                    Origen = f.Origen,
                                                    Destino = f.Destino,
                                                    TipoMovimiento = f.TipoMovimiento,
                                                    ResponsableDelMovimiento = f.ResponsableDelMovimiento,

                                                }).ToList();

            }

            return View(modelMovimientos);

             

        }



    }
}