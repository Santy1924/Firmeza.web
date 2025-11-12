using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Firmeza.web.Data;
using Firmeza.web.Data.Entity;
using Firmeza.web.Models.ViewModels;
using Firmeza.web.Services;

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
        // 🔍 Ahora permite búsqueda y filtrado
        public async Task<IActionResult> Index(string searchString, string categoriaFiltro)
        {
            // Obtener todas las categorías disponibles para el filtro
            var categorias = await _context.Productos
                .Select(p => p.Categoria)
                .Distinct()
                .ToListAsync();

            ViewData["Categorias"] = new SelectList(categorias);


            // Consulta base
            var productos = from p in _context.Productos
                            select p;

            // 🔎 Filtro por búsqueda (nombre o descripción)
            if (!string.IsNullOrEmpty(searchString))
            {
                productos = productos.Where(p =>
                    p.Nombre.ToLower().Contains(searchString.ToLower()) ||
                    p.Descripcion.ToLower().Contains(searchString.ToLower()));
            }
            // Filtro por categoría
            // 🏷️ Filtro por categoría
            if (!String.IsNullOrEmpty(categoriaFiltro))
            {
                productos = productos.Where(p => p.Categoria == categoriaFiltro);
            }

            // Retornar lista filtrada
            return View(await productos.ToListAsync());
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
        public async Task<IActionResult> Create(ProductoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Por favor verifica los datos ingresados antes de continuar.";
                return View(model);
            }

            try
            {
                // Validar duplicado
                bool existe = await _context.Productos.AnyAsync(p => p.Nombre == model.Nombre);
                if (existe)
                {
                    TempData["ErrorMessage"] = "Ya existe un producto con ese nombre.";
                    return View(model);
                }

                var producto = new Producto
                {
                    Nombre = model.Nombre,
                    Descripcion = model.Descripcion,
                    PrecioUnitario = model.PrecioUnitario,
                    Categoria = model.Categoria
                };

                _context.Add(producto);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "El producto se ha registrado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear el producto: {ex.Message}");
                TempData["ErrorMessage"] = "Ocurrió un error al registrar el producto. Por favor intenta nuevamente.";
                return View(model);
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

            var productoViewModel = new ProductoViewModel
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                PrecioUnitario = producto.PrecioUnitario,
                Categoria = producto.Categoria
            };

            return View(productoViewModel);
        }

        // POST: Producto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductoViewModel model)
        {
            if (id != model.Id)
            {
                TempData["ErrorMessage"] = "El producto especificado no existe o el identificador no coincide.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {   
                TempData["ErrorMessage"] = "Por favor verifica los datos ingresados antes de continuar.";
                return View(model);
            }

            try
            {
                // Buscar producto en la base de datos
                var productoExistente = await _context.Productos.FindAsync(id);
                if (productoExistente == null)
                {
                    TempData["ErrorMessage"] = "El producto ya no existe en la base de datos.";
                    return RedirectToAction(nameof(Index));
                }

                // Validar duplicado por nombre
                bool nombreDuplicado = await _context.Productos
                .AnyAsync(p => p.Nombre == model.Nombre && p.Id != model.Id);
                if (nombreDuplicado)
                {
                    TempData["ErrorMessage"] = "Ya existe otro producto con ese nombre.";
                    return View(model);
                }

                // Actualizar solo los campos permitidos
                productoExistente.Nombre = model.Nombre;
                productoExistente.Descripcion = model.Descripcion;
                productoExistente.PrecioUnitario = model.PrecioUnitario;
                productoExistente.Categoria = model.Categoria;

                _context.Update(productoExistente);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "El producto se ha actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Productos.AnyAsync(p => p.Id == model.Id))
                {
                    TempData["ErrorMessage"] = "El producto ya no existe en la base de datos.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["ErrorMessage"] = "Ocurrió un error de concurrencia al intentar actualizar el producto. Inténtalo nuevamente.";
                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al editar el producto: {ex.Message}");
                TempData["ErrorMessage"] = "Ocurrió un error inesperado. Por favor intenta nuevamente.";
                return View(model);
            }
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
            TempData["SuccessMessage"] = "🗑️ Producto eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CargarExcel(IFormFile archivoExcel, [FromServices] ExcelImportService excelService)
        {
            if (archivoExcel == null || archivoExcel.Length == 0)
            {
                TempData["ErrorMessage"] = "Por favor selecciona un archivo Excel válido.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                using var stream = archivoExcel.OpenReadStream();
                int registros = await excelService.ImportarProductosAsync(stream);

                if (registros > 0)
                    TempData["SuccessMessage"] = $"✅ Se cargaron {registros} productos correctamente.";
                else
                    TempData["ErrorMessage"] = "⚠️ No se encontraron productos válidos en el archivo.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar Excel: {ex.Message}");
                TempData["ErrorMessage"] = "❌ Ocurrió un error al procesar el archivo Excel.";
                return RedirectToAction(nameof(Index));
            }
        }


        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }
    }
}


