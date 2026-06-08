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
    public partial class FrmProductos : Form
    {
        private readonly ProductoDA _productoDA = new();
        private bool _esNuevo = false; // true=insertar, false=actualizar
        public FrmProductos()
        {
            InitializeComponent();
            ConfigurarGrilla();
            CargarProductos();
            //ModoVisualizacion();
        }

        // ─────────────────────────────────────────
        // CONFIGURACIÓN DE LA GRILLA
        // ─────────────────────────────────────────
        private void ConfigurarGrilla()
        {
            dgvProductos.AutoGenerateColumns = false;
            dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductos.MultiSelect = false;
            dgvProductos.ReadOnly = true;

            // Definir columnas manualmente para controlar lo que se muestra
            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "ID",
                DataPropertyName = "ProductoId",
                Width = 50
            });
            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Nombre",
                DataPropertyName = "Nombre",
                Width = 120
            });
            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Descripción",
                DataPropertyName = "Descripcion",
                Width = 180
            });
            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Precio Unitario",
                DataPropertyName = "PrecioUnitario",
                Width = 100
            });
            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Stock",
                DataPropertyName = "Stock",
                Width = 80
            });

        }

        // ─────────────────────────────────────────
        // CARGAR / REFRESCAR LA LISTA
        // ─────────────────────────────────────────
        private void CargarProductos()
        {
            try
            {
                var productos = _productoDA.ObtenerTodos();
                dgvProductos.DataSource = productos;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            _esNuevo = true;
            LimpiarCampos();
            ModoEdicion();
            txtNombre.Focus();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            try
            {
                var prodcuto = new Producto
                {
                    ProductoId = lblCodigo.Tag != null ? (int)lblCodigo.Tag : 0,
                    Nombre = txtNombre.Text.Trim(),
                    Descripcion = txtDescripcion.Text.Trim(),
                    PrecioUnitario = decimal.Parse(txtPrecioUnitario.Text.Trim()),
                    Stock = int.Parse(txtStock.Text.Trim())
                };


                bool exito;

                if (_esNuevo)
                    exito = _productoDA.Insertar(prodcuto);
                else
                    exito = _productoDA.Actualizar(prodcuto);

                if (exito)
                {
                    MessageBox.Show("Producto guardado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarProductos();
                    ModoVisualizacion();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (lblCodigo.Tag == null) return;

            var confirmar = MessageBox.Show(
                "¿Está seguro de eliminar este Producto?\nEsta acción no se puede deshacer.",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmar != DialogResult.Yes) return;

            try
            {
                bool exito = _productoDA.Eliminar((int)lblCodigo.Tag);
                if (exito)
                {
                    MessageBox.Show("Producto eliminado.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                    CargarProductos();
                    ModoVisualizacion();
                }
            }
            catch (Exception ex)
            {
                // Si el producto tiene facturas, la BD lanzará error de FK
                MessageBox.Show($"No se puede eliminar:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            ModoVisualizacion();
        }

        private void dgvProductos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProductos.CurrentRow?.DataBoundItem is not Producto producto) return;

            // Guardar el ID en la propiedad Tag del label (invisible)
            lblCodigo.Tag = producto.ProductoId;

            txtNombre.Text = producto.Nombre;
            txtDescripcion.Text = producto.Descripcion;
            txtPrecioUnitario.Text = producto.PrecioUnitario.ToString("F2");
            txtStock.Text = producto.Stock.ToString();

            _esNuevo = false;
            ModoEdicion(); // permite editar al seleccionar
        }

        // ─────────────────────────────────────────
        // HELPERS: Modos de UI y validación
        // ─────────────────────────────────────────

        /// <summary>Habilita los campos para editar.</summary>
        private void ModoEdicion()
        {
            txtNombre.Enabled = true;
            txtDescripcion.Enabled = true;
            txtPrecioUnitario.Enabled = true;
            txtStock.Enabled = true;
            btnGuardar.Enabled = true;
            btnCancelar.Enabled = true;
            btnEliminar.Enabled = !_esNuevo; // solo si estamos editando
        }

        /// <summary>Deshabilita los campos (solo lectura).</summary>
        private void ModoVisualizacion()
        {
            txtNombre.Enabled = false;
            txtDescripcion.Enabled = false;
            txtPrecioUnitario.Enabled = false;
            txtStock.Enabled = false;
            btnGuardar.Enabled = false;
            btnCancelar.Enabled = false;
            btnEliminar.Enabled = false;
        }   
           
      

        private void LimpiarCampos()
        {
            lblCodigo.Tag = null;
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            txtPrecioUnitario.Text = "";
            txtStock.Text = "";
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtPrecioUnitario.Text))
            {
                MessageBox.Show("El precio unitario es obligatorio.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecioUnitario.Focus();
                return false;
            }
            return true;
        }



    }
}
