using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace BancoSimulador
{
    public partial class Form1 : Form
    {
        //VARIABLES COMPARTIDAS - seran variables accedidas por multiples hilos

        //Label cuenta bancaria que ambos hilos van a modificar 
        private CuentaBancaria cuenta = new CuentaBancaria(1000);

        //es quien emite la señan de cancelacion
        private CancellationTokenSource cts;

        //Los hilos de trabjo
        private Thread hiloDeposito;

        private Thread hiloRetiros;

        //SemaphoreSlim(2,2): permite que maximo 2 hilos entren al recurso al mismo tiempo
        //si ya hay 2 antro el tercero espera
        private SemaphoreSlim semaphore = new SemaphoreSlim(2, 2);


        private bool usarSincronizacion = true;

        //contador para saber cuantos hilos han terminado de ejecutarse
        private int hilosTerminados;

        //objeto de bloque 
        private readonly object lockFinalizacion = new object();




        public Form1()
        {
            InitializeComponent();

            ActialzarUI();
        }


        private void ActialzarUI()
        {
            lblSaldo.Text = $"{cuenta.ObtenerSaldo()}";
            lblSaldo.ForeColor = Color.Green;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            //reiniciar la cuenta con saldo 
            cuenta = new CuentaBancaria(1000);

            //Crear un nuevo token de cancelacion
            cts = new CancellationTokenSource();

            hilosTerminados = 0;

            //leemos la opcion que seleccio el usuario
            usarSincronizacion= chkSincronizacion.Checked;

            ActialzarUI();


            AgregarLog("-----------------------------------------");
            AgregarLog($"▶ Iniciando Simulación..");
            AgregarLog($" Modo: {(usarSincronizacion ? "Con Sincronización 🔐": "SIN Sincronización ⚠")}");
            AgregarLog($" Saldo inicial: {cuenta.ObtenerSaldo():C}");
            AgregarLog($" Operaciones: 10 depositos de $100 | 10 retiros de $80");
            AgregarLog($" Saldo esperado al final: $1,200.00");
            AgregarLog($"-----------------------------------------");

            //desabilitamos el boton de iniciar
            //Para que el usuario no pueda iniciar dos simulaciones a la vez
            btnIniciar.Enabled = false;
            btnCancelar.Enabled = false;

            // Guardamos el token en una variable local para pasárselo
            // a los hilos. Así cada hilo tiene su propia referencia.
            CancellationToken token = cts.Token;


            hiloDeposito = new Thread(() =>
            {

                try
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        //revisamos si el usuario presiono cancelar
                        //si el token fue cancelado, lanza operation

                        token.ThrowIfCancellationRequested();

                        //Pedir permiso, si ya hay dos hilos adentro, este espera
                        semaphore.Wait(token);

                        try
                        {
                            Thread.Sleep(100);

                            if (usarSincronizacion)
                                cuenta.DepositarSeguro(100); //🔐🔐
                            else
                                cuenta.DepositarInseguro(100);

                            //leer el nuevo saldo 
                            int nuevoSaldo = cuenta.ObtenerSaldo();
                            AgregarLog($" [Deposito #{i:D2}] +$100 -> Saldo: {nuevoSaldo}");
                            ActualizarSaldoUI(nuevoSaldo);

                        }
                        finally
                        {
                            //Siempre liberamos el semaforo 
                            semaphore.Release();
                        }

                    }
                    AgregarLog("Hilo de DEPOSITOS completado");
                }
                catch (OperationCanceledException)
                {
                    AgregarLog("Hilo de deposito cancelado por el usuario");
                }
                catch (Exception ex) 
                {
                    AgregarLog($" Error en Depositos: {ex.Message}");

                }
                finally
                {
                    VerificarFinalizacion();
                }





            });


            hiloDeposito.Start();
        }

        private void AgregarLog( string mensaje)
        {
            //Añadimos la hora exacta
            string linea = $"[{DateTime.Now:HH:mm:ss}] {mensaje}";

            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke((Action)(() =>
                {
                    txtLog.AppendText(linea + Environment.NewLine);

                    txtLog.ScrollToCaret();
                }));
            }
            else
            {
                txtLog.AppendText(linea + Environment.NewLine);

                txtLog.ScrollToCaret();
            }
        }

        private void ActualizarSaldoUI(int saldo)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke((Action)(() =>
                {
                    lblSaldo.Text = $"{saldo}";

                    lblSaldo.ForeColor = saldo >= 1000 ? Color.Green : Color.OrangeRed;
                }));
            }
            else
            {
                lblSaldo.Text = $"{lblSaldo}";

            }
        }

        private void VerificarFinalizacion()
        {
            lock (lockFinalizacion)
            {
                hilosTerminados++;

                if(hilosTerminados >= 2)
                {
                    int saldoFinal = cuenta.ObtenerSaldo();

                    //el saldo correcto: 1000 Iniales  + (10 * 100) - (10*80 retiros)

                    bool esCorrecto = saldoFinal == 1200;

                    AgregarLog("-----------------------------");
                    AgregarLog(" RESULTADO FINAL");
                    AgregarLog($"   Saldo Final obtenido: {saldoFinal}");
                    AgregarLog($"   Saldo esperado:     $1,200.00");
                    AgregarLog(esCorrecto
                        ? "CORRECTO - No hubo condicion de carrera"
                        : "INCORRECTO - Condicion de carrera detectada");
                    AgregarLog("-----------------------------");



                    this.Invoke((Action)(() =>
                    {
                        btnIniciar.Enabled = true;
                        btnCancelar.Enabled = false;
                        ActialzarUI();
                    }));

                }
            }
        }

    }
}