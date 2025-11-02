using Microsoft.AspNetCore.Mvc;
using SFApp.DTOs;
using SFApp.Services;
using SFApp.ViewModels;



public class DevolucionController : Controller
{
    private readonly ITransaccionesService _transaccionesService;
    private readonly IInventarioService _inventarioService;

    public DevolucionController(ITransaccionesService transaccionesService, IInventarioService inventarioService)
    {
        _transaccionesService = transaccionesService;
        _inventarioService = inventarioService;

    }

    public async Task<IActionResult> Index(string? idTransaccion, DateTime? fecha, int pageNumber = 1, int pageSize = 10)
    {
        var todas = await _transaccionesService.ListarTodos();

        // Filtrar por fecha (si se selecciona)
        if (fecha.HasValue)
            todas = todas.Where(t => t.Fecha.Date == fecha.Value.Date).ToList();

        // Filtrar por ID de transacción (si se ingresa)
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

        // Ejemplo: podrías consultar los datos de esa transacción
        var inventarios = await _inventarioService.ConsultarPorTransaccionId(idTransaccion);

        if (inventarios == null || !inventarios.Any())
            return NotFound($"No se encontraron registros de inventario para la transacción {idTransaccion}.");

        return View(inventarios);
    }
        [HttpPost]
        public async Task<IActionResult> GuardarCambios(string IdTransaccion, List<InventarioDTO> Inventarios, string accion = "APPLY")
        {
            if (Inventarios == null || Inventarios.Count == 0)
                return BadRequest("No hay datos para guardar.");

            // Determinar el tipo desde el primer producto (puedes mejorarlo)
            string tipo = Inventarios.FirstOrDefault()?.Tipo ?? "EN";
            string? albaran = Inventarios.FirstOrDefault()?.Albaran;

            await _transaccionesService.EjecutarTransaccion(IdTransaccion, tipo, albaran, Inventarios, accion);

            TempData["Mensaje"] = accion == "CANCEL"
                ? "Transacción cancelada correctamente."
                : "Cambios guardados correctamente.";

            return RedirectToAction("Editar", new { idTransaccion = IdTransaccion });
        }







}
