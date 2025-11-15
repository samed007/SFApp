using AutoMapper;
using SFApp.DAOs;
using SFApp.DTOs;
using SFApp.Models;
using System.Security.Cryptography;
using System.Text;

namespace SFApp.Services
{
    public interface IUsuariosService
    {
        Task<UsuarioDTO?> Consultar(int id);
        Task<IEnumerable<UsuarioDTO>> ListarTodos();
        Task Agregar(UsuarioDTO usuarioDto);
        Task Actualizar(UsuarioDTO usuarioDto);
        Task Eliminar(int id);
        Task<UsuarioDTO?> Login(string codigo, string contrasena);
    }

    public class UsuariosService : IUsuariosService
    {
        private readonly IUsuariosDAO _usuariosDAO;
        private readonly IMapper _mapper;

        public UsuariosService(IUsuariosDAO usuariosDAO, IMapper mapper)
        {
            _usuariosDAO = usuariosDAO;
            _mapper = mapper;
        }

        public async Task<UsuarioDTO?> Consultar(int id)
        {
            var usuario = await _usuariosDAO.Consultar(id);
            return _mapper.Map<UsuarioDTO?>(usuario);
        }

        public async Task<IEnumerable<UsuarioDTO>> ListarTodos()
        {
            var usuarios = await _usuariosDAO.ListarTodos();
            return _mapper.Map<IEnumerable<UsuarioDTO>>(usuarios);
        }

        public async Task Agregar(UsuarioDTO usuarioDto)
        {
            usuarioDto.Contrasena = HashPassword(usuarioDto.Contrasena);
            var usuario = _mapper.Map<Usuario>(usuarioDto);
            await _usuariosDAO.Agregar(usuario);
        }

        public async Task Actualizar(UsuarioDTO usuarioDto)
        {
            if (!string.IsNullOrEmpty(usuarioDto.Contrasena))
            {
                usuarioDto.Contrasena = HashPassword(usuarioDto.Contrasena);
            }
            var usuario = _mapper.Map<Usuario>(usuarioDto);
            await _usuariosDAO.Actualizar(usuario);
        }

        public async Task Eliminar(int id)
        {
            await _usuariosDAO.Eliminar(id);
        }
    public async Task<UsuarioDTO?> Login(string codigo, string contrasena)
    {
        var usuario = await _usuariosDAO.ObtenerPorCodigo(codigo);
        if (usuario == null)
            return null;

        string hashIngresado = HashPassword(contrasena);
        string hashDB = usuario.Contrasena;
        
        if (hashDB != hashIngresado)
            return null;

        return _mapper.Map<UsuarioDTO>(usuario);
    }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hashBytes = sha.ComputeHash(bytes);

        
            var sb = new StringBuilder();
            foreach (var b in hashBytes)
                sb.Append(b.ToString("x2")); 

            return sb.ToString();
        }

    }
}
