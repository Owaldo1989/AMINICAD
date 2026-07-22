using AMINICAD.Models.Misioneros;
using System.Data;
using System.Data.SqlClient;

namespace AMINICAD.Data.Misioneros
{
    public sealed class MisioneroDetalleRepository
    {
        private readonly string _connectionString;

        public MisioneroDetalleRepository(
            IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString(
                    "DefaultConnection"
                )
                ?? throw new InvalidOperationException(
                    "No se encontró DefaultConnection."
                );
        }

        public async Task<MisioneroDetalleDto>
            ObtenerDetalleAsync(
                int idMisionero,
                int anio,
                CancellationToken cancellationToken = default)
        {
            var resultado =
                new MisioneroDetalleDto();

            await using var connection =
                new SqlConnection(_connectionString);

            await using var command =
                new SqlCommand(
                    "dbo.SpMisioneroDetalleResumen",
                    connection
                );

            command.CommandType =
                CommandType.StoredProcedure;

            command.CommandTimeout = 120;

            command.Parameters.Add(
                new SqlParameter(
                    "@IdMisionero",
                    SqlDbType.Int
                )
                {
                    Value = idMisionero
                }
            );

            command.Parameters.Add(
                new SqlParameter(
                    "@Anio",
                    SqlDbType.Int
                )
                {
                    Value = anio
                }
            );

            await connection.OpenAsync(
                cancellationToken
            );

            await using var reader =
                await command.ExecuteReaderAsync(
                    cancellationToken
                );

            // RESULTADO 1
            if (await reader.ReadAsync(
                    cancellationToken))
            {
                resultado.Informacion =
                    new MisioneroInformacionDto
                    {
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

                        Pais =
                            ObtenerString(
                                reader,
                                "Pais"
                            ),

                        Nota =
                            ObtenerString(
                                reader,
                                "Nota"
                            ),

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

                        Anio =
                            ObtenerInt(
                                reader,
                                "Anio"
                            ),

                        FechaInicial =
                            ObtenerDateTime(
                                reader,
                                "FechaInicial"
                            ),

                        FechaFinal =
                            ObtenerDateTime(
                                reader,
                                "FechaFinal"
                            )
                    };
            }

            // RESULTADO 2
            if (await reader.NextResultAsync(
                    cancellationToken)
                &&
                await reader.ReadAsync(
                    cancellationToken))
            {
                resultado.Resumen =
                    new MisioneroResumenFinancieroDto
                    {
                        SaldoInicial =
                            ObtenerDecimal(
                                reader,
                                "SaldoInicial"
                            ),

                        IngresosPeriodo =
                            ObtenerDecimal(
                                reader,
                                "IngresosPeriodo"
                            ),

                        EntregasPeriodo =
                            ObtenerDecimal(
                                reader,
                                "EntregasPeriodo"
                            ),

                        SaldoFinal =
                            ObtenerDecimal(
                                reader,
                                "SaldoFinal"
                            ),

                        FondosDisponibles =
                            ObtenerDecimal(
                                reader,
                                "FondosDisponibles"
                            ),

                        CantidadIngresos =
                            ObtenerInt(
                                reader,
                                "CantidadIngresos"
                            ),

                        CantidadPlanillas =
                            ObtenerInt(
                                reader,
                                "CantidadPlanillas"
                            ),

                        UltimoIngreso =
                            ObtenerDateTimeNullable(
                                reader,
                                "UltimoIngreso"
                            ),

                        UltimaEntrega =
                            ObtenerDateTimeNullable(
                                reader,
                                "UltimaEntrega"
                            ),

                        PorcentajeEntregado =
                            ObtenerDecimal(
                                reader,
                                "PorcentajeEntregado"
                            )
                    };
            }

            // RESULTADO 3
            if (await reader.NextResultAsync(
                    cancellationToken)
                &&
                await reader.ReadAsync(
                    cancellationToken))
            {
                resultado.Comparacion =
                    new MisioneroComparacionDto
                    {
                        IngresosPeriodoActual =
                            ObtenerDecimal(
                                reader,
                                "IngresosPeriodoActual"
                            ),

                        IngresosPeriodoAnterior =
                            ObtenerDecimal(
                                reader,
                                "IngresosPeriodoAnterior"
                            ),

                        VariacionMonto =
                            ObtenerDecimal(
                                reader,
                                "VariacionMonto"
                            ),

                        VariacionPorcentual =
                            ObtenerDecimal(
                                reader,
                                "VariacionPorcentual"
                            ),

                        EstadoVariacion =
                            ObtenerString(
                                reader,
                                "EstadoVariacion"
                            ),

                        FechaInicialAnterior =
                            ObtenerDateTime(
                                reader,
                                "FechaInicialAnterior"
                            ),

                        FechaFinalAnterior =
                            ObtenerDateTime(
                                reader,
                                "FechaFinalAnterior"
                            )
                    };
            }

            // RESULTADO 4
            if (await reader.NextResultAsync(
                    cancellationToken))
            {
                while (await reader.ReadAsync(
                           cancellationToken))
                {
                    resultado.Mensual.Add(
                        new MisioneroMensualDto
                        {
                            IdMes =
                                ObtenerInt(
                                    reader,
                                    "IdMes"
                                ),

                            Mes =
                                ObtenerString(
                                    reader,
                                    "Mes"
                                ),

                            InicioMes =
                                ObtenerDateTime(
                                    reader,
                                    "InicioMes"
                                ),

                            FinMes =
                                ObtenerDateTime(
                                    reader,
                                    "FinMes"
                                ),

                            SaldoInicial =
                                ObtenerDecimal(
                                    reader,
                                    "SaldoInicial"
                                ),

                            Ingresos =
                                ObtenerDecimal(
                                    reader,
                                    "Ingresos"
                                ),

                            Entregas =
                                ObtenerDecimal(
                                    reader,
                                    "Entregas"
                                ),

                            Variacion =
                                ObtenerDecimal(
                                    reader,
                                    "Variacion"
                                ),

                            SaldoFinal =
                                ObtenerDecimal(
                                    reader,
                                    "SaldoFinal"
                                ),

                            IngresosAnioAnterior =
                                ObtenerDecimal(
                                    reader,
                                    "IngresosAnioAnterior"
                                ),

                            VariacionInteranual =
                                ObtenerDecimal(
                                    reader,
                                    "VariacionInteranual"
                                ),

                            VariacionInteranualPorcentual =
                                ObtenerDecimal(
                                    reader,
                                    "VariacionInteranualPorcentual"
                                ),

                            EsMesFuturo =
                                ObtenerBool(
                                    reader,
                                    "EsMesFuturo"
                                )
                        }
                    );
                }
            }

            // RESULTADO 5
            if (await reader.NextResultAsync(
                    cancellationToken))
            {
                while (await reader.ReadAsync(
                           cancellationToken))
                {
                    resultado.Planillas.Add(
                        new MisioneroPlanillaDto
                        {
                            NoPlanilla =
                                ObtenerString(
                                    reader,
                                    "NoPlanilla"
                                ),

                            Fecha =
                                ObtenerDateTime(
                                    reader,
                                    "Fecha"
                                ),

                            Concepto =
                                ObtenerString(
                                    reader,
                                    "Concepto"
                                ),

                            NetoEntregado =
                                ObtenerDecimal(
                                    reader,
                                    "NetoEntregado"
                                )
                        }
                    );
                }
            }

            // RESULTADO 6
            if (await reader.NextResultAsync(
                    cancellationToken)
                &&
                await reader.ReadAsync(
                    cancellationToken))
            {
                resultado.PatrocinadoresResumen =
                    new MisioneroPatrocinadoresResumenDto
                    {
                        CantidadPatrocinadores =
                            ObtenerInt(
                                reader,
                                "CantidadPatrocinadores"
                            ),

                        CantidadPatrocinadores80 =
                            ObtenerInt(
                                reader,
                                "CantidadPatrocinadores80"
                            ),

                        PrincipalPatrocinador =
                            ObtenerString(
                                reader,
                                "PrincipalPatrocinador"
                            ),

                        TipoPrincipalPatrocinador =
                            ObtenerString(
                                reader,
                                "TipoPrincipalPatrocinador"
                            ),

                        TotalPrincipalPatrocinador =
                            ObtenerDecimal(
                                reader,
                                "TotalPrincipalPatrocinador"
                            ),

                        ParticipacionPrincipal =
                            ObtenerDecimal(
                                reader,
                                "ParticipacionPrincipal"
                            ),

                        DistritoPrincipal =
                            ObtenerString(
                                reader,
                                "DistritoPrincipal"
                            ),

                        TotalDistritoPrincipal =
                            ObtenerDecimal(
                                reader,
                                "TotalDistritoPrincipal"
                            ),

                        CantidadBajoPromedio =
                            ObtenerInt(
                                reader,
                                "CantidadBajoPromedio"
                            ),

                        CantidadNuncaAportaron =
                            ObtenerInt(
                                reader,
                                "CantidadNuncaAportaron"
                            ),

                        CantidadSinAportePeriodo =
                            ObtenerInt(
                                reader,
                                "CantidadSinAportePeriodo"
                            )
                    };
            }

            // RESULTADO 7
            if (await reader.NextResultAsync(
                    cancellationToken))
            {
                while (await reader.ReadAsync(
                           cancellationToken))
                {
                    resultado.PrincipalesPatrocinadores.Add(
                        new MisioneroPatrocinadorDto
                        {
                            IdAportante =
                                ObtenerInt(
                                    reader,
                                    "IdAportante"
                                ),

                            Aportante =
                                ObtenerString(
                                    reader,
                                    "Aportante"
                                ),

                            CodigoAportante =
                                ObtenerString(
                                    reader,
                                    "CodigoAportante"
                                ),

                            IdTipoDonante =
                                ObtenerInt(
                                    reader,
                                    "IdTipoDonante"
                                ),

                            TipoDonante =
                                ObtenerString(
                                    reader,
                                    "TipoDonante"
                                ),

                            IdDistrito =
                                ObtenerIntNullable(
                                    reader,
                                    "IdDistrito"
                                ),

                            Distrito =
                                ObtenerString(
                                    reader,
                                    "Distrito"
                                ),

                            CantidadAportes =
                                ObtenerInt(
                                    reader,
                                    "CantidadAportes"
                                ),

                            MesesConAporte =
                                ObtenerInt(
                                    reader,
                                    "MesesConAporte"
                                ),

                            TotalAportado =
                                ObtenerDecimal(
                                    reader,
                                    "TotalAportado"
                                ),

                            Participacion =
                                ObtenerDecimal(
                                    reader,
                                    "Participacion"
                                ),

                            ParticipacionAcumulada =
                                ObtenerDecimal(
                                    reader,
                                    "ParticipacionAcumulada"
                                ),

                            PrimerAporte =
                                ObtenerDateTimeNullable(
                                    reader,
                                    "PrimerAporte"
                                ),

                            UltimoAporte =
                                ObtenerDateTimeNullable(
                                    reader,
                                    "UltimoAporte"
                                )
                        }
                    );
                }
            }

            // RESULTADO 8
            if (await reader.NextResultAsync(
                    cancellationToken))
            {
                while (await reader.ReadAsync(
                           cancellationToken))
                {
                    resultado.TiposDonante.Add(
                        new MisioneroTipoDonanteResumenDto
                        {
                            IdTipoDonante =
                                ObtenerInt(
                                    reader,
                                    "IdTipoDonante"
                                ),

                            TipoDonante =
                                ObtenerString(
                                    reader,
                                    "TipoDonante"
                                ),

                            CantidadAportantes =
                                ObtenerInt(
                                    reader,
                                    "CantidadAportantes"
                                ),

                            TotalAportado =
                                ObtenerDecimal(
                                    reader,
                                    "TotalAportado"
                                ),

                            PromedioAporte =
                                ObtenerDecimal(
                                    reader,
                                    "PromedioAporte"
                                ),

                            CantidadBajoPromedio =
                                ObtenerInt(
                                    reader,
                                    "CantidadBajoPromedio"
                                ),

                            Participacion =
                                ObtenerDecimal(
                                    reader,
                                    "Participacion"
                                )
                        }
                    );
                }
            }

            // RESULTADO 9
            if (await reader.NextResultAsync(
                    cancellationToken))
            {
                while (await reader.ReadAsync(
                           cancellationToken))
                {
                    resultado.BajoPromedio.Add(
                        new MisioneroAportanteBajoPromedioDto
                        {
                            IdAportante =
                                ObtenerInt(
                                    reader,
                                    "IdAportante"
                                ),

                            Aportante =
                                ObtenerString(
                                    reader,
                                    "Aportante"
                                ),

                            CodigoAportante =
                                ObtenerString(
                                    reader,
                                    "CodigoAportante"
                                ),

                            IdTipoDonante =
                                ObtenerInt(
                                    reader,
                                    "IdTipoDonante"
                                ),

                            TipoDonante =
                                ObtenerString(
                                    reader,
                                    "TipoDonante"
                                ),

                            IdDistrito =
                                ObtenerIntNullable(
                                    reader,
                                    "IdDistrito"
                                ),

                            Distrito =
                                ObtenerString(
                                    reader,
                                    "Distrito"
                                ),

                            TotalAportado =
                                ObtenerDecimal(
                                    reader,
                                    "TotalAportado"
                                ),

                            PromedioTipoDonante =
                                ObtenerDecimal(
                                    reader,
                                    "PromedioTipoDonante"
                                ),

                            DiferenciaPromedio =
                                ObtenerDecimal(
                                    reader,
                                    "DiferenciaPromedio"
                                ),

                            CumplimientoPromedio =
                                ObtenerDecimal(
                                    reader,
                                    "CumplimientoPromedio"
                                ),

                            CantidadAportes =
                                ObtenerInt(
                                    reader,
                                    "CantidadAportes"
                                ),

                            MesesConAporte =
                                ObtenerInt(
                                    reader,
                                    "MesesConAporte"
                                ),

                            UltimoAporte =
                                ObtenerDateTimeNullable(
                                    reader,
                                    "UltimoAporte"
                                )
                        }
                    );
                }
            }

            // RESULTADO 10
            if (await reader.NextResultAsync(
                    cancellationToken))
            {
                while (await reader.ReadAsync(
                           cancellationToken))
                {
                    resultado.Distritos.Add(
                        new MisioneroDistritoAporteDto
                        {
                            IdDistrito =
                                ObtenerIntNullable(
                                    reader,
                                    "IdDistrito"
                                ),

                            Distrito =
                                ObtenerString(
                                    reader,
                                    "Distrito"
                                ),

                            CantidadAportantes =
                                ObtenerInt(
                                    reader,
                                    "CantidadAportantes"
                                ),

                            CantidadAportes =
                                ObtenerInt(
                                    reader,
                                    "CantidadAportes"
                                ),

                            TotalAportado =
                                ObtenerDecimal(
                                    reader,
                                    "TotalAportado"
                                ),

                            Participacion =
                                ObtenerDecimal(
                                    reader,
                                    "Participacion"
                                )
                        }
                    );
                }
            }

            // RESULTADO 11
            if (await reader.NextResultAsync(
                    cancellationToken))
            {
                while (await reader.ReadAsync(
                           cancellationToken))
                {
                    resultado.Movimientos.Add(
                        new MisioneroMovimientoDto
                        {
                            Fecha =
                                ObtenerDateTime(
                                    reader,
                                    "Fecha"
                                ),

                            Tipo =
                                ObtenerString(
                                    reader,
                                    "Tipo"
                                ),

                            Documento =
                                ObtenerString(
                                    reader,
                                    "Documento"
                                ),

                            Concepto =
                                ObtenerString(
                                    reader,
                                    "Concepto"
                                ),

                            Entrada =
                                ObtenerDecimal(
                                    reader,
                                    "Entrada"
                                ),

                            Salida =
                                ObtenerDecimal(
                                    reader,
                                    "Salida"
                                ),

                            SaldoAcumulado =
                                ObtenerDecimal(
                                    reader,
                                    "SaldoAcumulado"
                                )
                        }
                    );
                }
            }

            // RESULTADO 12
            if (await reader.NextResultAsync(
                    cancellationToken))
            {
                while (await reader.ReadAsync(
                           cancellationToken))
                {
                    resultado.Prestamos.Add(
                        new MisioneroPrestamoDto
                        {
                            NoPrestamo = ObtenerString(reader, "NoPrestamo"),
                            Persona = ObtenerString(reader, "Persona"),
                            Concepto = ObtenerString(reader, "Concepto"),
                            Fecha = ObtenerDateTime(reader, "Fecha"),
                            Monto = ObtenerDecimal(reader, "Monto"),
                            Saldo = ObtenerDecimal(reader, "Saldo")
                        }
                    );
                }
            }

            // RESULTADO 13
            if (await reader.NextResultAsync(
                    cancellationToken))
            {
                while (await reader.ReadAsync(
                           cancellationToken))
                {
                    resultado.FondosPorConcepto.Add(
                        new MisioneroFondoConceptoDto
                        {
                            IdRetencion = ObtenerInt(reader, "IdRetencion"),
                            Concepto = ObtenerString(reader, "Concepto"),
                            EsRetencion = ObtenerBool(reader, "EsRetencion"),
                            Ingresos = ObtenerDecimal(reader, "Ingresos"),
                            Entregas = ObtenerDecimal(reader, "Entregas"),
                            Saldo = ObtenerDecimal(reader, "Saldo")
                        }
                    );
                }
            }

            return resultado;
        }

