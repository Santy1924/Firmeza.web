using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Firmeza.web.Data;
using Firmeza.web.Models.Entity;
using Microsoft.AspNetCore.Authorization;

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
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NombreCompleto,Documento,Correo,Telefono,Direccion,Activo")] Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Por favor verifica los datos ingresados antes de continuar.";
                return View(cliente);
            }

            try
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "El cliente se ha registrado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Aquí podrías registrar el error si usas un sistema de logs
                // _logger.LogError(ex, "Error al crear cliente.");

                TempData["ErrorMessage"] = "Ocurrió un error al registrar el cliente. Por favor intenta nuevamente.";
                return View(cliente);
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
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NombreCompleto,Documento,Correo,Telefono,Direccion,Activo")] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                TempData["ErrorMessage"] = "El cliente especificado no existe o el identificador no coincide.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Por favor, verifica los datos ingresados antes de continuar.";
                return View(cliente);
            }

            try
            {
                _context.Update(cliente);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "El cliente se ha actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(cliente.Id))
                {
                    TempData["ErrorMessage"] = "El cliente ya no existe en la base de datos.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "Ocurrió un error al intentar actualizar el cliente. Inténtalo nuevamente.";
                    return View(cliente);
                }
            }
            catch (Exception ex)
            {
                // Puedes registrar el error con _logger.LogError(ex, "Error al editar cliente");
                TempData["ErrorMessage"] = "Se produjo un error inesperado. Por favor intenta de nuevo más tarde.";
                return View(cliente);
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
