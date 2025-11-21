using Firmeza.web.Data;
using Firmeza.web.Data.Entity;
using Firmeza.web.Web.Api.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductoController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        // GET: api/Producto  (Público o autenticado)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetProductos()
        {
            var productos = await _context.Productos
                .Select(p => new ProductoDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    PrecioUnitario = p.PrecioUnitario,
                    Categoria = p.Categoria
                })
                .ToListAsync();

            return Ok(productos);
        }

        // GET: api/Producto/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDto>> GetProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
                return NotFound(new { mensaje = "Producto no encontrado" });

            var productoDto = new ProductoDto
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                PrecioUnitario = producto.PrecioUnitario,
                Categoria = producto.Categoria
            };

            return Ok(productoDto);
        }
        
        // POST: api/Producto (Sólo ADMIN)
        [HttpPost]
        public async Task<ActionResult<ProductoDto>> PostProducto([FromBody] ProductoDto productoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var producto = new Producto
            {
                Nombre = productoDto.Nombre,
                Descripcion = productoDto.Descripcion,
                PrecioUnitario = productoDto.PrecioUnitario,
                Categoria = productoDto.Categoria
            };

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            productoDto.Id = producto.Id;

            return CreatedAtAction(nameof(GetProducto), new { id = producto.Id }, productoDto);
        }
        
        // PUT: api/Producto/5 (Sólo ADMIN)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, [FromBody] ProductoDto productoDto)
        {
            if (id != productoDto.Id)
                return BadRequest(new { mensaje = "El ID del producto no coincide" });

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return NotFound(new { mensaje = "Producto no encontrado" });

            producto.Nombre = productoDto.Nombre;
            producto.Descripcion = productoDto.Descripcion;
            producto.PrecioUnitario = productoDto.PrecioUnitario;
            producto.Categoria = productoDto.Categoria;

            _context.Entry(producto).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        // DELETE: api/Producto/5 (Sólo ADMIN)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return NotFound(new { mensaje = "Producto no encontrado" });

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }
    }
}


