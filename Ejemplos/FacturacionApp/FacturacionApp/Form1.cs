using FacturacionApp.Repositories;

namespace FacturacionApp
{
    public partial class Form1 : Form
    {
        private readonly ProductoRepository _repo = new ProductoRepository();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            ConfigurarDataGridView();
            CargarProductos();
        }

        private void ConfigurarDataGridView()
        {
            dgvProducto.AutoGenerateColumns = false;
            dgvProducto.Columns.AddRange();




            dgvProducto.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "ID",
                DataPropertyName = "ProductoId",
                Width = 50,
            });
            dgvProducto.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Nombre",
                DataPropertyName = "Nombre",
                Width = 200,
            });

            dgvProducto.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Precio",
                DataPropertyName = "Precio",
                Width = 100,
            });

            dgvProducto.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Stock",
                DataPropertyName = "Stock",
                Width = 80,
            });

            dgvProducto.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Activo",
                DataPropertyName = "Activo",
                Width = 30,
            });
        }

        private void CargarProductos()
        {
            var lista = _repo.ObtenerTodos();
            dgvProducto.DataSource = new BindingSource { DataSource = lista };
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            var Producto = new Model.Producto
            {
                Nombre = txtNombre.Text,
                Precio = decimal.Parse(txtPrecio.Text),
                Stock = int.Parse(txtStock.Text),
            };

            _repo.Agregar(Producto);
            CargarProductos();
        }

    }
}
