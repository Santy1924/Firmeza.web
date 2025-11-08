using Firmeza.web.Data;
using Firmeza.web.Data.Entity;
using OfficeOpenXml;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.web.Services
{
    public class ExcelImportService
    {
        private readonly ApplicationDbContext _context;

        public ExcelImportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> ImportarProductosAsync(Stream excelStream)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage(excelStream);
            var worksheet = package.Workbook.Worksheets.FirstOrDefault();
            if (worksheet == null)
                throw new Exception("No se encontró ninguna hoja en el archivo Excel.");

            var productos = new List<Producto>();
            int rowCount = worksheet.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++) // Empieza desde la fila 2 (encabezado)
            {
                string nombre = worksheet.Cells[row, 1].Text?.Trim();
                string descripcion = worksheet.Cells[row, 2].Text?.Trim();
                string precioText = worksheet.Cells[row, 3].Text?.Trim();
                string categoria = worksheet.Cells[row, 4].Text?.Trim();

                if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(categoria))
                    continue;

                if (!decimal.TryParse(precioText, out decimal precio))
                    continue;

                bool existe = await _context.Productos.AnyAsync(p => p.Nombre == nombre);
                if (existe)
                    continue;

                productos.Add(new Producto
                {
                    Nombre = nombre,
                    Descripcion = descripcion,
                    PrecioUnitario = precio,
                    Categoria = categoria
                });
            }

            if (productos.Any())
            {
                await _context.Productos.AddRangeAsync(productos);
                await _context.SaveChangesAsync();
            }

            return productos.Count;
        }
    }
}

