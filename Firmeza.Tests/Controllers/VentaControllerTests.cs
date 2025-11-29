using Firmeza.web.Data;
using Firmeza.web.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Web.Api.Controllers;
using Xunit;
using FluentAssertions;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Firmeza.Tests.Controllers
{
    public class VentaControllerTests
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetMyPurchases_ShouldReturnEmptyList_WhenClienteDoesNotExist()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new VentaController(context);
            
            // Mock user claims
            var claims = new List<Claim>
            {
                new Claim("email", "nonexistent@example.com")
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            // Act
            var result = await controller.GetMyPurchases();

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            var ventas = okResult.Value as System.Collections.Generic.IEnumerable<Firmeza.web.Web.Api.Models.DTOs.VentaDto>;
            ventas.Should().BeEmpty();
        }

        [Fact]
        public async Task GetMyPurchases_ShouldReturnVentas_WhenClienteExists()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            
            var cliente = new Cliente
            {
                NombreCompleto = "Test Cliente",
                Correo = "test@example.com",
                Documento = "123456",
                Telefono = "3001234567",
                Direccion = "Test Address",
                Activo = true,
                UserId = "test-user-id"
            };
            context.Clientes.Add(cliente);
            await context.SaveChangesAsync();

            var venta = new Venta
            {
                ClienteId = cliente.Id,
                Fecha = System.DateTime.UtcNow,
                Total = 100.50m,
                MetodoPago = "Efectivo",
                TipoVenta = "Presencial"
            };
            context.Ventas.Add(venta);
            await context.SaveChangesAsync();

            var controller = new VentaController(context);
            
            // Mock user claims
            var claims = new List<Claim>
            {
                new Claim("email", "test@example.com")
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            // Act
            var result = await controller.GetMyPurchases();

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            var ventas = okResult.Value as System.Collections.Generic.IEnumerable<Firmeza.web.Web.Api.Models.DTOs.VentaDto>;
            ventas.Should().HaveCount(1);
            ventas.First().Total.Should().Be(100.50m);
        }
    }
}
