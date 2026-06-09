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
    /// Formulario de gestión de clientes.
    /// Operaciones: Listar, Nuevo, Editar, Guardar, Eliminar, Cancelar.
    /// 
    /// CONTROLES NECESARIOS EN EL DESIGNER:
    ///   dgvClientes     → DataGridView (lista)
    ///   txtNombre       → TextBox
    ///   txtApellido     → TextBox
    ///   txtEmail        → TextBox
    ///   txtTelefono     → TextBox
    ///   txtDireccion    → TextBox
    ///   btnNuevo        → Button
    ///   btnGuardar      → Button
    ///   btnEliminar     → Button
    ///   btnCancelar     → Button
    ///   lblId           → Label (oculto, guarda el ID del registro seleccionado)
    /// </summary>
    public partial class FrmClientes : Form
    {
        private readonly ClienteDA _clienteDA = new();
        private bool _esNuevo = false; // true=insertar, false=actualizar

        public FrmClientes()
        {
            InitializeComponent();
            ConfigurarGrilla();
            CargarClientes();
            ModoVisualizacion(); // empieza en modo solo lectura
        }

        // ─────────────────────────────────────────
        // CONFIGURACIÓN DE LA GRILLA
        // ─────────────────────────────────────────
        private void ConfigurarGrilla()
        {
            dgvClientes.AutoGenerateColumns = false;
            dgvClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClientes.MultiSelect = false;
            dgvClientes.ReadOnly = true;

            // Definir columnas manualmente para controlar lo que se muestra
            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "ID",
                DataPropertyName = "ClienteId",
                Width = 50
            });
            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Nombre",
                DataPropertyName = "Nombre",
                Width = 120
            });
            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Apellido",
                DataPropertyName = "Apellido",
                Width = 120
            });
            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Email",
                DataPropertyName = "Email",
                Width = 180
            });
            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Teléfono",
                DataPropertyName = "Telefono",
                Width = 100
            });
        }


        // ─────────────────────────────────────────
        // CARGAR / REFRESCAR LA LISTA
        // ─────────────────────────────────────────
        private void CargarClientes()
        {
            try
            {
                var clientes = _clienteDA.ObtenerTodos();
                dgvClientes.DataSource = clientes;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar clientes:\n{ex.Message}", "Error",
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
                var cliente = new Cliente
                {
                    ClienteId = lblId.Tag != null ? (int)lblId.Tag : 0,
                    Nombre = txtNombre.Text.Trim(),
                    Apellido = txtApellido.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Telefono = txtTelefono.Text.Trim(),
                    Direccion = txtDireccion.Text.Trim()
                };

                bool exito;

                if (_esNuevo)
                    exito = _clienteDA.Insertar(cliente);
                else
                    exito = _clienteDA.Actualizar(cliente);

                if (exito)
                {
                    MessageBox.Show("Cliente guardado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarClientes();
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
            if (lblId.Tag == null) return;

            var confirmar = MessageBox.Show(
                "¿Está seguro de eliminar este cliente?\nEsta acción no se puede deshacer.",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmar != DialogResult.Yes) return;

            try
            {
                bool exito = _clienteDA.Eliminar((int)lblId.Tag);
                if (exito)
                {
                    MessageBox.Show("Cliente eliminado.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                    CargarClientes();
                    ModoVisualizacion();
                }
            }
            catch (Exception ex)
            {
                // Si el cliente tiene facturas, la BD lanzará error de FK
                MessageBox.Show($"No se puede eliminar:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            ModoVisualizacion();
        }

        private void dgvClientes_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvClientes.CurrentRow?.DataBoundItem is not Cliente cliente) return;

            // Guardar el ID en la propiedad Tag del label (invisible)
            lblId.Tag = cliente.ClienteId;

            txtNombre.Text = cliente.Nombre;
            txtApellido.Text = cliente.Apellido;
            txtEmail.Text = cliente.Email;
            txtTelefono.Text = cliente.Telefono;
            txtDireccion.Text = cliente.Direccion;

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
            txtApellido.Enabled = true;
            txtEmail.Enabled = true;
            txtTelefono.Enabled = true;
            txtDireccion.Enabled = true;
            btnGuardar.Enabled = true;
            btnCancelar.Enabled = true;
            btnEliminar.Enabled = !_esNuevo; // solo si estamos editando
        }

        /// <summary>Deshabilita los campos (solo lectura).</summary>
        private void ModoVisualizacion()
        {
            txtNombre.Enabled = false;
            txtApellido.Enabled = false;
            txtEmail.Enabled = false;
            txtTelefono.Enabled = false;
            txtDireccion.Enabled = false;
            btnGuardar.Enabled = false;
            btnCancelar.Enabled = false;
            btnEliminar.Enabled = false;
        }

        private void LimpiarCampos()
        {
            lblId.Tag = null;
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtEmail.Text = "";
            txtTelefono.Text = "";
            txtDireccion.Text = "";
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
            if (string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                MessageBox.Show("El apellido es obligatorio.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApellido.Focus();
                return false;
            }
            return true;
        }
    }
}
