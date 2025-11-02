namespace SFApp.Models
{
    public class Productos
    {
        public int IdProducto { get; set; }
        public string producto { get; set; }
        public string marca { get; set; }
        public string descripcion { get; set; }
        public string EAN { get; set; }
        public string SKU { get; set; }
        public decimal precioSIN { get; set; }
        public decimal IVA { get; set; }
        public decimal precioTotal { get; set; }
        public string estado { get; set; } = "A";
    
    }
    
}
