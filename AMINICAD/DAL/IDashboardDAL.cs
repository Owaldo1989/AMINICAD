using AMINICAD.Models;

namespace AMINICAD.DAL
{
    public interface IDashboardDAL
    {
        Task<DashboardKpis> GetKpisAsync(int anio, int mes, CancellationToken ct = default);
        Task<List<IngresoMensual>> GetIngresosPorMesAsync(int anio, CancellationToken ct = default);
        Task<List<IngresoMensual>> GetIngresosComparativo4AniosAsync(int anio, CancellationToken ct = default);
    }
}
