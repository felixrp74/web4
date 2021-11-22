using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web4.Models.ViewModels
{
    public class UltimoResponsableViewModel
    {
        public UltimoResponsableViewModel(string Nombre, string Descripcion, string Serie, string IMEI, string UsuarioPC)
        {
            this.Nombre = Nombre;
            this.Descripcion = Descripcion;
            this.Serie = Serie;
            this.IMEI = IMEI;
            this.UsuarioPC = UsuarioPC;
        }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Serie { get; set; }
        public string IMEI { get; set; }
        public string UsuarioPC { get; set; }
    }
}