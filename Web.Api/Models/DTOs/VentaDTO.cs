namespace Firmeza.web.Web.Api.Models.DTOs;

public class VentaDto
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public string ClienteNombre { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public string MetodoPago { get; set; } = string.Empty;
    public string TipoVenta { get; set; } = string.Empty;
}