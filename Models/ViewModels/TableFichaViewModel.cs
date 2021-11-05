using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web4.Models.ViewModels
{
    public class TableFichaViewModel
    {
        public int Clave_F { get; set; }
        public DateTime Fecha { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public string TipoMovimiento { get; set; }
        public string ResponsableDelMovimiento { get; set; }
        public string Observacion { get; set; }
        public string CargoDeLaEpoca { get; set; }
        public int Responsable_Clave_R { get; set; } 
    }
}