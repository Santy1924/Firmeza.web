using System.ComponentModel.DataAnnotations;

namespace Firmeza.web.Models.ViewModels
{
    public class VentaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha de la venta es obligatoria.")]
        [Display(Name = "Fecha de venta")]
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Debe seleccionar un cliente.")]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }

        [Display(Name = "Total de la venta")]
        [Range(0, double.MaxValue, ErrorMessage = "El total debe ser mayor o igual a 0.")]
        public decimal Total { get; set; }

        [Display(Name = "Método de pago")]
        [Required(ErrorMessage = "Debe especificar un método de pago.")]
        [StringLength(50, ErrorMessage = "El método de pago no puede superar los 50 caracteres.")]
        public string? MetodoPago { get; set; }

        [Display(Name = "Tipo de venta")]
        [Required(ErrorMessage = "Debe especificar el tipo de venta.")]
        [StringLength(50, ErrorMessage = "El tipo de venta no puede superar los 50 caracteres.")]
        public string? TipoVenta { get; set; }

        [Display(Name = "Fecha de venta (texto)")]
        public string? FechaVenta { get; set; }

        // Para mostrar nombre de cliente en las vistas
        public string? NombreCliente { get; set; }
    }
}