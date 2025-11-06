using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Firmeza.web.Data;
using Firmeza.web.Models.Entity;

namespace Firmeza.web.Controllers
{
    [Authorize(Roles = "Administrador")] // Solo los usuarios con este rol pueden acceder
    public class ProductoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Producto
        public async Task<IActionResult> Index()
        {
            return View(await _context.Productos.ToListAsync());
        }

        // GET: Producto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var producto = await _context.Productos
                .FirstOrDefaultAsync(m => m.Id == id);

            if (producto == null)
                return NotFound();

            return View(producto);
        }

        // GET: Producto/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Producto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Producto producto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(producto);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "✅ Producto creado correctamente.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["ErrorMessage"] = "❌ Los datos del producto no son válidos.";
                return View(producto);
            }
            catch (Exception ex)
            {
                // Registrar el error (para diagnóstico interno)
                Console.WriteLine($"Error al crear el producto: {ex.Message}");

                // Mostrar mensaje amigable al usuario
                TempData["ErrorMessage"] = "⚠️ Ocurrió un error al guardar el producto. Intenta nuevamente.";
                return View(producto);
            }
        }


        // GET: Producto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return NotFound();

            return View(producto);
        }

        // POST: Producto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Producto producto)
        {
            if (id != producto.Id)
            {
                TempData["ErrorMessage"] = "El producto especificado no existe.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "✅ Producto actualizado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "⚠️ No se pudo actualizar el producto. Intenta nuevamente.";
            }

            return View(producto);
        }

        // GET: Producto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var producto = await _context.Productos
                .FirstOrDefaultAsync(m => m.Id == id);

            if (producto == null)
                return NotFound();

            return View(producto);
        }

        // POST: Producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
                _context.Productos.Remove(producto);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }
    }
}

