using AMINICAD.Models;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace AMINICAD.DAL
{
    public sealed class DashboardDAL : IDashboardDAL
    {
        private readonly string _connectionString;

        public DashboardDAL(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "No se encontró ConnectionStrings:DefaultConnection.");
        }


        public async Task<DashboardAdministrativo> GetDashboardAsync(
            DateTime fechaInicial,
            DateTime fechaFinal,
            int? idRegion = null,
            int? idDistrito = null,
            int? idMisionero = null,
            int? idIglesia = null,
            int? idTipoMision = null,
            CancellationToken ct = default)
        {
            if (fechaFinal.Date < fechaInicial.Date)
            {
                throw new ArgumentException(
                    "La fecha final no puede ser menor que la fecha inicial.");
            }

            var resultado = new DashboardAdministrativo();

            await using var conexion =
                new SqlConnection(_connectionString);

            await conexion.OpenAsync(ct);

            await using var comando =
                new SqlCommand(
                    "dbo.SpDashboardAMINICAD",
                    conexion);

            comando.CommandType =
                CommandType.StoredProcedure;

            comando.CommandTimeout =
                180;

            comando.Parameters
                .Add("@FechaInicial", SqlDbType.Date)
                .Value = fechaInicial.Date;

            comando.Parameters
                .Add("@FechaFinal", SqlDbType.Date)
                .Value = fechaFinal.Date;

            AgregarParametroEnteroOpcional(
                comando,
                "@IdRegion",
                idRegion);

            AgregarParametroEnteroOpcional(
                comando,
                "@IdDistrito",
                idDistrito);

            AgregarParametroEnteroOpcional(
                comando,
                "@IdMisionero",
                idMisionero);

            AgregarParametroEnteroOpcional(
                comando,
                "@IdIglesia",
                idIglesia);

            AgregarParametroEnteroOpcional(
                comando,
                "@IdTipoMision",
                idTipoMision);

            await using var reader =
                await comando.ExecuteReaderAsync(ct);


            /*
             * RESULTADO 1:
             * KPI y resumen general.
             */
            if (await reader.ReadAsync(ct))
            {
                resultado.Resumen =
                    new DashboardResumen
                    {
                        FechaInicial =
                            LeerFecha(
                                reader,
                                "FechaInicial"),

                        FechaFinal =
                            LeerFecha(
                                reader,
                                "FechaFinal"),

                        TotalBruto =
                            LeerDecimal(
                                reader,
                                "TotalBruto"),

                        NetoBeneficiarios =
                            LeerDecimal(
                                reader,
                                "NetoBeneficiarios"),

                        TotalRetenciones =
                            LeerDecimal(
                                reader,
                                "TotalRetenciones"),

                        OtrosConceptos =
                            LeerDecimal(
                                reader,
                                "OtrosConceptos"),

                        TotalDistribuido =
                            LeerDecimal(
                                reader,
                                "TotalDistribuido"),

                        DiferenciaDistribucion =
                            LeerDecimal(
                                reader,
                                "DiferenciaDistribucion"),

                        CantidadIngresos =
                            LeerEntero(
                                reader,
                                "CantidadIngresos"),

                        CantidadRecibos =
                            LeerEntero(
                                reader,
                                "CantidadRecibos"),

                        CantidadIglesias =
                            LeerEntero(
                                reader,
                                "CantidadIglesias"),

                        CantidadMisioneros =
                            LeerEntero(
                                reader,
                                "CantidadMisioneros"),

                        CantidadRegiones =
                            LeerEntero(
                                reader,
                                "CantidadRegiones"),

                        CantidadDistritos =
                            LeerEntero(
                                reader,
                                "CantidadDistritos"),

                        PromedioAporte =
                            LeerDecimal(
                                reader,
                                "PromedioAporte"),

                        MedianaAporte =
                            LeerDecimal(
                                reader,
                                "MedianaAporte"),

                        PorcentajeNeto =
                            LeerDecimal(
                                reader,
                                "PorcentajeNeto"),

                        PorcentajeRetenciones =
                            LeerDecimal(
                                reader,
                                "PorcentajeRetenciones")
                    };
            }


            /*
             * RESULTADO 2:
             * Comportamiento mensual.
             */
            if (await reader.NextResultAsync(ct))
            {
                while (await reader.ReadAsync(ct))
                {
                    resultado.Mensual.Add(
                        new DashboardMensual
                        {
                            Periodo =
                                LeerFecha(
                                    reader,
                                    "Periodo"),

                            Anio =
                                LeerEntero(
                                    reader,
                                    "Anio"),

                            IdMes =
                                LeerEntero(
                                    reader,
                                    "IdMes"),

                            Mes =
                                FormatearNombreMes(
                                    LeerTexto(
                                        reader,
                                        "Mes")),

                            TotalBruto =
                                LeerDecimal(
                                    reader,
                                    "TotalBruto"),

                            NetoBeneficiarios =
                                LeerDecimal(
                                    reader,
                                    "NetoBeneficiarios"),

                            Retenciones =
                                LeerDecimal(
                                    reader,
                                    "Retenciones"),

                            OtrosConceptos =
                                LeerDecimal(
                                    reader,
                                    "OtrosConceptos"),

                            CantidadRecibos =
                                LeerEntero(
                                    reader,
                                    "CantidadRecibos"),

                            Iglesias =
                                LeerEntero(
                                    reader,
                                    "Iglesias"),

                            Misioneros =
                                LeerEntero(
                                    reader,
                                    "Misioneros")
                        });
                }
            }


            /*
             * RESULTADO 3:
             * Distribución por concepto.
             */
            if (await reader.NextResultAsync(ct))
            {
                while (await reader.ReadAsync(ct))
                {
                    resultado.Distribucion.Add(
                        new DashboardDistribucion
                        {
                            IdRetencion =
                                LeerEnteroNullable(
                                    reader,
                                    "IdRetencion"),

                            Concepto =
                                LeerTexto(
                                    reader,
                                    "Concepto"),

                            TipoDistribucion =
                                LeerTexto(
                                    reader,
                                    "TipoDistribucion"),

                            Monto =
                                LeerDecimal(
                                    reader,
                                    "Monto"),

                            CantidadRecibos =
                                LeerEntero(
                                    reader,
                                    "CantidadRecibos"),

                            PorcentajeTotal =
                                LeerDecimal(
                                    reader,
                                    "PorcentajeTotal")
                        });
                }
            }


            /*
             * RESULTADO 4:
             * Regiones.
             */
            if (await reader.NextResultAsync(ct))
            {
                while (await reader.ReadAsync(ct))
                {
                    resultado.Regiones.Add(
                        new DashboardRegion
                        {
                            IdRegion =
                                LeerEnteroNullable(
                                    reader,
                                    "IdRegion"),

                            Region =
                                LeerTexto(
                                    reader,
                                    "Region"),

                            TotalRecaudado =
                                LeerDecimal(
                                    reader,
                                    "TotalRecaudado"),

                            CantidadRecibos =
                                LeerEntero(
                                    reader,
                                    "CantidadRecibos"),

                            Iglesias =
                                LeerEntero(
                                    reader,
                                    "Iglesias"),

                            Misioneros =
                                LeerEntero(
                                    reader,
                                    "Misioneros"),

                            PromedioAporte =
                                LeerDecimal(
                                    reader,
                                    "PromedioAporte"),

                            UltimoAporte =
                                LeerFechaNullable(
                                    reader,
                                    "UltimoAporte")
                        });
                }
            }


            /*
             * RESULTADO 5:
             * Distritos principales.
             */
            if (await reader.NextResultAsync(ct))
            {
                while (await reader.ReadAsync(ct))
                {
                    resultado.Distritos.Add(
                        new DashboardDistrito
                        {
                            IdRegion =
                                LeerEnteroNullable(
                                    reader,
                                    "IdRegion"),

                            Region =
                                LeerTexto(
                                    reader,
                                    "Region"),

                            IdDistrito =
                                LeerEnteroNullable(
                                    reader,
                                    "IdDistrito"),

                            Distrito =
                                LeerTexto(
                                    reader,
                                    "Distrito"),

                            TotalRecaudado =
                                LeerDecimal(
                                    reader,
                                    "TotalRecaudado"),

                            CantidadRecibos =
                                LeerEntero(
                                    reader,
                                    "CantidadRecibos"),

                            Iglesias =
                                LeerEntero(
                                    reader,
                                    "Iglesias"),

                            Misioneros =
                                LeerEntero(
                                    reader,
                                    "Misioneros"),

                            PromedioAporte =
                                LeerDecimal(
                                    reader,
                                    "PromedioAporte"),

                            UltimoAporte =
                                LeerFechaNullable(
                                    reader,
                                    "UltimoAporte")
                        });
                }
            }


            /*
             * RESULTADO 6:
             * Misioneros y beneficiarios.
             */
            if (await reader.NextResultAsync(ct))
            {
                while (await reader.ReadAsync(ct))
                {
                    resultado.Misioneros.Add(
                        new DashboardMisionero
                        {
                            IdMisionero =
                                LeerEnteroNullable(
                                    reader,
                                    "IdMisionero"),

                            Misionero =
                                LeerTexto(
                                    reader,
                                    "Misionero"),

                            TotalBruto =
                                LeerDecimal(
                                    reader,
                                    "TotalBruto"),

                            Neto =
                                LeerDecimal(
                                    reader,
                                    "Neto"),

                            Retenciones =
                                LeerDecimal(
                                    reader,
                                    "Retenciones"),

                            Otros =
                                LeerDecimal(
                                    reader,
                                    "Otros"),

                            CantidadRecibos =
                                LeerEntero(
                                    reader,
                                    "CantidadRecibos"),

                            Iglesias =
                                LeerEntero(
                                    reader,
                                    "Iglesias"),

                            UltimoAporte =
                                LeerFechaNullable(
                                    reader,
                                    "UltimoAporte")
                        });
                }
            }


            /*
             * RESULTADO 7:
             * Iglesias y donantes.
             */
            if (await reader.NextResultAsync(ct))
            {
                while (await reader.ReadAsync(ct))
                {
                    resultado.Iglesias.Add(
                        new DashboardIglesia
                        {
                            IdIglesia =
                                LeerEnteroNullable(
                                    reader,
                                    "IdIglesia"),

                            Iglesia =
                                LeerTexto(
                                    reader,
                                    "Iglesia"),

                            IdRegion =
                                LeerEnteroNullable(
                                    reader,
                                    "IdRegion"),

                            Region =
                                LeerTexto(
                                    reader,
                                    "Region"),

                            IdDistrito =
                                LeerEnteroNullable(
                                    reader,
                                    "IdDistrito"),

                            Distrito =
                                LeerTexto(
                                    reader,
                                    "Distrito"),

                            TotalAportado =
                                LeerDecimal(
                                    reader,
                                    "TotalAportado"),

                            CantidadRecibos =
                                LeerEntero(
                                    reader,
                                    "CantidadRecibos"),

                            MisionerosApoyados =
                                LeerEntero(
                                    reader,
                                    "MisionerosApoyados"),

                            PromedioAporte =
                                LeerDecimal(
                                    reader,
                                    "PromedioAporte"),

                            PrimeraAportacion =
                                LeerFechaNullable(
                                    reader,
                                    "PrimeraAportacion"),

                            UltimaAportacion =
                                LeerFechaNullable(
                                    reader,
                                    "UltimaAportacion")
                        });
                }
            }


            /*
             * RESULTADO 8:
             * Control de calidad.
             */
            if (await reader.NextResultAsync(ct) &&
                await reader.ReadAsync(ct))
            {
                resultado.Calidad =
                    new DashboardCalidadDatos
                    {
                        RecibosSinIglesia =
                            LeerEntero(
                                reader,
                                "RecibosSinIglesia"),

                        RecibosSinMisionero =
                            LeerEntero(
                                reader,
                                "RecibosSinMisionero"),

                        RecibosSinRegion =
                            LeerEntero(
                                reader,
                                "RecibosSinRegion"),

                        RecibosSinDistrito =
                            LeerEntero(
                                reader,
                                "RecibosSinDistrito"),

                        FilasSinConcepto =
                            LeerEntero(
                                reader,
                                "FilasSinConcepto"),

                        ConceptosNoClasificados =
                            LeerEntero(
                                reader,
                                "ConceptosNoClasificados"),

                        RecibosDescuadrados =
                            LeerEntero(
                                reader,
                                "RecibosDescuadrados"),

                        DiferenciaTotal =
                            LeerDecimal(
                                reader,
                                "DiferenciaTotal")
                    };
            }

            return resultado;
        }


        public async Task<List<IngresoMensual>>
            GetIngresosComparativo4AniosAsync(
                int anio,
                CancellationToken ct = default)
        {
            const string procedimiento =
                "dbo.SpDashboard_IngresosComparativo4Anios_Meses";

            var resultado =
                new List<IngresoMensual>(48);

            await using var conexion =
                new SqlConnection(_connectionString);

            await conexion.OpenAsync(ct);

            await using var comando =
                new SqlCommand(
                    procedimiento,
                    conexion);

            comando.CommandType =
                CommandType.StoredProcedure;

            comando.CommandTimeout =
                120;

            comando.Parameters
                .Add("@Anio", SqlDbType.Int)
                .Value = anio;

            await using var reader =
                await comando.ExecuteReaderAsync(ct);

            while (await reader.ReadAsync(ct))
            {
                resultado.Add(
                    new IngresoMensual
                    {
                        Anio =
                            LeerEntero(
                                reader,
                                "Anio"),

                        IdMes =
                            LeerEntero(
                                reader,
                                "IdMes"),

                        Total =
                            LeerDecimal(
                                reader,
                                "Total"),

                        Mes =
                            string.Empty
                    });
            }

            return resultado;
        }


        private static void AgregarParametroEnteroOpcional(
            SqlCommand comando,
            string nombre,
            int? valor)
        {
            var parametro =
                comando.Parameters.Add(
                    nombre,
                    SqlDbType.Int);

            parametro.Value =
                valor.HasValue
                    ? valor.Value
                    : DBNull.Value;
        }


        private static string LeerTexto(
            SqlDataReader reader,
            string columna)
        {
            var valor =
                reader[columna];

            return valor == DBNull.Value
                ? string.Empty
                : Convert.ToString(
                    valor,
                    CultureInfo.InvariantCulture)
                  ?? string.Empty;
        }


        private static int LeerEntero(
            SqlDataReader reader,
            string columna)
        {
            var valor =
                reader[columna];

            return valor == DBNull.Value
                ? 0
                : Convert.ToInt32(
                    valor,
                    CultureInfo.InvariantCulture);
        }


        private static int? LeerEnteroNullable(
            SqlDataReader reader,
            string columna)
        {
            var valor =
                reader[columna];

            return valor == DBNull.Value
                ? null
                : Convert.ToInt32(
                    valor,
                    CultureInfo.InvariantCulture);
        }


        private static decimal LeerDecimal(
            SqlDataReader reader,
            string columna)
        {
            var valor =
                reader[columna];

            return valor == DBNull.Value
                ? 0m
                : Convert.ToDecimal(
                    valor,
                    CultureInfo.InvariantCulture);
        }


        private static DateTime LeerFecha(
            SqlDataReader reader,
            string columna)
        {
            var valor =
                reader[columna];

            return valor == DBNull.Value
                ? DateTime.MinValue
                : Convert.ToDateTime(
                    valor,
                    CultureInfo.InvariantCulture);
        }


        private static DateTime? LeerFechaNullable(
            SqlDataReader reader,
            string columna)
        {
            var valor =
                reader[columna];

            return valor == DBNull.Value
                ? null
                : Convert.ToDateTime(
                    valor,
                    CultureInfo.InvariantCulture);
        }


        private static string FormatearNombreMes(
            string nombreMes)
        {
            if (string.IsNullOrWhiteSpace(nombreMes))
            {
                return string.Empty;
            }

            var cultura =
                CultureInfo.GetCultureInfo("es-NI");

            nombreMes =
                nombreMes.Trim().ToLower(cultura);

            return cultura.TextInfo
                .ToTitleCase(nombreMes);
        }
    }
}