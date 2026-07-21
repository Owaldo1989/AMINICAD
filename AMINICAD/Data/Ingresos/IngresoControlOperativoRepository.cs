using AMINICAD.Models.Ingresos;
using System.Data;
using System.Data.SqlClient;

namespace AMINICAD.Data.Ingresos
{
    public class IngresoControlOperativoRepository
    {
        private readonly string _connectionString;

        public IngresoControlOperativoRepository(
            IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "No se encontró la cadena de conexión DefaultConnection."
                );
        }

        public async Task<IngresoControlOperativoDto>
            ObtenerControlOperativoAsync(
                DateTime fechaInicialRegistro,
                DateTime fechaFinalRegistro,
                CancellationToken cancellationToken = default)
        {
            var resultado = new IngresoControlOperativoDto();

            await using var connection =
                new SqlConnection(_connectionString);

            await using var command =
                new SqlCommand(
                    "dbo.SpIngresosControlOperativo",
                    connection
                );

            command.CommandType =
                CommandType.StoredProcedure;

            command.CommandTimeout = 120;

            command.Parameters.Add(
                new SqlParameter(
                    "@FechaInicialRegistro",
                    SqlDbType.Date
                )
                {
                    Value = fechaInicialRegistro.Date
                }
            );

            command.Parameters.Add(
                new SqlParameter(
                    "@FechaFinalRegistro",
                    SqlDbType.Date
                )
                {
                    Value = fechaFinalRegistro.Date
                }
            );

            await connection.OpenAsync(
                cancellationToken
            );

            await using var reader =
                await command.ExecuteReaderAsync(
                    cancellationToken
                );

            /*
             * RESULTADO 1
             * Resumen operativo
             */
            if (await reader.ReadAsync(
                    cancellationToken))
            {
                resultado.Resumen =
                    LeerResumen(reader);
            }

            /*
             * RESULTADO 2
             * Actividad por día de grabación
             */
            if (await reader.NextResultAsync(
                    cancellationToken))
            {
                while (await reader.ReadAsync(
                           cancellationToken))
                {
                    resultado.ActividadDiaria.Add(
                        LeerActividadDiaria(reader)
                    );
                }
            }

            /*
             * RESULTADO 3
             * Fechas contables procesadas
             */
            if (await reader.NextResultAsync(
                    cancellationToken))
            {
                while (await reader.ReadAsync(
                           cancellationToken))
                {
                    resultado.FechasProcesadas.Add(
                        LeerFechaProcesada(reader)
                    );
                }
            }

            /*
             * RESULTADO 4
             * Distribución por cuentas
             */
            if (await reader.NextResultAsync(
                    cancellationToken))
            {
                while (await reader.ReadAsync(
                           cancellationToken))
                {
                    resultado.Cuentas.Add(
                        LeerCuenta(reader)
                    );
                }
            }

            /*
             * RESULTADO 5
             * Alertas resumidas
             */
            if (await reader.NextResultAsync(
                    cancellationToken))
            {
                if (await reader.ReadAsync(
                        cancellationToken))
                {
                    resultado.Alertas =
                        LeerAlertas(reader);
                }
            }

            /*
             * RESULTADO 6
             * Registros que requieren revisión
             */
            if (await reader.NextResultAsync(
                    cancellationToken))
            {
                while (await reader.ReadAsync(
                           cancellationToken))
                {
                    resultado.RegistrosRevision.Add(
                        LeerRegistroRevision(reader)
                    );
                }
            }

            return resultado;
        }

