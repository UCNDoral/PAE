using System.Data;
using SistemaFacturacion.DataAccess;
using SistemaFacturacion.Services;

namespace SistemaFacturacion.Forms
{
    /// <summary>
    /// Pantalla de reportes del sistema.
    /// Permite consultar informacion consolidada por rango de fechas y exportarla
    /// a PDF, Excel o CSV segun el formato elegido por el usuario.
    /// </summary>
    public partial class FrmReportes : Form
    {
        private readonly ReporteDA _reporteDA = new();

        // Guarda el ultimo reporte consultado. Se usa tanto para mostrar la grilla
        // como para exportar exactamente los mismos datos que el usuario esta viendo.
        private DataTable _reporteActual = new();

        public FrmReportes()
        {
            InitializeComponent();
            ConfigurarGrilla();
            ConfigurarFiltros();
            // Cargar al abrir pero sin bloquear si hay error
            try
            {
                CargarReporte();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudieron cargar los datos iniciales:\n{ex.Message}",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ConfigurarFiltros()
        {
            // Los indices de esta lista coinciden con el switch de CargarReporte.
            // Si se agrega un nuevo reporte, debe agregarse aqui y en el switch.
            cboTipoReporte.Items.AddRange(new object[]
            {
                "Ventas por fecha",
                "Detalle de facturas",
                "Productos mas vendidos",
                "Clientes con mayor compra"
            });
            cboTipoReporte.SelectedIndex = 0;

            // Por defecto se consulta el mes actual, que suele ser el rango mas usado
            // para revision diaria de ventas.
            dtpDesde.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dtpHasta.Value = DateTime.Today;
        }

        private void ConfigurarGrilla()
        {
            dgvReporte.AutoGenerateColumns = true;
            dgvReporte.ReadOnly = true;
            dgvReporte.AllowUserToAddRows = false;
            dgvReporte.AllowUserToDeleteRows = false;
            dgvReporte.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReporte.MultiSelect = false;
            dgvReporte.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            CargarReporte();
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            if (_reporteActual.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para exportar.", "Reportes",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var dialog = new SaveFileDialog
            {
                Filter = "Archivo PDF (*.pdf)|*.pdf|Archivo Excel (*.xlsx)|*.xlsx|Archivo CSV (*.csv)|*.csv",
                FileName = $"Reporte_{DateTime.Now:yyyyMMdd_HHmm}.pdf",
                Title = "Exportar reporte"
            };

            if (dialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                var titulo = cboTipoReporte.Text;
                var rangoFechas = $"Desde {dtpDesde.Value:dd/MM/yyyy} hasta {dtpHasta.Value:dd/MM/yyyy}";

                ReporteExportador.Exportar(_reporteActual, dialog.FileName, titulo, rangoFechas);
                MessageBox.Show("Reporte exportado correctamente.", "Reportes",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar el reporte:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarReporte()
        {
            if (dtpDesde.Value.Date > dtpHasta.Value.Date)
            {
                MessageBox.Show("La fecha inicial no puede ser mayor que la fecha final.", "Validacion",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var desde = dtpDesde.Value.Date;
                var hasta = dtpHasta.Value.Date;

                // Cada opcion del combo ejecuta una consulta especializada.
                // Todas usan el mismo rango de fechas para mantener consistencia.
                _reporteActual = cboTipoReporte.SelectedIndex switch
                {
                    0 => _reporteDA.ObtenerVentasPorFecha(desde, hasta),
                    1 => _reporteDA.ObtenerFacturas(desde, hasta),
                    2 => _reporteDA.ObtenerProductosMasVendidos(desde, hasta),
                    3 => _reporteDA.ObtenerClientesConMayorCompra(desde, hasta),
                    _ => _reporteDA.ObtenerVentasPorFecha(desde, hasta)
                };

                dgvReporte.DataSource = _reporteActual;
                AplicarFormatoColumnas();
                CargarResumen(desde, hasta);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el reporte:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarResumen(DateTime desde, DateTime hasta)
        {
            // El resumen siempre se calcula sobre facturas del mismo rango,
            // independientemente del tipo de reporte elegido en la grilla.
            var resumen = _reporteDA.ObtenerResumen(desde, hasta);
            if (resumen.Rows.Count == 0) return;

            var fila = resumen.Rows[0];
            lblFacturasActivas.Text = Convert.ToInt32(fila["FacturasActivas"]).ToString();
            lblFacturasAnuladas.Text = Convert.ToInt32(fila["FacturasAnuladas"]).ToString();
            lblSubtotal.Text = Convert.ToDecimal(fila["Subtotal"]).ToString("C2");
            lblIVA.Text = Convert.ToDecimal(fila["IVA"]).ToString("C2");
            lblTotal.Text = Convert.ToDecimal(fila["Total"]).ToString("C2");
        }

        private void AplicarFormatoColumnas()
        {
            // El DataGridView genera columnas automaticamente desde el DataTable.
            // Despues de enlazar los datos, ajustamos visualmente montos y fechas.
            foreach (DataGridViewColumn columna in dgvReporte.Columns)
            {
                if (columna.ValueType == typeof(decimal) || columna.Name.Contains("Total") ||
                    columna.Name.Contains("Subtotal") || columna.Name == "IVA")
                {
                    columna.DefaultCellStyle.Format = "C2";
                    columna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                if (columna.ValueType == typeof(DateTime) || columna.Name == "Fecha")
                {
                    columna.DefaultCellStyle.Format = "dd/MM/yyyy";
                }
            }
        }
    }
}
