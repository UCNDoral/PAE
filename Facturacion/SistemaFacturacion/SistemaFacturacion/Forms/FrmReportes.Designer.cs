namespace SistemaFacturacion.Forms
{
    partial class FrmReportes
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            panelFiltros = new Panel();
            btnExportar = new Button();
            btnConsultar = new Button();
            cboTipoReporte = new ComboBox();
            lblTipoReporte = new Label();
            dtpHasta = new DateTimePicker();
            lblHasta = new Label();
            dtpDesde = new DateTimePicker();
            lblDesde = new Label();
            panelResumen = new Panel();
            lblTotal = new Label();
            label10 = new Label();
            lblIVA = new Label();
            label8 = new Label();
            lblSubtotal = new Label();
            label6 = new Label();
            lblFacturasAnuladas = new Label();
            label4 = new Label();
            lblFacturasActivas = new Label();
            label1 = new Label();
            dgvReporte = new DataGridView();
            panelFiltros.SuspendLayout();
            panelResumen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReporte).BeginInit();
            SuspendLayout();
            // 
            // panelFiltros
            // 
            panelFiltros.Controls.Add(btnExportar);
            panelFiltros.Controls.Add(btnConsultar);
            panelFiltros.Controls.Add(cboTipoReporte);
            panelFiltros.Controls.Add(lblTipoReporte);
            panelFiltros.Controls.Add(dtpHasta);
            panelFiltros.Controls.Add(lblHasta);
            panelFiltros.Controls.Add(dtpDesde);
            panelFiltros.Controls.Add(lblDesde);
            panelFiltros.Dock = DockStyle.Top;
            panelFiltros.Location = new Point(0, 0);
            panelFiltros.Name = "panelFiltros";
            panelFiltros.Padding = new Padding(12);
            panelFiltros.Size = new Size(984, 74);
            panelFiltros.TabIndex = 0;
            // 
            // btnExportar
            // 
            btnExportar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExportar.Location = new Point(881, 26);
            btnExportar.Name = "btnExportar";
            btnExportar.Size = new Size(91, 27);
            btnExportar.TabIndex = 7;
            btnExportar.Text = "Exportar";
            btnExportar.UseVisualStyleBackColor = true;
            btnExportar.Click += btnExportar_Click;
            // 
            // btnConsultar
            // 
            btnConsultar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnConsultar.Location = new Point(784, 26);
            btnConsultar.Name = "btnConsultar";
            btnConsultar.Size = new Size(91, 27);
            btnConsultar.TabIndex = 6;
            btnConsultar.Text = "Consultar";
            btnConsultar.UseVisualStyleBackColor = true;
            btnConsultar.Click += btnConsultar_Click;
            // 
            // cboTipoReporte
            // 
            cboTipoReporte.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTipoReporte.FormattingEnabled = true;
            cboTipoReporte.Location = new Point(462, 28);
            cboTipoReporte.Name = "cboTipoReporte";
            cboTipoReporte.Size = new Size(249, 23);
            cboTipoReporte.TabIndex = 5;
            // 
            // lblTipoReporte
            // 
            lblTipoReporte.AutoSize = true;
            lblTipoReporte.Location = new Point(462, 10);
            lblTipoReporte.Name = "lblTipoReporte";
            lblTipoReporte.Size = new Size(93, 15);
            lblTipoReporte.TabIndex = 4;
            lblTipoReporte.Text = "Tipo de reporte";
            // 
            // dtpHasta
            // 
            dtpHasta.Format = DateTimePickerFormat.Short;
            dtpHasta.Location = new Point(252, 28);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.Size = new Size(150, 23);
            dtpHasta.TabIndex = 3;
            // 
            // lblHasta
            // 
            lblHasta.AutoSize = true;
            lblHasta.Location = new Point(252, 10);
            lblHasta.Name = "lblHasta";
            lblHasta.Size = new Size(38, 15);
            lblHasta.TabIndex = 2;
            lblHasta.Text = "Hasta";
            // 
            // dtpDesde
            // 
            dtpDesde.Format = DateTimePickerFormat.Short;
            dtpDesde.Location = new Point(12, 28);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.Size = new Size(150, 23);
            dtpDesde.TabIndex = 1;
            // 
            // lblDesde
            // 
            lblDesde.AutoSize = true;
            lblDesde.Location = new Point(12, 10);
            lblDesde.Name = "lblDesde";
            lblDesde.Size = new Size(39, 15);
            lblDesde.TabIndex = 0;
            lblDesde.Text = "Desde";
            // 
            // panelResumen
            // 
            panelResumen.Controls.Add(lblTotal);
            panelResumen.Controls.Add(label10);
            panelResumen.Controls.Add(lblIVA);
            panelResumen.Controls.Add(label8);
            panelResumen.Controls.Add(lblSubtotal);
            panelResumen.Controls.Add(label6);
            panelResumen.Controls.Add(lblFacturasAnuladas);
            panelResumen.Controls.Add(label4);
            panelResumen.Controls.Add(lblFacturasActivas);
            panelResumen.Controls.Add(label1);
            panelResumen.Dock = DockStyle.Top;
            panelResumen.Location = new Point(0, 74);
            panelResumen.Name = "panelResumen";
            panelResumen.Padding = new Padding(12, 8, 12, 8);
            panelResumen.Size = new Size(984, 68);
            panelResumen.TabIndex = 1;
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTotal.Location = new Point(801, 31);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(49, 20);
            lblTotal.TabIndex = 9;
            lblTotal.Text = "C$0.00";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(801, 12);
            label10.Name = "label10";
            label10.Size = new Size(32, 15);
            label10.TabIndex = 8;
            label10.Text = "Total";
            // 
            // lblIVA
            // 
            lblIVA.AutoSize = true;
            lblIVA.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblIVA.Location = new Point(612, 31);
            lblIVA.Name = "lblIVA";
            lblIVA.Size = new Size(49, 20);
            lblIVA.TabIndex = 7;
            lblIVA.Text = "C$0.00";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(612, 12);
            label8.Name = "label8";
            label8.Size = new Size(24, 15);
            label8.TabIndex = 6;
            label8.Text = "IVA";
            // 
            // lblSubtotal
            // 
            lblSubtotal.AutoSize = true;
            lblSubtotal.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblSubtotal.Location = new Point(423, 31);
            lblSubtotal.Name = "lblSubtotal";
            lblSubtotal.Size = new Size(49, 20);
            lblSubtotal.TabIndex = 5;
            lblSubtotal.Text = "C$0.00";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(423, 12);
            label6.Name = "label6";
            label6.Size = new Size(51, 15);
            label6.TabIndex = 4;
            label6.Text = "Subtotal";
            // 
            // lblFacturasAnuladas
            // 
            lblFacturasAnuladas.AutoSize = true;
            lblFacturasAnuladas.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblFacturasAnuladas.Location = new Point(234, 31);
            lblFacturasAnuladas.Name = "lblFacturasAnuladas";
            lblFacturasAnuladas.Size = new Size(18, 20);
            lblFacturasAnuladas.TabIndex = 3;
            lblFacturasAnuladas.Text = "0";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(234, 12);
            label4.Name = "label4";
            label4.Size = new Size(105, 15);
            label4.TabIndex = 2;
            label4.Text = "Facturas anuladas";
            // 
            // lblFacturasActivas
            // 
            lblFacturasActivas.AutoSize = true;
            lblFacturasActivas.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblFacturasActivas.Location = new Point(12, 31);
            lblFacturasActivas.Name = "lblFacturasActivas";
            lblFacturasActivas.Size = new Size(18, 20);
            lblFacturasActivas.TabIndex = 1;
            lblFacturasActivas.Text = "0";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 12);
            label1.Name = "label1";
            label1.Size = new Size(91, 15);
            label1.TabIndex = 0;
            label1.Text = "Facturas activas";
            // 
            // dgvReporte
            // 
            dgvReporte.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvReporte.Dock = DockStyle.Fill;
            dgvReporte.Location = new Point(0, 142);
            dgvReporte.Name = "dgvReporte";
            dgvReporte.Size = new Size(984, 419);
            dgvReporte.TabIndex = 2;
            // 
            // FrmReportes
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 561);
            Controls.Add(dgvReporte);
            Controls.Add(panelResumen);
            Controls.Add(panelFiltros);
            Name = "FrmReportes";
            Text = "Reportes";
            panelFiltros.ResumeLayout(false);
            panelFiltros.PerformLayout();
            panelResumen.ResumeLayout(false);
            panelResumen.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReporte).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelFiltros;
        private Button btnExportar;
        private Button btnConsultar;
        private ComboBox cboTipoReporte;
        private Label lblTipoReporte;
        private DateTimePicker dtpHasta;
        private Label lblHasta;
        private DateTimePicker dtpDesde;
        private Label lblDesde;
        private Panel panelResumen;
        private Label lblTotal;
        private Label label10;
        private Label lblIVA;
        private Label label8;
        private Label lblSubtotal;
        private Label label6;
        private Label lblFacturasAnuladas;
        private Label label4;
        private Label lblFacturasActivas;
        private Label label1;
        private DataGridView dgvReporte;
    }
}
