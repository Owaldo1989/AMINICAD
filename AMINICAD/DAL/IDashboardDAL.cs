using AMINICAD.Models;

namespace AMINICAD.DAL
{
    public interface IDashboardDAL
    {
        Task<DashboardAdministrativo> GetDashboardAsync(
            DateTime fechaInicial,
            DateTime fechaFinal,
            int? idRegion = null,
            int? idDistrito = null,
            int? idMisionero = null,
            int? idIglesia = null,
            int? idTipoMision = null,
            CancellationToken ct = default);

        Task<List<IngresoMensual>> GetIngresosComparativo4AniosAsync(
            int anio,
            CancellationToken ct = default);
    }
}