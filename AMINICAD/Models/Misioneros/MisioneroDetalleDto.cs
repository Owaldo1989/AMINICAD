namespace AMINICAD.Models.Misioneros
{
    public sealed class MisioneroDetalleDto
    {
        public MisioneroInformacionDto Informacion { get; set; }
            = new();

        public MisioneroResumenFinancieroDto Resumen { get; set; }
            = new();

        public MisioneroComparacionDto Comparacion { get; set; }
            = new();

        public List<MisioneroMensualDto> Mensual { get; set; }
            = new();

        public List<MisioneroPlanillaDto> Planillas { get; set; }
            = new();

        public MisioneroPatrocinadoresResumenDto PatrocinadoresResumen
        {
            get;
            set;
        } = new();

        public List<MisioneroPatrocinadorDto> PrincipalesPatrocinadores
        {
            get;
            set;
        } = new();

        public List<MisioneroTipoDonanteResumenDto> TiposDonante
        {
            get;
            set;
        } = new();

        public List<MisioneroAportanteBajoPromedioDto> BajoPromedio
        {
            get;
            set;
        } = new();

        public List<MisioneroDistritoAporteDto> Distritos { get; set; }
            = new();

        public List<MisioneroMovimientoDto> Movimientos { get; set; }
            = new();

        public List<MisioneroPrestamoDto> Prestamos { get; set; }
            = new();

        public List<MisioneroFondoConceptoDto> FondosPorConcepto { get; set; }
            = new();
    }

    public sealed class MisioneroInformacionDto
    {
        public int IdMisionero { get; set; }

        public string Misionero { get; set; } = string.Empty;

        public string Pais { get; set; } = string.Empty;

        public string Nota { get; set; } = string.Empty;

        public int IdTipoMision { get; set; }

        public string TipoMision { get; set; } = string.Empty;

        public int Anio { get; set; }

        public DateTime FechaInicial { get; set; }

        public DateTime FechaFinal { get; set; }
    }

    public sealed class MisioneroResumenFinancieroDto
    {
        public decimal SaldoInicial { get; set; }

        public decimal IngresosPeriodo { get; set; }

        public decimal EntregasPeriodo { get; set; }

        public decimal SaldoFinal { get; set; }

        public decimal FondosDisponibles { get; set; }

        public int CantidadIngresos { get; set; }

        public int CantidadPlanillas { get; set; }

        public DateTime? UltimoIngreso { get; set; }

        public DateTime? UltimaEntrega { get; set; }

        public decimal PorcentajeEntregado { get; set; }
    }

    public sealed class MisioneroComparacionDto
    {
        public decimal IngresosPeriodoActual { get; set; }

        public decimal IngresosPeriodoAnterior { get; set; }

        public decimal VariacionMonto { get; set; }

        public decimal VariacionPorcentual { get; set; }

        public string EstadoVariacion { get; set; } = string.Empty;

        public DateTime FechaInicialAnterior { get; set; }

        public DateTime FechaFinalAnterior { get; set; }
    }

    public sealed class MisioneroMensualDto
    {
        public int IdMes { get; set; }

        public string Mes { get; set; } = string.Empty;

        public DateTime InicioMes { get; set; }

        public DateTime FinMes { get; set; }

        public decimal SaldoInicial { get; set; }

        public decimal Ingresos { get; set; }

        public decimal Entregas { get; set; }

        public decimal Variacion { get; set; }

        public decimal SaldoFinal { get; set; }

        public decimal IngresosAnioAnterior { get; set; }

        public decimal VariacionInteranual { get; set; }

        public decimal VariacionInteranualPorcentual { get; set; }

        public bool EsMesFuturo { get; set; }
    }

    public sealed class MisioneroPlanillaDto
    {
        public string NoPlanilla { get; set; } = string.Empty;

        public DateTime Fecha { get; set; }

        public string Concepto { get; set; } = string.Empty;

        public decimal NetoEntregado { get; set; }
    }

    public sealed class MisioneroPatrocinadoresResumenDto
    {
        public int CantidadPatrocinadores { get; set; }

        public int CantidadPatrocinadores80 { get; set; }

        public string PrincipalPatrocinador { get; set; }
            = string.Empty;

        public string TipoPrincipalPatrocinador { get; set; }
            = string.Empty;

        public decimal TotalPrincipalPatrocinador { get; set; }

        public decimal ParticipacionPrincipal { get; set; }

        public string DistritoPrincipal { get; set; }
            = string.Empty;

        public decimal TotalDistritoPrincipal { get; set; }

        public int CantidadBajoPromedio { get; set; }

        public int CantidadNuncaAportaron { get; set; }

        public int CantidadSinAportePeriodo { get; set; }
    }

    public sealed class MisioneroPatrocinadorDto
    {
        public int IdAportante { get; set; }

        public string Aportante { get; set; } = string.Empty;

        public string CodigoAportante { get; set; } = string.Empty;

        public int IdTipoDonante { get; set; }

        public string TipoDonante { get; set; } = string.Empty;

        public int? IdDistrito { get; set; }

        public string Distrito { get; set; } = string.Empty;

        public int CantidadAportes { get; set; }

        public int MesesConAporte { get; set; }

        public decimal TotalAportado { get; set; }

        public decimal Participacion { get; set; }

        public decimal ParticipacionAcumulada { get; set; }

        public DateTime? PrimerAporte { get; set; }

        public DateTime? UltimoAporte { get; set; }
    }

    public sealed class MisioneroTipoDonanteResumenDto
    {
        public int IdTipoDonante { get; set; }

        public string TipoDonante { get; set; } = string.Empty;

        public int CantidadAportantes { get; set; }

        public decimal TotalAportado { get; set; }

        public decimal PromedioAporte { get; set; }

        public int CantidadBajoPromedio { get; set; }

        public decimal Participacion { get; set; }
    }

    public sealed class MisioneroAportanteBajoPromedioDto
    {
        public int IdAportante { get; set; }

        public string Aportante { get; set; } = string.Empty;

        public string CodigoAportante { get; set; } = string.Empty;

        public int IdTipoDonante { get; set; }

        public string TipoDonante { get; set; } = string.Empty;

        public int? IdDistrito { get; set; }

        public string Distrito { get; set; } = string.Empty;

        public decimal TotalAportado { get; set; }

        public decimal PromedioTipoDonante { get; set; }

        public decimal DiferenciaPromedio { get; set; }

        public decimal CumplimientoPromedio { get; set; }

        public int CantidadAportes { get; set; }

        public int MesesConAporte { get; set; }

        public DateTime? UltimoAporte { get; set; }
    }

    public sealed class MisioneroDistritoAporteDto
    {
        public int? IdDistrito { get; set; }

        public string Distrito { get; set; } = string.Empty;

        public int CantidadAportantes { get; set; }

        public int CantidadAportes { get; set; }

        public decimal TotalAportado { get; set; }

        public decimal Participacion { get; set; }
    }

    public sealed class MisioneroMovimientoDto
    {
        public DateTime Fecha { get; set; }

        public string Tipo { get; set; } = string.Empty;

        public string Documento { get; set; } = string.Empty;

        public string Concepto { get; set; } = string.Empty;

        public decimal Entrada { get; set; }

        public decimal Salida { get; set; }

        public decimal SaldoAcumulado { get; set; }
    }

    public sealed class MisioneroPrestamoDto
    {
        public string NoPrestamo { get; set; } = string.Empty;

        public string Persona { get; set; } = string.Empty;

        public string Concepto { get; set; } = string.Empty;

        public DateTime Fecha { get; set; }

        public decimal Monto { get; set; }

        public decimal Saldo { get; set; }
    }

    public sealed class MisioneroFondoConceptoDto
    {
        public int IdRetencion { get; set; }

        public string Concepto { get; set; } = string.Empty;

        public bool EsRetencion { get; set; }

        public decimal Ingresos { get; set; }

        public decimal Entregas { get; set; }

        public decimal Saldo { get; set; }
    }
}
