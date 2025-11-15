using Microsoft.AspNetCore.Mvc;
using SFApp.Services;
using SFApp.DTOs;
using SFApp.ViewModels;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class EntradaController : Controller
{
    private readonly IProductosService _productosService;
    private readonly ITransaccionesService _transaccionesService;
    private readonly IInventarioService _inventarioService;

    public EntradaController(IProductosService productosService, ITransaccionesService transaccionesService, IInventarioService inventarioService)
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

    
    var existente = vm.ProductosSeleccionados.FirstOrDefault(p => p.Producto.IdProducto == idProducto);

    if (existente != null)
    {
        
        existente.Cantidad += vm.Cantidad;

        
        if (existente.Cantidad <= 0)
            vm.ProductosSeleccionados.Remove(existente);
    }
    else
    {
        
        if (vm.Cantidad > 0)
        {
            vm.ProductosSeleccionados.Add(new ProductoSeleccionadoVM
            {
                Producto = producto,
                Cantidad = vm.Cantidad
            });
        }
    }

    
    vm.ProductoSeleccionado = "";
    vm.Cantidad = 1;

    return View("Index", vm);
}
[HttpPost]
public async Task<IActionResult> ConfirmarTransaccion(TransaccionesViewModel vm, string? albaran)
{
    if (!vm.ProductosSeleccionados.Any())
    {
        ModelState.AddModelError("", "No hay productos seleccionados.");
        var productos = await _productosService.ListarTodos();
        vm.Productos = productos;
        return View("Index", vm);
    }

    try
    {
        var productosDto = vm.ProductosSeleccionados
            .Select(p => new InventarioDTO
            {
                IdProducto = p.Producto.IdProducto,
                Cantidad = p.Cantidad
            })
            .ToList();

        
    
        var codigoUsuario = User.Claims.FirstOrDefault(c => c.Type == "CodigoUsuario")?.Value ?? "SYSTEM";


        await _inventarioService.RegistrarTransaccion(
            importeTotal: vm.PrecioCompra,
            tipo: "EN",
            albaran: albaran,
            productosDto: productosDto,
            usuario: codigoUsuario 
        );

        TempData["SuccessMessage"] = "✅ Transacción registrada y stock actualizado.";
    }
    catch (Exception ex)
    {
        ModelState.AddModelError("", $"Error al registrar la transacción: {ex.Message}");
        var productos = await _productosService.ListarTodos();
        vm.Productos = productos;
        return View("Index", vm);
    }

    return RedirectToAction("Index");
}


}