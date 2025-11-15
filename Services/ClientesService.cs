using AutoMapper;
using SFApp.DAOs;
using SFApp.Models;
using SFApp.DTOs;

namespace SFApp.Services


{

    public interface IClientesService
    {
        Task<ClientesDTO?> Consultar(int id);
        Task<IEnumerable<ClientesDTO>> ListarTodos();
        Task<IEnumerable<ClientesDTO>> ListarPorNombre(string nombre);
        Task Agregar(ClientesDTO clienteDto);
        Task Actualizar(ClientesDTO clienteDto);
        Task Eliminar(int id);
    }
    public class ClientesService : IClientesService
    {
        private readonly IClientesDAO _clientesDAO;
        private readonly IMapper _mapper;

        public ClientesService(IClientesDAO clientesDAO, IMapper mapper)
        {
            _clientesDAO = clientesDAO;
            _mapper = mapper;
        }

        public async Task<ClientesDTO?> Consultar(int id)
        {
            var cliente = await _clientesDAO.Consultar(id);
            return _mapper.Map<ClientesDTO?>(cliente);
        }

        public async Task<IEnumerable<ClientesDTO>> ListarTodos()
        {
            var clientes = await _clientesDAO.ListarTodos();
            return _mapper.Map<IEnumerable<ClientesDTO>>(clientes);
        }

        public async Task<IEnumerable<ClientesDTO>> ListarPorNombre(string nombre)
        {
            var clientes = await _clientesDAO.ListarPorNombre(nombre);
            return _mapper.Map<IEnumerable<ClientesDTO>>(clientes);
        }

        public async Task Agregar(ClientesDTO clienteDto)
        {
            var cliente = _mapper.Map<Clientes>(clienteDto);
            await _clientesDAO.Agregar(cliente);
        }

        public async Task Actualizar(ClientesDTO clienteDto)
        {
            var cliente = _mapper.Map<Clientes>(clienteDto);
            await _clientesDAO.Actualizar(cliente);
        }

        public async Task Eliminar(int id)
        {
            await _clientesDAO.Eliminar(id);
        }
    }
}
