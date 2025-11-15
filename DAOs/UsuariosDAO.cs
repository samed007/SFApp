using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using SFApp.Models;

namespace SFApp.DAOs
{
    public interface IUsuariosDAO
    {
        Task<Usuario?> Consultar(int id);
        Task<IEnumerable<Usuario>> ListarTodos();
        Task Agregar(Usuario usuario);
        Task Actualizar(Usuario usuario);
        Task Eliminar(int id);
        Task<Usuario?> ObtenerPorCodigo(string codigo); // Para login
    }

    public class UsuariosDAO : IUsuariosDAO
    {
        private readonly IConfiguration _configuration;

        public UsuariosDAO(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Usuario?> Consultar(int id)
        {
            using IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            string sql = "SELECT * FROM Usuarios WHERE IdUsuario = @IdUsuario";
            return await db.QueryFirstOrDefaultAsync<Usuario>(sql, new { IdUsuario = id });
        }

        public async Task<IEnumerable<Usuario>> ListarTodos()
        {
            using IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            string sql = "SELECT * FROM Usuarios WHERE Estado='A'";
            return await db.QueryAsync<Usuario>(sql);
        }

        public async Task Agregar(Usuario usuario)
        {
            using IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            string sql = @"
                INSERT INTO Usuarios (Nombre, Codigo, Contrasena, Rol, Estado, FechaCreacion, FechaMod)
                VALUES (@Nombre, @Codigo, @Contrasena, @Rol, @Estado, GETDATE(), GETDATE())";
            await db.ExecuteAsync(sql, usuario);
        }

        public async Task Actualizar(Usuario usuario)
        {
            using IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            string sql = @"
                UPDATE Usuarios SET
                    Nombre = @Nombre,
                    Codigo = @Codigo,
                    Contrasena = @Contrasena,
                    Rol = @Rol,
                    Estado = @Estado,
                    FechaMod = GETDATE()
                WHERE IdUsuario = @IdUsuario";
            await db.ExecuteAsync(sql, usuario);
        }

        public async Task Eliminar(int id)
        {
            using IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            string sql = "UPDATE Usuarios SET Estado='I', FechaMod=GETDATE() WHERE IdUsuario = @IdUsuario";
            await db.ExecuteAsync(sql, new { IdUsuario = id });
        }

        public async Task<Usuario?> ObtenerPorCodigo(string codigo)
        {
            using IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            string sql = "SELECT * FROM Usuarios WHERE Codigo = @Codigo AND Estado='A'";
            return await db.QueryFirstOrDefaultAsync<Usuario>(sql, new { Codigo = codigo });
        }
    }
}
