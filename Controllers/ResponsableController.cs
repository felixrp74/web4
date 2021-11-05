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
        
        [HttpPost]
        public ActionResult Imprimir()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Imprimir(int id_R, int id_F)
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


                return new ViewAsPdf("Imprimir", modelMovimiento)
                {
                    // Establece la Cabecera y el Pie de página
                    /*
                    CustomSwitches = "--header-html " + _headerUrl + " --header-spacing 0 " +
                                     "--footer-html " + _footerUrl + " --footer-spacing 0"
                    ,
                    */
                    PageSize = Rotativa.Options.Size.A4,
                    //FileName = "CustomersLista.pdf", // SI QUEREMOS QUE EL ARCHIVO SE DESCARGUE DIRECTAMENTE
                    PageMargins = new Rotativa.Options.Margins(40, 10, 10, 10)
                };
            }
            //return View();
        }


        [HttpGet]
        public ActionResult Ficha(int id_R, int id_F)
        {
            MovimientoViewModel modelMovimiento = new MovimientoViewModel();
            List<TableResponsableViewModel> modelResponsable = new List<TableResponsableViewModel>();
            List<TableFichaViewModel> modelFicha = new List<TableFichaViewModel>();

            using ( Prueba1Entities db = new Prueba1Entities() )
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
                                  Fecha = (DateTime) f.Fecha,
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

                modelMovimiento.Equipos = ( from b in db.Bien 
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

        public ActionResult Movimientos(int id)
        {
            List<Fichas> lista = new List<Fichas>();
            List<TableResponsableViewModel> modelResponsable = new List<TableResponsableViewModel>();
            TablaMovimientoViewModels modelMovimientos = new TablaMovimientoViewModels();


            //conexion base datos
            using (Prueba1Entities db = new Prueba1Entities())
            {

                var mResponsable = db.Responsable.Find(id);

                //id = id - 1;
                modelMovimientos.Clave_R = mResponsable.Clave_R;
                modelMovimientos.Nombre = mResponsable.Nombre;
                modelMovimientos.Cargo = mResponsable.Cargo;

                modelMovimientos.ListaFichas = (from f in db.Ficha
                                                where f.Responsable_Clave_R == id
                                                select new Fichas
                                                {
                                                    Clave_F = f.Clave_F,
                                                    Fecha = (DateTime) f.Fecha,
                                                    Origen = f.Origen,
                                                    Destino = f.Destino,
                                                    TipoMovimiento = f.TipoMovimiento,
                                                    ResponsableDelMovimiento = f.ResponsableDelMovimiento
                                                    //TipoFicha = (int) f.TipoFicha

                                                }).ToList();

            }

            return View(modelMovimientos);

        }


    }
}