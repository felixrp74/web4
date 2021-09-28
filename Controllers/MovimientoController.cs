using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web4.Models;
using Web4.Models.ViewModels;

namespace Web4.Controllers
{
    public class MovimientoController : Controller
    {
        // GET: Movimiento
        public ActionResult Anadir()
        {
            return View();
        }
        //POST: Movimiento

        [HttpPost]
        public ActionResult Anadir(MovimientoViewModel model)
        {
            try
            {
                using (Prueba4Entities1 db = new Prueba4Entities1())
                {
                    Responsable oResponsable = new Responsable();
                    oResponsable.Nombre = model.Nombre;
                    oResponsable.Cargo = model.Cargo;

                    db.Responsable.Add(oResponsable);
                    db.SaveChanges();

                    
                    Ficha oFicha = new Ficha();
                    oFicha.Fecha = DateTime.Now;
                    oFicha.Origen = model.Origen;
                    oFicha.Destino = model.Destino;
                    oFicha.TipoMovimiento = model.TipoMovimiento;
                    oFicha.ResponsableDelMovimiento = model.ResponsableDelMovimiento;
                    oFicha.Responsable_Clave_R = oResponsable.Clave_R;

                    db.Ficha.Add(oFicha);
                    db.SaveChanges();


                    foreach (var mE in model.Equipos)
                    {
                        Bien oBien = new Bien();
                        oBien.Descripcion = mE.Descripcion;
                        oBien.Marca = mE.Marca;
                        oBien.Modelo = mE.Modelo;
                        oBien.Serie = mE.Serie;
                        oBien.Estado = mE.Estado;
                        oBien.UsuarioPC = mE.UsuarioPC;
                        oBien.NombrePC = mE.NombrePC;
                        oBien.Devuelto = mE.Devuelto;

                        db.Bien.Add(oBien);

                        db.SaveChanges();

                        Detalle oDetalle = new Detalle();
                        oDetalle.Ficha_Clave_F = oFicha.Clave_F;
                        oDetalle.Bien_Clave_B = oBien.Clave_B;

                        db.Detalle.Add(oDetalle);

                    } 

                    db.SaveChanges();
                    
                    ViewBag.Message = "Registro insertado";

                    return View();

                }
            }
            catch(Exception ex)
            {
                ViewBag.Message = "No insertado Error";
                return View();
            }
             
             
        }
    }
}