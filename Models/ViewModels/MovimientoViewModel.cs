using System;
using System.Collections.Generic;
using System.Linq;
using System.Web; 

namespace Web4.Models.ViewModels
{
    public class MovimientoViewModel
    {
        public string Nombre { set; get; }
        public string Cargo { set; get; }

        public DateTime Fecha { set; get; }
        public string Origen { set; get; }
        public string Destino { set; get; }
        public string TipoMovimiento { set; get; }
        public string ResponsableDelMovimiento { set; get; }

        public List<Bienes> Equipos { set; get; }
        
    }

    public class Bienes
    {
        public string Descripcion { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Serie { get; set; }
        public string Estado { get; set; }
        public string UsuarioPC { get; set; }
        public string NombrePC { get; set; }
        public bool Devuelto { get; set; }
        
    }
}