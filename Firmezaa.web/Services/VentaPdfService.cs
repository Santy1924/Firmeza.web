using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Firmeza.web.Data.Entity;

namespace Firmeza.web.Services
{
    public class VentaPdfService
    {
        public byte[] GenerateVentaPdf(Venta venta)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);
                    page.Size(PageSizes.A4);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().Text($"Factura de Venta #{venta.Id}")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    page.Content().PaddingVertical(10).Column(col =>
                    {
                        // Datos del cliente
                        col.Item().Text($"Cliente: {venta.Cliente?.NombreCompleto}");
                        col.Item().Text($"Documento: {venta.Cliente?.Documento}");
                        col.Item().Text($"Correo: {venta.Cliente?.Correo}");
                        col.Item().Text($"Fecha: {venta.Fecha:dd/MM/yyyy}");

                        col.Item().PaddingVertical(10);

                        // Tabla de productos
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(4);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Producto").SemiBold();
                                header.Cell().Text("Cantidad").SemiBold();
                                header.Cell().Text("Precio Unitario").SemiBold();
                                header.Cell().Text("Subtotal").SemiBold();
                            });

                            foreach (var item in venta.DetallesVenta)
                            {
                                table.Cell().Text(item.Producto?.Nombre ?? "");
                                table.Cell().Text(item.Cantidad.ToString());
                                table.Cell().Text($"${item.PrecioUnitario:N0}");
                                table.Cell().Text($"${item.Subtotal:N0}");
                            }
                        });

                        col.Item().PaddingVertical(10);
                        col.Item().AlignRight().Text($"Total: ${venta.Total:N0}")
                            .Bold().FontSize(14);
                    });

                    page.Footer().AlignCenter().Text(txt =>
                    {
                        txt.Span("Gracias por su compra").FontSize(10).Italic();
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}

