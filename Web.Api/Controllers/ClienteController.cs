using Firmeza.web.Data;
using Firmeza.web.Data.Entity;
using Firmeza.web.Web.Api.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClienteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClienteController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Cliente
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteDto>>> GetClientes()
        {
            var clientes = await _context.Clientes
                .Select(c => new ClienteDto
                {
                    NombreCompleto = c.NombreCompleto,
                    Documento = c.Documento,
                    Correo = c.Correo,
                    Telefono = c.Telefono,
                    Direccion = c.Direccion,
                    Activo = c.Activo
                })
                .ToListAsync();

            return Ok(clientes);
        }

        // GET: api/Cliente/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDto>> GetCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return NotFound(new { mensaje = "Cliente no encontrado" });

            var clienteDTO = new ClienteDto
            {
                NombreCompleto = cliente.NombreCompleto,
                Documento = cliente.Documento,
                Correo = cliente.Correo,
                Telefono = cliente.Telefono,
                Direccion = cliente.Direccion,
                Activo = cliente.Activo
            };

            return Ok(clienteDTO);
        }

        // POST: api/Cliente
        [HttpPost]
        public async Task<ActionResult<ClienteDto>> PostCliente([FromBody] ClienteDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Crear usuario en Identity
            var user = new ApplicationUser
            {
                UserName = model.Correo,
                Email = model.Correo
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Crear registro Cliente
            var cliente = new Cliente
            {
                NombreCompleto = model.NombreCompleto,
                Documento = model.Documento,
                Correo = model.Correo,
                Telefono = model.Telefono,
                Direccion = model.Direccion,
                Activo = model.Activo,
                UserId = user.Id
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            var clienteDTO = new ClienteDto
            {
                NombreCompleto = cliente.NombreCompleto,
                Documento = cliente.Documento,
                Correo = cliente.Correo,
                Telefono = cliente.Telefono,
                Direccion = cliente.Direccion,
                Activo = cliente.Activo
            };

            return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, clienteDTO);
        }


        // PUT: api/Cliente/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, [FromBody] ClienteDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var clienteExistente = await _context.Clientes.FindAsync(id);
            if (clienteExistente == null)
                return NotFound(new { mensaje = "Cliente no encontrado" });

            // Actualizar solo los campos permitidos
            clienteExistente.NombreCompleto = model.NombreCompleto;
            clienteExistente.Documento = model.Documento;
            clienteExistente.Correo = model.Correo;
            clienteExistente.Telefono = model.Telefono;
            clienteExistente.Direccion = model.Direccion;
            clienteExistente.Activo = model.Activo;

            await _context.SaveChangesAsync();

            return NoContent();
        }


        // DELETE: api/Cliente/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound(new { mensaje = "Cliente no encontrado" });

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

