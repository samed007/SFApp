using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using SFApp.DTOs;
using SFApp.Models;

namespace SFApp.DAOs
{
    public interface IProductosDAO
    {
        Task<Productos?> Consultar(int id);
        Task<IEnumerable<Productos>> ListarTodosActivos();
        Task<IEnumerable<Productos>> ListarTodos();
        Task<IEnumerable<Productos>> ListarPorProducto(string producto);
        Task Agregar(Productos producto);
        Task Actualizar(Productos producto);
        Task Eliminar(int id);
        Task<IEnumerable<Productos>> ObtenerInformeStock(int idProducto, DateTime? fechaDesde);
        Task<IEnumerable<VentasPorDiaDTO>> ObtenerVentasPorDia(int idProducto, DateTime fechaDesde);
    }

    public class ProductosDAO : IProductosDAO
    {
        private readonly IConfiguration _configuration;

        public ProductosDAO(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Productos?> Consultar(int id)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sqlQuery = "SELECT * FROM Productos WHERE IdProducto = @IdProducto";
                return await db.QueryFirstOrDefaultAsync<Productos>(sqlQuery, new { IdProducto = id });
            }
        }

        public async Task<IEnumerable<Productos>> ListarTodosActivos()
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sqlQuery = "SELECT * FROM Productos WHERE Estado='A'";
                return await db.QueryAsync<Productos>(sqlQuery);
            }
        }
           public async Task<IEnumerable<Productos>> ListarTodos()
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sqlQuery = "SELECT * FROM Productos";
                return await db.QueryAsync<Productos>(sqlQuery);
            }
        }

        public async Task<IEnumerable<Productos>> ListarPorProducto(string producto)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sqlQuery = "SELECT * FROM Productos WHERE Producto LIKE @Producto and Estado='A'";
                return await db.QueryAsync<Productos>(sqlQuery, new { Producto = $"%{producto}%" });
            }
        }

        public async Task Agregar(Productos producto)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sqlQuery = @"
                    INSERT INTO Productos 
                    (Producto, Marca, Descripcion, EAN, SKU, PrecioCompra, IVA, PrecioTotal, 
                    Categoria, Talla, Color, Estado)
                    VALUES 
                    (@Producto, @Marca, @Descripcion, @EAN, @SKU, @PrecioCompra, @IVA, @PrecioTotal,
                    @Categoria, @Talla, @Color, @Estado)";


                await db.ExecuteAsync(sqlQuery, producto);
            }
        }

        public async Task Actualizar(Productos producto)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sqlQuery = @"
                UPDATE Productos SET
                    Producto = @Producto,
                    Marca = @Marca,
                    Descripcion = @Descripcion,
                    EAN = @EAN,
                    SKU = @SKU,
                    PrecioCompra = @PrecioCompra,
                    IVA = @IVA,
                    PrecioTotal = @PrecioTotal,
                    Categoria = @Categoria,
                    Talla = @Talla,
                    Color = @Color,
                    Estado = @Estado
                WHERE IdProducto = @IdProducto";
       

                
                await db.ExecuteAsync(sqlQuery, producto);
            }
        }

        public async Task Eliminar(int id)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sqlQuery = @"UPDATE Productos SET
                        Estado = 'I'
                    WHERE IdProducto = @IdProducto";
                await db.ExecuteAsync(sqlQuery, new { IdProducto = id });
            }
        }

        public async Task<IEnumerable<Productos>> ObtenerInformeStock(int idProducto, DateTime? fechaDesde)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sqlQuery = "sp_ObtenerStock";

                var parametros = new 
                { 
                    IdProducto = idProducto, 
                    FechaDesde = fechaDesde   
                };

                return await db.QueryAsync<Productos>(
                    sqlQuery, 
                    parametros, 
                    commandType: CommandType.StoredProcedure
                );
            }
        }
       public async Task<IEnumerable<VentasPorDiaDTO>> ObtenerVentasPorDia(int idProducto, DateTime fechaDesde)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var parametros = new { IdProducto = idProducto, FechaDesde = fechaDesde };
                return await db.QueryAsync<VentasPorDiaDTO>(
                    "sp_VentasPorDia", 
                    parametros, 
                    commandType: CommandType.StoredProcedure
                );
            }
        }




    }
}
