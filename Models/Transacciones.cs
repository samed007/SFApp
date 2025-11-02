namespace SFApp.Models
{
    public class Transacciones
    {
        public int Id { get; set; }
        public string IdTransaccion { get; set; }
        public DateTime Fecha { get; set; }
        public decimal ImporteTotal { get; set; }
        public DateTime? FechaMod { get; set; }
        public string Tipo { get; set; } = "VE";
        public string Estado { get; set; }
    }
}
