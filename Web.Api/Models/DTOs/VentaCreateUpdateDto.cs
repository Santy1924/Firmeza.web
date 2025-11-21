public class VentaCreateUpdateDto
{
    public int ClienteId { get; set; }
    public string MetodoPago { get; set; } = string.Empty;
    public string TipoVenta { get; set; } = string.Empty;
    public decimal Total { get; set; }
}