using AMINICAD.Data.Misioneros;
using AMINICAD.Models.Misioneros;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMINICAD.Pages.Misioneros
{
    public class IndexModel : PageModel
    {
        private readonly MisioneroRepository _repository;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
            MisioneroRepository repository,
            ILogger<IndexModel> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public MisioneroIndexDto Datos { get; private set; }
            = new();

        [BindProperty(SupportsGet = true)]
        public int? IdTipoMision { get; set; }

        public MisioneroResumenGeneralDto Resumen =>
            Datos.Resumen;

        public List<TipoMisionResumenDto> TiposMision =>
            Datos.TiposMision;

        public List<MisioneroListadoDto> Misioneros =>
            Datos.Misioneros;

        public string? MensajeError { get; private set; }

        public bool TieneDatos =>
            TiposMision.Count > 0;

        public bool TieneDetalle =>
            Misioneros.Count > 0;

        public TipoMisionResumenDto? TipoMisionSeleccionado =>
            IdTipoMision.HasValue
                ? TiposMision.FirstOrDefault(
                    x => x.IdTipoMision == IdTipoMision.Value)
                : null;

        public async Task OnGetAsync(
            CancellationToken cancellationToken)
        {
            try
            {
                Datos =
                    await _repository.ObtenerIndexAsync(
                        IdTipoMision,
                        cancellationToken
                    );
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning(
                    "La consulta del módulo de misioneros fue cancelada."
                );

                MensajeError =
                    "La consulta fue cancelada.";
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error al consultar el portafolio de misioneros."
                );

                MensajeError =
                    "No fue posible cargar la información de misioneros. "
                    + "Revisa la conexión y el procedimiento almacenado.";
            }
        }

    }
}
