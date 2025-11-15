using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFApp.DTOs;
using SFApp.Services;

namespace SFApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductoController : Controller
    {
        private readonly IProductosService _productosService;

        public ProductoController(IProductosService productosService)
        {
            _productosService = productosService;
        }

        
        public async Task<IActionResult> Index()
        {
            var productos = await _productosService.ListarTodos();
            return View(productos);
        }


        
        [HttpPost]
        public async Task<IActionResult> Eliminar(int idProducto)
        {
            Console.WriteLine($"[DEBUG] POST Eliminar -> idProducto: {idProducto}");
            try
            {
                await _productosService.Eliminar(idProducto);
                Console.WriteLine("[DEBUG] Producto marcado como eliminado correctamente.");
                TempData["SuccessMessage"] = $"Producto {idProducto} eliminado correctamente.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error al eliminar el producto: {ex.Message}");
                TempData["ErrorMessage"] = $"Error al eliminar el producto: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
        
        
public async Task<IActionResult> Editar(int idProducto)
{
    var producto = await _productosService.Consultar(idProducto);
    if (producto == null)
    {
        TempData["ErrorMessage"] = "Producto no encontrado.";
        return RedirectToAction("Index");
    }
    return View(producto);
}


[HttpPost]
public async Task<IActionResult> Editar(ProductosDTO producto)
{
    if (!ModelState.IsValid)
    {
        foreach (var key in ModelState.Keys)
        {
            var errors = ModelState[key].Errors;
            if (errors.Count > 0)
            {
                Console.WriteLine($"{key}: {string.Join(", ", errors.Select(e => e.ErrorMessage))}");
            }
        }
        return View(producto);
    }

    await _productosService.Actualizar(producto);
    TempData["SuccessMessage"] = "Producto actualizado correctamente.";
    return RedirectToAction("Index");
}

public IActionResult Agregar()
{
    return View();
}


[HttpPost]
public async Task<IActionResult> Agregar(ProductosDTO producto)
{
    if (!ModelState.IsValid)
    {
        return View(producto);
    }

    try
    {
        await _productosService.Agregar(producto);
        TempData["SuccessMessage"] = "Producto agregado correctamente.";
        return RedirectToAction("Index");
    }
    catch (Exception ex)
    {
        TempData["ErrorMessage"] = $"Error al agregar el producto: {ex.Message}";
        return View(producto);
    }
}


    }
}
