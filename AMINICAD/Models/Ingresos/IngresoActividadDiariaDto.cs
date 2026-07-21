namespace AMINICAD.Models.Ingresos
{
    public class IngresoActividadDiariaDto
    {
        public DateTime FechaGrabacion { get; set; }

        public int CantidadIngresos { get; set; }

        public int CantidadRecibosFisicos { get; set; }

        public int CantidadFechasContables { get; set; }

        public decimal TotalRegistrado { get; set; }

        public decimal TotalCajaGeneral { get; set; }

        public decimal TotalOtrasCuentas { get; set; }

        public int IngresosFechasAnteriores { get; set; }

        public int CantidadAlertas { get; set; }
    }
}
