using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SistemaFacturacion.Forms;

namespace SistemaFacturacion.Forms
{
    /// <summary>
    /// Ventana principal con menú de navegación (MDI Parent).
    /// Todas las demás ventanas se abren dentro de esta.
    /// </summary>
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
            // Convertir esta ventana en contenedor MDI
            this.IsMdiContainer = true;
            this.WindowState = FormWindowState.Maximized;
            this.Text = "Sistema de Facturación v1.0";
        }

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmClientes());
        }

        private void productosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmProductos());

        }

        private void nuevaFacturaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmFactura());
        }

        /// <summary>
        /// Abre el formulario como hijo MDI.
        /// Si ya está abierto, lo trae al frente en vez de duplicarlo.
        /// </summary>
        private void AbrirFormulario(Form formulario)
        {
            // Buscar si ya hay una instancia abierta del mismo tipo
            var existente = this.MdiChildren
                .FirstOrDefault(f => f.GetType() == formulario.GetType());

            if (existente != null)
            {
                existente.BringToFront();
                formulario.Dispose(); // descartamos la nueva instancia
            }
            else
            {
                formulario.MdiParent = this;
                formulario.Show();
            }
        }
    }
}
