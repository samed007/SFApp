using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SFApp.DTOs;
using SFApp.Services;

namespace SFApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ClienteController : Controller
    {
        private readonly IClientesService _clientesService;

        public ClienteController(IClientesService clientesService)
        {
            _clientesService = clientesService;
        }

        
        public async Task<IActionResult> Index()
        {
            var clientes = await _clientesService.ListarTodos();
            return View(clientes);
        }

        
        [HttpPost]
        public async Task<IActionResult> Eliminar(int idCliente)
        {
            Console.WriteLine($"[DEBUG] POST Eliminar -> idCliente: {idCliente}");
            try
            {
                await _clientesService.Eliminar(idCliente);
                Console.WriteLine("[DEBUG] Cliente marcado como eliminado correctamente.");
                TempData["SuccessMessage"] = $"Cliente {idCliente} eliminado correctamente.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error al eliminar el cliente: {ex.Message}");
                TempData["ErrorMessage"] = $"Error al eliminar el cliente: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

       public async Task<IActionResult> Editar(int idCliente)
        {
            var cliente = await _clientesService.Consultar(idCliente);
            if (cliente == null)
            {
                TempData["ErrorMessage"] = "Cliente no encontrado.";
                return RedirectToAction("Index");
            }

            // Lista de estados
            ViewBag.Estados = new SelectList(
                new List<SelectListItem>
                {
                    new SelectListItem { Value = "A", Text = "Activo" },
                    new SelectListItem { Value = "I", Text = "Inactivo" }
                },
                "Value",
                "Text",
                cliente.Estado // Selecciona automáticamente
            );

            return View(cliente);
        }

            [HttpPost]
            public async Task<IActionResult> Editar(ClientesDTO cliente)
            {
                if (!ModelState.IsValid)
                    return View(cliente);

                
                var clienteOriginal = await _clientesService.Consultar(cliente.IdCliente);
                if (clienteOriginal == null)
                {
                    TempData["ErrorMessage"] = "Cliente no encontrado.";
                    return RedirectToAction("Index");
                }

                
                cliente.FechaRegistro = clienteOriginal.FechaRegistro;

                await _clientesService.Actualizar(cliente);
                TempData["SuccessMessage"] = "Cliente actualizado correctamente.";
                return RedirectToAction("Index");
            }

                            
        public IActionResult Agregar()
        {
            // Crear lista de estados con valor por defecto
            ViewBag.Estados = new SelectList(
                new List<SelectListItem>
                {
                    new SelectListItem { Value = "A", Text = "Activo" },
                    new SelectListItem { Value = "I", Text = "Inactivo" }
                },
                "Value",
                "Text",
                "A" // Activo por defecto
            );

            return View(new ClientesDTO());
        }


            
            [HttpPost]
            public async Task<IActionResult> Agregar(ClientesDTO cliente)
            {
                if (!ModelState.IsValid)
                {
                    return View(cliente);
                }

    try
    {
       
        cliente.FechaRegistro = DateTime.Now;

        await _clientesService.Agregar(cliente);
        TempData["SuccessMessage"] = "Cliente agregado correctamente.";
        return RedirectToAction("Index");
    }
    catch (Exception ex)
    {
        TempData["ErrorMessage"] = $"Error al agregar el cliente: {ex.Message}";
        return View(cliente);
    }
}

    }
}
