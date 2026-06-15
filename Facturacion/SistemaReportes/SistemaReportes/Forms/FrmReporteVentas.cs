using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SistemaReportes.Data;
using SistemaReportes.Exportadores;
using SistemaReportes.Model;

namespace SistemaReportes.Forms
{
    public partial class FrmReporteVentas : Form
    {
        // Guardamos los datos para exportarlos sin repetir la consulta
        private List<VentaReporte> _datos = new();
        private readonly ReporteRepository _repo = new();

        public FrmReporteVentas()
        {
            InitializeComponent();
            ConfigurarGrilla();
        }

        // Configura columnas, colores y formato del DataGridView
        private void ConfigurarGrilla()
        {
            dgvReporte.AutoGenerateColumns = false;
            dgvReporte.AllowUserToAddRows = false;
            dgvReporte.ReadOnly = true;
            dgvReporte.AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(235, 243, 251);
            dgvReporte.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(46, 117, 182);
            dgvReporte.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvReporte.EnableHeadersVisualStyles = false;

            dgvReporte.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { HeaderText="ID",       DataPropertyName="IdVenta",    Width=50  },
                new DataGridViewTextBoxColumn { HeaderText="Cliente",  DataPropertyName="Cliente",    Width=160 },
                new DataGridViewTextBoxColumn { HeaderText="Producto", DataPropertyName="Producto",   Width=160 },
                new DataGridViewTextBoxColumn { HeaderText="Cantidad", DataPropertyName="Cantidad",   Width=70  },
                new DataGridViewTextBoxColumn {
                    HeaderText="Precio Unit.", DataPropertyName="PrecioUnit", Width=100,
                    DefaultCellStyle = new DataGridViewCellStyle {
                        Format="C2", Alignment=DataGridViewContentAlignment.MiddleRight }
                },
                new DataGridViewTextBoxColumn {
                    HeaderText="Total", DataPropertyName="Total", Width=100,
                    DefaultCellStyle = new DataGridViewCellStyle {
                        Format="C2", Alignment=DataGridViewContentAlignment.MiddleRight }
                },
                new DataGridViewTextBoxColumn {
                    HeaderText="Fecha", DataPropertyName="FechaVenta", Width=90,
                    DefaultCellStyle = new DataGridViewCellStyle { Format="dd/MM/yyyy" }
                },
            });
        }

        // Botón Generar Reporte
        private void btnGenerar_Click(object sender, EventArgs e)
        {
            if (dtpInicio.Value.Date > dtpFin.Value.Date)
            {
                MessageBox.Show("La fecha de inicio no puede ser mayor a la final.",
                    "Fechas inválidas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;
                _datos = _repo.ObtenerVentasPorFecha(
                    dtpInicio.Value.Date,
                    dtpFin.Value.Date.AddHours(23).AddMinutes(59));

                dgvReporte.DataSource = _datos;

                decimal total = _datos.Sum(v => v.Total);
                lblTotal.Text = $"Total: C${total:N2}  |  Registros: {_datos.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            finally
            {
                Cursor = Cursors.Default;
            }
        }

        // Botón Exportar a Excel
        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            if (_datos.Count == 0) { MensajeSinDatos(); return; }

            using var dlg = new SaveFileDialog
            {
                Title = "Guardar como Excel",
                Filter = "Archivo Excel (*.xlsx)|*.xlsx",
                FileName = $"Reporte_{DateTime.Now:yyyyMMdd_HHmm}.xlsx"
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ExportadorExcel.ExportarVentas(_datos, dlg.FileName);
                    AbrirArchivo(dlg.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al exportar: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Botón Exportar a PDF
        private void btnExportarPDF_Click(object sender, EventArgs e)
        {
            if (_datos.Count == 0) { MensajeSinDatos(); return; }

            using var dlg = new SaveFileDialog
            {
                Title = "Guardar como PDF",
                Filter = "Archivo PDF (*.pdf)|*.pdf",
                FileName = $"Reporte_{DateTime.Now:yyyyMMdd_HHmm}.pdf"
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ExportadorPDF.ExportarVentas(_datos, dlg.FileName);
                    AbrirArchivo(dlg.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al exportar: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Abre el archivo con la aplicación predeterminada del sistema
        private static void AbrirArchivo(string ruta)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = ruta,
                UseShellExecute = true
            });
        }


        private static void MensajeSinDatos() =>
            MessageBox.Show("Primero genera el reporte.", "Sin datos",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
    



}
}
