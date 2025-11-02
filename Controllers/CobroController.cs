using Microsoft.AspNetCore.Mvc;
using SFApp.Services;
using SFApp.DTOs;
using SFApp.ViewModels;

public class CobroController : Controller
{
    private readonly IProductosService _productosService;
    private readonly ITransaccionesService _transaccionesService;
    private readonly IInventarioService _inventarioService;

    public CobroController(IProductosService productosService, ITransaccionesService transaccionesService, IInventarioService inventarioService)
    {
        _productosService = productosService;
        _transaccionesService = transaccionesService;
        _inventarioService = inventarioService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var productos = await _productosService.ListarTodos();

        var vm = new TransaccionesViewModel
        {
            Productos = productos,
            Cantidad = 1
        };

        return View(vm);
    }
[HttpPost]
public async Task<IActionResult> AgregarProducto(TransaccionesViewModel vm)
{
    var productos = await _productosService.ListarTodos();
    vm.Productos = productos;

    if (!int.TryParse(vm.ProductoSeleccionado, out int idProducto))
    {
        ModelState.AddModelError("", "ID de producto inválido.");
        return View("Index", vm);
    }

    var producto = productos.FirstOrDefault(p => p.IdProducto == idProducto);
    if (producto == null)
    {
        ModelState.AddModelError("", "Producto no encontrado.");
        return View("Index", vm);
    }

    // Buscar si ya existe
    var existente = vm.ProductosSeleccionados.FirstOrDefault(p => p.Producto.IdProducto == idProducto);

    if (existente != null)
    {
        // Sumar o restar cantidad
        existente.Cantidad += vm.Cantidad;

        // Si la cantidad queda <= 0, eliminar
        if (existente.Cantidad <= 0)
            vm.ProductosSeleccionados.Remove(existente);
    }
    else
    {
        // Solo agregar si cantidad > 0
        if (vm.Cantidad > 0)
        {
            vm.ProductosSeleccionados.Add(new ProductoSeleccionadoVM
            {
                Producto = producto,
                Cantidad = vm.Cantidad
            });
        }
    }

    // Limpiar inputs
    vm.ProductoSeleccionado = "";
    vm.Cantidad = 1;

    return View("Index", vm);
}
[HttpPost]
public async Task<IActionResult> ConfirmarTransaccion(TransaccionesViewModel vm)
{
    if (!vm.ProductosSeleccionados.Any())
    {
        ModelState.AddModelError("", "No hay productos seleccionados.");
        return View("Index", vm);
    }

    try
    {
        // Mapear los productos seleccionados a InventarioDTO
        var productosDto = vm.ProductosSeleccionados
            .Select(p => new InventarioDTO
            {
                IdProducto = p.Producto.IdProducto,
                Cantidad = p.Cantidad
                // Fecha, Tipo, IdTransaccion y Albaran no se necesitan; el SP los maneja
            })
            .ToList();

        // Llamar al service que ejecuta el SP
        await _inventarioService.RegistrarTransaccion(
            importeTotal: vm.PrecioTotal,
            tipo: "VE",       // Venta
            albaran: null,    // No hay albarán
            productosDto: productosDto
        );

        TempData["SuccessMessage"] = "✅ Transacción registrada y stock actualizado.";
    }
    catch (Exception ex)
    {
        ModelState.AddModelError("", $"Error al registrar la transacción: {ex.Message}");
        return View("Index", vm);
    }

    return RedirectToAction("Index");
}

}