        private static IngresoResumenOperativoDto
            LeerResumen(SqlDataReader reader)
        {
            return new IngresoResumenOperativoDto
            {
                FechaInicialRegistro =
                    ObtenerDateTime(
                        reader,
                        "FechaInicialRegistro"
                    ),

                FechaFinalRegistro =
                    ObtenerDateTime(
                        reader,
                        "FechaFinalRegistro"
                    ),

                CantidadIngresos =
                    ObtenerInt(
                        reader,
                        "CantidadIngresos"
                    ),

                CantidadRecibosFisicos =
                    ObtenerInt(
                        reader,
                        "CantidadRecibosFisicos"
                    ),

                TotalRegistrado =
                    ObtenerDecimal(
                        reader,
                        "TotalRegistrado"
                    ),

                TotalCajaGeneral =
                    ObtenerDecimal(
                        reader,
                        "TotalCajaGeneral"
                    ),

                TotalOtrasCuentas =
                    ObtenerDecimal(
                        reader,
                        "TotalOtrasCuentas"
                    ),

                IngresosMismoDia =
                    ObtenerInt(
                        reader,
                        "IngresosMismoDia"
                    ),

                IngresosFechasAnteriores =
                    ObtenerInt(
                        reader,
                        "IngresosFechasAnteriores"
                    ),

                IngresosFechaFutura =
                    ObtenerInt(
                        reader,
                        "IngresosFechaFutura"
                    ),

                FechaContableMasAntigua =
                    ObtenerDateTimeNullable(
                        reader,
                        "FechaContableMasAntigua"
                    ),

                MayorDiferenciaDias =
                    ObtenerInt(
                        reader,
                        "MayorDiferenciaDias"
                    ),

                PromedioDiasDiferencia =
                    ObtenerDecimal(
                        reader,
                        "PromedioDiasDiferencia"
                    ),

                CantidadCuentasUtilizadas =
                    ObtenerInt(
                        reader,
                        "CantidadCuentasUtilizadas"
                    )
            };
        }

        private static IngresoActividadDiariaDto
            LeerActividadDiaria(SqlDataReader reader)
        {
            return new IngresoActividadDiariaDto
            {
                FechaGrabacion =
                    ObtenerDateTime(reader, "FechaGrabacion"),

                CantidadIngresos =
                    ObtenerInt(reader, "CantidadIngresos"),

                CantidadRecibosFisicos =
                    ObtenerInt(reader, "CantidadRecibosFisicos"),

                CantidadFechasContables =
                    ObtenerInt(reader, "CantidadFechasContables"),

                TotalRegistrado =
                    ObtenerDecimal(reader, "TotalRegistrado"),

                TotalCajaGeneral =
                    ObtenerDecimal(reader, "TotalCajaGeneral"),

                TotalOtrasCuentas =
                    ObtenerDecimal(reader, "TotalOtrasCuentas"),

                IngresosFechasAnteriores =
                    ObtenerInt(reader, "IngresosFechasAnteriores"),

                CantidadAlertas =
                    ObtenerInt(reader, "CantidadAlertas")
            };
        }

        private static IngresoFechaProcesadaDto
            LeerFechaProcesada(SqlDataReader reader)
        {
            return new IngresoFechaProcesadaDto
            {
                FechaContable =
                    ObtenerDateTime(
                        reader,
                        "FechaContable"
                    ),

                DiasDiferencia =
                    ObtenerInt(
                        reader,
                        "DiasDiferencia"
                    ),

                CantidadIngresos =
                    ObtenerInt(
                        reader,
                        "CantidadIngresos"
                    ),

                CantidadRecibosFisicos =
                    ObtenerInt(
                        reader,
                        "CantidadRecibosFisicos"
                    ),

                TotalRegistrado =
                    ObtenerDecimal(
                        reader,
                        "TotalRegistrado"
                    ),

                TotalCajaGeneral =
                    ObtenerDecimal(
                        reader,
                        "TotalCajaGeneral"
                    ),

                TotalOtrasCuentas =
                    ObtenerDecimal(
                        reader,
                        "TotalOtrasCuentas"
                    ),

                PrimeraGrabacion =
                    ObtenerDateTimeNullable(
                        reader,
                        "PrimeraGrabacion"
                    ),

                UltimaGrabacion =
                    ObtenerDateTimeNullable(
                        reader,
                        "UltimaGrabacion"
                    )
            };
        }

        private static IngresoCuentaDto
            LeerCuenta(SqlDataReader reader)
        {
            return new IngresoCuentaDto
            {
                CodCuenta =
                    ObtenerStringNullable(
                        reader,
                        "CodCuenta"
                    ),

                Cuenta =
                    ObtenerString(
                        reader,
                        "Cuenta"
                    ),

                CantidadIngresos =
                    ObtenerInt(
                        reader,
                        "CantidadIngresos"
                    ),

                CantidadRecibosFisicos =
                    ObtenerInt(
                        reader,
                        "CantidadRecibosFisicos"
                    ),

                TotalRegistrado =
                    ObtenerDecimal(
                        reader,
                        "TotalRegistrado"
                    ),

                PorcentajeParticipacion =
                    ObtenerDecimal(
                        reader,
                        "PorcentajeParticipacion"
                    ),

                FechaContableMasAntigua =
                    ObtenerDateTimeNullable(
                        reader,
                        "FechaContableMasAntigua"
                    ),

                MayorDiferenciaDias =
                    ObtenerInt(
                        reader,
                        "MayorDiferenciaDias"
                    ),

                EsCajaGeneral =
                    ObtenerBool(
                        reader,
                        "EsCajaGeneral"
                    )
            };
        }

