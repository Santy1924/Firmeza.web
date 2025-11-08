using System.ComponentModel.DataAnnotations;

namespace Firmeza.web.Data.Entity;
public class Producto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(250, ErrorMessage = "La descripción no puede superar los 250 caracteres.")]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0.")]
    public decimal PrecioUnitario { get; set; }

    public ICollection<DetalleVenta>? DetallesVenta { get; set; }
    
    [Required(ErrorMessage = "La categoría es obligatoria.")]
    [StringLength(100)]
    public string Categoria { get; set; } = string.Empty;
}