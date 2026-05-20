using FacturacionApp2.Data;
using FacturacionApp2.Models;
using FacturacionApp2.Repositories;

namespace FacturacionApp2
{
    public partial class Form1 : Form
    {

        //instancia del repositorio
        private readonly ProductoRespository _repo = new ProductoRespository();


        //Almacenar el ID del registro seleccionado para editar
        private int _idSeleccionado = 0;


        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            CargarProductos();
        }

        private void CargarProductos()
        {
            var productos = _repo.ObtenerProductos();
            dgvProductos.DataSource = productos;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            var texto = txtBuscar.Text.Trim();

            if (string.IsNullOrEmpty(texto))
            {
                CargarProductos();
            }
            else
            {
                var resultados = _repo.ObtenerPorId(Convert.ToInt32(texto));
                dgvProductos.DataSource = new BindingSource { DataSource = resultados };
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();


            }


            //Construir el objeto producto a partir de los datos del formulario

            var Producto = new Producto
            {
                Nombre = txtNombre.Text,
                Precio = Convert.ToDecimal(txtPrecio.Text),
                Stock = Convert.ToInt32(txtStock.Text)
            };


            if (_idSeleccionado == 0)
            {
                //CREATE
                _repo.Agregar(Producto);
                MessageBox.Show("Producto Agregado Correctamente", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpisrFormulario();


            }
            else
            {
                //UPDATE
                Producto.IdProducto = _idSeleccionado;
                _repo.Actualizar(Producto);
                MessageBox.Show("Producto Actualizado Correctamente", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpisrFormulario();
            }

            CargarProductos();


        }

        public void LimpisrFormulario()
        {
            txtNombre.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
            _idSeleccionado = 0;
        }

        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; //Ignorar clicks 

            var fila = dgvProductos.Rows[e.RowIndex];

            //guardar el ID para saber que vamos a editar
            _idSeleccionado = Convert.ToInt32(fila.Cells["IdProducto"].Value);

            //MessageBox.Show(Convert.ToString(_idSeleccionado));


            //llenar los campos del formulario con los datos de la fila seleccionada
            txtNombre.Text = Convert.ToString(fila.Cells["Nombre"].Value?.ToString());
            txtPrecio.Text = Convert.ToString(fila.Cells["Precio"].Value?.ToString());
            txtStock.Text = Convert.ToString(fila.Cells["Stock"].Value?.ToString());


        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {

            if (_idSeleccionado == 0) return;

            var confirm = MessageBox.Show("¿Estas seguro de eliminar este producto?", "Confirmar Eliminacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);


            if (confirm == DialogResult.Yes)
            {
                _repo.Eliminar(_idSeleccionado);
                MessageBox.Show("Producto Eliminado Correctamente", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpisrFormulario();
                CargarProductos();
            }
        }
    }
}
