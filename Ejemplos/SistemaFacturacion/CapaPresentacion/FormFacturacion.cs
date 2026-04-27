using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using CapaNegocios;
using CapaDatos;

namespace CapaPresentacion
{
    public partial class FormFacturacion : Form
    {
        private readonly CN_Factura cnFactura = new CN_Factura();
        private List<DetalleFacturaItem> carrito = new List<DetalleFacturaItem>();

        public FormFacturacion()
        {
            InitializeComponent();
            CargarCombos();
        }

        // Cargar clientes y productos activos en los ComboBox
        private void CargarCombos()
        {
            // Clientes
            using (var conn = new CD_Conexion().ObtenerConexion())
            using (var cmd = new SqlCommand("SELECT IdCliente, Nombre + ' ' + Apellido AS NombreCompleto FROM Clientes WHERE Activo = 1", conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    var dt = new DataTable();
                    dt.Load(reader);
                    cmbClientes.DataSource = dt;
                    cmbClientes.DisplayMember = "NombreCompleto";
                    cmbClientes.ValueMember = "IdCliente";
                }
            }

            // Productos
            using (var conn = new CD_Conexion().ObtenerConexion())
            using (var cmd = new SqlCommand("SELECT IdProducto, Nombre, Precio, Stock FROM Productos WHERE Activo = 1 AND Stock > 0", conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    var dt = new DataTable();
                    dt.Load(reader);
                    cmbProductos.DataSource = dt;
                    cmbProductos.DisplayMember = "Nombre";
                    cmbProductos.ValueMember = "IdProducto";
                }
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (cmbProductos.SelectedValue == null)
            {
                MessageBox.Show("Seleccione un producto.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idProd = Convert.ToInt32(cmbProductos.SelectedValue);
            int cantidad = (int)nudCantidad.Value;

            // Obtener precio y stock del DataRow seleccionado
            var filaSeleccionada = ((DataRowView)cmbProductos.SelectedItem).Row;
            decimal precio = Convert.ToDecimal(filaSeleccionada["Precio"]);
            int stockDisponible = Convert.ToInt32(filaSeleccionada["Stock"]);

            if (cantidad > stockDisponible)
            {
                MessageBox.Show($"Stock insuficiente. Disponible: {stockDisponible}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Agregar al carrito temporal
            carrito.Add(new DetalleFacturaItem
            {
                IdProducto = idProd,
                NombreProducto = cmbProductos.Text,
                Cantidad = cantidad,
                Precio = precio
            });

            ActualizarGrid();
            nudCantidad.Value = 1; // Resetear cantidad
        }


        private void ActualizarGrid()
        {
            dgvDetalles.DataSource = null;
            dgvDetalles.DataSource = carrito;

            // Formatear columnas de moneda
            if (dgvDetalles.Columns.Contains("Precio")) dgvDetalles.Columns["Precio"].DefaultCellStyle.Format = "C2";
            if (dgvDetalles.Columns.Contains("Subtotal")) dgvDetalles.Columns["Subtotal"].DefaultCellStyle.Format = "C2";

            // Calcular total general
            decimal total = carrito.Sum(d => d.Subtotal);
            lblTotal.Text = $"Total: {total:C2}";
        }

        private void btnFacturar_Click(object sender, EventArgs e)
        {
            if (cmbClientes.SelectedValue == null)
            {
                MessageBox.Show("Seleccione un cliente.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (carrito.Count == 0)
            {
                MessageBox.Show("Agregue al menos un producto a la factura.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int idFactura = cnFactura.RegistrarFactura(
                    Convert.ToInt32(cmbClientes.SelectedValue),
                    DateTime.Now,
                    carrito
                );

                MessageBox.Show($"Factura #{idFactura} generada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                carrito.Clear();
                ActualizarGrid();
                CargarCombos(); // Refrescar ComboBox de productos (stock actualizado)
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al facturar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            carrito.Clear();
            ActualizarGrid();
        }

        private void FormFacturacion_Load(object sender, EventArgs e)
        {

        }
    }
}
