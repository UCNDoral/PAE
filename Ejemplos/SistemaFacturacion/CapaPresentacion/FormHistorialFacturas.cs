using CapaNegocios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class FormHistorialFacturas : Form
    {
        private readonly CN_HistorialFactura cnHistorial = new CN_HistorialFactura();
        public FormHistorialFacturas()
        {
            InitializeComponent();
            CargarHistorial();
            ConfigurarFormatos();
        }

        private void CargarHistorial()
        {
            try
            {
                dgvHistorial.DataSource = cnHistorial.ListarHistorial();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar historial: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarFormatos()
        {
            //Formato moneda y fecha
            if (dgvHistorial.Columns.Contains("Total"))
                dgvHistorial.Columns["Total"].DefaultCellStyle.Format = "C2";
            if (dgvHistorial.Columns.Contains("Fecha"))
                dgvHistorial.Columns["Fecha"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";

            if (dgvDetalles.Columns.Contains("Precio"))
                dgvDetalles.Columns["Precio"].DefaultCellStyle.Format = "C2";
            if (dgvDetalles.Columns.Contains("Subtotal"))
                dgvDetalles.Columns["Subtotal"].DefaultCellStyle.Format = "C2";
        }

        private void dgvHistorial_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Cargar detalles solo si se hace clic en una fila válida
            if (e.RowIndex >= 0)
            {
                var celdaId = dgvHistorial.Rows[e.RowIndex].Cells["IdFactura"].Value;
                if (celdaId != null && int.TryParse(celdaId.ToString(), out int idFactura))
                {
                    try
                    {
                        dgvDetalles.DataSource = cnHistorial.ObtenerDetalles(idFactura);
                        ConfigurarFormatos(); // Reaplicar formatos al cambiar datasource
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al cargar detalles: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
