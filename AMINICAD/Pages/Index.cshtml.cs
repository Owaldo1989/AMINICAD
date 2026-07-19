using AMINICAD.DAL;
using AMINICAD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace AMINICAD.Pages
{
    public sealed class IndexModel : PageModel
    {
        private readonly IDashboardDAL _dashboardDAL;

        public IndexModel(
            IDashboardDAL dashboardDAL)
        {
            _dashboardDAL = dashboardDAL;
        }


        [BindProperty(SupportsGet = true)]
        public string Modo { get; set; } = "anio";


        [BindProperty(SupportsGet = true)]
        public int? Anio { get; set; }


        [BindProperty(SupportsGet = true)]
        public int? Mes { get; set; }


        [BindProperty(SupportsGet = true)]
        public int? IdRegion { get; set; }


        [BindProperty(SupportsGet = true)]
        public int? IdDistrito { get; set; }


        [BindProperty(SupportsGet = true)]
        public int? IdMisionero { get; set; }


        [BindProperty(SupportsGet = true)]
        public int? IdIglesia { get; set; }


        [BindProperty(SupportsGet = true)]
        public int? IdTipoMision { get; set; }


        public string ModoSeleccionado { get; private set; } = "anio";

        public bool EsVistaMensual { get; private set; }

        public bool EsPeriodoEnCurso { get; private set; }

        public int AnioSeleccionado { get; private set; }

        public int MesSeleccionado { get; private set; }

        public int MesActual { get; private set; }

        public string NombreMesSeleccionado { get; private set; } =
            string.Empty;

        public string TituloPeriodo { get; private set; } =
            string.Empty;

        public DateTime FechaInicial { get; private set; }

        public DateTime FechaFinal { get; private set; }

        public DateTime FechaFinalProgramada { get; private set; }


        public DashboardAdministrativo Dashboard { get; private set; } =
            new();


        /*
         * Se conservan para compatibilidad con otras partes
         * del proyecto que todavía puedan utilizarlas.
         */
        public DashboardKpis Kpis { get; private set; } =
            new();

        public List<IngresoMensual> SerieMensual { get; private set; } =
            new();

        public List<IngresoMensual> SerieComparativo4Anios
        {
            get;
            private set;
        } = new();


        public List<int> OpcionesAnio { get; private set; } =
            new();

        public List<int> OpcionesMes { get; private set; } =
            Enumerable.Range(1, 12).ToList();


        public async Task OnGetAsync(
            CancellationToken ct)
        {
            var hoy = DateTime.Today;

            MesActual = hoy.Month;

            AnioSeleccionado =
                ValidarAnio(Anio, hoy.Year);

            ModoSeleccionado =
                NormalizarModo(Modo);

            EsVistaMensual =
                ModoSeleccionado == "mes";

            MesSeleccionado =
                ValidarMes(
                    Mes,
                    AnioSeleccionado,
                    hoy);

            NombreMesSeleccionado =
                ObtenerNombreMes(
                    MesSeleccionado);

            ConfigurarPeriodo(hoy);

            OpcionesAnio =
                Enumerable
                    .Range(
                        hoy.Year - 5,
                        6)
                    .Reverse()
                    .ToList();


            /*
             * Una sola consulta alimenta KPI, distribución,
             * regiones, distritos, beneficiarios e iglesias.
             *
             * En vista anual recibe el rango del año.
             * En vista mensual recibe únicamente el mes elegido.
             */
            Dashboard =
                await _dashboardDAL.GetDashboardAsync(
                    FechaInicial,
                    FechaFinal,
                    IdRegion,
                    IdDistrito,
                    IdMisionero,
                    IdIglesia,
                    IdTipoMision,
                    ct);


            /*
             * El comparativo de cuatro años solamente se consulta
             * en modo anual. La vista mensual no lo necesita.
             */
            if (!EsVistaMensual)
            {
                SerieComparativo4Anios =
                    await _dashboardDAL
                        .GetIngresosComparativo4AniosAsync(
                            AnioSeleccionado,
                            ct);
            }


            SerieMensual =
                Dashboard.Mensual
                    .Select(
                        x => new IngresoMensual
                        {
                            Anio = x.Anio,
                            IdMes = x.IdMes,
                            Mes = x.Mes,
                            Total = x.TotalBruto
                        })
                    .OrderBy(x => x.Anio)
                    .ThenBy(x => x.IdMes)
                    .ToList();


            Kpis =
                new DashboardKpis
                {
                    TotalAnio =
                        Dashboard.Resumen.TotalBruto,

                    TotalMes =
                        EsVistaMensual
                            ? Dashboard.Resumen.TotalBruto
                            : Dashboard.Mensual
                                .Where(
                                    x =>
                                        x.Anio == AnioSeleccionado
                                        &&
                                        x.IdMes == MesActual)
                                .Select(x => x.TotalBruto)
                                .FirstOrDefault()
                };
        }


        public string ObtenerNombreMesOpcion(
            int mes)
        {
            return ObtenerNombreMes(mes);
        }


        public bool MesEsFuturo(
            int mes)
        {
            var hoy = DateTime.Today;

            return AnioSeleccionado == hoy.Year
                   &&
                   mes > hoy.Month;
        }


        private void ConfigurarPeriodo(
            DateTime hoy)
        {
            if (EsVistaMensual)
            {
                FechaInicial =
                    new DateTime(
                        AnioSeleccionado,
                        MesSeleccionado,
                        1);

                FechaFinalProgramada =
                    FechaInicial
                        .AddMonths(1)
                        .AddDays(-1);

                TituloPeriodo =
                    $"Resumen de {NombreMesSeleccionado} {AnioSeleccionado}";
            }
            else
            {
                FechaInicial =
                    new DateTime(
                        AnioSeleccionado,
                        1,
                        1);

                FechaFinalProgramada =
                    new DateTime(
                        AnioSeleccionado,
                        12,
                        31);

                TituloPeriodo =
                    $"Resumen anual {AnioSeleccionado}";
            }

            /*
             * Para el año o mes actual no consultamos fechas futuras.
             * Así el encabezado y los resultados reflejan el corte real.
             */
            FechaFinal =
                FechaFinalProgramada > hoy
                &&
                FechaInicial <= hoy
                    ? hoy
                    : FechaFinalProgramada;

            EsPeriodoEnCurso =
                FechaFinal < FechaFinalProgramada;
        }


        private static string NormalizarModo(
            string? modo)
        {
            return string.Equals(
                modo,
                "mes",
                StringComparison.OrdinalIgnoreCase)
                    ? "mes"
                    : "anio";
        }


        private static int ValidarAnio(
            int? anio,
            int anioActual)
        {
            var resultado =
                anio ?? anioActual;

            if (resultado < 2000)
            {
                return anioActual;
            }

            if (resultado > anioActual)
            {
                return anioActual;
            }

            return resultado;
        }


        private static int ValidarMes(
            int? mes,
            int anioSeleccionado,
            DateTime hoy)
        {
            var resultado =
                mes is >= 1 and <= 12
                    ? mes.Value
                    : hoy.Month;

            /*
             * Evita seleccionar meses futuros dentro del año actual.
             */
            if (anioSeleccionado == hoy.Year &&
                resultado > hoy.Month)
            {
                return hoy.Month;
            }

            return resultado;
        }


        private static string ObtenerNombreMes(
            int mes)
        {
            var cultura =
                CultureInfo.GetCultureInfo("es-NI");

            var nombre =
                cultura.DateTimeFormat
                    .GetMonthName(mes);

            return cultura.TextInfo
                .ToTitleCase(
                    nombre.ToLower(cultura));
        }
    }
}
