namespace Firmeza.web.Models.Entity;

public class Venta
{
    public int Id { get; set; }
    
    public DateTime Fecha { get; set; } = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
    
    public int ClienteId { get; set; }
    
    public Cliente? Cliente { get; set; } 
    
    public decimal Total { get; set; }
    
    public string? MetodoPago { get; set; }
    
    public string? TipoVenta { get; set; }
    
    public ICollection<DetalleVenta>? DetallesVenta { get; set; }
}