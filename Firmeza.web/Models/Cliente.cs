namespace Firmeza.web.Models.Entity;

public class Cliente
{
    public int Id { get; set; }
    
    public string NombreCompleto { get; set; } = string.Empty;
    
    public string? Documento { get; set; }
    
    public string? Correo { get; set; }
    
    public string? Telefono { get; set; }
    
    public string? Direccion { get; set; }

    public bool Activo { get; set; } = true;
    
    public ICollection<Venta>? Ventas { get; set; }
}