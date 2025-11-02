using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using SFApp.Models;

namespace SFApp.DAOs
{
    public interface IInventarioDAO
    {
        Task<Inventario?> Consultar(int id);
        Task<IEnumerable<Inventario>> ListarTodos();
        Task<IEnumerable<Inventario>> ListarPorProducto(string idProducto);
        Task Agregar(Inventario inventario);
        Task Actualizar(Inventario inventario);
        Task Eliminar(int id);
        Task<int> ObtenerStock(int idProducto);
        Task RegistrarTransaccion(decimal importeTotal, string tipo, string? albaran, IEnumerable<Inventario> productos);
        Task RegistrarDevolucion(string idTransaccion, string tipo, string? albaran, IEnumerable<Inventario> productos);
        Task<IEnumerable<Inventario>> ConsultarPorTransaccionId(string transaccionId);
        
    }

    public class InventarioDAO : IInventarioDAO
    {
        private readonly IConfiguration _configuration;

        public InventarioDAO(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Inventario?> Consultar(int id)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sqlQuery = "SELECT * FROM Inventario WHERE id = @Id";
                return await db.QueryFirstOrDefaultAsync<Inventario>(sqlQuery, new { Id = id });
            }
        }

        public async Task<IEnumerable<Inventario>> ListarTodos()
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sqlQuery = "SELECT * FROM Inventario";
                return await db.QueryAsync<Inventario>(sqlQuery);
            }
        }

        public async Task<IEnumerable<Inventario>> ListarPorProducto(string idProducto)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sqlQuery = "SELECT * FROM Inventario WHERE IdProducto LIKE @IdProducto";
                return await db.QueryAsync<Inventario>(sqlQuery, new { IdProducto = $"%{idProducto}%" });
            }
        }

        public async Task Agregar(Inventario inventario)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sqlQuery = @"
                    INSERT INTO Inventario (IdProducto, Fecha, Cantidad, Albaran, Tipo, IdTransaccion)
                    VALUES (@IdProducto, @Fecha, @Cantidad, @Albaran, @Tipo, @IdTransaccion)";

                await db.ExecuteAsync(sqlQuery, inventario);
            }
        }

        public async Task Actualizar(Inventario inventario)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sqlQuery = @"
                    UPDATE Inventario SET
                        IdProducto = @IdProducto,
                        Fecha = @Fecha,
                        Cantidad = @Cantidad,
                        Albaran = @Albaran,
                        Tipo = @Tipo,
                        IdTransaccion = @IdTransaccion
                       
                    WHERE Id = @Id";

                await db.ExecuteAsync(sqlQuery, inventario);
            }
        }

        public async Task Eliminar(int id)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sqlQuery = "DELETE FROM Inventario WHERE Id = @Id";
                await db.ExecuteAsync(sqlQuery, new { Id = id });
            }
        }

        // Obtiene el último stock de un producto
        public async Task<int> ObtenerStock(int idProducto)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sqlQuery = @"
                    SELECT TOP 1 Stock 
                    FROM Inventario 
                    WHERE IdProducto = @IdProducto 
                    ORDER BY Fecha DESC, id DESC";

                var stock = await db.QueryFirstOrDefaultAsync<int?>(sqlQuery, new { IdProducto = idProducto });
                return stock ?? 0; // Si no hay registros, devuelve 0
            }
        }

        public async Task RegistrarTransaccion(decimal importeTotal, string tipo, string? albaran, IEnumerable<Inventario> productos)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                // Crear DataTable para enviar como Table-Valued Parameter (TVP)
                var productosTable = new DataTable();
                productosTable.Columns.Add("IdProducto", typeof(int));
                productosTable.Columns.Add("Cantidad", typeof(int));

                foreach (var prod in productos)
                {
                    productosTable.Rows.Add(prod.IdProducto, prod.Cantidad);
                }

                // Llamar al stored procedure
                await db.ExecuteAsync(
                    "sp_RegistrarTransaccionVenta",
                    new
                    {
                        ImporteTotal = importeTotal,
                        Tipo = tipo,
                        Albaran = albaran,
                        Productos = productosTable.AsTableValuedParameter("TipoProductos")
                    },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task RegistrarDevolucion(string idTransaccion, string tipo, string? albaran, IEnumerable<Inventario> productos)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                // Crear DataTable para enviar como Table-Valued Parameter (TVP)
                var productosTable = new DataTable();
                productosTable.Columns.Add("IdProducto", typeof(int));
                productosTable.Columns.Add("Cantidad", typeof(int));

                foreach (var prod in productos)
                {
                    productosTable.Rows.Add(prod.IdProducto, prod.Cantidad);
                }

                // Llamar al stored procedure
                await db.ExecuteAsync(
                    "sp_RegistrarDevolucion",
                    new
                    {
                        IdTransaccion = idTransaccion,
                        Tipo = tipo,                 // 'DE' o 'DV'
                        Albaran = albaran,
                        Productos = productosTable.AsTableValuedParameter("TipoProductos")
                    },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

          public async Task<IEnumerable<Inventario>> ConsultarPorTransaccionId(string transaccionId)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sqlQuery = "SELECT * FROM Inventario WHERE idTransaccion = @transaccionId";
                return await db.QueryAsync<Inventario>(sqlQuery, new { transaccionId });

            }
        }




    }
}
