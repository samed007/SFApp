using System;
using System.ComponentModel.DataAnnotations;

namespace SFApp.DTOs
{
    public class UsuarioDTO
    {
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El código es obligatorio.")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Contrasena { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio.")]
        public string Rol { get; set; }

        public string Estado { get; set; } = "A";

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime FechaMod { get; set; } = DateTime.Now;
    }
}
