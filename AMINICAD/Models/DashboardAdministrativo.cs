namespace AMINICAD.Models
{
    public sealed class DashboardAdministrativo
    {
        public DashboardResumen Resumen { get; set; } = new();

        public List<DashboardMensual> Mensual { get; set; } = new();

        public List<DashboardDistribucion> Distribucion { get; set; } = new();

        public List<DashboardRegion> Regiones { get; set; } = new();

        public List<DashboardDistrito> Distritos { get; set; } = new();

        public List<DashboardMisionero> Misioneros { get; set; } = new();

        public List<DashboardIglesia> Iglesias { get; set; } = new();

        public DashboardCalidadDatos Calidad { get; set; } = new();

        public List<DashboardDiario> Diario { get; set; } = new();

        public List<DashboardIngresoSalidaMensual> IngresosVsSalidas { get; set; }
            = new();

        public List<DashboardDistribucionSalida> DistribucionSalidas { get; set; }
            = new();

        public List<DashboardBeneficiarioFondos> PrincipalesBeneficiarios { get; set; }
            = new();
    }

    public sealed class DashboardIngresoSalidaMensual
    {
        public DateTime Periodo { get; set; }

        public int Anio { get; set; }

        public int IdMes { get; set; }

        public string Mes { get; set; } = string.Empty;

        public decimal Ingresos { get; set; }

        public decimal Salidas { get; set; }

        public decimal Diferencia { get; set; }

        public decimal PorcentajeEntregado { get; set; }

        public int CantidadPlanillas { get; set; }

        public int BeneficiariosPagados { get; set; }
    }

    public sealed class DashboardDistribucionSalida
    {
        public int? IdTipoMision { get; set; }

        public string TipoMision { get; set; } = string.Empty;

        public decimal MontoEntregado { get; set; }

        public decimal PorcentajeTotal { get; set; }

        public int CantidadPlanillas { get; set; }

        public int CantidadBeneficiarios { get; set; }
    }

    public sealed class DashboardBeneficiarioFondos
    {
        public int? IdMisionero { get; set; }

        public string Misionero { get; set; } = string.Empty;

        public int? IdTipoMision { get; set; }

        public string TipoMision { get; set; } = string.Empty;

        public decimal MontoEntregado { get; set; }

        public int CantidadPlanillas { get; set; }

        public DateTime? UltimaEntrega { get; set; }
    }

    public sealed class DashboardDiario
    {
        public DateTime Fecha { get; set; }

        public int Dia { get; set; }

        public decimal TotalBruto { get; set; }

        public decimal NetoBeneficiarios { get; set; }

        public decimal Retenciones { get; set; }

        public int CantidadRecibos { get; set; }
    }

    public sealed class DashboardResumen
    {
        public DateTime FechaInicial { get; set; }

        public DateTime FechaFinal { get; set; }

        public decimal TotalBruto { get; set; }

        public decimal NetoBeneficiarios { get; set; }

        public decimal TotalRetenciones { get; set; }

        public decimal OtrosConceptos { get; set; }

        public decimal TotalDistribuido { get; set; }

        public decimal DiferenciaDistribucion { get; set; }

        public int CantidadIngresos { get; set; }

        public int CantidadRecibos { get; set; }

        public int CantidadIglesias { get; set; }

        public int CantidadMisioneros { get; set; }

        public int CantidadRegiones { get; set; }

        public int CantidadDistritos { get; set; }

        public decimal PromedioAporte { get; set; }

        public decimal MedianaAporte { get; set; }

        public decimal PorcentajeNeto { get; set; }

        public decimal PorcentajeRetenciones { get; set; }
    }


    public sealed class DashboardMensual
    {
        public DateTime Periodo { get; set; }

        public int Anio { get; set; }

        public int IdMes { get; set; }

        public string Mes { get; set; } = string.Empty;

        public decimal TotalBruto { get; set; }

        public decimal NetoBeneficiarios { get; set; }

        public decimal Retenciones { get; set; }

        public decimal OtrosConceptos { get; set; }

        public int CantidadRecibos { get; set; }

        public int Iglesias { get; set; }

        public int Misioneros { get; set; }
    }


    public sealed class DashboardDistribucion
    {
        public int? IdRetencion { get; set; }

        public string Concepto { get; set; } = string.Empty;

        public string TipoDistribucion { get; set; } = string.Empty;

        public decimal Monto { get; set; }

        public int CantidadRecibos { get; set; }

        public decimal PorcentajeTotal { get; set; }
    }


    public sealed class DashboardRegion
    {
        public int? IdRegion { get; set; }

        public string Region { get; set; } = string.Empty;

        public decimal TotalRecaudado { get; set; }

        public int CantidadRecibos { get; set; }

        public int Iglesias { get; set; }

        public int Misioneros { get; set; }

        public decimal PromedioAporte { get; set; }

        public DateTime? UltimoAporte { get; set; }
    }


    public sealed class DashboardDistrito
    {
        public int? IdRegion { get; set; }

        public string Region { get; set; } = string.Empty;

        public int? IdDistrito { get; set; }

        public string Distrito { get; set; } = string.Empty;

        public decimal TotalRecaudado { get; set; }

        public int CantidadRecibos { get; set; }

        public int Iglesias { get; set; }

        public int Misioneros { get; set; }

        public decimal PromedioAporte { get; set; }

        public DateTime? UltimoAporte { get; set; }
    }


    public sealed class DashboardMisionero
    {
        public int? IdMisionero { get; set; }

        public string Misionero { get; set; } = string.Empty;

        public decimal TotalBruto { get; set; }

        public decimal Neto { get; set; }

        public decimal Retenciones { get; set; }

        public decimal Otros { get; set; }

        public int CantidadRecibos { get; set; }

        public int Iglesias { get; set; }

        public DateTime? UltimoAporte { get; set; }
    }


    public sealed class DashboardIglesia
    {
        public int? IdIglesia { get; set; }

        public string Iglesia { get; set; } = string.Empty;

        public int? IdRegion { get; set; }

        public string Region { get; set; } = string.Empty;

        public int? IdDistrito { get; set; }

        public string Distrito { get; set; } = string.Empty;

        public decimal TotalAportado { get; set; }

        public int CantidadRecibos { get; set; }

        public int MisionerosApoyados { get; set; }

        public decimal PromedioAporte { get; set; }

        public DateTime? PrimeraAportacion { get; set; }

        public DateTime? UltimaAportacion { get; set; }
    }


    public sealed class DashboardCalidadDatos
    {
        public int RecibosSinIglesia { get; set; }

        public int RecibosSinMisionero { get; set; }

        public int RecibosSinRegion { get; set; }

        public int RecibosSinDistrito { get; set; }

        public int FilasSinConcepto { get; set; }

        public int ConceptosNoClasificados { get; set; }

        public int RecibosDescuadrados { get; set; }

        public decimal DiferenciaTotal { get; set; }
    }
}
