using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TinyCsvParser.Mapping;

namespace WebApplication6.Entities
{
    public class CsvPersonMapping : CsvMapping<Socios>
    {
        public CsvPersonMapping()
            : base()
        {
            MapProperty(0, x => x.Nombre);
            MapProperty(1, x => x.Edad);
            MapProperty(2, x => x.Equipo);
            MapProperty(3, x => x.EstadoCivil);
            MapProperty(4, x => x.NivelEstudios);
        }
    }
}