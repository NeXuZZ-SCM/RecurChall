using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TinyCsvParser.Mapping;
using TinyCsvParser;
using System.Text;
using WebApplication6.Entities;

namespace WebApplication6
{
    public partial class Default : System.Web.UI.Page
    {
        #region Definicion variables & constantes
        int cantidadSocios, edadTotales, sociosRacing;
        float promedioEdadesRacing;
        ListCienPrimeros ListadoCien;
        IEnumerable<KeyValuePair<string, int>> top5;
        Equipos ListadoEquipos;
        private const string CSS_CLASS_BULLETED = "BulletedListCssClass";
        private const string RIVER = "River";
        private const string RACING = "Racing";
        private const int LISTADO_CIEN = 100;
        private const string ESTADO_CIVIL = "Casado";
        private const string NIVEL_DE_ESTUDIO = "Universitario";
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            BulletedList1.CssClass = CSS_CLASS_BULLETED;
            BulletedList2.CssClass = CSS_CLASS_BULLETED;
            BulletedList3.CssClass = CSS_CLASS_BULLETED;
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (!ComprobarCargaCSV())
            {
                return;
            }
            

            string filePath = Server.MapPath($"~/Files/{fileUpload.FileName}");
            fileUpload.SaveAs(filePath);
            
            CsvParserOptions csvParserOptions = new CsvParserOptions(true, ';');
            CsvPersonMapping csvMapper = new CsvPersonMapping();
            CsvParser<Socios> csvParser = new CsvParser<Socios>(csvParserOptions, csvMapper);

            List<CsvMappingResult<Socios>> socios = csvParser
                .ReadFromFile(filePath, Encoding.Default)
                .ToList();

            cantidadSocios = socios.Count + 1;
            edadTotales = 0;
            sociosRacing = 0;

            ListadoCien = new ListCienPrimeros();
            ListadoCien.AlarmaObjeto = new List<CienPrimeros>();
            int countSocios = 0;

            IDictionary<string, int> cantidadNombresRiver = new Dictionary<string, int>();

            foreach (CsvMappingResult<Socios> socio in socios)
            {
                if (socio.Result.Equipo == RACING)
                {
                    sociosRacing += 1;
                    edadTotales += socio.Result.Edad;
                }
                if (socio.Result.Equipo == RIVER)
                {
                    if (cantidadNombresRiver.ContainsKey(socio.Result.Nombre))
                    {
                        cantidadNombresRiver[socio.Result.Nombre] += 1;
                    }
                    else
                    {
                        cantidadNombresRiver.Add(socio.Result.Nombre, 1);
                    }

                }


                if (socio.Result.EstadoCivil == ESTADO_CIVIL && socio.Result.NivelEstudios == NIVEL_DE_ESTUDIO && countSocios < LISTADO_CIEN)
                {
                    CienPrimeros socioLista = new CienPrimeros();

                    socioLista.Nombre = socio.Result.Nombre;
                    socioLista.Edad = socio.Result.Edad;
                    socioLista.Equipo = socio.Result.Equipo;

                    ListadoCien.AlarmaObjeto.Add(socioLista);
                    ListadoCien.AlarmaObjeto.Sort((x, y) => x.Edad.CompareTo(y.Edad));

                    countSocios++;
                }


            }

            top5 = cantidadNombresRiver.OrderByDescending(pair => pair.Value).Take(5);

            IEnumerable<IGrouping<string, CsvMappingResult<Socios>>> grouped = socios.OrderBy(x => x.Result.Equipo.Length)
                   .GroupBy(x => x.Result.Equipo);

            ListadoEquipos = new Equipos();
            ListadoEquipos.Equipo = new List<DatosEquipo>();

