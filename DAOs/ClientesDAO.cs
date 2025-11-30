using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using SFApp.DTOs;
using SFApp.Models;

namespace SFApp.DAOs
{

    public interface IClientesDAO
    {
        Task<Clientes?> Consultar(int id);
        Task<IEnumerable<Clientes>> ListarTodos();
        Task<IEnumerable<Clientes>> ListarPorNombre(string nombre);
        Task Agregar(Clientes cliente);
        Task Actualizar(Clientes cliente);
        Task Eliminar(int id);
    }
    public class ClientesDAO : IClientesDAO
    {
        private readonly IConfiguration _configuration;

        public ClientesDAO(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Clientes?> Consultar(int id)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sqlQuery = "SELECT * FROM Clientes WHERE IdCliente = @IdCliente";
                return await db.QueryFirstOrDefaultAsync<Clientes>(sqlQuery, new { IdCliente = id });
            }
        }

        public async Task<IEnumerable<Clientes>> ListarTodos()
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sqlQuery = "SELECT * FROM Clientes";
                return await db.QueryAsync<Clientes>(sqlQuery);
            }
        }

        public async Task<IEnumerable<Clientes>> ListarPorNombre(string nombre)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sqlQuery = "SELECT * FROM Clientes WHERE Nombre LIKE @Nombre AND Estado='A'";
                return await db.QueryAsync<Clientes>(sqlQuery, new { Nombre = $"%{nombre}%" });
            }
        }

        public async Task Agregar(Clientes cliente)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sqlQuery = @"
                    INSERT INTO Clientes 
                    (Nombre, Apellido, Email, Telefono, Direccion, Ciudad, CodigoPostal, FechaRegistro, Estado)
                    VALUES 
                    (@Nombre, @Apellido, @Email, @Telefono, @Direccion, @Ciudad, @CodigoPostal, @FechaRegistro, @Estado)";
                
                await db.ExecuteAsync(sqlQuery, cliente);
            }
        }

        public async Task Actualizar(Clientes cliente)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sqlQuery = @"
                    UPDATE Clientes SET
                        Nombre = @Nombre,
                        Apellido = @Apellido,
                        Email = @Email,
                        Telefono = @Telefono,
                        Direccion = @Direccion,
                        Ciudad = @Ciudad,
                        CodigoPostal = @CodigoPostal,
                        FechaRegistro = @FechaRegistro,
                        Estado = @Estado
                    WHERE IdCliente = @IdCliente";

                await db.ExecuteAsync(sqlQuery, cliente);
            }
        }

        public async Task Eliminar(int id)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sqlQuery = "UPDATE Clientes SET Estado='I' WHERE IdCliente = @IdCliente";
                await db.ExecuteAsync(sqlQuery, new { IdCliente = id });
            }
        }
    }
}
