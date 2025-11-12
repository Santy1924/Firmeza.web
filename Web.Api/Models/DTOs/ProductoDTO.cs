namespace Firmeza.web.Web.Api.Models.DTOs;

public class ProductoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal PrecioUnitario { get; set; }
    public string Categoria { get; set; } = string.Empty;
}