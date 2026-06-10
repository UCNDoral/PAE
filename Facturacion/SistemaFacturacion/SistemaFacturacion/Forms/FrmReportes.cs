using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using FastReport;
using FastReport.Export.PdfSimple;
using SistemaFacturacion.DataAccess;
using SistemaFacturacion.Reports;


namespace SistemaFacturacion.Forms
{

    /// <summary>
    /// Formulario de reportes con FastReport.OpenSource.
    /// Los reportes se construyen por código (sin archivos .frx externos).
    /// El PDF se genera con FastReport.OpenSource.Export.PdfSimple.
    /// </summary>
    public partial class FrmReportes : Form
    {
        private readonly ReporteDA _reporteDA = new();
        private readonly FacturaDA _facturaDA = new();

        public FrmReportes()
        {
            InitializeComponent();

            CargarTiposReporte();
            CargarFacturas();
            dtpDesde.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpHastas.Value = DateTime.Now;
            ActualizarFiltros();
        }


        private void CargarTiposReporte()
        {
            cboTipoReporte.Items.Add("Factura Individual");
            cboTipoReporte.Items.Add("Ventas por Período");
            cboTipoReporte.SelectedIndex = 0;
        }

        private void CargarFacturas()
        {
            var facturas = _facturaDA.ObtenerTodas();
            cboFactura.DataSource = facturas;
            cboFactura.DisplayMember = "NumeroFactura";
            cboFactura.ValueMember = "FacturaId";
        }

        private void cboTipoReporte_SelectedIndexChanged(object s, EventArgs e)
            => ActualizarFiltros();

        private void ActualizarFiltros()
        {
            bool esFactura = cboTipoReporte.SelectedIndex == 0;
            lblFactura.Visible = cboFactura.Visible = esFactura;
            lblDesde.Visible = dtpDesde.Visible = !esFactura;
            lblHasta.Visible = lblHasta.Visible = !esFactura;
        }

        // ─────────────────────────────────────────
        // GENERAR REPORTE → exportar a PDF y abrir
        // ─────────────────────────────────────────
        private void btnGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboTipoReporte.SelectedIndex == 0)
                    ProcesarFactura();
                else
                    ProcesarVentas();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar reporte:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProcesarFactura()
        {
            if (cboFactura.SelectedValue == null)
            {
                MessageBox.Show("Seleccione una factura.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int facturaId = (int)cboFactura.SelectedValue;
            var datos = _reporteDA.ObtenerDatosFactura(facturaId);

            if (datos.Count == 0)
            {
                MessageBox.Show("La factura no tiene líneas de detalle.");
                return;
            }

            // Construir el reporte por código
            using var report = ReportBuilder.CrearReporteFactura(datos);
            ExportarYAbrir(report,
                $"Factura_{datos[0].NumeroFactura}.pdf");
        }

        private void ProcesarVentas()
        {
            if (dtpDesde.Value.Date > dtpHastas.Value.Date)
            {
                MessageBox.Show("La fecha inicial no puede ser mayor que la final.");
                return;
            }

            var datos = _reporteDA.ObtenerVentasPorPeriodo(
                dtpDesde.Value, dtpHastas.Value);

            if (datos.Count == 0)
            {
                MessageBox.Show("No hay facturas en el período seleccionado.");
                return;
            }

            using var report = ReportBuilder.CrearReporteVentas(
                datos,
                dtpDesde.Value.ToString("dd/MM/yyyy"),
                dtpHastas.Value.ToString("dd/MM/yyyy"));

            ExportarYAbrir(report,
                $"Ventas_{dtpDesde.Value:yyyyMMdd}_{dtpHastas.Value:yyyyMMdd}.pdf");
        }

        // ─────────────────────────────────────────
        // Exportar a PDF y abrir con el visor del SO
        // ─────────────────────────────────────────
        private void ExportarYAbrir(Report report, string nombreSugerido)
        {
            using var dlg = new SaveFileDialog
            {
                Title = "Guardar reporte como PDF",
                Filter = "Archivo PDF (*.pdf)|*.pdf",
                FileName = nombreSugerido
            };

            if (dlg.ShowDialog() != DialogResult.OK) return;

            // Preparar el reporte (procesar datos)
            report.Prepare();

            // Exportar a PDF con PdfSimpleExport
            using var pdfExport = new PDFSimpleExport();
            report.Export(pdfExport, dlg.FileName);

            MessageBox.Show("PDF generado correctamente.", "Éxito",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Abrir el PDF con el visor predeterminado del sistema operativo
            System.Diagnostics.Process.Start(
                new System.Diagnostics.ProcessStartInfo
                {
                    FileName = dlg.FileName,
                    UseShellExecute = true
                });
        }
    }
}
