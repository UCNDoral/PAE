namespace BancoSimulador
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
            grpCuenta = new GroupBox();
            lblSaldo = new Label();
            grpOpciones = new GroupBox();
            chkSincronizacion = new CheckBox();
            grpBotones = new GroupBox();
            btnCancelar = new Button();
            btnIniciar = new Button();
            btnLimpiar = new Button();
            grpLog = new GroupBox();
            txtLog = new RichTextBox();
            grpCuenta.SuspendLayout();
            grpOpciones.SuspendLayout();
            grpBotones.SuspendLayout();
            grpLog.SuspendLayout();
            SuspendLayout();
            // 
            // grpCuenta
            // 
            grpCuenta.Controls.Add(lblSaldo);
            grpCuenta.Location = new Point(12, 12);
            grpCuenta.Name = "grpCuenta";
            grpCuenta.Size = new Size(260, 100);
            grpCuenta.TabIndex = 0;
            grpCuenta.TabStop = false;
            grpCuenta.Text = "💰 Saldo de la Cuenta";
            // 
            // lblSaldo
            // 
            lblSaldo.AutoSize = true;
            lblSaldo.ForeColor = Color.Green;
            lblSaldo.Location = new Point(10, 45);
            lblSaldo.Name = "lblSaldo";
            lblSaldo.Size = new Size(55, 15);
            lblSaldo.TabIndex = 0;
            lblSaldo.Text = "$1,000.00";
            // 
            // grpOpciones
            // 
            grpOpciones.Controls.Add(chkSincronizacion);
            grpOpciones.Location = new Point(12, 120);
            grpOpciones.Name = "grpOpciones";
            grpOpciones.Size = new Size(260, 80);
            grpOpciones.TabIndex = 1;
            grpOpciones.TabStop = false;
            grpOpciones.Text = "⚙️ Configuración";
            // 
            // chkSincronizacion
            // 
            chkSincronizacion.AutoSize = true;
            chkSincronizacion.Checked = true;
            chkSincronizacion.CheckState = CheckState.Checked;
            chkSincronizacion.Location = new Point(10, 30);
            chkSincronizacion.Name = "chkSincronizacion";
            chkSincronizacion.Size = new Size(161, 19);
            chkSincronizacion.TabIndex = 0;
            chkSincronizacion.Text = "Usar sincronización (lock)";
            chkSincronizacion.UseVisualStyleBackColor = true;
            // 
            // grpBotones
            // 
            grpBotones.Controls.Add(btnCancelar);
            grpBotones.Controls.Add(btnIniciar);
            grpBotones.Location = new Point(12, 210);
            grpBotones.Name = "grpBotones";
            grpBotones.Size = new Size(260, 150);
            grpBotones.TabIndex = 2;
            grpBotones.TabStop = false;
            grpBotones.Text = "🎮 Controles";
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.Firebrick;
            btnCancelar.Enabled = false;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(10, 78);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(238, 35);
            btnCancelar.TabIndex = 1;
            btnCancelar.Text = "⏹ CANCELAR";
            btnCancelar.UseVisualStyleBackColor = false;
            // 
            // btnIniciar
            // 
            btnIniciar.BackColor = Color.SeaGreen;
            btnIniciar.FlatStyle = FlatStyle.Flat;
            btnIniciar.ForeColor = SystemColors.ControlLightLight;
            btnIniciar.Location = new Point(10, 30);
            btnIniciar.Name = "btnIniciar";
            btnIniciar.Size = new Size(238, 38);
            btnIniciar.TabIndex = 0;
            btnIniciar.Text = "▶ INICIAR SIMULACIÓN";
            btnIniciar.UseVisualStyleBackColor = false;
            btnIniciar.Click += btnIniciar_Click;
            // 
            // btnLimpiar
            // 
            btnLimpiar.BackColor = Color.SlateGray;
            btnLimpiar.FlatStyle = FlatStyle.Flat;
            btnLimpiar.ForeColor = Color.White;
            btnLimpiar.Location = new Point(12, 370);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(260, 32);
            btnLimpiar.TabIndex = 2;
            btnLimpiar.Text = "🗑️ Limpiar Log";
            btnLimpiar.UseVisualStyleBackColor = false;
            // 
            // grpLog
            // 
            grpLog.Controls.Add(txtLog);
            grpLog.Location = new Point(284, 12);
            grpLog.Name = "grpLog";
            grpLog.Size = new Size(595, 660);
            grpLog.TabIndex = 3;
            grpLog.TabStop = false;
            grpLog.Text = "📋 Registro de operaciones";
            // 
            // txtLog
            // 
            txtLog.BackColor = SystemColors.WindowText;
            txtLog.Font = new Font("Consolas", 9.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtLog.ForeColor = Color.LightCyan;
            txtLog.Location = new Point(10, 25);
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = RichTextBoxScrollBars.Vertical;
            txtLog.Size = new Size(573, 625);
            txtLog.TabIndex = 0;
            txtLog.Text = "";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(884, 681);
            Controls.Add(grpLog);
            Controls.Add(btnLimpiar);
            Controls.Add(grpBotones);
            Controls.Add(grpOpciones);
            Controls.Add(grpCuenta);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Simulador Bancario - UCN";
            Load += Form1_Load;
            grpCuenta.ResumeLayout(false);
            grpCuenta.PerformLayout();
            grpOpciones.ResumeLayout(false);
            grpOpciones.PerformLayout();
            grpBotones.ResumeLayout(false);
            grpLog.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox grpCuenta;
        private Label lblSaldo;
        private GroupBox grpOpciones;
        private CheckBox chkSincronizacion;
        private GroupBox grpBotones;
        private Button btnLimpiar;
        private Button btnCancelar;
        private Button btnIniciar;
        private GroupBox grpLog;
        private RichTextBox txtLog;
    }
}
