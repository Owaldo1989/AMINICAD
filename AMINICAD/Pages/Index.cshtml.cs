using AMINICAD.DAL;
using AMINICAD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMINICAD.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IDashboardDAL _dal;

        public IndexModel(IDashboardDAL dal)
        {
            _dal = dal;
        }

        [BindProperty(SupportsGet = true)]
        public int? Anio { get; set; }

        public int AnioSeleccionado { get; private set; }
        public int MesActual { get; private set; }

        public DashboardKpis Kpis { get; private set; } = new DashboardKpis();

        // Gráfica 1: 12 meses del año seleccionado
        public List<IngresoMensual> SerieMensual { get; private set; } = new List<IngresoMensual>();

        // Gráfica 2: 4 años (anio-3..anio) x 12 meses
        public List<IngresoMensual> SerieComparativo4Anios { get; private set; } = new List<IngresoMensual>();

        public List<int> OpcionesAnio { get; private set; } = new List<int>();

        public async Task OnGetAsync(CancellationToken ct)
        {
            AnioSeleccionado = ValidarAnio(Anio);
            MesActual = DateTime.Today.Month;

            // últimos 6 años (ajuste a gusto)
            OpcionesAnio = Enumerable.Range(DateTime.Today.Year - 5, 6).Reverse().ToList();

            Kpis = await _dal.GetKpisAsync(AnioSeleccionado, MesActual, ct);
            SerieMensual = await _dal.GetIngresosPorMesAsync(AnioSeleccionado, ct);
            SerieComparativo4Anios = await _dal.GetIngresosComparativo4AniosAsync(AnioSeleccionado, ct);
        }

        private static int ValidarAnio(int? anio)
        {
            var actual = DateTime.Today.Year;
            var y = anio ?? actual;

            if (y < 2000) return actual;
            if (y > actual) return actual;

            return y;
        }
    }
}
