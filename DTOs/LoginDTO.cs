using System.ComponentModel.DataAnnotations;

public class LoginDTO
{
    [Required]
    public string Codigo { get; set; }

    [Required]
    public string Contrasena { get; set; }
}
