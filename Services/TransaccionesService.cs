using AutoMapper;
using SFApp.DAOs;
using SFApp.Models;
using SFApp.DTOs;

namespace SFApp.Services
{
    public interface ITransaccionesService
    {
        Task<TransaccionesDTO?> Consultar(int id);
        Task<IEnumerable<TransaccionesDTO>> ListarTodos();
        Task Agregar(TransaccionesDTO transaccionDto);
        Task Actualizar(TransaccionesDTO transaccionDto);
        Task Eliminar(int id);
        Task<TransaccionesDTO?> ConsultarPorTransaccionId(string transaccionesId);
        Task EjecutarTransaccion(string idTransaccion, string tipo, string? albaran, List<InventarioDTO> productos, string accion = "APPLY");

    }

    public class TransaccionesService : ITransaccionesService
    {
        private readonly ITransaccionesDAO _transaccionesDAO;
        private readonly IMapper _mapper;

        public TransaccionesService(ITransaccionesDAO transaccionesDAO, IMapper mapper)
        {
            _transaccionesDAO = transaccionesDAO;
            _mapper = mapper;
        }

        public async Task<TransaccionesDTO?> Consultar(int id)
        {
            var transaccion = await _transaccionesDAO.Consultar(id);
            return _mapper.Map<TransaccionesDTO?>(transaccion);
        }

        public async Task<IEnumerable<TransaccionesDTO>> ListarTodos()
        {
            var transacciones = await _transaccionesDAO.ListarTodos();
            return _mapper.Map<IEnumerable<TransaccionesDTO>>(transacciones);
        }

      

        public async Task Agregar(TransaccionesDTO transaccionDto)
        {
            var transaccion = _mapper.Map<Transacciones>(transaccionDto);
            await _transaccionesDAO.Agregar(transaccion);
        }

        public async Task Actualizar(TransaccionesDTO transaccionDto)
        {
            var transaccion = _mapper.Map<Transacciones>(transaccionDto);
            await _transaccionesDAO.Actualizar(transaccion);
        }

        public async Task Eliminar(int id)
        {
            await _transaccionesDAO.Eliminar(id);
        }

        public async Task<TransaccionesDTO?> ConsultarPorTransaccionId(string transaccionId)
        {
            var transaccion = await _transaccionesDAO.ConsultarPorTransaccionId(transaccionId);
            return _mapper.Map<TransaccionesDTO?>(transaccion);
        }
       public async Task EjecutarTransaccion(string idTransaccion, string tipo, string? albaran, List<InventarioDTO> productos, string accion = "APPLY")
        {
            // Llamar al nuevo método del DAO que ejecuta el SP
            await _transaccionesDAO.EjecutarTransaccionAsync(idTransaccion, tipo, albaran, productos, accion);
        }

    }
}
