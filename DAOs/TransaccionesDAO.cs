using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using SFApp.DTOs;
using SFApp.Models;

namespace SFApp.DAOs
{
    public interface ITransaccionesDAO
    {
        Task<Transacciones?> Consultar(int id);
        Task<IEnumerable<Transacciones>> ListarTodos();
        Task<IEnumerable<Transacciones>> ListarPorTipo(string tipo);
        Task Agregar(Transacciones transaccion);
        Task Actualizar(Transacciones transaccion);
        Task Eliminar(int id);
        Task<Transacciones?> ConsultarPorTransaccionId(string transaccionId);
        Task EjecutarTransaccionAsync(string idTransaccion, string tipo, string? albaran, List<InventarioDTO> productos, string accion = "APPLY", string usuario = "SYSTEM");

    }

    public class TransaccionesDAO : ITransaccionesDAO
    {
        private readonly IConfiguration _configuration;

        public TransaccionesDAO(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Transacciones?> Consultar(int id)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sqlQuery = "SELECT * FROM Transacciones WHERE Id = @Id";
                return await db.QueryFirstOrDefaultAsync<Transacciones>(sqlQuery, new { Id = id });
            }
        }

        public async Task<IEnumerable<Transacciones>> ListarTodos()
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sqlQuery = "SELECT * FROM Transacciones WHERE Estado='A' ORDER BY Id DESC";

                return await db.QueryAsync<Transacciones>(sqlQuery);
            }
        }

        public async Task<IEnumerable<Transacciones>> ListarPorTipo(string tipo)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sqlQuery = "SELECT * FROM Transacciones WHERE Tipo = @Tipo and Estado='A'";
                return await db.QueryAsync<Transacciones>(sqlQuery, new { Tipo = tipo });
            }
        }

        public async Task Agregar(Transacciones transaccion)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sqlQuery = @"
                    INSERT INTO Transacciones (IdTransaccion, Fecha,ImporteTotal, FechaMod, Tipo, Estado)
                    VALUES (@IdTransaccion, @Fecha, @ImporteTotal, @FechaMod, @Tipo, @Estado)";
                
                await db.ExecuteAsync(sqlQuery, transaccion);
            }
        }

        public async Task Actualizar(Transacciones transaccion)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sqlQuery = @"
                    UPDATE Transacciones SET
                        IdTransaccion = @IdTransaccion,
                        Fecha = @Fecha,
                        ImporteTotal = @ImporteTotal,
                        FechaMod = @FechaMod,
                        Tipo = @Tipo,
                        Estado = @Estado
                    WHERE Id = @Id";
                
                await db.ExecuteAsync(sqlQuery, transaccion);
            }
        }

        public async Task Eliminar(int id)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sqlQuery = "DELETE FROM Transacciones WHERE Id = @Id";
                await db.ExecuteAsync(sqlQuery, new { Id = id });
            }
        }

        public async Task<Transacciones?> ConsultarPorTransaccionId(string transaccionId)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sqlQuery = "SELECT * FROM Transacciones WHERE IdTransaccion = @transaccionId";
                return await db.QueryFirstOrDefaultAsync<Transacciones>(sqlQuery, new { transaccionId });

            }
        }
        
        public async Task EjecutarTransaccionAsync(
    string idTransaccion, 
    string tipo, 
    string? albaran, 
    List<InventarioDTO> productos, 
    string accion = "APPLY",
    string usuario = "SYSTEM")   
{
    using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
    using (var cmd = new SqlCommand("sp_ActualizarTransaccion", conn))
    {
        cmd.CommandType = CommandType.StoredProcedure;

        
        cmd.Parameters.AddWithValue("@IdTransaccion", idTransaccion);
        cmd.Parameters.AddWithValue("@Tipo", tipo);
        cmd.Parameters.AddWithValue("@Albaran", (object?)albaran ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Accion", accion);

        
        cmd.Parameters.AddWithValue("@Usuario", usuario);

        
        var tvp = new DataTable();
        tvp.Columns.Add("IdProducto", typeof(int));
        tvp.Columns.Add("Cantidad", typeof(int));

        foreach (var p in productos)
        {
            tvp.Rows.Add(p.IdProducto, p.Cantidad);
        }

        var param = cmd.Parameters.AddWithValue("@Productos", tvp);
        param.SqlDbType = SqlDbType.Structured;
        param.TypeName = "TipoProductos";

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }
}

    }
}
