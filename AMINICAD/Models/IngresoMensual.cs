namespace AMINICAD.Models
{
    public class IngresoMensual
    {
        public int Anio { get; set; }
        public int IdMes { get; set; }   // 1..12
        public decimal Total { get; set; }
        public string Mes { get; set; } = ""; // lo armamos en C#
    }
}
