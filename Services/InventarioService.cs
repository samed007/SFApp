using AutoMapper;
using SFApp.DAOs;
using SFApp.Models;
using SFApp.DTOs;

namespace SFApp.Services
{
    public interface IInventarioService
    {
        Task<InventarioDTO?> Consultar(int id);
        Task<IEnumerable<InventarioDTO>> ListarTodos();
        Task<IEnumerable<InventarioDTO>> ListarPorProducto(string idProducto);
        Task Agregar(InventarioDTO inventarioDto);
        Task Actualizar(InventarioDTO inventarioDto);
        Task Eliminar(int id);
        Task<int> ObtenerStock(int idProducto);
        Task RegistrarTransaccion(decimal importeTotal, string tipo, string? albaran, IEnumerable<InventarioDTO> productosDto);
        Task RegistrarDevolucion(string idTransaccion, string tipo, string? albaran, IEnumerable<InventarioDTO> productosDto);
         Task<IEnumerable<InventarioDTO>> ConsultarPorTransaccionId(string transaccionesId);

    }

    public class InventarioService : IInventarioService
    {
        private readonly IInventarioDAO _inventarioDAO;
        private readonly IMapper _mapper;

        public InventarioService(IInventarioDAO inventarioDAO, IMapper mapper)
        {
            _inventarioDAO = inventarioDAO;
            _mapper = mapper;
        }

        public async Task<InventarioDTO?> Consultar(int id)
        {
            var inventario = await _inventarioDAO.Consultar(id);
            return _mapper.Map<InventarioDTO?>(inventario);
        }

        public async Task<IEnumerable<InventarioDTO>> ListarTodos()
        {
            var inventarios = await _inventarioDAO.ListarTodos();
            return _mapper.Map<IEnumerable<InventarioDTO>>(inventarios);
        }

        public async Task<IEnumerable<InventarioDTO>> ListarPorProducto(string idProducto)
        {
            var inventarios = await _inventarioDAO.ListarPorProducto(idProducto);
            return _mapper.Map<IEnumerable<InventarioDTO>>(inventarios);
        }

        public async Task Agregar(InventarioDTO inventarioDto)
        {
            var inventario = _mapper.Map<Inventario>(inventarioDto);
            await _inventarioDAO.Agregar(inventario);
        }

        public async Task Actualizar(InventarioDTO inventarioDto)
        {
            var inventario = _mapper.Map<Inventario>(inventarioDto);
            await _inventarioDAO.Actualizar(inventario);
        }

        public async Task Eliminar(int id)
        {
            await _inventarioDAO.Eliminar(id);
        }

        public async Task<int> ObtenerStock(int idProducto)
        {
            return await _inventarioDAO.ObtenerStock(idProducto);
        }

        public async Task RegistrarTransaccion(decimal importeTotal, string tipo, string? albaran, IEnumerable<InventarioDTO> productosDto)
        {
            // Mapear DTOs a la entidad Inventario
            var productos = productosDto.Select(p => _mapper.Map<Inventario>(p));

            // Llamar al método del DAO que ejecuta el SP
            await _inventarioDAO.RegistrarTransaccion(importeTotal, tipo, albaran, productos);
        }

        public async Task RegistrarDevolucion(string idTransaccion, string tipo, string? albaran, IEnumerable<InventarioDTO> productosDto)
        {
            // Mapear DTOs a la entidad Inventario
            var productos = productosDto.Select(p => _mapper.Map<Inventario>(p));

            // Llamar al método del DAO que ejecuta el SP de devoluciones
            await _inventarioDAO.RegistrarDevolucion(idTransaccion, tipo, albaran, productos);
        }

        public async Task<IEnumerable<InventarioDTO>> ConsultarPorTransaccionId(string transaccionesId)
        {
            var inventarios = await _inventarioDAO.ConsultarPorTransaccionId(transaccionesId);
            return _mapper.Map<IEnumerable<InventarioDTO>>(inventarios);
        }



    }
}
