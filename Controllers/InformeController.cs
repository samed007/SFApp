using Microsoft.AspNetCore.Mvc;
using SFApp.Services;
using SFApp.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace SFApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class InformeController : Controller
    {
        private readonly IProductosService _productosService;

        public InformeController(IProductosService productosService)
        {
            _productosService = productosService;
        }

      
       public async Task<IActionResult> Index()
        {
            var productos = await _productosService.ListarTodos();
            ViewBag.Productos = productos;
            return View(new List<ProductosDTO>());
        }
        
    [HttpPost]
    public async Task<IActionResult> ObtenerStock(int idProducto)
    {
        var producto = await _productosService.ObtenerStockPorProducto(idProducto, null);

        var productos = await _productosService.ListarTodos();
        ViewBag.Productos = productos;

        if (producto == null)
        {
            ViewBag.Mensaje = "No se encontró el producto.";
            return View("Index", new List<ProductosDTO>());
        }

        return View("Index", new List<ProductosDTO> { producto });
    }

  
      [HttpPost]
        public async Task<IActionResult> ObtenerStockPorFecha(int idProducto, DateTime fechaDesde)
        {
            
            var producto = await _productosService.ObtenerStockPorProducto(idProducto, fechaDesde);

            
            var productos = await _productosService.ListarTodos();
            ViewBag.Productos = productos;

            
            ViewBag.FechaDesde = fechaDesde;

            
            if (producto == null)
            {
                ViewBag.Mensaje = "No se encontró el producto.";
                return View("Index", new List<ProductosDTO>());
            }

            return View("Index", new List<ProductosDTO> { producto });
        }

    }
}
