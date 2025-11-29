using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Firmeza.web.Data;

namespace Firmeza.web.Controllers
{
    [Authorize(Roles = "Administrador, Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            try
            {
                var totalProductos = _context.Productos.Count();
                var totalClientes = _context.Clientes.Count();
                var totalVentas = _context.Ventas.Count();

                ViewData["TotalProductos"] = totalProductos;
                ViewData["TotalClientes"] = totalClientes;
                ViewData["TotalVentas"] = totalVentas;

                return View();
            }
            catch (OperationCanceledException)
            {
                TempData["Error"] = "La solicitud tardó demasiado. Por favor, intente nuevamente.";
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Dashboard Index failed: {ex.Message}");
                TempData["Error"] = "Ocurrió un error al cargar el dashboard. Por favor, intente nuevamente más tarde.";
                
                // Valores por defecto si hay error
                ViewData["TotalProductos"] = 0;
                ViewData["TotalClientes"] = 0;
                ViewData["TotalVentas"] = 0;
                
                return View();
            }
        }
    }
}
