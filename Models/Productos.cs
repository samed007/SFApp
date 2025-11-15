using SFApp.DTOs;

namespace SFApp.Models
{
    public class Productos
    {
        public int IdProducto { get; set; }
        public string Producto { get; set; }
        public string Marca { get; set; }
        public string Descripcion { get; set; }
        public string EAN { get; set; }
        public string SKU { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal IVA { get; set; }
        public decimal PrecioTotal { get; set; }
        public string Categoria { get; set; }
        public string Talla { get; set; }
        public string Color { get; set; }
        public string Estado { get; set; } = "A";
        public int Stock { get; set; }

        
        public DateTime? UltimaFechaVenta { get; set; }

       
        public int TotalVentas { get; set; }

       
        public int? VentasDelDia { get; set; }

        
        public int? VentasDesdeFecha { get; set; }

        
        public int TotalDevoluciones { get; set; }

        
        public List<VentasPorDiaDTO>? VentasPorDia { get; set; } = new List<VentasPorDiaDTO>();
    }

   
}
