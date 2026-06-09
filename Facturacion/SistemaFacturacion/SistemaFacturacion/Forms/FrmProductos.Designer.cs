namespace SistemaFacturacion.Forms
{
    partial class FrmProductos
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
            lblCodigo = new Label();
            lblActivo = new Label();
            lblStock = new Label();
            lblPrecioUnitario = new Label();
            lblDescripcion = new Label();
            lblNombre = new Label();
            btnCancelar = new Button();
            btnEliminar = new Button();
            btnGuardar = new Button();
            btnNuevo = new Button();
            txtStock = new TextBox();
            txtPrecioUnitario = new TextBox();
            txtDescripcion = new TextBox();
            txtNombre = new TextBox();
            dgvProductos = new DataGridView();
            ckbActivo = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)dgvProductos).BeginInit();
            SuspendLayout();
            // 
            // lblCodigo
            // 
            lblCodigo.AutoSize = true;
            lblCodigo.Location = new Point(158, 206);
            lblCodigo.Name = "lblCodigo";
            lblCodigo.Size = new Size(46, 15);
            lblCodigo.TabIndex = 14;
            lblCodigo.Text = "Codigo";
            // 
            // lblActivo
            // 
            lblActivo.AutoSize = true;
            lblActivo.Location = new Point(156, 173);
            lblActivo.Name = "lblActivo";
            lblActivo.Size = new Size(41, 15);
            lblActivo.TabIndex = 15;
            lblActivo.Text = "Activo";
            lblActivo.TextAlign = ContentAlignment.TopCenter;
            // 
            // lblStock
            // 
            lblStock.AutoSize = true;
            lblStock.Location = new Point(162, 130);
            lblStock.Name = "lblStock";
            lblStock.Size = new Size(36, 15);
            lblStock.TabIndex = 16;
            lblStock.Text = "Stock";
            // 
            // lblPrecioUnitario
            // 
            lblPrecioUnitario.AutoSize = true;
            lblPrecioUnitario.Location = new Point(128, 97);
            lblPrecioUnitario.Name = "lblPrecioUnitario";
            lblPrecioUnitario.Size = new Size(85, 15);
            lblPrecioUnitario.TabIndex = 17;
            lblPrecioUnitario.Text = "Precio Unitario";
            // 
            // lblDescripcion
            // 
            lblDescripcion.AutoSize = true;
            lblDescripcion.Location = new Point(162, 52);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(69, 15);
            lblDescripcion.TabIndex = 18;
            lblDescripcion.Text = "Descripcion";
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(162, 21);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(51, 15);
            lblNombre.TabIndex = 19;
            lblNombre.Text = "Nombre";
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(462, 102);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(75, 23);
            btnCancelar.TabIndex = 10;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // btnEliminar
            // 
            btnEliminar.Location = new Point(462, 73);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(75, 23);
            btnEliminar.TabIndex = 11;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = true;
            btnEliminar.Click += btnEliminar_Click;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(453, 44);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(75, 23);
            btnGuardar.TabIndex = 12;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnNuevo
            // 
            btnNuevo.Location = new Point(453, 15);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(75, 23);
            btnNuevo.TabIndex = 13;
            btnNuevo.Text = "Nuevo";
            btnNuevo.UseVisualStyleBackColor = true;
            btnNuevo.Click += btnNuevo_Click;
            // 
            // txtStock
            // 
            txtStock.Location = new Point(225, 127);
            txtStock.Name = "txtStock";
            txtStock.Size = new Size(100, 23);
            txtStock.TabIndex = 6;
            // 
            // txtPrecioUnitario
            // 
            txtPrecioUnitario.Location = new Point(225, 89);
            txtPrecioUnitario.Name = "txtPrecioUnitario";
            txtPrecioUnitario.Size = new Size(100, 23);
            txtPrecioUnitario.TabIndex = 7;
            // 
            // txtDescripcion
            // 
            txtDescripcion.Location = new Point(225, 46);
            txtDescripcion.Name = "txtDescripcion";
            txtDescripcion.Size = new Size(100, 23);
            txtDescripcion.TabIndex = 8;
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(225, 17);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(100, 23);
            txtNombre.TabIndex = 9;
            // 
            // dgvProductos
            // 
            dgvProductos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProductos.Location = new Point(156, 286);
            dgvProductos.Name = "dgvProductos";
            dgvProductos.Size = new Size(489, 150);
            dgvProductos.TabIndex = 4;
            dgvProductos.SelectionChanged += dgvProductos_SelectionChanged;
            // 
            // ckbActivo
            // 
            ckbActivo.AutoSize = true;
            ckbActivo.Location = new Point(228, 169);
            ckbActivo.Name = "ckbActivo";
            ckbActivo.Size = new Size(15, 14);
            ckbActivo.TabIndex = 20;
            ckbActivo.UseVisualStyleBackColor = true;
            // 
            // FrmProductos
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(ckbActivo);
            Controls.Add(lblCodigo);
            Controls.Add(lblActivo);
            Controls.Add(lblStock);
            Controls.Add(lblPrecioUnitario);
            Controls.Add(lblDescripcion);
            Controls.Add(lblNombre);
            Controls.Add(btnCancelar);
            Controls.Add(btnEliminar);
            Controls.Add(btnGuardar);
            Controls.Add(btnNuevo);
            Controls.Add(txtStock);
            Controls.Add(txtPrecioUnitario);
            Controls.Add(txtDescripcion);
            Controls.Add(txtNombre);
            Controls.Add(dgvProductos);
            Name = "FrmProductos";
            Text = "FrmProductos";
            ((System.ComponentModel.ISupportInitialize)dgvProductos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblCodigo;
        private Label lblActivo;
        private Label lblStock;
        private Label lblPrecioUnitario;
        private Label lblDescripcion;
        private Label lblNombre;
        private Button btnCancelar;
        private Button btnEliminar;
        private Button btnGuardar;
        private Button btnNuevo;
        private TextBox txtStock;
        private TextBox txtPrecioUnitario;
        private TextBox txtDescripcion;
        private TextBox txtNombre;
        private DataGridView dgvProductos;
        private CheckBox ckbActivo;
    }
}