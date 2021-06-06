using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication6.Entities
{
    public class Equipos
    {
        public List<DatosEquipo> Equipo { get; set; }
    }
    public class DatosEquipo
    {
        public string Nombre { get; set; }
        public int CantidadSocios { get; set; }
        public int PromedioEdad { get; set; }
        public int MenorEdad { get; set; }
        public int MayorEdad { get; set; }

    }
}