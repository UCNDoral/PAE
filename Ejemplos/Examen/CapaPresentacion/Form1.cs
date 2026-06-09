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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new frmClientes();
       
            frm.Show();
        }

        private void serviciosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new frmServicios(); // El formulario de servicios que ya tienes
        
            frm.Show();
        }
    }
}
