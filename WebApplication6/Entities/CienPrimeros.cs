using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication6.Entities
{
    public class ListCienPrimeros
    {
        public List<CienPrimeros> AlarmaObjeto { get; set; }
    }
    public class CienPrimeros
    {
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public string Equipo { get; set; }
    }
}