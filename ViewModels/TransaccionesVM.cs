using System.Collections.Generic;
using System.Linq;
using SFApp.DTOs;

namespace SFApp.ViewModels
{
    public class ProductoSeleccionadoVM
{
    public ProductosDTO Producto { get; set; }
    public int Cantidad { get; set; }

    public decimal Subtotal => Producto.precioTotal * Cantidad;
}

public class TransaccionesViewModel
{
    public IEnumerable<ProductosDTO> Productos { get; set; } = new List<ProductosDTO>();
    public string ProductoSeleccionado { get; set; } = string.Empty;
    public int Cantidad { get; set; } = 1;
    public List<ProductoSeleccionadoVM> ProductosSeleccionados { get; set; } = new List<ProductoSeleccionadoVM>();

    public decimal PrecioTotal => ProductosSeleccionados.Sum(p => p.Subtotal);
}

}
