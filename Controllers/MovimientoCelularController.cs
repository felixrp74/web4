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
                             CargoView = d.Cargo,
                         }).ToList();

            }

            return View(lista); 
        }
        

        [HttpPost]
        public ActionResult AnadirCelular(MovimientoCelularViewModel model)
        {
            try
            {
                if (model.EquiposCelulares == null)
                    return Redirect("~/MovimientoCelular/AnadirCelular");
                /*List<ViewModelResponsable> lista = new List<ViewModelResponsable>();*/

                Responsable oResponsable = new Responsable(); 

                using (Prueba1Entities db = new Prueba1Entities())
                {
                    oResponsable = db.Responsable.Find(model.Clave_R);                     
                    /* 
                    model.Cargo = oResponsable.Cargo;
                    */
                    model.Nombre = oResponsable.Nombre;
                    model.DNI = oResponsable.DNI;
                    model.CodPlanilla = oResponsable.CodPlanilla;
                    
                    Ficha oFicha = new Ficha();
                    oFicha.Fecha = DateTime.Now;
                    oFicha.Origen = model.Origen;
                    oFicha.Destino = model.Destino;
                    oFicha.TipoMovimiento = model.TipoMovimiento;
                    oFicha.ResponsableDelMovimiento = model.ResponsableDelMovimiento;
                    oFicha.Observacion = model.Observacion;
                    oFicha.Responsable_Clave_R = model.Clave_R;
                    oFicha.TipoFicha = model.TipoFicha;
                    oFicha.CargoDeLaEpoca = model.Cargo;

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

                        //esta linea debe estar despues de save changes para poder enviar id del bien. 
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