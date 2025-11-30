using System;
using System.ComponentModel.DataAnnotations;

namespace SFApp.DTOs
{
    public class ProductosDTO
    {
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        public string Producto { get; set; }

        [Required(ErrorMessage = "La marca es obligatoria.")]
        public string Marca { get; set; }

        public string? Descripcion { get; set; }
        public string? EAN { get; set; }
        public string? SKU { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal? IVA { get; set; }

        [Required(ErrorMessage = "El precio total es obligatorio.")]
        public decimal PrecioTotal { get; set; }

        public string? Categoria { get; set; }
        public string? Talla { get; set; }
        public string? Color { get; set; }

        public int Stock { get; set; }

        
        public string? Estado { get; set; }
        public DateTime? UltimaFechaVenta { get; set; }

        
        public int TotalVentas { get; set; }

        public int? VentasDelDia { get; set; }

        
        public int? VentasDesdeFecha { get; set; }

        
        public int TotalDevoluciones { get; set; }
       public List<VentasPorDiaDTO> VentasPorDia { get; set; } = new List<VentasPorDiaDTO>();


    }
    
        public class VentasPorDiaDTO
        {
            public DateTime FechaVenta { get; set; }
            public int Ventas { get; set; }
        }
}
