namespace SistemaReportes.Forms
{
    partial class FrmReporteVentas
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
            label1 = new Label();
            dtpInicio = new DateTimePicker();
            label2 = new Label();
            dtpFin = new DateTimePicker();
            btnGenerar = new Button();
            btnExportarExcel = new Button();
            btnExportarPDF = new Button();
            dgvReporte = new DataGridView();
            lblTotal = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvReporte).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(27, 29);
            label1.Name = "label1";
            label1.Size = new Size(73, 15);
            label1.TabIndex = 0;
            label1.Text = "Fecha Inicio:";
            // 
            // dtpInicio
            // 
            dtpInicio.Location = new Point(106, 23);
            dtpInicio.Name = "dtpInicio";
            dtpInicio.Size = new Size(200, 23);
            dtpInicio.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(353, 29);
            label2.Name = "label2";
            label2.Size = new Size(60, 15);
            label2.TabIndex = 0;
            label2.Text = "Fecha Fin:";
            // 
            // dtpFin
            // 
            dtpFin.Location = new Point(432, 23);
            dtpFin.Name = "dtpFin";
            dtpFin.Size = new Size(200, 23);
            dtpFin.TabIndex = 1;
            // 
            // btnGenerar
            // 
            btnGenerar.Location = new Point(27, 72);
            btnGenerar.Name = "btnGenerar";
            btnGenerar.Size = new Size(111, 32);
            btnGenerar.TabIndex = 2;
            btnGenerar.Text = "Generar Reporte";
            btnGenerar.UseVisualStyleBackColor = true;
            btnGenerar.Click += btnGenerar_Click;
            // 
            // btnExportarExcel
            // 
            btnExportarExcel.Location = new Point(155, 72);
            btnExportarExcel.Name = "btnExportarExcel";
            btnExportarExcel.Size = new Size(111, 32);
            btnExportarExcel.TabIndex = 2;
            btnExportarExcel.Text = "Exportar a Excel";
            btnExportarExcel.UseVisualStyleBackColor = true;
            btnExportarExcel.Click += btnExportarExcel_Click;
            // 
            // btnExportarPDF
            // 
            btnExportarPDF.Location = new Point(293, 72);
            btnExportarPDF.Name = "btnExportarPDF";
            btnExportarPDF.Size = new Size(111, 32);
            btnExportarPDF.TabIndex = 2;
            btnExportarPDF.Text = "Exportar a PDF";
            btnExportarPDF.UseVisualStyleBackColor = true;
            btnExportarPDF.Click += btnExportarPDF_Click;
            // 
            // dgvReporte
            // 
            dgvReporte.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvReporte.Location = new Point(12, 151);
            dgvReporte.Name = "dgvReporte";
            dgvReporte.Size = new Size(634, 184);
            dgvReporte.TabIndex = 3;
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Location = new Point(500, 84);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(13, 15);
            lblTotal.TabIndex = 4;
            lblTotal.Text = "0";
            // 
            // FrmReporteVentas
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(658, 346);
            Controls.Add(lblTotal);
            Controls.Add(dgvReporte);
            Controls.Add(btnExportarPDF);
            Controls.Add(btnExportarExcel);
            Controls.Add(btnGenerar);
            Controls.Add(dtpFin);
            Controls.Add(label2);
            Controls.Add(dtpInicio);
            Controls.Add(label1);
            Name = "FrmReporteVentas";
            Text = "FrmReporteVentas";
            ((System.ComponentModel.ISupportInitialize)dgvReporte).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private DateTimePicker dtpInicio;
        private Label label2;
        private DateTimePicker dtpFin;
        private Button btnGenerar;
        private Button btnExportarExcel;
        private Button btnExportarPDF;
        private DataGridView dgvReporte;
        private Label lblTotal;
    }
}