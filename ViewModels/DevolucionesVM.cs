using SFApp.DTOs;

namespace SFApp.ViewModels{
    public class DevolucionesViewModel
    {
        public IEnumerable<TransaccionesDTO> Transacciones { get; set; } = new List<TransaccionesDTO>();

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalRecords { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);

        // Filtros
        public string? IdTransaccionFiltro { get; set; }
        public DateTime? Fecha { get; set; }
    }

}
