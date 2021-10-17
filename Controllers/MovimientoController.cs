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


        [HttpPost]
        public ActionResult Editar(MovimientoViewModel modelMov)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    List<ViewModelResponsable> lista = new List<ViewModelResponsable>();

                    using (Prueba1Entities db = new Prueba1Entities())
                    {
                        var oResponsable = db.Responsable.Find(modelMov.Clave_R);
                        oResponsable.Cargo = modelMov.Nombre;
                        db.Entry(oResponsable).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        var oFicha = db.Ficha.Find(modelMov.Clave_F);                        
                        oFicha.Fecha = DateTime.Now;
                        oFicha.Origen = modelMov.Origen;
                        oFicha.Destino = modelMov.Destino;
                        oFicha.TipoMovimiento = modelMov.TipoMovimiento;
                        oFicha.ResponsableDelMovimiento = modelMov.ResponsableDelMovimiento;
                        oFicha.Responsable_Clave_R = modelMov.Clave_R; //borrar linea para no perder la relacion entre Responsable y Ficha.
                        db.Entry(oFicha).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        var oDetalle = db.Detalle.Find(modelMov.Clave_F);

                        foreach (var mE in modelMov.Equipos)
                        {
                            /*
                            var oBien = db.Bien.Find(modelMov.);
                            oBien.Descripcion = mE.Descripcion;
                            oBien.Marca = mE.Marca;
                            oBien.Modelo = mE.Modelo;
                            oBien.Serie = mE.Serie;
                            oBien.Estado = mE.Estado;
                            oBien.UsuarioPC = mE.UsuarioPC;
                            oBien.NombrePC = mE.NombrePC;
                            oBien.Devuelto = mE.Devuelto;

                            //db.Bien.Add(oBien);
                            db.Entry(oBien).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();

                            Detalle oDetalle = new Detalle();
                            oDetalle.Ficha_Clave_F = oFicha.Clave_F;
                            oDetalle.Bien_Clave_B = oBien.Clave_B;

                            db.Detalle.Add(oDetalle);
                            */
                        }

                        db.SaveChanges();

                        ViewBag.Message = "Registro insertado";


                        TempData["MovimientoViewModel"] = modelMov;
                        return Redirect("~/Movimiento/Ficha");
                        //return RedirectToAction("Imprimir", "Home2", data);
                        //return RedirectToAction("Ficha", "Home", model);
                        //return RedirectToAction("~/Controllers/Home/Ficha", model);
                        //return View();


                    }
                     
                }
                return View(modelMov);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [HttpGet]
        public ActionResult Editar(int id_R, int id_F )
        {
            MovimientoViewModel modelMovimiento = new MovimientoViewModel();
            List<TableResponsableViewModel> modelResponsable = new List<TableResponsableViewModel>();
            List<TableFichaViewModel> modelFicha = new List<TableFichaViewModel>();

            using (Prueba1Entities db = new Prueba1Entities())
            {
                modelResponsable = (from d in db.Responsable
                                    where d.Clave_R == id_R
                                    select new TableResponsableViewModel
                                    {
                                        Clave_R = d.Clave_R,
                                        Nombre = d.Nombre,
                                        Cargo = d.Cargo
                                    }).ToList();

                modelMovimiento.Clave_R = modelResponsable[0].Clave_R;
                modelMovimiento.Nombre = modelResponsable[0].Nombre;
                modelMovimiento.Cargo = modelResponsable[0].Cargo;

                modelFicha = (from f in db.Ficha
                              where f.Clave_F == id_F // las fichas son unicas y pertenecen a alguien
                              select new TableFichaViewModel
                              {
                                  Clave_F = f.Clave_F,
                                  Fecha = (DateTime)f.Fecha,
                                  Origen = f.Origen,
                                  Destino = f.Destino,
                                  TipoMovimiento = f.TipoMovimiento,
                                  ResponsableDelMovimiento = f.ResponsableDelMovimiento

                              }).ToList();

                modelMovimiento.Fecha = modelFicha[0].Fecha;
                modelMovimiento.Origen = modelFicha[0].Origen;
                modelMovimiento.Destino = modelFicha[0].Destino;
                modelMovimiento.TipoMovimiento = modelFicha[0].TipoMovimiento;
                modelMovimiento.ResponsableDelMovimiento = modelFicha[0].ResponsableDelMovimiento;

                modelMovimiento.Equipos = (from b in db.Bien
                                           from d in db.Detalle
                                           where d.Ficha_Clave_F == id_F
                                               && d.Bien_Clave_B == b.Clave_B
                                           select new Bienes
                                           {
                                               Descripcion = b.Descripcion,
                                               Marca = b.Marca,
                                               Modelo = b.Modelo,
                                               Serie = b.Serie,
                                               Estado = b.Estado,
                                               UsuarioPC = b.UsuarioPC,
                                               NombrePC = b.NombrePC,
                                               Entregado = b.Entregado
                                           }
                              ).ToList();
                 
            }
            return View(modelMovimiento);
        }


        

        // GET: Movimiento
        public ActionResult Anadir()
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

        
        //POST: Movimiento

        [HttpPost]
        public ActionResult Anadir(MovimientoViewModel model)
        {
             
            try
            {
                List<ViewModelResponsable> lista = new List<ViewModelResponsable>();

                using ( Prueba1Entities db = new Prueba1Entities() )
                {

                    lista = (from d in db.Responsable
                                   where d.Clave_R == model.Clave_R
                                   select new ViewModelResponsable
                                   {
                                       Clave_R = d.Clave_R,
                                       Nombre = d.Nombre,
                                       Cargo = d.Cargo

                                   }).ToList();

                
                    //Responsable oResponsable = new Responsable();
                    //oResponsable.Cargo = lista.Cargo;

                    //db.Responsable.Add(oResponsable);
                    //db.SaveChanges();

                    model.Nombre = lista[0].Nombre;

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
                        oBien.Entregado = mE.Entregado;

                        db.Bien.Add(oBien);

                        db.SaveChanges();

                        //aqui 
                        model.Equipos[i].Clave_B = oBien.Clave_B;
                        i = i + 1;

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
            catch (Exception ex)
            {
                ViewBag.Message = "No insertado Error";
                return View(ex);
            }

        }

        
        public ActionResult Ficha()
        {
            MovimientoViewModel model = TempData["MovimientoViewModel"] as MovimientoViewModel;
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
                       || d.Cargo.Contains(searchValue)
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