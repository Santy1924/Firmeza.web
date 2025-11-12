using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Firmeza.web.Data;
using Firmeza.web.Data.Entity;
using Firmeza.web.Models.ViewModels;
using Firmeza.web.Services;
using Microsoft.AspNetCore.Authorization;

namespace Firmeza.web.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class VentaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly VentaPdfService _ventaPdfService;

        public VentaController(ApplicationDbContext context)
        {
            _context = context;
            _ventaPdfService = new VentaPdfService();
        }
        
        public async Task<IActionResult> GeneratePdf(int id)
        {
            var venta = await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.DetallesVenta)
                .ThenInclude(dv => dv.Producto)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venta == null)
            {
                return NotFound();
            }

            var pdfBytes = _ventaPdfService.GenerateVentaPdf(venta);

            return File(pdfBytes, "application/pdf", $"Venta_{venta.Id}.pdf");
        }

        // GET: Venta
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Ventas.Include(v => v.Cliente);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Venta/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas
                .Include(v => v.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

        // GET: Venta/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id");
            return View();
        }

        // POST: Venta/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VentaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Por favor verifica los datos ingresados antes de continuar.";
                ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "NombreCompleto", model.ClienteId);
                return View(model);
            }

            try
            {
                // Validar que el cliente exista
                var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == model.ClienteId);
                if (!clienteExiste)
                {
                    TempData["ErrorMessage"] = "El cliente seleccionado no existe.";
                    ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "NombreCompleto", model.ClienteId);
                    return View(model);
                }

                // Crear la entidad Venta a partir del ViewModel
                var venta = new Venta
                {
                    Fecha = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
                    ClienteId = model.ClienteId,
                    MetodoPago = model.MetodoPago,
                    TipoVenta = model.TipoVenta,
                    Total = model.Total
                };

                _context.Add(venta);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "La venta se ha registrado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear la venta: {ex.Message}");
                TempData["ErrorMessage"] = "Ocurrió un error al registrar la venta. Por favor intenta nuevamente.";

                ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "NombreCompleto", model.ClienteId);
                return View(model);
            }
        }

        // GET: Venta/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id", venta.ClienteId);
            return View(venta);
        }

        // POST: Venta/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VentaViewModel model)
        {
            if (id != model.Id)
            {
                TempData["ErrorMessage"] = "La venta especificada no existe o el identificador no coincide.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Por favor verifica los datos ingresados antes de continuar.";
                ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "NombreCompleto", model.ClienteId);
                return View(model);
            }

            try
            {
                // Buscar venta existente en la base de datos
                var ventaExistente = await _context.Ventas.FindAsync(id);
                if (ventaExistente == null)
                {
                    TempData["ErrorMessage"] = "La venta ya no existe en la base de datos.";
                    return RedirectToAction(nameof(Index));
                }

                // Validar que el cliente exista
                var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == model.ClienteId);
                if (!clienteExiste)
                {   
                    TempData["ErrorMessage"] = "El cliente seleccionado no existe.";
                    ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "NombreCompleto", model.ClienteId);
                    return View(model);
                }

                // Actualizar los campos editables
                ventaExistente.ClienteId = model.ClienteId;
                ventaExistente.Fecha = DateTime.SpecifyKind(model.Fecha, DateTimeKind.Utc);
                ventaExistente.MetodoPago = model.MetodoPago;
                ventaExistente.TipoVenta = model.TipoVenta;
                ventaExistente.Total = model.Total;

                _context.Update(ventaExistente);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "La venta se ha actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Ventas.AnyAsync(v => v.Id == model.Id))
                {
                    TempData["ErrorMessage"] = "La venta ya no existe en la base de datos.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["ErrorMessage"] = "Ocurrió un error de concurrencia al intentar actualizar la venta. Inténtalo nuevamente.";
                ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "NombreCompleto", model.ClienteId);
                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al editar la venta: {ex.Message}");
                TempData["ErrorMessage"] = "Ocurrió un error inesperado. Por favor intenta nuevamente.";
                ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "NombreCompleto", model.ClienteId);
                return View(model);
            }
        }


        public object ClienteId { get; set; }

        public int Id { get; set; }

        // GET: Venta/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas
                .Include(v => v.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

        // POST: Venta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta != null)
            {
                _context.Ventas.Remove(venta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VentaExists(int id)
        {
            return _context.Ventas.Any(e => e.Id == id);
        }
    }
}
