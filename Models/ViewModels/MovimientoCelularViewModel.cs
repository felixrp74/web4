using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web4.Models;

namespace Web4.Models.ViewModels
{
    public class MovimientoCelularViewModel
    {
        public int Clave_R { get; set; }
        public string Nombre { set; get; }
        public string Cargo { set; get; }
        public string DNI { get; set; }
        public string CodPlanilla { get; set; }
        public string CargoDeLaEpoca { get; set; }

        public int Clave_F { get; set; }
        public DateTime Fecha { set; get; }
        public string Origen { set; get; }
        public string Destino { set; get; }
        public string TipoMovimiento { set; get; }
        public string ResponsableDelMovimiento { set; get; }
        public string Observacion { get; set; }
        public int TipoFicha { get; set; }

        public List<Celular> EquiposCelulares { set; get; }
    }

    public class Celular
    {
        public int Clave_B { get; set; }
        public string Descripcion { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string IMEI { get; set; }
        public string Linea { get; set; }
        public string Cargador { get; set; }
        public string CableDatos { get; set; }
        public string Audifono { get; set; }
        public string Estado { get; set; }
        public string Entregado { get; set; }

    }
}