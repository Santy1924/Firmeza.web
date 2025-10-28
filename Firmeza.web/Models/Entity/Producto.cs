namespace Firmeza.web.Models.Entity;

public class Producto
{
    public int Id { get; set; }
    
    public string Nombre { get; set; } = string.Empty;
    
    public string? Descripcion { get; set; }
    
    public decimal PrecioUnitario { get; set; }
    
    public string? UnidadMedida { get; set; } 

    public bool Activo { get; set; } = true;
    
    public ICollection<DetalleVenta>? DetallesVenta { get; set; }
}