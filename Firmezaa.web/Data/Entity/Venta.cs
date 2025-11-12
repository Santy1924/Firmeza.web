using System.ComponentModel.DataAnnotations;

namespace Firmeza.web.Data.Entity
{
    public class Venta
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha de la venta es obligatoria.")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un cliente.")]
        public int ClienteId { get; set; }

        public Cliente? Cliente { get; set; }

        [Required(ErrorMessage = "Debe especificar el total de la venta.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El total debe ser mayor que 0.")]
        public decimal Total { get; set; }

        [Required(ErrorMessage = "Debe especificar un método de pago.")]
        [StringLength(50, ErrorMessage = "El método de pago no puede superar los 50 caracteres.")]
        public string MetodoPago { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe especificar el tipo de venta.")]
        [StringLength(50, ErrorMessage = "El tipo de venta no puede superar los 50 caracteres.")]
        public string TipoVenta { get; set; } = string.Empty;

        // Relación 1:N con DetalleVenta
        public ICollection<DetalleVenta>? DetallesVenta { get; set; }
    }
}