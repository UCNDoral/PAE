using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using SistemaReportes.Model;
using iText.Kernel.Font;
using iText.IO.Font.Constants;


namespace SistemaReportes.Exportadores
{
    public static class ExportadorPDF
    {
        public static void ExportarVentas(List<VentaReporte> datos, string ruta)
        {
            using var writer = new PdfWriter(ruta);
            using var pdf = new PdfDocument(writer);
            using var doc = new Document(pdf);
            doc.SetMargins(40, 40, 40, 40);

            // Fuente negrita reutilizable (iText7 no expone SetBold() en Paragraph)
            var fontBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

            // -- Título --
            doc.Add(new Paragraph("REPORTE DE VENTAS")
                .SetFontSize(18).SetFont(fontBold)
                .SetTextAlignment(TextAlignment.CENTER));

            doc.Add(new Paragraph($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}")
                .SetFontSize(10).SetTextAlignment(TextAlignment.RIGHT));

            doc.Add(new Paragraph("\n"));

            // -- Tabla 7 columnas --
            var tabla = new Table(new float[] { 5f, 20f, 25f, 10f, 15f, 15f, 15f })
                .UseAllAvailableWidth();

            var azul = new DeviceRgb(46, 117, 182);

            // Helper para celdas de encabezado
            Cell Enc(string t) => new Cell()
                .Add(new Paragraph(t).SetFont(fontBold)
                     .SetFontColor(ColorConstants.WHITE).SetFontSize(9))

                .SetBackgroundColor(azul)
                .SetTextAlignment(TextAlignment.CENTER);

            tabla.AddHeaderCell(Enc("ID"));
            tabla.AddHeaderCell(Enc("Cliente"));
            tabla.AddHeaderCell(Enc("Producto"));
            tabla.AddHeaderCell(Enc("Cant."));
            tabla.AddHeaderCell(Enc("P.Unit."));
            tabla.AddHeaderCell(Enc("Total"));
            tabla.AddHeaderCell(Enc("Fecha"));

            // -- Filas de datos --
            var azulClaro = new DeviceRgb(235, 243, 251);
            bool alterno = false;
            decimal granTotal = 0;

            foreach (var v in datos)
            {
                var fondo = alterno ? azulClaro : null;

                Cell C(string t, TextAlignment a = TextAlignment.LEFT) =>
                    new Cell().Add(new Paragraph(t).SetFontSize(8))
                              .SetTextAlignment(a)
                              .SetBackgroundColor(fondo);

                tabla.AddCell(C(v.IdVenta.ToString(), TextAlignment.CENTER));
                tabla.AddCell(C(v.Cliente));
                tabla.AddCell(C(v.Producto));
                tabla.AddCell(C(v.Cantidad.ToString(), TextAlignment.CENTER));
                tabla.AddCell(C($"C${v.PrecioUnit:N2}", TextAlignment.RIGHT));
                tabla.AddCell(C($"C${v.Total:N2}", TextAlignment.RIGHT));
                tabla.AddCell(C(v.FechaVenta.ToString("dd/MM/yy"),
                               TextAlignment.CENTER));

                granTotal += v.Total;
                alterno = !alterno;
            }

            // -- Fila de total --
            tabla.AddCell(new Cell(1, 5)
                .Add(new Paragraph("TOTAL GENERAL:").SetFont(fontBold)
                     .SetFontColor(ColorConstants.WHITE).SetFontSize(9))
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetBackgroundColor(azul));

            tabla.AddCell(new Cell()
                .Add(new Paragraph($"C${granTotal:N2}").SetFont(fontBold)
                     .SetFontColor(ColorConstants.WHITE).SetFontSize(9))
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetBackgroundColor(azul));

            tabla.AddCell(new Cell().SetBackgroundColor(azul));

            doc.Add(tabla);
            doc.Add(new Paragraph($"\nTotal de registros: {datos.Count}")
                .SetFontSize(9).SetTextAlignment(TextAlignment.RIGHT));
        }
    }
}
