namespace SFApp.DTOs
{
    public class InventarioDTO
    {
        public int IdProducto { get; set; }
        public DateTime Fecha { get; set; }
        public int Cantidad { get; set; }
        public string? Albaran { get; set; }
        public string Tipo { get; set; } = "EN";
        public string IdTransaccion { get; set; }
        
    }
}