        private static int ObtenerInt(
            SqlDataReader reader,
            string columna)
        {
            var ordinal =
                reader.GetOrdinal(columna);

            return reader.IsDBNull(ordinal)
                ? 0
                : Convert.ToInt32(
                    reader.GetValue(ordinal)
                );
        }

        private static int? ObtenerIntNullable(
            SqlDataReader reader,
            string columna)
        {
            var ordinal =
                reader.GetOrdinal(columna);

            return reader.IsDBNull(ordinal)
                ? null
                : Convert.ToInt32(
                    reader.GetValue(ordinal)
                );
        }

        private static decimal ObtenerDecimal(
            SqlDataReader reader,
            string columna)
        {
            var ordinal =
                reader.GetOrdinal(columna);

            return reader.IsDBNull(ordinal)
                ? 0m
                : Convert.ToDecimal(
                    reader.GetValue(ordinal)
                );
        }

        private static string ObtenerString(
            SqlDataReader reader,
            string columna)
        {
            var ordinal =
                reader.GetOrdinal(columna);

            return reader.IsDBNull(ordinal)
                ? string.Empty
                : Convert.ToString(
                    reader.GetValue(ordinal)
                )?.Trim() ?? string.Empty;
        }

        private static DateTime ObtenerDateTime(
            SqlDataReader reader,
            string columna)
        {
            var ordinal =
                reader.GetOrdinal(columna);

            return reader.IsDBNull(ordinal)
                ? DateTime.MinValue
                : Convert.ToDateTime(
                    reader.GetValue(ordinal)
                );
        }

        private static DateTime? ObtenerDateTimeNullable(
            SqlDataReader reader,
            string columna)
        {
            var ordinal =
                reader.GetOrdinal(columna);

            return reader.IsDBNull(ordinal)
                ? null
                : Convert.ToDateTime(
                    reader.GetValue(ordinal)
                );
        }

        private static bool ObtenerBool(
            SqlDataReader reader,
            string columna)
        {
            var ordinal =
                reader.GetOrdinal(columna);

            return !reader.IsDBNull(ordinal)
                   &&
                   Convert.ToBoolean(
                       reader.GetValue(ordinal)
                   );
        }
    }
}
