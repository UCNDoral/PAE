namespace SistemaFacturacion.Forms
{
    partial class FrmFactura
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblNumero = new Label();
            cboCliente = new ComboBox();
            dtpFecha = new DateTimePicker();
            cboProducto = new ComboBox();
            nudCantidad = new NumericUpDown();
            lblPrecio = new Label();
            dgvDetalle = new DataGridView();
            btnAgregarLinea = new Button();
            btnQuitarLinea = new Button();
            lblSubtotal = new Label();
            lblIVA = new Label();
            lblTotal = new Label();
            btnGuardar = new Button();
            btnNueva = new Button();
            btnAnular = new Button();
            dgvFacturas = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)nudCantidad).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvDetalle).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvFacturas).BeginInit();
            SuspendLayout();
            // 
            // lblNumero
            // 
            lblNumero.AutoSize = true;
            lblNumero.Location = new Point(38, 38);
            lblNumero.Name = "lblNumero";
            lblNumero.Size = new Size(0, 15);
            lblNumero.TabIndex = 0;
            // 
            // cboCliente
            // 
            cboCliente.FormattingEnabled = true;
            cboCliente.Location = new Point(90, 29);
            cboCliente.Name = "cboCliente";
            cboCliente.Size = new Size(121, 23);
            cboCliente.TabIndex = 1;
            cboCliente.Text = "Seleccione unCliente";
            // 
            // dtpFecha
            // 
            dtpFecha.Location = new Point(231, 29);
            dtpFecha.Name = "dtpFecha";
            dtpFecha.Size = new Size(200, 23);
            dtpFecha.TabIndex = 2;
            // 
            // cboProducto
            // 
            cboProducto.FormattingEnabled = true;
            cboProducto.Location = new Point(90, 87);
            cboProducto.Name = "cboProducto";
            cboProducto.Size = new Size(121, 23);
            cboProducto.TabIndex = 1;
            cboProducto.Text = "Seleccione un Producto";
            // 
            // nudCantidad
            // 
            nudCantidad.Location = new Point(268, 87);
            nudCantidad.Name = "nudCantidad";
            nudCantidad.Size = new Size(120, 23);
            nudCantidad.TabIndex = 3;
            // 
            // lblPrecio
            // 
            lblPrecio.AutoSize = true;
            lblPrecio.Location = new Point(107, 159);
            lblPrecio.Name = "lblPrecio";
            lblPrecio.Size = new Size(19, 15);
            lblPrecio.TabIndex = 4;
            lblPrecio.Text = "$0";
            // 
            // dgvDetalle
            // 
            dgvDetalle.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDetalle.Location = new Point(12, 246);
            dgvDetalle.Name = "dgvDetalle";
            dgvDetalle.Size = new Size(240, 150);
            dgvDetalle.TabIndex = 5;
            // 
            // btnAgregarLinea
            // 
            btnAgregarLinea.Location = new Point(231, 155);
            btnAgregarLinea.Name = "btnAgregarLinea";
            btnAgregarLinea.Size = new Size(75, 23);
            btnAgregarLinea.TabIndex = 6;
            btnAgregarLinea.Text = "Agregar";
            btnAgregarLinea.UseVisualStyleBackColor = true;
            btnAgregarLinea.Click += btnAgregarLinea_Click;
            // 
            // btnQuitarLinea
            // 
            btnQuitarLinea.Location = new Point(177, 217);
            btnQuitarLinea.Name = "btnQuitarLinea";
            btnQuitarLinea.Size = new Size(75, 23);
            btnQuitarLinea.TabIndex = 6;
            btnQuitarLinea.Text = "Quitar";
            btnQuitarLinea.UseVisualStyleBackColor = true;
            btnQuitarLinea.Click += btnQuitarLinea_Click;
            // 
            // lblSubtotal
            // 
            lblSubtotal.AutoSize = true;
            lblSubtotal.Location = new Point(282, 255);
            lblSubtotal.Name = "lblSubtotal";
            lblSubtotal.Size = new Size(19, 15);
            lblSubtotal.TabIndex = 4;
            lblSubtotal.Text = "$0";
            // 
            // lblIVA
            // 
            lblIVA.AutoSize = true;
            lblIVA.Location = new Point(282, 287);
            lblIVA.Name = "lblIVA";
            lblIVA.Size = new Size(24, 15);
            lblIVA.TabIndex = 4;
            lblIVA.Text = "IVA";
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Location = new Point(282, 311);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(33, 15);
            lblTotal.TabIndex = 4;
            lblTotal.Text = "Total";
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(336, 249);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(75, 23);
            btnGuardar.TabIndex = 7;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnNueva
            // 
            btnNueva.Location = new Point(336, 283);
            btnNueva.Name = "btnNueva";
            btnNueva.Size = new Size(75, 23);
            btnNueva.TabIndex = 7;
            btnNueva.Text = "Nueva";
            btnNueva.UseVisualStyleBackColor = true;
            btnNueva.Click += btnNueva_Click;
            // 
            // btnAnular
            // 
            btnAnular.Location = new Point(336, 322);
            btnAnular.Name = "btnAnular";
            btnAnular.Size = new Size(75, 23);
            btnAnular.TabIndex = 7;
            btnAnular.Text = "Anular";
            btnAnular.UseVisualStyleBackColor = true;
            btnAnular.Click += btnAnular_Click;
            // 
            // dgvFacturas
            // 
            dgvFacturas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvFacturas.Location = new Point(464, 246);
            dgvFacturas.Name = "dgvFacturas";
            dgvFacturas.Size = new Size(240, 150);
            dgvFacturas.TabIndex = 5;
            // 
            // FrmFactura
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnAnular);
            Controls.Add(btnNueva);
            Controls.Add(btnGuardar);
            Controls.Add(btnQuitarLinea);
            Controls.Add(btnAgregarLinea);
            Controls.Add(dgvFacturas);
            Controls.Add(dgvDetalle);
            Controls.Add(lblTotal);
            Controls.Add(lblIVA);
            Controls.Add(lblSubtotal);
            Controls.Add(lblPrecio);
            Controls.Add(nudCantidad);
            Controls.Add(dtpFecha);
            Controls.Add(cboProducto);
            Controls.Add(cboCliente);
            Controls.Add(lblNumero);
            Name = "FrmFactura";
            Text = "FrmFactura";
            ((System.ComponentModel.ISupportInitialize)nudCantidad).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvDetalle).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvFacturas).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblNumero;
        private ComboBox cboCliente;
        private DateTimePicker dtpFecha;
        private ComboBox cboProducto;
        private NumericUpDown nudCantidad;
        private Label lblPrecio;
        private DataGridView dgvDetalle;
        private Button btnAgregarLinea;
        private Button btnQuitarLinea;
        private Label lblSubtotal;
        private Label lblIVA;
        private Label lblTotal;
        private Button btnGuardar;
        private Button btnNueva;
        private Button btnAnular;
        private DataGridView dgvFacturas;
    }
}