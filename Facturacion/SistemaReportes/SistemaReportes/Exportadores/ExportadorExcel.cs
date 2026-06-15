using ClosedXML.Excel;
using SistemaReportes.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaReportes.Exportadores
{
    public static class ExportadorExcel
    {
        public static void ExportarVentas(List<VentaReporte> datos, string ruta)
        {
            using var libro = new XLWorkbook();
            var hoja = libro.Worksheets.Add("Reporte de Ventas");

            // -- Título --
            hoja.Cell(1, 1).Value = "REPORTE DE VENTAS";
            hoja.Range(1, 1, 1, 7).Merge();
            hoja.Cell(1, 1).Style.Font.Bold = true;
            hoja.Cell(1, 1).Style.Font.FontSize = 14;
            hoja.Cell(1, 1).Style.Alignment.Horizontal =
                XLAlignmentHorizontalValues.Center;

            hoja.Cell(2, 1).Value = $"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}";

            // -- Encabezados de columna (fila 4) --
            string[] cols = { "ID", "Cliente", "Producto",
                              "Cantidad", "Precio Unit.", "Total", "Fecha" };
            for (int c = 0; c < cols.Length; c++)
            {
                var cel = hoja.Cell(4, c + 1);
                cel.Value = cols[c];
                cel.Style.Font.Bold = true;
                cel.Style.Fill.BackgroundColor = XLColor.FromHtml("#2E75B6");
                cel.Style.Font.FontColor = XLColor.White;
            }

            // -- Datos --
            int fila = 5;
            foreach (var v in datos)
            {
                hoja.Cell(fila, 1).Value = v.IdVenta;
                hoja.Cell(fila, 2).Value = v.Cliente;
                hoja.Cell(fila, 3).Value = v.Producto;
                hoja.Cell(fila, 4).Value = v.Cantidad;
                hoja.Cell(fila, 5).Value = v.PrecioUnit;
                hoja.Cell(fila, 6).Value = v.Total;
                hoja.Cell(fila, 7).Value = v.FechaVenta.ToString("dd/MM/yyyy");

                // Formato moneda en columnas 5 y 6
                hoja.Cell(fila, 5).Style.NumberFormat.Format = "C$#,##0.00";
                hoja.Cell(fila, 6).Style.NumberFormat.Format = "C$#,##0.00";

                // Color alterno en filas pares
                if (fila % 2 == 0)
                    hoja.Row(fila).Style.Fill.BackgroundColor =
                        XLColor.FromHtml("#EBF3FB");
                fila++;
            }

            // -- Fila de total --
            hoja.Cell(fila, 5).Value = "TOTAL:";
            hoja.Cell(fila, 5).Style.Font.Bold = true;
            hoja.Cell(fila, 6).FormulaA1 = $"=SUM(F5:F{fila - 1})";
            hoja.Cell(fila, 6).Style.Font.Bold = true;
            hoja.Cell(fila, 6).Style.NumberFormat.Format = "C$#,##0.00";

            hoja.Columns().AdjustToContents(); // Ajustar ancho automático
            libro.SaveAs(ruta);
        }
    }
}
