using Rotativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Web4.Models;
using Web4.Models.ViewModels;

namespace Web4.Controllers
{
    public class MovimientoCelularController : Controller
    {
        // GET: MovimientoCelular
        public ActionResult AnadirCelular()
        {
            List<ViewModelResponsable> lista = new List<ViewModelResponsable>();

            using ( Prueba1Entities db = new Prueba1Entities())
            {
                lista = (from d in db.Responsable
                         select new ViewModelResponsable
                         {
                             Clave_R = d.Clave_R,
                             Nombre = d.Nombre,
                             Cargo = d.Cargo,
                         }).ToList();

            }

            return View(lista); 
        }

        [HttpPost]
        public ActionResult AnadirCelular(MovimientoCelularViewModel model)
        {
            try
            {
                /*List<ViewModelResponsable> lista = new List<ViewModelResponsable>();*/

                Responsable oResponsable= new Responsable();

                using (Prueba1Entities db = new Prueba1Entities())
                {
                    oResponsable = db.Responsable.Find(model.Clave_R);

                    /*lista = (from d in db.Responsable
                             where d.Clave_R == model.Clave_R
                             select new ViewModelResponsable
                             {
                                 Clave_R = d.Clave_R,
                                 Nombre = d.Nombre,
                                 Cargo = d.Cargo

                             }).ToList();*/


                    //Responsable oResponsable = new Responsable();
                    //oResponsable.Cargo = lista.Cargo;

                    //db.Responsable.Add(oResponsable);
                    //db.SaveChanges();

                    /*model.Nombre = lista[0].Nombre;*/
                    model.Nombre = oResponsable.Nombre;

                    Ficha oFicha = new Ficha();
                    oFicha.Fecha = DateTime.Now;
                    oFicha.Origen = model.Origen;
                    oFicha.Destino = model.Destino;
                    oFicha.TipoMovimiento = model.TipoMovimiento;
                    oFicha.ResponsableDelMovimiento = model.ResponsableDelMovimiento;
                    oFicha.Responsable_Clave_R = model.Clave_R;

                    db.Ficha.Add(oFicha);
                    db.SaveChanges();

                    model.Clave_F = oFicha.Clave_F;

                    int i = 0;
                    
                    foreach (var mE in model.EquiposCelulares)
                    {
                        Bien oBien = new Bien();
                        oBien.Descripcion = mE.Descripcion;
                        oBien.Marca = mE.Marca;
                        oBien.Modelo = mE.Modelo;
                        oBien.IMEI = mE.IMEI;
                        oBien.Linea = mE.Linea;
                        oBien.Cargador = mE.Cargador;
                        oBien.CableDatos = mE.CableDatos;
                        oBien.Audifono = mE.Audifono;
                        oBien.Estado = mE.Estado;
                        oBien.Entregado = mE.Entregado;

                        db.Bien.Add(oBien);

                        db.SaveChanges();

                        //aqui 
                        model.EquiposCelulares[i].Clave_B = oBien.Clave_B;
                        i = i + 1;

                        Detalle oDetalle = new Detalle();
                        oDetalle.Ficha_Clave_F = oFicha.Clave_F;
                        oDetalle.Bien_Clave_B = oBien.Clave_B;

                        db.Detalle.Add(oDetalle);

                    }
                    
                    db.SaveChanges();

                    ViewBag.Message = "Registro insertado";

                    TempData["MovimientoCelularViewModel"] = model;
                    return Redirect("~/MovimientoCelular/Ficha"); 


                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "No insertado Error";
                return View(ex);
            }
        }

        [HttpGet]
        public ActionResult Ficha()
        {
            MovimientoCelularViewModel model = TempData["MovimientoCelularViewModel"] 
                as MovimientoCelularViewModel;
        
            if (model != null)
                return View(model);
            else
                return View();
        }
         
    }
}