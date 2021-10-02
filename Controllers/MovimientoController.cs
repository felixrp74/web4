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
        public string draw = "";
        public string start = "";
        public string length = "";
        public string sortColumn = "";
        public string sortColumnDir = "";
        public string searchValue = "";
        public int pageSize, skip, recordsTotal;

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
                using ( Prueba1Entities db = new Prueba1Entities())
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

                    TempData["MovimientoViewModel"] = model;
                    return Redirect("~/Movimiento/Ficha");
                    //return RedirectToAction("Imprimir", "Home2", data);
                    //return RedirectToAction("Ficha", "Home", model);
                    //return RedirectToAction("~/Controllers/Home/Ficha", model);
                    //return View();


                }
            }
            catch(Exception ex)
            {
                ViewBag.Message = "No insertado Error";
                return View(ex);
            }
             
             
        }
        
        public ActionResult Ficha()
        {
            var model = TempData["MovimientoViewModel"] as MovimientoViewModel;
            if (model != null)
                return View(model);
            else
                return View();
        }
    
        public ActionResult Vista()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Json()
        {
            List<TableResponsableViewModel> lst = new List<TableResponsableViewModel>();

            //logistica datatables
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortColumn = Request.Form.GetValues("columns["+Request.Form.GetValues("order[0][column]").FirstOrDefault()+"][name]" ).FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

            pageSize = length != null ? Convert.ToInt32(length) : 0;
            skip = start != null ? Convert.ToInt32(start) : 0;
            recordsTotal = 0;

            //conexion base datos
            using (Prueba1Entities db = new Prueba1Entities())
            {
                lst = (from d in db.Responsable
                       where d.Nombre.Contains(searchValue)
                       select new TableResponsableViewModel
                       {
                           Clave_R = d.Clave_R,
                           Nombre = d.Nombre,
                           Cargo = d.Cargo
                       }).ToList();

                recordsTotal = lst.Count();
                lst = lst.Skip(skip).Take(pageSize).ToList();

            }

            return Json(new { draw=draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data=lst });
        }

    }
}