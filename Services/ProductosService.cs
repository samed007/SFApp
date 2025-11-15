using AutoMapper;
using SFApp.DAOs;
using SFApp.Models;
using SFApp.DTOs;

namespace SFApp.Services
{
    public interface IProductosService
    {
        Task<ProductosDTO?> Consultar(int id);
        Task<IEnumerable<ProductosDTO>> ListarTodos();
        Task<IEnumerable<ProductosDTO>> ListarPorProducto(string producto);
        Task Agregar(ProductosDTO productoDto);
        Task Actualizar(ProductosDTO productoDto);
        Task Eliminar(int id);
        Task<ProductosDTO> ObtenerStockPorProducto(int idProducto, DateTime? fechaDesde);
        Task<IEnumerable<VentasPorDiaDTO>> ObtenerVentasPorDia(int idProducto, DateTime fechaDesde);
        

    }

    public class ProductosService : IProductosService
    {
        private readonly IProductosDAO _productosDAO;
        private readonly IMapper _mapper;

        public ProductosService(IProductosDAO productosDAO, IMapper mapper)
        {
            _productosDAO = productosDAO;
            _mapper = mapper;
        }

        public async Task<ProductosDTO?> Consultar(int id)
        {
            var producto = await _productosDAO.Consultar(id);
            return _mapper.Map<ProductosDTO?>(producto);
        }

        public async Task<IEnumerable<ProductosDTO>> ListarTodos()
        {
            var productos = await _productosDAO.ListarTodos();
            return _mapper.Map<IEnumerable<ProductosDTO>>(productos);
        }

        public async Task<IEnumerable<ProductosDTO>> ListarPorProducto(string producto)
        {
            var productos = await _productosDAO.ListarPorProducto(producto);
            return _mapper.Map<IEnumerable<ProductosDTO>>(productos);
        }

        public async Task Agregar(ProductosDTO productoDto)
        {
            var producto = _mapper.Map<Productos>(productoDto);
            await _productosDAO.Agregar(producto);
        }

        public async Task Actualizar(ProductosDTO productoDto)
        {
            var producto = _mapper.Map<Productos>(productoDto);
            await _productosDAO.Actualizar(producto);
        }

        public async Task Eliminar(int id)
        {
            await _productosDAO.Eliminar(id);
        }
        public async Task<ProductosDTO?> ObtenerStockPorProducto(int idProducto, DateTime? fechaDesde)
        {
            
            var resultado = await _productosDAO.ObtenerInformeStock(idProducto, fechaDesde);
            var producto = resultado.FirstOrDefault();

            if (producto == null)
                return null;

            var productoDTO = _mapper.Map<ProductosDTO>(producto);

        
            if (fechaDesde.HasValue)
            {
            
                var ventasPorDia = await ObtenerVentasPorDia(idProducto, fechaDesde.Value);
                productoDTO.VentasPorDia = ventasPorDia.ToList();
            }

            return productoDTO;
        }

        
        public async Task<IEnumerable<VentasPorDiaDTO>> ObtenerVentasPorDia(int idProducto, DateTime fechaDesde)
        {
            var ventas = await _productosDAO.ObtenerVentasPorDia(idProducto, fechaDesde);
            return ventas.Select(v => new VentasPorDiaDTO 
            { 
                FechaVenta = v.FechaVenta, 
                Ventas = v.Ventas 
            });
        }



    }
}
