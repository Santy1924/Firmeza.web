using System.ComponentModel.DataAnnotations;

namespace Firmeza.web.Data.Entity
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\u00f1\u00d1\s'-]+$", ErrorMessage = "El nombre contiene caracteres inválidos.")]
        public string NombreCompleto { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "El documento no puede superar los 20 caracteres.")]
        public string? Documento { get; set; }

        [EmailAddress(ErrorMessage = "El correo ingresado no es válido.")]
        public string? Correo { get; set; }

        [Phone(ErrorMessage = "El número de teléfono no es válido.")]
        public string? Telefono { get; set; }

        public string? Direccion { get; set; }

        public bool Activo { get; set; } = true;

        public ICollection<Venta>? Ventas { get; set; }
        public string UserId { get; set; }
    }
}
