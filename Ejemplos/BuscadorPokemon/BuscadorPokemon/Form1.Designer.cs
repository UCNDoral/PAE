namespace BuscadorPokemon
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
            label1 = new Label();
            txtNombre = new TextBox();
            btnBuscar = new Button();
            lblId = new Label();
            lblTipo = new Label();
            lblPeso = new Label();
            lblAltura = new Label();
            picSprite = new PictureBox();
            lblEstado = new Label();
            ((System.ComponentModel.ISupportInitialize)picSprite).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(26, 34);
            label1.Name = "label1";
            label1.Size = new Size(127, 15);
            label1.TabIndex = 0;
            label1.Text = "Nombre del Pokémon:";
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(26, 52);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(127, 23);
            txtNombre.TabIndex = 1;
            txtNombre.TextChanged += txtNombre_TextChanged;
            // 
            // btnBuscar
            // 
            btnBuscar.Location = new Point(159, 52);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(47, 23);
            btnBuscar.TabIndex = 2;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // lblId
            // 
            lblId.AutoSize = true;
            lblId.Location = new Point(26, 96);
            lblId.Name = "lblId";
            lblId.Size = new Size(36, 15);
            lblId.TabIndex = 3;
            lblId.Text = "ID: —";
            // 
            // lblTipo
            // 
            lblTipo.AutoSize = true;
            lblTipo.Location = new Point(96, 96);
            lblTipo.Name = "lblTipo";
            lblTipo.Size = new Size(49, 15);
            lblTipo.TabIndex = 3;
            lblTipo.Text = "Tipo: —";
            // 
            // lblPeso
            // 
            lblPeso.AutoSize = true;
            lblPeso.Location = new Point(26, 127);
            lblPeso.Name = "lblPeso";
            lblPeso.Size = new Size(50, 15);
            lblPeso.TabIndex = 3;
            lblPeso.Text = "Peso: —";
            // 
            // lblAltura
            // 
            lblAltura.AutoSize = true;
            lblAltura.Location = new Point(96, 127);
            lblAltura.Name = "lblAltura";
            lblAltura.Size = new Size(57, 15);
            lblAltura.TabIndex = 3;
            lblAltura.Text = "Altura: —";
            // 
            // picSprite
            // 
            picSprite.Location = new Point(26, 158);
            picSprite.Name = "picSprite";
            picSprite.Size = new Size(180, 84);
            picSprite.SizeMode = PictureBoxSizeMode.Zoom;
            picSprite.TabIndex = 4;
            picSprite.TabStop = false;
            // 
            // lblEstado
            // 
            lblEstado.AutoSize = true;
            lblEstado.Location = new Point(26, 254);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(70, 15);
            lblEstado.TabIndex = 3;
            lblEstado.Text = "Estado: listo";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(234, 281);
            Controls.Add(picSprite);
            Controls.Add(lblEstado);
            Controls.Add(lblAltura);
            Controls.Add(lblPeso);
            Controls.Add(lblTipo);
            Controls.Add(lblId);
            Controls.Add(btnBuscar);
            Controls.Add(txtNombre);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)picSprite).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtNombre;
        private Button btnBuscar;
        private Label lblId;
        private Label lblTipo;
        private Label lblPeso;
        private Label lblAltura;
        private PictureBox picSprite;
        private Label lblEstado;
    }
}
