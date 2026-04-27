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
    public partial class FrmProductos : Form
    {
        //Instancia única para evitar crear objetos innecesarios
        private readonly CNProductos negocio = new CNProductos();
        private int idSeleccionado = 0;
        private bool editando = false;


        public FrmProductos()
        {
            InitializeComponent();
        }

        private void FrmProductos_Load(object sender, EventArgs e)
        {
            MostrarProductos();
            ConfigurarGrid();
        }

        private void MostrarProductos()
        {
            try
            {
                dgvProductos.DataSource = negocio.MostrarProducto();
                // Formatear columna de precio si existe
                if (dgvProductos.Columns.Contains("Precio"))
                    dgvProductos.Columns["Precio"].DefaultCellStyle.Format = "C2";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarGrid()
        {
            if (dgvProductos.Columns.Contains("Precio"))
                dgvProductos.Columns["Precio"].DefaultCellStyle.Format = "C2";
            foreach (DataGridViewColumn col in dgvProductos.Columns)
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvProductos.Rows[e.RowIndex];
                idSeleccionado = Convert.ToInt32(row.Cells["IdProducto"].Value);
                txtId.Text = row.Cells["IdProducto"].Value.ToString();
                txtNombre.Text = row.Cells["Nombre"].Value.ToString();
                txtPrecio.Text = row.Cells["Precio"].Value.ToString();
                txtStock.Text = row.Cells["Stock"].Value.ToString();
                editando = true;
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }


        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtPrecio.Text, out decimal precio) || precio <= 0)
            { MessageBox.Show("Ingrese un precio válido mayor a 0.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
            { MessageBox.Show("Ingrese un stock válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            try
            {
                if (editando)
                    negocio.ActualizarProducto(idSeleccionado, txtNombre.Text, precio, stock);
                else
                    negocio.InsertarProducto(txtNombre.Text, precio, stock);

                MessageBox.Show(editando ? "Actualizado." : "Registrado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                MostrarProductos();
            }
            catch (Exception ex)
            { MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == 0) { MessageBox.Show("Seleccione un producto.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (MessageBox.Show("¿Eliminar este producto?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    negocio.EliminarProducto(idSeleccionado);
                    MessageBox.Show("Producto eliminado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                    MostrarProductos();
                }
                catch (Exception ex) { MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            txtId.Clear(); txtNombre.Clear(); txtPrecio.Clear(); txtStock.Clear();
            idSeleccionado = 0; editando = false; txtNombre.Focus();
        }
    }
}
