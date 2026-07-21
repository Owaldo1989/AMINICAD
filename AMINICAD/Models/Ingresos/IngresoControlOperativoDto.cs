namespace AMINICAD.Models.Ingresos
{
    public class IngresoControlOperativoDto
    {
        public IngresoResumenOperativoDto Resumen { get; set; }
           = new();

        public List<IngresoActividadDiariaDto> ActividadDiaria { get; set; }
            = new();

        public List<IngresoFechaProcesadaDto> FechasProcesadas { get; set; }
            = new();

        public List<IngresoCuentaDto> Cuentas { get; set; }
            = new();

        public IngresoAlertasDto Alertas { get; set; }
            = new();

        public List<IngresoRevisionDto> RegistrosRevision { get; set; }
            = new();
    }
}
