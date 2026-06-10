using FastReport;
using FastReport.Data;
using SistemaFacturacion.Models;
using System;
using System.Collections.Generic;
using System.Text;

using FastReport;
using FastReport.Data;
using SistemaFacturacion.Models;
using FastReport.Utils;

namespace SistemaFacturacion.Reports
{
    /// <summary>
    /// Clase que construye los reportes dinámicamente por código.
    /// No usa archivos .frx externos; todo el diseño se define aquí.
    /// Ventaja: no hay dependencias de archivos, todo está embebido en el ensamblado.
    /// </summary>
    public static class ReportBuilder
    {
        // ─────────────────────────────────────────────────────
        // HELPER: crea y configura un TextObject
        // Centralizado para no repetir código
        // ─────────────────────────────────────────────────────
        private static TextObject CrearTexto(
            string nombre,
            string texto,
            float xCm, float yCm,
            float anchoCm, float altoCm,
            bool negrita = false,
            float fontSize = 9,
            HorzAlign alineacion = HorzAlign.Left)
        {
            var t = new TextObject();
            t.Name = nombre;
            t.Text = texto;
            t.Bounds = new RectangleF(
                Units.Centimeters * xCm,
                Units.Centimeters * yCm,
                Units.Centimeters * anchoCm,
                Units.Centimeters * altoCm);
            t.Font = new Font("Arial", fontSize,
                negrita ? FontStyle.Bold : FontStyle.Regular);
            t.HorzAlign = alineacion;
            return t;
        }

