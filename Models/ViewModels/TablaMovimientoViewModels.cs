using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web4.Models.ViewModels
{
    public class TablaMovimientoViewModels
    {
        public int Clave_R { get; set; }
        public string Nombre { get; set; }
        public string Cargo { get; set; }
        public List<Fichas> ListaFichas { get; set; }
    }

    public class Fichas
    {
        public int Clave_F { get; set; }
        public DateTime Fecha { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public string TipoMovimiento { get; set; }
        public string ResponsableDelMovimiento { get; set; }
        public int TipoFicha { get; set; }
        public int Responsable_Clave_R { get; set; }

    }

}