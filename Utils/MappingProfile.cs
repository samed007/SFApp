using AutoMapper;
using SFApp.Models;
using SFApp.DTOs;

namespace SFApp.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Productos, ProductosDTO>().ReverseMap();
            CreateMap<Transacciones, TransaccionesDTO>().ReverseMap();
            CreateMap<Inventario, InventarioDTO>().ReverseMap();
           
        }
    }
}