        // ─────────────────────────────────────────────────────
        // REPORTE 1: Factura Individual
        // ─────────────────────────────────────────────────────
        public static Report CrearReporteFactura(
            List<RptFacturaItem> datos)
        {
            var report = new Report();

            // 1) Registrar los datos y habilitarlos
            report.RegisterData(datos, "DsFactura");
            report.GetDataSource("DsFactura").Enabled = true;

            // 2) Crear página tamaño carta
            var page = new ReportPage();
            page.Name = "Page1";
            page.PaperWidth = 21.59f;  // carta en cm
            page.PaperHeight = 27.94f;
            page.LeftMargin = 1.5f;
            page.RightMargin = 1.5f;
            page.TopMargin = 1.5f;
            page.BottomMargin = 1.5f;
            report.Pages.Add(page);

            // ── BANDA: Título ──────────────────────────────
            page.ReportTitle = new ReportTitleBand();
            page.ReportTitle.Name = "ReportTitle1";
            page.ReportTitle.Height = Units.Centimeters * 1.5f;

            var txtTitulo = CrearTexto("txtTitulo", "FACTURA",
                0, 0, 18.59f, 1.2f, true, 18, HorzAlign.Center);
            page.ReportTitle.Objects.Add(txtTitulo);

            // ── BANDA: Encabezado de página (datos del cliente)
            page.PageHeader = new PageHeaderBand();
            page.PageHeader.Name = "PageHeader1";
            page.PageHeader.Height = Units.Centimeters * 4f;

            // Fila 1: Número y Fecha
            page.PageHeader.Objects.Add(CrearTexto("lbl1",
                "N° Factura:", 0, 0, 3.5f, 0.6f, true));
            page.PageHeader.Objects.Add(CrearTexto("val1",
                "[DsFactura.NumeroFactura]", 3.6f, 0, 8f, 0.6f));

            page.PageHeader.Objects.Add(CrearTexto("lbl2",
                "Fecha:", 0, 0.7f, 3.5f, 0.6f, true));
            page.PageHeader.Objects.Add(CrearTexto("val2",
                "[DsFactura.FechaEmision]", 3.6f, 0.7f, 8f, 0.6f));

            page.PageHeader.Objects.Add(CrearTexto("lbl3",
                "Estado:", 0, 1.4f, 3.5f, 0.6f, true));
            page.PageHeader.Objects.Add(CrearTexto("val3",
                "[DsFactura.Estado]", 3.6f, 1.4f, 8f, 0.6f));

            // Fila 2: Cliente y dirección
            page.PageHeader.Objects.Add(CrearTexto("lbl4",
                "Cliente:", 0, 2.1f, 3.5f, 0.6f, true));
            page.PageHeader.Objects.Add(CrearTexto("val4",
                "[DsFactura.ClienteNombre]", 3.6f, 2.1f, 14f, 0.6f));

            page.PageHeader.Objects.Add(CrearTexto("lbl5",
                "Dirección:", 0, 2.8f, 3.5f, 0.6f, true));
            page.PageHeader.Objects.Add(CrearTexto("val5",
                "[DsFactura.ClienteDireccion]", 3.6f, 2.8f, 14f, 0.6f));

            page.PageHeader.Objects.Add(CrearTexto("lbl6",
                "Teléfono:", 0, 3.5f, 3.5f, 0.6f, true));
            page.PageHeader.Objects.Add(CrearTexto("val6",
                "[DsFactura.ClienteTelefono]", 3.6f, 3.5f, 8f, 0.6f));

            // ── BANDA: Encabezado de columnas ──────────────
            page.ColumnHeader = new ColumnHeaderBand();
            page.ColumnHeader.Name = "ColumnHeader1";
            page.ColumnHeader.Height = Units.Centimeters * 0.8f;

            // Crea encabezado de columna con fondo azul
            TextObject ColHead(string nombre, string texto,
                float x, float w)
            {
                var t = CrearTexto(nombre, texto, x, 0, w, 0.7f,
                    true, 9, HorzAlign.Center);
                t.Fill = new SolidFill(Color.FromArgb(46, 117, 182));
                t.TextColor = Color.White;
                t.Border.Lines = BorderLines.All;
                return t;
            }

            page.ColumnHeader.Objects.Add(ColHead("hCod", "Código", 0, 3));
            page.ColumnHeader.Objects.Add(ColHead("hProd", "Producto", 3.1f, 8));
            page.ColumnHeader.Objects.Add(ColHead("hCant", "Cantidad", 11.2f, 2.5f));
            page.ColumnHeader.Objects.Add(ColHead("hPrecio", "P.Unit.", 13.8f, 2.5f));
            page.ColumnHeader.Objects.Add(ColHead("hSub", "Subtotal", 16.4f, 2.2f));

            // ── BANDA: Datos (una fila por producto) ───────
            var dataBand = new DataBand();
            dataBand.Name = "Data1";
            dataBand.Height = Units.Centimeters * 0.7f;
            dataBand.DataSource = report.GetDataSource("DsFactura");
            page.Bands.Add(dataBand);

            TextObject ColData(string nombre, string expr,
                float x, float w,
                HorzAlign ha = HorzAlign.Left)
            {
                var t = CrearTexto(nombre, expr, x, 0, w, 0.6f,
                    false, 9, ha);
                t.Border.Lines = BorderLines.All;
                return t;
            }

            dataBand.Objects.Add(ColData("dCod",
                "[DsFactura.CodigoProducto]", 0, 3));
            dataBand.Objects.Add(ColData("dProd",
                "[DsFactura.NombreProducto]", 3.1f, 8));
            dataBand.Objects.Add(ColData("dCant",
                "[DsFactura.Cantidad]", 11.2f, 2.5f, HorzAlign.Center));
            dataBand.Objects.Add(ColData("dPrecio",
                "[DsFactura.PrecioUnitario]", 13.8f, 2.5f, HorzAlign.Right));
            dataBand.Objects.Add(ColData("dSub",
                "[DsFactura.Subtotal]", 16.4f, 2.2f, HorzAlign.Right));

            // ── BANDA: Pie de página (totales) ─────────────
            page.PageFooter = new PageFooterBand();
            page.PageFooter.Name = "PageFooter1";
            page.PageFooter.Height = Units.Centimeters * 2.8f;

            // Línea separadora visual
            var lineaSep = CrearTexto("sepTotales",
                "─────────────────────────────────────────",
                0, 0, 18.59f, 0.5f, false, 8, HorzAlign.Right);
            page.PageFooter.Objects.Add(lineaSep);

            void FilaTotal(string nomLbl, string nomVal,
                string etiqueta, string expr, float y)
            {
                page.PageFooter.Objects.Add(CrearTexto(nomLbl,
                    etiqueta, 11f, y, 4f, 0.6f, true, 9,
                    HorzAlign.Right));
                page.PageFooter.Objects.Add(CrearTexto(nomVal,
                    expr, 15.2f, y, 3.4f, 0.6f, false, 9,
                    HorzAlign.Right));
            }

            FilaTotal("lblSub", "valSub",
                "Subtotal:",
                "[DsFactura.TotalSubtotal]", 0.6f);
            FilaTotal("lblIva", "valIva",
                "IVA (13%):",
                "[DsFactura.TotalIVA]", 1.3f);
            FilaTotal("lblTot", "valTot",
                "TOTAL:",
                "[DsFactura.TotalFactura]", 2.0f);

            // Número de página
            page.PageFooter.Objects.Add(CrearTexto("nroPag",
                "Página [Page#] de [TotalPages#]",
                0, 2.0f, 8f, 0.5f, false, 8));

            return report;
        }

