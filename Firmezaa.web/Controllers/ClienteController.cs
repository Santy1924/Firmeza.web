using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Firmeza.web.Data;
using Firmeza.web.Data.Entity;
using Microsoft.AspNetCore.Authorization;
using Firmeza.web.Data.Entity;
using Firmeza.web.Models.ViewModels;

namespace Firmeza.web.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ClienteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClienteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cliente
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clientes.ToListAsync());
        }

        // GET: Cliente/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Cliente/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cliente/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClienteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Por favor verifica los datos ingresados antes de continuar.";
                return View(model);
            }

            try
            {
                // Verificar duplicado (opcional)
                var existe = await _context.Clientes.AnyAsync(c => c.Documento == model.Documento);
                if (existe)
                {
                    TempData["ErrorMessage"] = "Ya existe un cliente con ese número de documento.";
                    return View(model);
                }

                var cliente = new Cliente
                {
                    NombreCompleto = model.NombreCompleto,
                    Documento = model.Documento,
                    Correo = model.Correo,
                    Telefono = model.Telefono,
                    Direccion = model.Direccion,
                    Activo = model.Activo
                };

                _context.Add(cliente);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "El cliente se ha registrado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Si usas ILogger
                //_logger.LogError(ex, "Error al crear cliente");
                TempData["ErrorMessage"] = "Ocurrió un error al registrar el cliente. Por favor intenta nuevamente.";
                return View(model);
            }
        }



        // GET: Cliente/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Cliente/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClienteViewModel model)
        {
            if (id != model.Id)
            {
                TempData["ErrorMessage"] = "El cliente especificado no existe o el identificador no coincide.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Por favor, verifica los datos ingresados antes de continuar."; return View(model);
            }

            try
            {
                var cliente = await _context.Clientes.FindAsync(id);
                if (cliente == null)
                {
                    TempData["ErrorMessage"] = "El cliente ya no existe en la base de datos."; return RedirectToAction(nameof(Index));
                }

                // Validar duplicado (si aplica)
                bool documentoDuplicado = await _context.Clientes
                .AnyAsync(c => c.Documento == model.Documento && c.Id != model.Id);

            if (documentoDuplicado)
            {
                TempData["ErrorMessage"] = "Ya existe otro cliente con ese número de documento.";
                return View(model);
            }

            // Actualizar solo los campos permitidos
            cliente.NombreCompleto = model.NombreCompleto;
            cliente.Documento = model.Documento;
            cliente.Correo = model.Correo;
            cliente.Telefono = model.Telefono;
            cliente.Direccion = model.Direccion;
            cliente.Activo = model.Activo;

            _context.Update(cliente);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "El cliente se ha actualizado correctamente.";
            return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            { 
                if (!await _context.Clientes.AnyAsync(c => c.Id == model.Id))
                {
                    TempData["ErrorMessage"] = "El cliente ya no existe en la base de datos.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["ErrorMessage"] = "Ocurrió un error al intentar actualizar el cliente. Inténtalo nuevamente.";
                return View(model);
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "Error al editar cliente {Id}", id);
                TempData["ErrorMessage"] = "Se produjo un error inesperado. Por favor intenta de nuevo más tarde."; return View(model);
            }
        }
        

        public int Id { get; set; }

        // GET: Cliente/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Cliente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }
    }
}
