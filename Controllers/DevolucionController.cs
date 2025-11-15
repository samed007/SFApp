using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFApp.DTOs;
using SFApp.Services;
using SFApp.ViewModels;

[Authorize]
public class DevolucionController : Controller
{
    private readonly ITransaccionesService _transaccionesService;
    private readonly IInventarioService _inventarioService;
    private readonly IProductosService _productoService;

    public DevolucionController(
        ITransaccionesService transaccionesService,
        IInventarioService inventarioService,
        IProductosService productoService)
    {
        _transaccionesService = transaccionesService;
        _inventarioService = inventarioService;
        _productoService = productoService;
    }

    
    public async Task<IActionResult> Index(string? idTransaccion, DateTime? fecha, int pageNumber = 1, int pageSize = 10)
    {
        var todas = await _transaccionesService.ListarTodos();

        if (fecha.HasValue)
            todas = todas.Where(t => t.Fecha.Date == fecha.Value.Date).ToList();

        if (!string.IsNullOrWhiteSpace(idTransaccion))
            todas = todas.Where(t => t.IdTransaccion.Contains(idTransaccion)).ToList();

        var totalRecords = todas.Count();

        var transaccionesPaginadas = todas
            .OrderByDescending(t => t.Fecha)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        var vm = new DevolucionesViewModel
        {
            Transacciones = transaccionesPaginadas,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            Fecha = fecha,
            IdTransaccionFiltro = idTransaccion
        };

        return View(vm);
    }

    
    [HttpGet]
    public async Task<IActionResult> Editar(string idTransaccion)
    {
        if (string.IsNullOrEmpty(idTransaccion))
            return BadRequest("Debe especificar un ID de transacción.");

        var inventarios = await _inventarioService.ConsultarPorTransaccionId(idTransaccion);

        if (inventarios == null || !inventarios.Any())
            return NotFound($"No se encontraron registros de inventario para la transacción {idTransaccion}.");

        var tipo = inventarios.First().Tipo;
        var albaran = inventarios.First().Albaran;

        
        bool esDevolucion = tipo == "DV" || tipo == "DE";
        bool permiteCancelar = !esDevolucion;
        bool esEditable = !esDevolucion;

        var productos = await _productoService.ListarTodos();
        ViewBag.Productos = productos;
        ViewBag.EsDevolucion = esDevolucion;
        ViewBag.PermiteCancelar = permiteCancelar;
        ViewBag.EsEditable = esEditable;
        ViewBag.Tipo = tipo;
        ViewBag.Albaran = albaran;

        return View(inventarios);
    }

 [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> GuardarCambios(
    string IdTransaccion,
    string Tipo,
    string? Albaran,
    List<InventarioDTO> Inventarios,
    string accion = "APPLY")
{
    if (string.IsNullOrWhiteSpace(IdTransaccion))
        return BadRequest("ID de transacción no válido.");

    var transaccionReal = await _inventarioService.ConsultarPorTransaccionId(IdTransaccion);
    if (transaccionReal == null || !transaccionReal.Any())
        return NotFound("Transacción no encontrada.");

    var tipoReal = transaccionReal.First().Tipo;
    bool esDevolucion = tipoReal == "DV" || tipoReal == "DE";

    accion = accion?.ToUpperInvariant() ?? "APPLY";

   
    if (accion == "REVERTIR")
    {
        if (!esDevolucion)
            return BadRequest("Solo se puede revertir una devolución.");
        accion = "CANCEL"; 
    }

    if (esDevolucion && accion != "CANCEL")
        return BadRequest("No se pueden modificar devoluciones existentes.");
    else if (!esDevolucion && accion != "APPLY" && accion != "CANCEL")
        return BadRequest("Acción no válida para este tipo de transacción.");

    if (Inventarios == null || Inventarios.Count == 0)
        return BadRequest("No hay productos para procesar.");

    var productosValidos = await _productoService.ListarTodos();
    foreach (var item in Inventarios)
    {
        var prod = productosValidos.FirstOrDefault(p => p.IdProducto == item.IdProducto);
        if (prod == null)
            return BadRequest($"Producto con ID {item.IdProducto} no existe.");
        if (item.Cantidad <= 0)
            return BadRequest($"Cantidad inválida para el producto {item.IdProducto}.");

        item.Precio = (Tipo == "EN" || Tipo == "DE") ? prod.PrecioCompra : prod.PrecioTotal;
    }

    Albaran = string.IsNullOrWhiteSpace(Albaran) ? null : Albaran.Trim();
    if (Albaran != null && Albaran.Length > 50)
        return BadRequest("El albarán no puede superar 50 caracteres.");

    
     var codigoUsuario = User.Claims.FirstOrDefault(c => c.Type == "CodigoUsuario")?.Value ?? "SYSTEM";

    
    await _transaccionesService.EjecutarTransaccion(
        IdTransaccion,
        Tipo,
        Albaran,
        Inventarios,
        accion,
        codigoUsuario 
    );

    string mensaje = accion switch
    {
        "CANCEL" when esDevolucion => "↩️ Reversión de devolución ejecutada correctamente.",
        "CANCEL" => "❌ Transacción cancelada correctamente.",
        _ when Tipo == "DV" => "♻️ Devolución generada correctamente.",
        _ when Tipo == "DE" => "📦 Devolución sobre entrada registrada correctamente.",
        _ when Tipo == "EN" => "📥 Entrada actualizada correctamente.",
        _ when Tipo == "VE" => "💰 Venta actualizada correctamente.",
        _ => "✅ Cambios guardados correctamente."
    };

    TempData["Mensaje"] = mensaje;

    if (accion == "CANCEL" && !esDevolucion) 
        return RedirectToAction("Index");
    else
        return RedirectToAction("Editar", new { idTransaccion = IdTransaccion });
}


}
