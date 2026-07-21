using AMINICAD.Data.Ingresos;
using AMINICAD.Models.Ingresos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMINICAD.Pages.Ingresos
{
    public class IndexModel : PageModel
    {
        private readonly IngresoControlOperativoRepository _repository;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
            IngresoControlOperativoRepository repository,
            ILogger<IndexModel> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [BindProperty(SupportsGet = true)]
        public DateTime? FechaInicial { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? FechaFinal { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? FechaRegistro { get; set; }

        public IngresoControlOperativoDto ControlOperativo { get; set; }
            = new();

        public IngresoResumenOperativoDto Resumen =>
            ControlOperativo.Resumen;

        public List<IngresoFechaProcesadaDto> FechasProcesadas =>
            ControlOperativo.FechasProcesadas;

        public List<IngresoActividadDiariaDto> ActividadDiaria =>
            ControlOperativo.ActividadDiaria;

        public List<IngresoCuentaDto> Cuentas =>
            ControlOperativo.Cuentas;

        public IngresoAlertasDto Alertas =>
            ControlOperativo.Alertas;

        public List<IngresoRevisionDto> RegistrosRevision =>
            ControlOperativo.RegistrosRevision;

        public string? MensajeError { get; set; }

        public bool TieneDatos =>
            Resumen.CantidadIngresos > 0;

        public bool TieneRegistrosRevision =>
            RegistrosRevision.Count > 0;

        public int CantidadDiasConActividad =>
            ActividadDiaria.Count;

        public async Task OnGetAsync(
            CancellationToken cancellationToken)
        {
            var hoy = DateTime.Today;

            if (FechaRegistro.HasValue
                && !FechaInicial.HasValue
                && !FechaFinal.HasValue)
            {
                FechaInicial = FechaRegistro.Value.Date;
                FechaFinal = FechaRegistro.Value.Date;
            }

            var fechaFinalConsulta =
                FechaFinal?.Date
                ?? hoy;

            var fechaInicialConsulta =
                FechaInicial?.Date
                ?? InicioDeSemana(fechaFinalConsulta);

            FechaInicial = fechaInicialConsulta;
            FechaFinal = fechaFinalConsulta;

            if (fechaInicialConsulta > fechaFinalConsulta)
            {
                MensajeError =
                    "La fecha inicial no puede ser posterior a la fecha final.";
                return;
            }

            if ((fechaFinalConsulta - fechaInicialConsulta).TotalDays > 30)
            {
                MensajeError =
                    "El rango máximo permitido es de 31 días.";
                return;
            }

            try
            {
                ControlOperativo =
                    await _repository.ObtenerControlOperativoAsync(
                        fechaInicialConsulta,
                        fechaFinalConsulta,
                        cancellationToken
                    );
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning(
                    "La consulta del control operativo de ingresos fue cancelada."
                );

                MensajeError =
                    "La consulta fue cancelada.";
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error al consultar el control operativo de ingresos del {FechaInicial} al {FechaFinal}.",
                    fechaInicialConsulta,
                    fechaFinalConsulta
                );

                MensajeError =
                    "No fue posible cargar la información de ingresos. "
                    + "Revisa la conexión y el procedimiento almacenado.";
            }
        }

        private static DateTime InicioDeSemana(DateTime fecha)
        {
            var diasDesdeLunes =
                ((int)fecha.DayOfWeek + 6) % 7;

            return fecha.AddDays(-diasDesdeLunes).Date;
        }
    }
}
