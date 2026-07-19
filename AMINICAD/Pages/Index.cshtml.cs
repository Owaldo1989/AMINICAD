using AMINICAD.DAL;
using AMINICAD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMINICAD.Pages
{
    public sealed class IndexModel : PageModel
    {
        private readonly IDashboardDAL _dashboardDAL;

        public IndexModel(
            IDashboardDAL dashboardDAL)
        {
            _dashboardDAL =
                dashboardDAL;
        }


        [BindProperty(SupportsGet = true)]
        public int? Anio { get; set; }


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


        public int AnioSeleccionado { get; private set; }

        public int MesActual { get; private set; }

        public DateTime FechaInicial { get; private set; }

        public DateTime FechaFinal { get; private set; }


        /*
         * Resultado completo del nuevo procedimiento.
         */
        public DashboardAdministrativo Dashboard { get; private set; } =
            new();


        /*
         * Propiedades de compatibilidad.
         *
         * Permiten que el Index.cshtml actual continúe funcionando
         * mientras incorporamos las nuevas secciones.
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


        public async Task OnGetAsync(
            CancellationToken ct)
        {
            AnioSeleccionado =
                ValidarAnio(Anio);

            MesActual =
                DateTime.Today.Month;

            FechaInicial =
                new DateTime(
                    AnioSeleccionado,
                    1,
                    1);

            FechaFinal =
                new DateTime(
                    AnioSeleccionado,
                    12,
                    31);

            OpcionesAnio =
                Enumerable
                    .Range(
                        DateTime.Today.Year - 5,
                        6)
                    .Reverse()
                    .ToList();


            /*
             * Ejecutamos de forma secuencial para evitar abrir dos
             * conexiones simultáneas durante períodos de presión en SQL Server.
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

            SerieComparativo4Anios =
                await _dashboardDAL
                    .GetIngresosComparativo4AniosAsync(
                        AnioSeleccionado,
                        ct);


            /*
             * Convertimos la serie nueva al modelo que ya utiliza
             * Index.cshtml.
             */
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
                    .OrderBy(x => x.IdMes)
                    .ToList();


            /*
             * Alimentamos las tarjetas actuales.
             */
            Kpis =
                new DashboardKpis
                {
                    TotalAnio =
                        Dashboard.Resumen.TotalBruto,

                    TotalMes =
                        Dashboard.Mensual
                            .Where(
                                x =>
                                    x.Anio ==
                                    AnioSeleccionado
                                    &&
                                    x.IdMes ==
                                    MesActual)
                            .Select(
                                x => x.TotalBruto)
                            .FirstOrDefault()
                };
        }


        private static int ValidarAnio(
            int? anio)
        {
            var anioActual =
                DateTime.Today.Year;

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
    }
}
