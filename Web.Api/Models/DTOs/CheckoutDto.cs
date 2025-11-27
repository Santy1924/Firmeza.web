namespace Firmeza.web.Web.Api.Models.DTOs;

public class CheckoutDto
{
    public string MetodoPago { get; set; } = "Efectivo";
    public string TipoVenta { get; set; } = "Contado";
    public List<CheckoutItemDto> Items { get; set; } = new();
}

public class CheckoutItemDto
{
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
}
