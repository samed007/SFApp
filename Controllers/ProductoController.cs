using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        // Recopilar todos los errores de validación y pasarlos a TempData para el modal
        var errors = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        if (errors.Any())
        {
            TempData["ErrorMessage"] = string.Join("\n", errors);
        }

        // Volver a mostrar la vista con el producto y la lista de estados
        ViewBag.Estados = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem { Value = "A", Text = "Activo" },
                new SelectListItem { Value = "I", Text = "Inactivo" }
            },
            "Value",
            "Text",
            producto.Estado
        );

        return View(producto);
    }

    try
    {
        await _productosService.Actualizar(producto);
        TempData["SuccessMessage"] = "Producto actualizado correctamente.";
    }
    catch (Exception ex)
    {
        // Captura errores de base de datos u otros
        TempData["ErrorMessage"] = $"Error al actualizar el producto: {ex.Message}";
        
        // Volver a mostrar la vista con el producto y la lista de estados
       ViewBag.Estados = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem { Value = "A", Text = "Activo" },
                new SelectListItem { Value = "I", Text = "Inactivo" }
            },
            "Value",
            "Text",
            producto.Estado
        );


        return View(producto);
    }

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
