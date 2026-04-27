namespace DemoCongelada
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnAccion = new Button();
            lblEstado = new Label();
            progressBar1 = new ProgressBar();
            lstLog = new ListBox();
            SuspendLayout();
            // 
            // btnAccion
            // 
            btnAccion.Location = new Point(264, 10);
            btnAccion.Name = "btnAccion";
            btnAccion.Size = new Size(75, 23);
            btnAccion.TabIndex = 0;
            btnAccion.Text = "Iniciar Proceso";
            btnAccion.UseVisualStyleBackColor = true;
            btnAccion.Click += btnAccion_Click;
            // 
            // lblEstado
            // 
            lblEstado.AutoSize = true;
            lblEstado.Location = new Point(12, 14);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(32, 15);
            lblEstado.TabIndex = 1;
            lblEstado.Text = "Listo";
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(12, 169);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(327, 23);
            progressBar1.TabIndex = 2;
            // 
            // lstLog
            // 
            lstLog.FormattingEnabled = true;
            lstLog.ItemHeight = 15;
            lstLog.Location = new Point(12, 39);
            lstLog.Name = "lstLog";
            lstLog.Size = new Size(327, 124);
            lstLog.TabIndex = 3;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(357, 206);
            Controls.Add(lstLog);
            Controls.Add(progressBar1);
            Controls.Add(lblEstado);
            Controls.Add(btnAccion);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnAccion;
        private Label lblEstado;
        private ProgressBar progressBar1;
        private ListBox lstLog;
    }
}