        private static IngresoAlertasDto
            LeerAlertas(SqlDataReader reader)
        {
            return new IngresoAlertasDto
            {
                IngresosSinReciboFisico =
                    ObtenerInt(
                        reader,
                        "IngresosSinReciboFisico"
                    ),

                IngresosSinCuenta =
                    ObtenerInt(
                        reader,
                        "IngresosSinCuenta"
                    ),

                IngresosCuentaNoIdentificada =
                    ObtenerInt(
                        reader,
                        "IngresosCuentaNoIdentificada"
                    ),

                IngresosFechaPosterior =
                    ObtenerInt(
                        reader,
                        "IngresosFechaPosterior"
                    ),

                IngresosConMasTresDias =
                    ObtenerInt(
                        reader,
                        "IngresosConMasTresDias"
                    ),

                CajaGeneralConFechaAnterior =
                    ObtenerInt(
                        reader,
                        "CajaGeneralConFechaAnterior"
                    )
            };
        }

        private static IngresoRevisionDto
            LeerRegistroRevision(SqlDataReader reader)
        {
            return new IngresoRevisionDto
            {
                NoIngreso =
                    ObtenerString(
                        reader,
                        "NoIngreso"
                    ),

                NoRecibo =
                    ObtenerStringNullable(
                        reader,
                        "NoRecibo"
                    ),

                FechaContable =
                    ObtenerDateTime(
                        reader,
                        "FechaContable"
                    ),

                FechaHoraGrabacion =
                    ObtenerDateTime(
                        reader,
                        "FechaHoraGrabacion"
                    ),

                DiasDiferencia =
                    ObtenerInt(
                        reader,
                        "DiasDiferencia"
                    ),

                CodCuenta =
                    ObtenerStringNullable(
                        reader,
                        "CodCuenta"
                    ),

                Cuenta =
                    ObtenerString(
                        reader,
                        "Cuenta"
                    ),

                Total =
                    ObtenerDecimal(
                        reader,
                        "Total"
                    ),

                MotivoRevision =
                    ObtenerString(
                        reader,
                        "MotivoRevision"
                    )
            };
        }

        private static int ObtenerInt(
            SqlDataReader reader,
            string columna)
        {
            var ordinal =
                reader.GetOrdinal(columna);

            if (reader.IsDBNull(ordinal))
                return 0;

            return Convert.ToInt32(
                reader.GetValue(ordinal)
            );
        }

        private static decimal ObtenerDecimal(
            SqlDataReader reader,
            string columna)
        {
            var ordinal =
                reader.GetOrdinal(columna);

            if (reader.IsDBNull(ordinal))
                return 0m;

            return Convert.ToDecimal(
                reader.GetValue(ordinal)
            );
        }

        private static bool ObtenerBool(
            SqlDataReader reader,
            string columna)
        {
            var ordinal =
                reader.GetOrdinal(columna);

            if (reader.IsDBNull(ordinal))
                return false;

            return Convert.ToBoolean(
                reader.GetValue(ordinal)
            );
        }

        private static DateTime ObtenerDateTime(
            SqlDataReader reader,
            string columna)
        {
            var ordinal =
                reader.GetOrdinal(columna);

            if (reader.IsDBNull(ordinal))
                return DateTime.MinValue;

            return Convert.ToDateTime(
                reader.GetValue(ordinal)
            );
        }

        private static DateTime?
            ObtenerDateTimeNullable(
                SqlDataReader reader,
                string columna)
        {
            var ordinal =
                reader.GetOrdinal(columna);

            if (reader.IsDBNull(ordinal))
                return null;

            return Convert.ToDateTime(
                reader.GetValue(ordinal)
            );
        }

        private static string ObtenerString(
            SqlDataReader reader,
            string columna)
        {
            var ordinal =
                reader.GetOrdinal(columna);

            if (reader.IsDBNull(ordinal))
                return string.Empty;

            return Convert.ToString(
                       reader.GetValue(ordinal)
                   )
                   ?? string.Empty;
        }

        private static string?
            ObtenerStringNullable(
                SqlDataReader reader,
                string columna)
        {
            var ordinal =
                reader.GetOrdinal(columna);

            if (reader.IsDBNull(ordinal))
                return null;

            return Convert.ToString(
                reader.GetValue(ordinal)
            );
        }
    }
}
