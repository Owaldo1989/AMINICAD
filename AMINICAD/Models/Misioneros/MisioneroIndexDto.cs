namespace AMINICAD.Models.Misioneros
{
    public sealed class MisioneroIndexDto
    {
        public MisioneroResumenGeneralDto Resumen { get; set; }
            = new();

        public List<TipoMisionResumenDto> TiposMision { get; set; }
            = new();

        public List<MisioneroListadoDto> Misioneros { get; set; }
            = new();
    }

    public sealed class MisioneroResumenGeneralDto
    {
        public int CantidadMisioneros { get; set; }

        public int CantidadConMovimiento { get; set; }

        public int CantidadConSaldo { get; set; }

        public int CantidadConciliados { get; set; }

        public int CantidadSinMovimiento { get; set; }

        public int CantidadSaldoNegativo { get; set; }

        public decimal TotalIngresos { get; set; }

        public decimal TotalSalidas { get; set; }

        public decimal SaldoTotal { get; set; }

        public decimal PorcentajeEjecucion { get; set; }
    }

    public sealed class TipoMisionResumenDto
    {
        public int IdTipoMision { get; set; }

        public string TipoMision { get; set; } = string.Empty;

        public int CantidadMisioneros { get; set; }

        public int CantidadConSaldo { get; set; }

        public int CantidadSaldoNegativo { get; set; }

        public decimal TotalIngresos { get; set; }

        public decimal TotalSalidas { get; set; }

        public decimal Saldo { get; set; }

        public decimal PorcentajeEjecucion { get; set; }

        public decimal PorcentajeSaldo { get; set; }
    }

    public sealed class MisioneroListadoDto
    {
        public int IdTipoMision { get; set; }

        public string TipoMision { get; set; } = string.Empty;

        public int IdMisionero { get; set; }

        public string Misionero { get; set; } = string.Empty;

        public decimal Ingreso { get; set; }

        public decimal Salida { get; set; }

        public decimal Saldo { get; set; }

        public decimal PorcentajeEjecucion { get; set; }

        public string EstadoSaldo { get; set; } = string.Empty;
    }

}
