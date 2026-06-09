using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaNegocio;

namespace CapaPresentacion
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MostrarProductos();
        }


        private void MostrarProductos()
        {
            CNProductos objetoCN = new CNProductos();
            dgvProductos.DataSource = objetoCN.MostrarProducto();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            CNProductos objetoCN = new CNProductos();
            try
            {
                objetoCN.InsertarProducto(txtProducto.Text, txtDescripcion.Text, txtMarca.Text, float.Parse(txtPrecio.Text), int.Parse(txtStock.Text));
                MessageBox.Show("Producto registrado correctamente");
                MostrarProductos();
            }
            catch (Exception ex) 
            { 
                MessageBox.Show($"No se pudo insertar el producto: {ex} ");

            }


        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(255, 192, 255);
        }
    }
}
