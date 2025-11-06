namespace Firmeza.web.Models.Entity;
using System.ComponentModel.DataAnnotations;

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

    [Required(ErrorMessage = "Debe seleccionar una unidad de medida.")]
    [StringLength(50, ErrorMessage = "La unidad de medida no puede superar los 50 caracteres.")]
    public string? UnidadMedida { get; set; }

    public bool Activo { get; set; } = true;

    public ICollection<DetalleVenta>? DetallesVenta { get; set; }
}