using System.ComponentModel.DataAnnotations;

namespace Firmeza.web.Models.ViewModels;

public class ClienteViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre completo es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
    [RegularExpression(@"^[a-zA-ZÀ-ÿ\u00f1\u00d1\s'-]+$", ErrorMessage = "El nombre contiene caracteres inválidos.")]
    public string NombreCompleto { get; set; } = string.Empty;

    [StringLength(20, ErrorMessage = "El documento no puede superar los 20 caracteres.")]
    public string? Documento { get; set; }

    [EmailAddress(ErrorMessage = "El correo ingresado no es válido.")]
    [StringLength(100, ErrorMessage = "El correo no puede superar los 100 caracteres.")]
    public string? Correo { get; set; }

    [Phone(ErrorMessage = "El número de teléfono no es válido.")]
    [StringLength(20, ErrorMessage = "El número de teléfono no puede superar los 20 caracteres.")]
    public string? Telefono { get; set; }

    [StringLength(150, ErrorMessage = "La dirección no puede superar los 150 caracteres.")]
    public string? Direccion { get; set; }

    public bool Activo { get; set; } = true;
}