namespace AMINICAD.Models.Ingresos
{
    public class IngresoAlertasDto
    {
        public int IngresosSinReciboFisico { get; set; }

        public int IngresosSinCuenta { get; set; }

        public int IngresosCuentaNoIdentificada { get; set; }

        public int IngresosFechaPosterior { get; set; }

        public int IngresosConMasTresDias { get; set; }

        public int CajaGeneralConFechaAnterior { get; set; }

        public int TotalAlertas =>
            IngresosSinReciboFisico
            + IngresosSinCuenta
            + IngresosCuentaNoIdentificada
            + IngresosFechaPosterior
            + IngresosConMasTresDias
            + CajaGeneralConFechaAnterior;

        public bool TieneAlertas =>
            TotalAlertas > 0;
    }
}
