namespace DemoCongelada
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnAccion_Click(object sender, EventArgs e)
        {

            
            // 1. Preparación de la UI
            btnAccion.Enabled = false; // Deshabilitamos el botón para evitar doble clic
            lblEstado.Text = "Procesando datos...";
            progressBar1.Value = 0;
            lstLog.Items.Clear();

            // 2. EJECUCIÓN BLOQUEANTE
            // Al llamar directamente a ProcesarDatosPesados(), el código se ejecuta
            // línea por línea en este mismo hilo.
            ProcesarDatosPesados();
            // PROBLEMA: Mientras ProcessData corre, el mensaje loop de Windows
            // está detenido. La ventana parece "colgada". No puedes moverla ni minimizarla.


            // 3. Finalización
            // Esta línea solo se ejecuta cuando ProcesarDatosPesados() termina completamente.
            lblEstado.Text = "Proceso finalizado.";
            btnAccion.Enabled = true;
        }

        /// <summary>
        /// Simula una tarea pesada (CPU o I/O) que tarda varios segundos.
        /// Thread.Sleep simula el tiempo de espera de una operación real.
        /// </summary>
        private void ProcesarDatosPesados()
        {
            // Simulamos 100 pasos de procesamiento
            for (int i = 1; i <= 100; i++)
            {
                // Simulamos una operación que toma 500 milisegundos
                // (Podría ser leer un archivo, calcular una matriz, consultar DB)
                Thread.Sleep(500);

                // Intentamos actualizar la barra de progreso
                progressBar1.Value = i;

                // Intentamos agregar un log
                // NOTA: En este ejemplo síncrono, la UI no se refresca visualmente
                // hasta que el método termine, aunque los valores internos cambien.
                lstLog.Items.Add($"Procesando item {i}");

                // Forzar el repaint manualmente (Truco sucio, NO recomendado en producción)
                // Incluso con esto, la aplicación sigue sin responder a inputs del usuario.
                Application.DoEvents();
            }
        }
    }
}
