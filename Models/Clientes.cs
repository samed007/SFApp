using SFApp.DTOs;

namespace SFApp.Models
{
    public class Clientes
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Ciudad { get; set; }
        public string CodigoPostal { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Estado { get; set; } = "A";

        
    }
}