            foreach (IGrouping<string, CsvMappingResult<Socios>> Equipo in grouped.ToList())
            {
                DatosEquipo DatosDelEquipo = new DatosEquipo();
                long totalJugadoresXEquipo = Equipo.LongCount();
                int contJugadoresXEquipo = 0;
                int edadTotal = 0;
                foreach (CsvMappingResult<Socios> Socio in Equipo)
                {

                    DatosDelEquipo.Nombre = Equipo.Key; // Obtengo el nombre del equipo
                    DatosDelEquipo.CantidadSocios++; // Obtengo la cantidad de socios

                    edadTotal += Socio.Result.Edad; //Acumulamos las edades de todos los socios

                    if (Socio.Result.Edad < DatosDelEquipo.MenorEdad || DatosDelEquipo.MenorEdad == 0)
                    {
                        DatosDelEquipo.MenorEdad = Socio.Result.Edad;
                    }

                    if (Socio.Result.Edad > DatosDelEquipo.MayorEdad)
                    {
                        DatosDelEquipo.MayorEdad = Socio.Result.Edad;
                    }
                    contJugadoresXEquipo += 1;
                    if (contJugadoresXEquipo == totalJugadoresXEquipo)
                    {
                        DatosDelEquipo.PromedioEdad = (int)(edadTotal / DatosDelEquipo.CantidadSocios);
                        ListadoEquipos.Equipo.Add(DatosDelEquipo);
                    }
                }
            }
            promedioEdadesRacing = edadTotales / sociosRacing;


            ListadoEquipos.Equipo.Sort((x, y) => x.CantidadSocios.CompareTo(y.CantidadSocios));

            MostraInfoClientSide();


        }

        protected void MostraInfoClientSide()
        {
            LblCantidadPersonasRegistradas.Text = $"Cantidad de socios: {cantidadSocios.ToString()}";
            LblPromedioSociosRacing.Text = $"Promedio de edad en Racing: {promedioEdadesRacing.ToString()}";

            BulletedList1.Items.Add("Los primeros 100 casados y con estudios universitarios");
            int iteraciones = 1;
            foreach (CienPrimeros item in ListadoCien.AlarmaObjeto)
            {
                BulletedList1.Items.Add($"{iteraciones++}) Nombre: {item.Nombre}, Edad: {item.Edad}, Equipo: {item.Equipo}");
            }
            iteraciones = 1;
            BulletedList2.Items.Add("Los 5 nombres más comunes en River");
            foreach (KeyValuePair<string, int> item in top5.ToList())
            {
                BulletedList2.Items.Add($"{iteraciones++}) {item.Key.ToString()}");
            }
            iteraciones = 1;
            BulletedList3.Items.Add("Edad promedio de socios, menor edad registrada y mayor edad registrada (Lista ordenada por cantidad de socios)");
            foreach (DatosEquipo item in ListadoEquipos.Equipo)
            {
                BulletedList3.Items.Add($"{iteraciones++}) Equipo: {item.Nombre}, Edad promedio: {item.PromedioEdad}, Menor edad: {item.MenorEdad}, Mayor edad: {item.MayorEdad}");
            }
        }

        protected bool ComprobarCargaCSV()
        {
            if (!fileUpload.HasFile)
            {
                LblFileName.Text = "Seleccione un archivo CSV";
                BulletedList1.Items.Clear();
                BulletedList2.Items.Clear();
                BulletedList3.Items.Clear();
                LblCantidadPersonasRegistradas.Text = "";
                LblPromedioSociosRacing.Text = "";
                LblCantidadPersonasRegistradas.Visible = false;
                LblPromedioSociosRacing.Visible = false;
                return false;
            }
            else
            {
                BulletedList1.Items.Clear();
                BulletedList2.Items.Clear();
                BulletedList3.Items.Clear();
                LblCantidadPersonasRegistradas.Text = "";
                LblPromedioSociosRacing.Text = "";
                LblCantidadPersonasRegistradas.Visible = true;
                LblPromedioSociosRacing.Visible = true;
            }
            if (!fileUpload.FileName.ToUpper().Contains(".CSV"))
            {
                LblFileName.Text = "Seleccione un archivo VALIDO";
                LblCantidadPersonasRegistradas.Visible = false;
                LblPromedioSociosRacing.Visible = false;
                return false;
            }
            else
            {
                LblFileName.Text = fileUpload.FileName;
                return true;
            }
        }


    }
}