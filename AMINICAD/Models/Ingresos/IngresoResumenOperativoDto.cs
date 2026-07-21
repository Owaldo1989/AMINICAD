namespace AMINICAD.Models.Ingresos
{
    public class IngresoResumenOperativoDto
    {
        public DateTime FechaInicialRegistro { get; set; }

        public DateTime FechaFinalRegistro { get; set; }

        public int CantidadIngresos { get; set; }

        public int CantidadRecibosFisicos { get; set; }

        public decimal TotalRegistrado { get; set; }

        public decimal TotalCajaGeneral { get; set; }

        public decimal TotalOtrasCuentas { get; set; }

        public int IngresosMismoDia { get; set; }

        public int IngresosFechasAnteriores { get; set; }

        public int IngresosFechaFutura { get; set; }

        public DateTime? FechaContableMasAntigua { get; set; }

        public int MayorDiferenciaDias { get; set; }

        public decimal PromedioDiasDiferencia { get; set; }

        public int CantidadCuentasUtilizadas { get; set; }

        public decimal PorcentajeMismoDia
        {
            get
            {
                if (CantidadIngresos == 0)
                    return 0;

                return Math.Round(
                    IngresosMismoDia * 100m / CantidadIngresos,
                    2
                );
            }
        }

        public decimal PorcentajeFechasAnteriores
        {
            get
            {
                if (CantidadIngresos == 0)
                    return 0;

                return Math.Round(
                    IngresosFechasAnteriores * 100m / CantidadIngresos,
                    2
                );
            }
        }

        public bool TieneTrabajoAcumulado =>
            IngresosFechasAnteriores > 0;

        public bool TieneFechasFuturas =>
            IngresosFechaFutura > 0;
    }
}
