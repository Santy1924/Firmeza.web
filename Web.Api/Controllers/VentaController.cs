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
    public class VentaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VentaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Venta
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VentaDto>>> GetVentas()
        {
            var ventas = await _context.Ventas
                .Include(v => v.Cliente)
                .Select(v => new VentaDto
                {
                    Id = v.Id,
                    Fecha = v.Fecha,
                    ClienteNombre = v.Cliente != null ? v.Cliente.NombreCompleto : "Sin cliente",
                    Total = v.Total,
                    MetodoPago = v.MetodoPago,
                    TipoVenta = v.TipoVenta
                })
                .ToListAsync();

            return Ok(ventas);
        }

        // GET: api/Venta/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VentaDto>> GetVenta(int id)
        {
            var venta = await _context.Ventas
                .Include(v => v.Cliente)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venta == null)
            {
                return NotFound();
            }

            var ventaDto = new VentaDto
            {
                Id = venta.Id,
                Fecha = venta.Fecha,
                ClienteNombre = venta.Cliente != null ? venta.Cliente.NombreCompleto : "Sin cliente",
                Total = venta.Total,
                MetodoPago = venta.MetodoPago,
                TipoVenta = venta.TipoVenta
            };

            return Ok(ventaDto);
        }

        // POST: api/Venta
        [HttpPost]
        public async Task<ActionResult<VentaDto>> PostVenta([FromBody] VentaCreateUpdateDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cliente = await _context.Clientes.FindAsync(model.ClienteId);
            if (cliente == null)
                return BadRequest($"El cliente con ID {model.ClienteId} no existe.");

            var venta = new Venta
            {
                ClienteId = model.ClienteId,
                Fecha = DateTime.UtcNow,
                MetodoPago = model.MetodoPago,
                TipoVenta = model.TipoVenta,
                Total = model.Total
            };

            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();

            var ventaDto = new VentaDto
            {
                Id = venta.Id,
                Fecha = venta.Fecha,
                ClienteNombre = cliente.NombreCompleto,
                MetodoPago = venta.MetodoPago,
                TipoVenta = venta.TipoVenta,
                Total = venta.Total
            };

            return CreatedAtAction(nameof(GetVenta), new { id = venta.Id }, ventaDto);
        }


        // PUT: api/Venta/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVenta(int id, [FromBody] VentaCreateUpdateDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null)
                return NotFound(new { mensaje = "Venta no encontrada" });

            var cliente = await _context.Clientes.FindAsync(model.ClienteId);
            if (cliente == null)
                return BadRequest($"El cliente con ID {model.ClienteId} no existe.");

            venta.ClienteId = model.ClienteId;
            venta.MetodoPago = model.MetodoPago;
            venta.TipoVenta = model.TipoVenta;
            venta.Total = model.Total;

            await _context.SaveChangesAsync();

            return NoContent();
        }


        // DELETE: api/Venta/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVenta(int id)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null)
                return NotFound();

            _context.Ventas.Remove(venta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VentaExists(int id)
        {
            return _context.Ventas.Any(e => e.Id == id);
        }
    }
}

