using AMINICAD.Data.Misioneros;
using AMINICAD.Models.Misioneros;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMINICAD.Pages.Misioneros
{
    public sealed class DetalleModel : PageModel
    {
        private readonly MisioneroDetalleRepository _repository;
        private readonly ILogger<DetalleModel> _logger;

        public DetalleModel(
            MisioneroDetalleRepository repository,
            ILogger<DetalleModel> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [BindProperty(SupportsGet = true)]
        public int IdMisionero { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? Anio { get; set; }

        public int AnioSeleccionado { get; private set; }

        public List<int> AniosDisponibles { get; private set; }
            = new();

        public MisioneroDetalleDto Detalle { get; private set; }
            = new();

        public string? MensajeError { get; private set; }

        public bool TieneMisionero =>
            Detalle.Informacion.IdMisionero > 0;

        public async Task OnGetAsync(
            CancellationToken cancellationToken)
        {
            var anioActual =
                DateTime.Today.Year;

            AnioSeleccionado =
                Anio is >= 2000
                && Anio <= anioActual
                    ? Anio.Value
                    : anioActual;

            AniosDisponibles =
                Enumerable
                    .Range(
                        anioActual - 5,
                        6
                    )
                    .Reverse()
                    .ToList();

            if (IdMisionero <= 0)
            {
                MensajeError =
                    "No se especificó un misionero válido.";

                return;
            }

            try
            {
                Detalle =
                    await _repository.ObtenerDetalleAsync(
                        IdMisionero,
                        AnioSeleccionado,
                        cancellationToken
                    );

                if (!TieneMisionero)
                {
                    MensajeError =
                        "No se encontró el misionero solicitado.";
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning(
                    "La consulta del misionero {IdMisionero} fue cancelada.",
                    IdMisionero
                );

                MensajeError =
                    "La consulta fue cancelada.";
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error al consultar el detalle del misionero {IdMisionero}.",
                    IdMisionero
                );

                MensajeError =
                    "No fue posible cargar la ficha del misionero. "
                    + "Revisa el procedimiento almacenado y la conexión.";
            }
        }
    }
}