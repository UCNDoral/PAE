using System;
using System.Reflection.PortableExecutable;
using System.Threading;
using System.Windows.Forms;

namespace BancoSimulador
{
    public partial class Form1 : Form
    {

        //varibles compartidas entre hilos
        //estas variables son accedidas por multiples hilos almismo tiempo

        //la cuenta bancaria que ambos hilos van a modificar
        private CuentaBancaria cuenta = new CuentaBancaria(1000);

        private CancellationTokenSource cst;

        private Thread hiloDeposito;
        private Thread hiloRetiro;

        //SEMAPHORESLIM
        private SemaphoreSlim semaphore = new SemaphoreSlim(2, 2);

        private bool usarSincronizacion = false;

        private int hilossTerminado = 0;


        private readonly object lockFinalizacion = new object();




        public Form1()
        {
            InitializeComponent();

            ActualizarUI();
        }



        private void ActualizarUI()
        {
            lblSaldo.Text = $"{cuenta.ObtenerSaldo():C}";
            lblSaldo.ForeColor = Color.Green;
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            //reiniciar cuenta bancaria
             cuenta = new CuentaBancaria(1000);

            //creamos un nuevo token de cancelacion
            //si el otro fue cancelado, este empieza en limpio.
            cst = new CancellationTokenSource();

            //reiniciar el contador de hilos terminados

            hilossTerminado = 0;


            usarSincronizacion = chkSincronizacion.Checked;

        }
    }
}