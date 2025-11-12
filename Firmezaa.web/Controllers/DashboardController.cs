using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Firmeza.web.Data;

namespace Firmeza.web.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var totalProductos = _context.Productos.Count();
            var totalClientes = _context.Clientes.Count();
            var totalVentas = _context.Ventas.Count();

            ViewData["TotalProductos"] = totalProductos;
            ViewData["TotalClientes"] = totalClientes;
            ViewData["TotalVentas"] = totalVentas;

            return View();
        }
    }
}