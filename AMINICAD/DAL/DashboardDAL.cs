using AMINICAD.Models;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace AMINICAD.DAL
{
    public sealed class DashboardDAL : IDashboardDAL
    {
        private readonly string _cs;

        public DashboardDAL(IConfiguration config)
        {
            _cs = config.GetConnectionString("DefaultConnection")
                  ?? throw new InvalidOperationException("Falta ConnectionStrings:DefaultConnection en appsettings.json");
        }
        public async Task<DashboardKpis> GetKpisAsync(int anio, int mes, CancellationToken ct = default)
        {
            var inicioAnio = new DateTime(anio, 1, 1);
            var finAnio = inicioAnio.AddYears(1);

            var inicioMes = new DateTime(anio, mes, 1);
            var finMes = inicioMes.AddMonths(1);

            const string sql = @"
SELECT
    TotalMes  = ISNULL(SUM(CASE WHEN A.Fecha >= @InicioMes AND A.Fecha < =@FinMes THEN A.Total ELSE 0 END), 0),
    TotalAnio = ISNULL(SUM(A.Total), 0)
FROM tbIngresosMaestros A
WHERE A.Fecha >= @InicioAnio AND A.Fecha <= @FinAnio;";

            using var cn = new SqlConnection(_cs);
            await cn.OpenAsync(ct);

            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add("@InicioAnio", SqlDbType.DateTime).Value = inicioAnio;
            cmd.Parameters.Add("@FinAnio", SqlDbType.DateTime).Value = finAnio;
            cmd.Parameters.Add("@InicioMes", SqlDbType.DateTime).Value = inicioMes;
            cmd.Parameters.Add("@FinMes", SqlDbType.DateTime).Value = finMes;

            using var rd = await cmd.ExecuteReaderAsync(ct);

            var kpi = new DashboardKpis();
            if (await rd.ReadAsync(ct))
            {
                kpi.TotalMes = rd.GetDecimal(0);
                kpi.TotalAnio = rd.GetDecimal(1);
            }

            return kpi;
        }

        public async Task<List<IngresoMensual>> GetIngresosPorMesAsync(int anio, CancellationToken ct = default)
        {
            const string sql = @"
;WITH Meses AS
(
    SELECT 1 AS IdMes UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4
    UNION ALL SELECT 5 UNION ALL SELECT 6 UNION ALL SELECT 7 UNION ALL SELECT 8
    UNION ALL SELECT 9 UNION ALL SELECT 10 UNION ALL SELECT 11 UNION ALL SELECT 12
),
Totales AS
(
    SELECT
        IdMes = MONTH(A.Fecha),
        Total = SUM(A.Total)
    FROM tbIngresosMaestros A
    WHERE A.Fecha >= @InicioAnio AND A.Fecha < @FinAnio
    GROUP BY MONTH(A.Fecha)
)
SELECT
    IdMes = M.IdMes,
    Total = ISNULL(T.Total, 0)
FROM Meses M
LEFT JOIN Totales T ON T.IdMes = M.IdMes
ORDER BY M.IdMes;";

            var inicioAnio = new DateTime(anio, 1, 1);
            var finAnio = inicioAnio.AddYears(1);

            using var cn = new SqlConnection(_cs);
            await cn.OpenAsync(ct);

            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add("@InicioAnio", SqlDbType.DateTime).Value = inicioAnio;
            cmd.Parameters.Add("@FinAnio", SqlDbType.DateTime).Value = finAnio;

            using var rd = await cmd.ExecuteReaderAsync(ct);

            var culture = CultureInfo.GetCultureInfo("es-NI");
            var list = new List<IngresoMensual>(12);

            while (await rd.ReadAsync(ct))
            {
                var idMes = rd.GetInt32(0);
                var total = rd.GetDecimal(1);

                var mesNombre = culture.DateTimeFormat.GetMonthName(idMes);
                if (!string.IsNullOrWhiteSpace(mesNombre))
                    mesNombre = char.ToUpper(mesNombre[0], culture) + mesNombre.Substring(1);

                list.Add(new IngresoMensual
                {
                    Anio = anio,
                    IdMes = idMes,
                    Total = total,
                    Mes = mesNombre
                });
            }

            return list;
        }

        public async Task<List<IngresoMensual>> GetIngresosComparativo4AniosAsync(int anio, CancellationToken ct = default)
        {
            const string sp = "dbo.SpDashboard_IngresosComparativo4Anios_Meses";

            using var cn = new SqlConnection(_cs);
            await cn.OpenAsync(ct);

            using var cmd = new SqlCommand(sp, cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Anio", SqlDbType.Int).Value = anio;

            using var rd = await cmd.ExecuteReaderAsync(ct);

            var list = new List<IngresoMensual>(48);
            while (await rd.ReadAsync(ct))
            {
                list.Add(new IngresoMensual
                {
                    Anio = rd.GetInt32(0),
                    IdMes = rd.GetInt32(1),
                    Total = rd.GetDecimal(2),
                    Mes = "" // opcional; para la gráfica usaremos IdMes
                });
            }

            return list;
        }


    }
}
