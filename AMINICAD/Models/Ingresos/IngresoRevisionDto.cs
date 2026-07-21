namespace AMINICAD.Models.Ingresos
{
    public class IngresoRevisionDto
    {
        public string NoIngreso { get; set; } = string.Empty;

        public string? NoRecibo { get; set; }

        public DateTime FechaContable { get; set; }

        public DateTime FechaHoraGrabacion { get; set; }

        public int DiasDiferencia { get; set; }

        public string? CodCuenta { get; set; }

        public string Cuenta { get; set; } = string.Empty;

        public decimal Total { get; set; }

        public string MotivoRevision { get; set; } = string.Empty;
    }
}
