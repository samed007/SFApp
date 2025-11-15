using System;

namespace SFApp.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }           
        public string Nombre { get; set; }           
        public string Codigo { get; set; }           
        public string Contrasena { get; set; }       
        public string Rol { get; set; }               
        public string Estado { get; set; } = "A";    
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime FechaMod { get; set; } = DateTime.Now;
    }
}
