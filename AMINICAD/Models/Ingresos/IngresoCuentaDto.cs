namespace AMINICAD.Models.Ingresos
{
    public class IngresoCuentaDto
    {
        public string? CodCuenta { get; set; }

        public string Cuenta { get; set; } = string.Empty;

        public int CantidadIngresos { get; set; }

        public int CantidadRecibosFisicos { get; set; }

        public decimal TotalRegistrado { get; set; }

        public decimal PorcentajeParticipacion { get; set; }

        public DateTime? FechaContableMasAntigua { get; set; }

        public int MayorDiferenciaDias { get; set; }

        public bool EsCajaGeneral { get; set; }
    }
}
