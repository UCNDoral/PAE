using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SistemaFacturacion.DataAccess;
using SistemaFacturacion.Models;

namespace SistemaFacturacion.Forms
{
    /// <summary>
    /// Formulario de facturación (maestro-detalle).
    /// 
    /// CONTROLES REQUERIDOS:
    ///   [Encabezado]
    ///   lblNumero        → Label (muestra el número autogenerado)
    ///   cboCliente       → ComboBox (lista de clientes)
    ///   dtpFecha         → DateTimePicker
    ///
    ///   [Agregar línea]
    ///   cboProducto      → ComboBox
    ///   nudCantidad      → NumericUpDown (mínimo 1)
    ///   lblPrecio        → Label (se llena al seleccionar producto)
    ///   btnAgregarLinea  → Button "Agregar"
    ///
    ///   [Detalle / grilla]
    ///   dgvDetalle       → DataGridView
    ///   btnQuitarLinea   → Button "Quitar"
    ///
    ///   [Totales]
    ///   lblSubtotal, lblIVA, lblTotal → Labels
    ///
    ///   [Acciones]
    ///   btnGuardar, btnNueva, btnAnular → Buttons
    ///
    ///   [Lista de facturas]
    ///   dgvFacturas      → DataGridView
    /// </summary>
    public partial class FrmFactura : Form
    {
        private readonly FacturaDA _facturaDA = new();
        private readonly ClienteDA _clienteDA = new();
        private readonly ProductoDA _productoDA = new();

        // Lista temporal de detalles que el usuario va armando
        private List<DetalleFactura> _detalles = new();

        private const decimal PORCENTAJE_IVA = 0.15m; // 15%
        public FrmFactura()
        {
            InitializeComponent();
            ConfigurarGrillas();
            CargarDatosIniciales();
            PrepararNuevaFactura();
        }

