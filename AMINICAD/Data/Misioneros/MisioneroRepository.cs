using AMINICAD.Models.Misioneros;
using System.Data;
using System.Data.SqlClient;

namespace AMINICAD.Data.Misioneros
{
    public sealed class MisioneroRepository
    {
        private readonly string _connectionString;

        public MisioneroRepository(
            IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "No se encontró la cadena de conexión DefaultConnection."
                );
        }

        public async Task<MisioneroIndexDto>
            ObtenerIndexAsync(
                CancellationToken cancellationToken = default)
        {
            var resultado = new MisioneroIndexDto();

            await using var connection =
                new SqlConnection(_connectionString);

            await using var command =
                new SqlCommand(
                    "dbo.SpMisionerosIndex",
                    connection
                );

            command.CommandType =
                CommandType.StoredProcedure;

            command.CommandTimeout = 120;

            await connection.OpenAsync(
                cancellationToken
            );

            await using var reader =
                await command.ExecuteReaderAsync(
                    cancellationToken
                );

            /*
             * RESULTADO 1:
             * Resumen general.
             */
            if (await reader.ReadAsync(
                    cancellationToken))
            {
                resultado.Resumen =
                    LeerResumen(reader);
            }

            /*
             * RESULTADO 2:
             * Resumen por tipo de misión.
             */
            if (await reader.NextResultAsync(
                    cancellationToken))
            {
                while (await reader.ReadAsync(
                           cancellationToken))
                {
                    resultado.TiposMision.Add(
                        LeerTipoMision(reader)
                    );
                }
            }

            /*
             * RESULTADO 3:
             * Misioneros, familias, proyectos y fondos.
             */
            if (await reader.NextResultAsync(
                    cancellationToken))
            {
                while (await reader.ReadAsync(
                           cancellationToken))
                {
                    resultado.Misioneros.Add(
                        LeerMisionero(reader)
                    );
                }
            }

            return resultado;
        }

        private static MisioneroResumenGeneralDto
            LeerResumen(
                SqlDataReader reader)
        {
            return new MisioneroResumenGeneralDto
            {
                CantidadMisioneros =
                    ObtenerInt(
                        reader,
                        "CantidadMisioneros"
                    ),

                CantidadConMovimiento =
                    ObtenerInt(
                        reader,
                        "CantidadConMovimiento"
                    ),

                CantidadConSaldo =
                    ObtenerInt(
                        reader,
                        "CantidadConSaldo"
                    ),

                CantidadConciliados =
                    ObtenerInt(
                        reader,
                        "CantidadConciliados"
                    ),

                CantidadSinMovimiento =
                    ObtenerInt(
                        reader,
                        "CantidadSinMovimiento"
                    ),

                CantidadSaldoNegativo =
                    ObtenerInt(
                        reader,
                        "CantidadSaldoNegativo"
                    ),

                TotalIngresos =
                    ObtenerDecimal(
                        reader,
                        "TotalIngresos"
                    ),

                TotalSalidas =
                    ObtenerDecimal(
                        reader,
                        "TotalSalidas"
                    ),

                SaldoTotal =
                    ObtenerDecimal(
                        reader,
                        "SaldoTotal"
                    ),

                PorcentajeEjecucion =
                    ObtenerDecimal(
                        reader,
                        "PorcentajeEjecucion"
                    )
            };
        }

        private static TipoMisionResumenDto
            LeerTipoMision(
                SqlDataReader reader)
        {
            return new TipoMisionResumenDto
            {
                IdTipoMision =
                    ObtenerInt(
                        reader,
                        "IdTipoMision"
                    ),

                TipoMision =
                    ObtenerString(
                        reader,
                        "TipoMision"
                    ),

                CantidadMisioneros =
                    ObtenerInt(
                        reader,
                        "CantidadMisioneros"
                    ),

                CantidadConSaldo =
                    ObtenerInt(
                        reader,
                        "CantidadConSaldo"
                    ),

                CantidadSaldoNegativo =
                    ObtenerInt(
                        reader,
                        "CantidadSaldoNegativo"
                    ),

                TotalIngresos =
                    ObtenerDecimal(
                        reader,
                        "TotalIngresos"
                    ),

                TotalSalidas =
                    ObtenerDecimal(
                        reader,
                        "TotalSalidas"
                    ),

                Saldo =
                    ObtenerDecimal(
                        reader,
                        "Saldo"
                    ),

                PorcentajeEjecucion =
                    ObtenerDecimal(
                        reader,
                        "PorcentajeEjecucion"
                    ),

                PorcentajeSaldo =
                    ObtenerDecimal(
                        reader,
                        "PorcentajeSaldo"
                    )
            };
        }

        private static MisioneroListadoDto
            LeerMisionero(
                SqlDataReader reader)
        {
            return new MisioneroListadoDto
            {
                IdTipoMision =
                    ObtenerInt(
                        reader,
                        "IdTipoMision"
                    ),

                TipoMision =
                    ObtenerString(
                        reader,
                        "TipoMision"
                    ),

                IdMisionero =
                    ObtenerInt(
                        reader,
                        "IdMisionero"
                    ),

                Misionero =
                    ObtenerString(
                        reader,
                        "Misionero"
                    ),

                Ingreso =
                    ObtenerDecimal(
                        reader,
                        "Ingreso"
                    ),

                Salida =
                    ObtenerDecimal(
                        reader,
                        "Salida"
                    ),

                Saldo =
                    ObtenerDecimal(
                        reader,
                        "Saldo"
                    ),

                PorcentajeEjecucion =
                    ObtenerDecimal(
                        reader,
                        "PorcentajeEjecucion"
                    ),

                EstadoSaldo =
                    ObtenerString(
                        reader,
                        "EstadoSaldo"
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
            {
                return 0;
            }

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
            {
                return 0m;
            }

            return Convert.ToDecimal(
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
            {
                return string.Empty;
            }

            return Convert.ToString(
                       reader.GetValue(ordinal)
                   )
                   ?.Trim()
                   ?? string.Empty;
        }

    }
}