        // ─────────────────────────────────────────────────────
        // REPORTE 2: Ventas por Período
        // ─────────────────────────────────────────────────────
        public static Report CrearReporteVentas(
            List<RptVentaItem> datos,
            string fechaInicio, string fechaFin)
        {
            var report = new Report();

            report.RegisterData(datos, "DsVentas");
            report.GetDataSource("DsVentas").Enabled = true;

            var page = new ReportPage();
            page.Name = "Page1";
            page.PaperWidth = 21.59f;
            page.PaperHeight = 27.94f;
            page.LeftMargin = 1.5f;
            page.RightMargin = 1.5f;
            page.TopMargin = 1.5f;
            page.BottomMargin = 1.5f;
            report.Pages.Add(page);

            // ── BANDA: Título ──────────────────────────────
            page.ReportTitle = new ReportTitleBand();
            page.ReportTitle.Name = "ReportTitle1";
            page.ReportTitle.Height = Units.Centimeters * 2f;

            page.ReportTitle.Objects.Add(CrearTexto("tit",
                "Reporte de Ventas por Período",
                0, 0, 18.59f, 1f, true, 16, HorzAlign.Center));

            page.ReportTitle.Objects.Add(CrearTexto("periodo",
                $"Del {fechaInicio}   al   {fechaFin}",
                0, 1.1f, 18.59f, 0.6f, false, 10, HorzAlign.Center));

            // ── BANDA: Encabezado de columnas ──────────────
            page.ColumnHeader = new ColumnHeaderBand();
            page.ColumnHeader.Name = "ColumnHeader1";
            page.ColumnHeader.Height = Units.Centimeters * 0.8f;

            TextObject CH(string nombre, string texto,
                float x, float w)
            {
                var t = CrearTexto(nombre, texto, x, 0, w, 0.7f,
                    true, 8, HorzAlign.Center);
                t.Fill = new SolidFill(Color.FromArgb(46, 117, 182));
                t.TextColor = Color.White;
                t.Border.Lines = BorderLines.All;
                return t;
            }

            page.ColumnHeader.Objects.Add(CH("hNum", "N° Factura", 0, 3.5f));
            page.ColumnHeader.Objects.Add(CH("hFec", "Fecha", 3.6f, 2.5f));
            page.ColumnHeader.Objects.Add(CH("hCli", "Cliente", 6.2f, 5.5f));
            page.ColumnHeader.Objects.Add(CH("hLin", "Líneas", 11.8f, 1.5f));
            page.ColumnHeader.Objects.Add(CH("hSub", "Subtotal", 13.4f, 1.8f));
            page.ColumnHeader.Objects.Add(CH("hIva", "IVA", 15.3f, 1.5f));
            page.ColumnHeader.Objects.Add(CH("hTot", "Total", 16.9f, 1.7f));

            // ── BANDA: Datos ───────────────────────────────
            var dataBand = new DataBand();
            dataBand.Name = "Data1";
            dataBand.Height = Units.Centimeters * 0.7f;
            dataBand.DataSource = report.GetDataSource("DsVentas");
            page.Bands.Add(dataBand);

            TextObject CD(string nombre, string expr,
                float x, float w,
                HorzAlign ha = HorzAlign.Left)
            {
                var t = CrearTexto(nombre, expr, x, 0, w, 0.6f,
                    false, 8, ha);
                t.Border.Lines = BorderLines.All;
                return t;
            }

            dataBand.Objects.Add(CD("dNum",
                "[DsVentas.NumeroFactura]", 0, 3.5f));
            dataBand.Objects.Add(CD("dFec",
                "[DsVentas.FechaEmision]", 3.6f, 2.5f, HorzAlign.Center));
            dataBand.Objects.Add(CD("dCli",
                "[DsVentas.Cliente]", 6.2f, 5.5f));
            dataBand.Objects.Add(CD("dLin",
                "[DsVentas.CantLineas]", 11.8f, 1.5f, HorzAlign.Center));
            dataBand.Objects.Add(CD("dSub",
                "[DsVentas.Subtotal]", 13.4f, 1.8f, HorzAlign.Right));
            dataBand.Objects.Add(CD("dIva",
                "[DsVentas.IVA]", 15.3f, 1.5f, HorzAlign.Right));
            dataBand.Objects.Add(CD("dTot",
                "[DsVentas.Total]", 16.9f, 1.7f, HorzAlign.Right));

            // ── BANDA: Resumen (totales generales) ─────────
            // Usamos Total de FastReport para sumar columnas
            var totalGeneral = new Total();
            totalGeneral.Name = "SumaTotal";
            totalGeneral.TotalType = TotalType.Sum;
            totalGeneral.Evaluator = dataBand;
            totalGeneral.Expression = "[DsVentas.Total]";
            report.Dictionary.Totals.Add(totalGeneral);

            var totalCuenta = new Total();
            totalCuenta.Name = "CuentaFacturas";
            totalCuenta.TotalType = TotalType.Count;
            totalCuenta.Evaluator = dataBand;
            report.Dictionary.Totals.Add(totalCuenta);

            var summaryBand = new ReportSummaryBand();
            summaryBand.Name = "ReportSummary1";
            summaryBand.Height = Units.Centimeters * 1.5f;
            page.ReportSummary = summaryBand;

            summaryBand.Objects.Add(CrearTexto("lblTotGen",
                "TOTAL GENERAL DEL PERÍODO:",
                0, 0.2f, 13f, 0.7f, true, 10, HorzAlign.Right));

            summaryBand.Objects.Add(CrearTexto("valTotGen",
                "[SumaTotal]",
                13.1f, 0.2f, 5.5f, 0.7f, true, 10, HorzAlign.Right));

            summaryBand.Objects.Add(CrearTexto("cuentaFact",
                "[CuentaFacturas] factura(s) en el período",
                0, 1f, 18.59f, 0.5f, false, 8));

            // ── BANDA: Pie de página ───────────────────────
            page.PageFooter = new PageFooterBand();
            page.PageFooter.Name = "PageFooter1";
            page.PageFooter.Height = Units.Centimeters * 0.8f;

            page.PageFooter.Objects.Add(CrearTexto("nroPag",
                "Página [Page#] de [TotalPages#]",
                0, 0, 8f, 0.6f, false, 8));

            return report;
        }
    }
}