        // ─────────────────────────────────────────
        // CONFIGURACIÓN
        // ─────────────────────────────────────────
        private void ConfigurarGrillas()
        {
            // Grilla de DETALLE (líneas de la factura actual)
            dgvDetalle.AutoGenerateColumns = false;
            dgvDetalle.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Código", DataPropertyName = "CodigoProducto", Width = 80 });
            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Producto", DataPropertyName = "NombreProducto", Width = 200 });
            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Cantidad", DataPropertyName = "Cantidad", Width = 70 });
            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "P.Unitario", DataPropertyName = "PrecioUnitario", Width = 100, DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" } });
            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Subtotal", DataPropertyName = "Subtotal", Width = 100, DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" } });

            // Grilla de FACTURAS guardadas
            dgvFacturas.AutoGenerateColumns = false;
            dgvFacturas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvFacturas.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Número", DataPropertyName = "NumeroFactura", Width = 100 });
            dgvFacturas.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Cliente", DataPropertyName = "NombreCliente", Width = 150 });
            dgvFacturas.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Fecha", DataPropertyName = "FechaEmision", Width = 120, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" } });
            dgvFacturas.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Total", DataPropertyName = "Total", Width = 100, DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" } });
            dgvFacturas.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Estado", DataPropertyName = "Estado", Width = 80 });
        }

        private void CargarDatosIniciales()
        {
            // Cargar clientes en ComboBox
            var clientes = _clienteDA.ObtenerTodos();
            cboCliente.DataSource = clientes;
            cboCliente.DisplayMember = "NombreCompleto"; // propiedad calculada del modelo
            cboCliente.ValueMember = "ClienteId";

            // Cargar productos activos en ComboBox
            var productos = _productoDA.ObtenerActivos();
            cboProducto.DataSource = productos;
            cboProducto.DisplayMember = "Nombre";
            cboProducto.ValueMember = "ProductoId";

            // Cargar lista de facturas existentes
            CargarFacturas();
        }

        private void CargarFacturas()
        {
            dgvFacturas.DataSource = _facturaDA.ObtenerTodas();
        }

        // ─────────────────────────────────────────
        // NUEVA FACTURA
        // ─────────────────────────────────────────
        private void PrepararNuevaFactura()
        {
            lblNumero.Text = _facturaDA.GenerarNumeroFactura();
            dtpFecha.Value = DateTime.Now;
            _detalles.Clear();
            RefrescarDetalle();
            nudCantidad.Value = 1;
            ActualizarTotales();
        }

        private void btnNueva_Click(object sender, EventArgs e)
        {
            PrepararNuevaFactura();
        }

        // ─────────────────────────────────────────
        // SELECCIONAR PRODUCTO → mostrar precio
        // ─────────────────────────────────────────
        private void cboProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProducto.SelectedItem is Producto p)
            {
                lblPrecio.Text = p.PrecioUnitario.ToString("C2");
            }
        }

        private void btnAgregarLinea_Click(object sender, EventArgs e)
        {
            if (cboProducto.SelectedItem is not Producto productoSeleccionado) return;

            int cantidad = (int)nudCantidad.Value;

            // Si el producto ya está en la lista, sumar cantidad
            var existente = _detalles.FirstOrDefault(d => d.ProductoId == productoSeleccionado.ProductoId);

            if (existente != null)
            {
                existente.Cantidad += cantidad;
            }
            else
            {
                _detalles.Add(new DetalleFactura
                {
                    ProductoId = productoSeleccionado.ProductoId,
                    Cantidad = cantidad,
                    PrecioUnitario = productoSeleccionado.PrecioUnitario,
                    Producto = productoSeleccionado
                });
            }

            RefrescarDetalle();
            ActualizarTotales();
        }

        // ─────────────────────────────────────────
        // QUITAR LÍNEA DEL DETALLE
        // ─────────────────────────────────────────
        private void btnQuitarLinea_Click(object sender, EventArgs e)
        {
            if (dgvDetalle.CurrentRow == null) return;
            int index = dgvDetalle.CurrentRow.Index;
            if (index >= 0 && index < _detalles.Count)
            {
                _detalles.RemoveAt(index);
                RefrescarDetalle();
                ActualizarTotales();
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (cboCliente.SelectedValue == null)
            {
                MessageBox.Show("Seleccione un cliente.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_detalles.Count == 0)
            {
                MessageBox.Show("Agregue al menos un producto a la factura.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                decimal subtotal = _detalles.Sum(d => d.Subtotal);
                decimal iva = subtotal * PORCENTAJE_IVA;
                decimal total = subtotal + iva;

                var factura = new Factura
                {
                    NumeroFactura = lblNumero.Text,
                    ClienteId = (int)cboCliente.SelectedValue!,
                    FechaEmision = dtpFecha.Value,
                    Subtotal = subtotal,
                    IVA = iva,
                    Total = total,
                    Estado = "ACTIVA",
                    Detalles = _detalles
                };

                bool exito = _facturaDA.Guardar(factura);

                if (exito)
                {
                    MessageBox.Show($"Factura {factura.NumeroFactura} guardada correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarFacturas();
                    PrepararNuevaFactura();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar factura:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAnular_Click(object sender, EventArgs e)
        {
            if (dgvFacturas.CurrentRow?.DataBoundItem is not Factura f) return;

            if (f.Estado == "ANULADA")
            {
                MessageBox.Show("Esta factura ya está anulada.", "Información",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmar = MessageBox.Show(
                $"¿Anular la factura {f.NumeroFactura}?",
                "Confirmar anulación",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmar == DialogResult.Yes)
            {
                _facturaDA.Anular(f.FacturaId);
                CargarFacturas();
            }
        }

        // ─────────────────────────────────────────
        // HELPERS
        // ─────────────────────────────────────────

        /// <summary>Crea una vista plana de los detalles para la grilla.</summary>
        private void RefrescarDetalle()
        {
            // DataGridView necesita propiedades simples; creamos una proyección anónima
            dgvDetalle.DataSource = _detalles.Select(d => new
            {
                CodigoProducto = d.Producto?.Codigo ?? "",
                NombreProducto = d.Producto?.Nombre ?? "",
                d.Cantidad,
                d.PrecioUnitario,
                d.Subtotal
            }).ToList();
        }

        private void ActualizarTotales()
        {
            decimal subtotal = _detalles.Sum(d => d.Subtotal);
            decimal iva = subtotal * PORCENTAJE_IVA;
            decimal total = subtotal + iva;

            lblSubtotal.Text = subtotal.ToString("C2");
            lblIVA.Text = iva.ToString("C2");
            lblTotal.Text = total.ToString("C2");
        }
    }
}
