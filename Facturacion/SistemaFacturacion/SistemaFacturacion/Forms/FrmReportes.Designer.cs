namespace SistemaFacturacion.Forms
{
    partial class FrmReportes
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
            lblTipoReporte = new Label();
            cboTipoReporte = new ComboBox();
            lblFactura = new Label();
            cboFactura = new ComboBox();
            lblDesde = new Label();
            dtpDesde = new DateTimePicker();
            lblHasta = new Label();
            dtpHastas = new DateTimePicker();
            btnGenerar = new Button();
            btnCerrar = new Button();
            SuspendLayout();
            // 
            // lblTipoReporte
            // 
            lblTipoReporte.AutoSize = true;
            lblTipoReporte.Location = new Point(23, 40);
            lblTipoReporte.Name = "lblTipoReporte";
            lblTipoReporte.Size = new Size(94, 15);
            lblTipoReporte.TabIndex = 0;
            lblTipoReporte.Text = "Tipo de Reporte:";
            // 
            // cboTipoReporte
            // 
            cboTipoReporte.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTipoReporte.FormattingEnabled = true;
            cboTipoReporte.Location = new Point(135, 37);
            cboTipoReporte.Name = "cboTipoReporte";
            cboTipoReporte.Size = new Size(121, 23);
            cboTipoReporte.TabIndex = 1;
            // 
            // lblFactura
            // 
            lblFactura.AutoSize = true;
            lblFactura.Location = new Point(23, 85);
            lblFactura.Name = "lblFactura";
            lblFactura.Size = new Size(46, 15);
            lblFactura.TabIndex = 0;
            lblFactura.Text = "Factura";
            // 
            // cboFactura
            // 
            cboFactura.DropDownStyle = ComboBoxStyle.DropDownList;
            cboFactura.FormattingEnabled = true;
            cboFactura.Location = new Point(135, 82);
            cboFactura.Name = "cboFactura";
            cboFactura.Size = new Size(121, 23);
            cboFactura.TabIndex = 1;
            // 
            // lblDesde
            // 
            lblDesde.AutoSize = true;
            lblDesde.Location = new Point(316, 40);
            lblDesde.Name = "lblDesde";
            lblDesde.Size = new Size(42, 15);
            lblDesde.TabIndex = 0;
            lblDesde.Text = "Desde:";
            // 
            // dtpDesde
            // 
            dtpDesde.Location = new Point(373, 37);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.Size = new Size(200, 23);
            dtpDesde.TabIndex = 2;
            // 
            // lblHasta
            // 
            lblHasta.AutoSize = true;
            lblHasta.Location = new Point(316, 85);
            lblHasta.Name = "lblHasta";
            lblHasta.Size = new Size(40, 15);
            lblHasta.TabIndex = 0;
            lblHasta.Text = "Hasta:";
            // 
            // dtpHastas
            // 
            dtpHastas.Location = new Point(373, 82);
            dtpHastas.Name = "dtpHastas";
            dtpHastas.Size = new Size(200, 23);
            dtpHastas.TabIndex = 2;
            // 
            // btnGenerar
            // 
            btnGenerar.Location = new Point(177, 145);
            btnGenerar.Name = "btnGenerar";
            btnGenerar.Size = new Size(114, 46);
            btnGenerar.TabIndex = 3;
            btnGenerar.Text = "🖨️ Generar PDF";
            btnGenerar.UseVisualStyleBackColor = true;
            btnGenerar.Click += btnGenerar_Click;
            // 
            // btnCerrar
            // 
            btnCerrar.Location = new Point(316, 145);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(114, 46);
            btnCerrar.TabIndex = 3;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            // 
            // FrmReportes
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(642, 217);
            Controls.Add(btnCerrar);
            Controls.Add(btnGenerar);
            Controls.Add(dtpHastas);
            Controls.Add(dtpDesde);
            Controls.Add(cboFactura);
            Controls.Add(cboTipoReporte);
            Controls.Add(lblHasta);
            Controls.Add(lblDesde);
            Controls.Add(lblFactura);
            Controls.Add(lblTipoReporte);
            Name = "FrmReportes";
            Text = "FrmReportes";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTipoReporte;
        private ComboBox cboTipoReporte;
        private Label lblFactura;
        private ComboBox cboFactura;
        private Label lblDesde;
        private DateTimePicker dtpDesde;
        private Label lblHasta;
        private DateTimePicker dtpHastas;
        private Button btnGenerar;
        private Button btnCerrar;
    }
}