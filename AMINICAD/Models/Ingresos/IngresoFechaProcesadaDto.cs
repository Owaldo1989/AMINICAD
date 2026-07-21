namespace AMINICAD.Models.Ingresos
{
    public class IngresoFechaProcesadaDto
    {
        public DateTime FechaContable { get; set; }

        public int DiasDiferencia { get; set; }

        public int CantidadIngresos { get; set; }

        public int CantidadRecibosFisicos { get; set; }

        public decimal TotalRegistrado { get; set; }

        public decimal TotalCajaGeneral { get; set; }

        public decimal TotalOtrasCuentas { get; set; }

        public DateTime? PrimeraGrabacion { get; set; }

        public DateTime? UltimaGrabacion { get; set; }

        public string Estado
        {
            get
            {
                if (DiasDiferencia < 0)
                    return "Fecha futura";

                if (DiasDiferencia == 0)
                    return "Mismo día";

                if (DiasDiferencia == 1)
                    return "Un día de diferencia";

                if (DiasDiferencia <= 3)
                    return "Trabajo anterior";

                return "Requiere revisión";
            }
        }
    }
}
