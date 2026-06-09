<<<<<<< HEAD
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




=======
﻿namespace BancoSimulador
{
    public partial class Form1 : Form
    {
        // ─────────────────────────────────────────────────────────────
        // VARIABLES COMPARTIDAS ENTRE HILOS
        // Estas variables son accedidas por múltiples hilos al mismo
        // tiempo, por eso debemos tener cuidado con cómo las usamos.
        // ─────────────────────────────────────────────────────────────

        // La cuenta bancaria que ambos hilos van a modificar
        private CuentaBancaria cuenta = new CuentaBancaria(1000);

        // CancellationTokenSource: es quien EMITE la señal de cancelación.
        // Cuando llamamos cts.Cancel(), todos los hilos que usen su token
        // recibirán la señal y podrán terminar limpiamente.
        private CancellationTokenSource cts;

        // Los dos hilos de trabajo: uno deposita, otro retira
        private Thread hiloDepositos;
        private Thread hiloRetiros;

        // SemaphoreSlim(2, 2): permite que máximo 2 hilos entren
        // al recurso al mismo tiempo. Si ya hay 2 adentro, el tercero
        // espera hasta que alguno salga y libere un espacio.
        private SemaphoreSlim semaphore = new SemaphoreSlim(2, 2);

        // Controla si usamos sincronización o no (lo elige el usuario)
        private bool usarSincronizacion = true;

        // Contador para saber cuántos hilos han terminado.
        // Lo usamos para detectar cuando AMBOS hilos finalizaron.
        private int hilosTerminados = 0;

        // Objeto de bloqueo para proteger el contador de hilos terminados.
        // Necesitamos esto porque ambos hilos actualizan el contador al mismo tiempo.
        private readonly object lockFinalizacion = new object();

        // ─────────────────────────────────────────────────────────────
        // CONSTRUCTOR
        // ─────────────────────────────────────────────────────────────
>>>>>>> 291eee6f95dd3218b95768c67933d7bf9cc3b88d
        public Form1()
        {
            InitializeComponent();

<<<<<<< HEAD
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

=======
            // Mostramos el saldo inicial en la pantalla al arrancar
            ActualizarUI();
        }

        // ─────────────────────────────────────────────────────────────
        // BOTÓN: INICIAR SIMULACIÓN
        // Crea y arranca los dos hilos de trabajo
        // ─────────────────────────────────────────────────────────────
        private void btnIniciar_Click(object sender, EventArgs e)
        {
            // Reiniciamos la cuenta con saldo inicial de $1000
            // para que cada simulación empiece igual
            cuenta = new CuentaBancaria(1000);

            // Creamos un nuevo token de cancelación fresco.
            // Si el anterior fue cancelado, este empieza limpio.
            cts = new CancellationTokenSource();

            // Reiniciamos el contador de hilos terminados
            hilosTerminados = 0;

            // Leemos la opción que el usuario eligió en el checkbox
            usarSincronizacion = chkSincronizacion.Checked;

            // Actualizamos el saldo en pantalla
            ActualizarUI();

            // Mostramos en el log qué modo se está usando
            AgregarLog("─────────────────────────────────────────");
            AgregarLog($"▶ Iniciando simulación...");
            AgregarLog($"   Modo: {(usarSincronizacion ? "CON sincronización 🔒" : "SIN sincronización ⚠️")}");
            AgregarLog($"   Saldo inicial: {cuenta.ObtenerSaldo():C}");
            AgregarLog($"   Operaciones: 10 depósitos de $100 | 10 retiros de $80");
            AgregarLog($"   Saldo esperado al final: $1,200.00");
            AgregarLog("─────────────────────────────────────────");

            // Deshabilitamos el botón Iniciar y habilitamos Cancelar
            // para que el usuario no pueda iniciar dos simulaciones a la vez
            btnIniciar.Enabled = false;
            btnCancelar.Enabled = true;

            // Guardamos el token en una variable local para pasárselo
            // a los hilos. Así cada hilo tiene su propia referencia.
            CancellationToken token = cts.Token;

            // ─────────────────────────────────────────────────────
            // HILO 1: DEPÓSITOS
            // Este hilo realiza 10 depósitos de $100 cada uno
            // ─────────────────────────────────────────────────────
            hiloDepositos = new Thread(() =>
            {
                try
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        // Revisamos si el usuario presionó Cancelar.
                        // Si el token fue cancelado, lanza OperationCanceledException
                        // y saltamos directo al bloque catch de abajo.
                        token.ThrowIfCancellationRequested();

                        // Pedimos permiso al semáforo para entrar.
                        // Si ya hay 2 hilos adentro, este espera aquí
                        // hasta que alguno libere su lugar.
                        semaphore.Wait(token);

                        try
                        {
                            // Simulamos que la operación bancaria tarda un poco
                            Thread.Sleep(300);

                            // Depositamos usando el método que el usuario eligió
                            if (usarSincronizacion)
                                cuenta.DepositarSeguro(100);   // Con lock 🔒
                            else
                                cuenta.DepositarInseguro(100); // Sin protección ⚠️

                            // Leemos el saldo actualizado para mostrarlo
                            int nuevoSaldo = cuenta.ObtenerSaldo();

                            // Actualizamos el log y la pantalla desde el hilo
                            // (usamos Invoke porque la UI solo puede tocarse
                            //  desde el hilo principal)
                            AgregarLog($"💰 [Depósito #{i:D2}] +$100 → Saldo: {nuevoSaldo:C}");
                            ActualizarSaldoUI(nuevoSaldo);
                        }
                        finally
                        {
                            // SIEMPRE liberamos el semáforo aunque haya un error.
                            // Si no lo liberamos, los demás hilos quedan bloqueados forever.
                            semaphore.Release();
                        }
                    }

                    AgregarLog("✅ Hilo de DEPÓSITOS completado.");
                }
                catch (OperationCanceledException)
                {
                    // El usuario presionó Cancelar. Salimos limpiamente.
                    AgregarLog("🛑 Hilo de DEPÓSITOS cancelado por el usuario.");
                }
                catch (Exception ex)
                {
                    // Cualquier otro error inesperado lo mostramos en el log
                    AgregarLog($"❌ Error en depósitos: {ex.Message}");
                }
                finally
                {
                    // Este bloque se ejecuta SIEMPRE: al terminar normal,
                    // al cancelar, o si hubo un error. Avisamos que este
                    // hilo terminó para saber cuándo acabaron los dos.
                    VerificarFinalizacion();
                }
            });

            // ─────────────────────────────────────────────────────
            // HILO 2: RETIROS
            // Este hilo realiza 10 retiros de $80 cada uno
            // ─────────────────────────────────────────────────────
            hiloRetiros = new Thread(() =>
            {
                try
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        // Misma verificación de cancelación que en el hilo de depósitos
                        token.ThrowIfCancellationRequested();

                        // Pedimos permiso al semáforo
                        semaphore.Wait(token);

                        try
                        {
                            // Este hilo va un poco más lento que el de depósitos
                            // para que los mensajes del log sean más variados
                            Thread.Sleep(400);

                            if (usarSincronizacion)
                                cuenta.RetirarSeguro(80);   // Con lock 🔒
                            else
                                cuenta.RetirarInseguro(80); // Sin protección ⚠️

                            int nuevoSaldo = cuenta.ObtenerSaldo();

                            AgregarLog($"🏧 [Retiro  #{i:D2}]  -$80  → Saldo: {nuevoSaldo:C}");
                            ActualizarSaldoUI(nuevoSaldo);
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    }

                    AgregarLog("✅ Hilo de RETIROS completado.");
                }
                catch (OperationCanceledException)
                {
                    AgregarLog("🛑 Hilo de RETIROS cancelado por el usuario.");
                }
                catch (Exception ex)
                {
                    AgregarLog($"❌ Error en retiros: {ex.Message}");
                }
                finally
                {
                    VerificarFinalizacion();
                }
            });

            // IsBackground = true significa que si el usuario cierra
            // la ventana, los hilos se detienen automáticamente.
            // Sin esto, el programa seguiría corriendo en segundo plano.
            hiloDepositos.IsBackground = true;
            hiloRetiros.IsBackground = true;

            // ¡Arrancamos los dos hilos!
            // A partir de aquí corren en paralelo de forma independiente.
            hiloDepositos.Start();
            hiloRetiros.Start();
        }

        // ─────────────────────────────────────────────────────────────
        // BOTÓN: CANCELAR
        // Envía la señal de cancelación a ambos hilos
        // ─────────────────────────────────────────────────────────────
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            // Cancel() activa el token. La próxima vez que cualquier hilo
            // llame a token.ThrowIfCancellationRequested() o semaphore.Wait(token),
            // recibirá la señal y terminará limpiamente.
            cts?.Cancel();

            AgregarLog("⚠️  Señal de cancelación enviada a los hilos...");

            // Deshabilitamos el botón para no cancelar dos veces
            btnCancelar.Enabled = false;
        }

        // ─────────────────────────────────────────────────────────────
        // VERIFICAR SI AMBOS HILOS TERMINARON
        // Este método lo llaman los dos hilos al finalizar.
        // Usamos lock para que el conteo sea seguro (es un recurso compartido).
        // ─────────────────────────────────────────────────────────────
        private void VerificarFinalizacion()
        {
            lock (lockFinalizacion)
            {
                hilosTerminados++;

                // Si los dos hilos ya terminaron, mostramos el resultado final
                if (hilosTerminados >= 2)
                {
                    int saldoFinal = cuenta.ObtenerSaldo();

                    // El saldo correcto sería:
                    // $1000 inicial + (10 × $100 depósitos) - (10 × $80 retiros)
                    // = $1000 + $1000 - $800 = $1200
                    bool esCorrecto = saldoFinal == 1200;

                    AgregarLog("─────────────────────────────────────────");
                    AgregarLog("📊 RESULTADO FINAL:");
                    AgregarLog($"   Saldo final obtenido: {saldoFinal:C}");
                    AgregarLog($"   Saldo esperado:       $1,200.00");
                    AgregarLog(esCorrecto
                        ? "   ✅ CORRECTO — No hubo condición de carrera"
                        : "   ❌ INCORRECTO — ¡Condición de carrera detectada!");
                    AgregarLog("─────────────────────────────────────────");

                    // Rehabilitamos los botones desde el hilo principal
                    // usando Invoke, ya que estamos en un hilo secundario
                    this.Invoke((Action)(() =>
                    {
                        btnIniciar.Enabled = true;
                        btnCancelar.Enabled = false;
                        ActualizarUI();
                    }));
                }
            }
        }

        // ─────────────────────────────────────────────────────────────
        // ACTUALIZAR EL SALDO EN LA PANTALLA (thread-safe)
        // Los hilos NO pueden tocar la UI directamente.
        // Usamos Invoke para ejecutar el cambio en el hilo principal.
        // ─────────────────────────────────────────────────────────────
        private void ActualizarSaldoUI(int saldo)
        {
            // InvokeRequired nos dice si estamos en un hilo secundario
            if (lblSaldo.InvokeRequired)
            {
                // Invoke empaqueta el código y lo manda a ejecutar
                // en el hilo principal (el único que puede tocar la UI)
                lblSaldo.Invoke((Action)(() =>
                {
                    lblSaldo.Text = $"{saldo:C}";

                    // Cambiamos el color según si el saldo es alto o bajo
                    lblSaldo.ForeColor = saldo >= 1000
                        ? System.Drawing.Color.Green
                        : System.Drawing.Color.OrangeRed;
                }));
            }
            else
            {
                // Si ya estamos en el hilo principal, actualizamos directo
                lblSaldo.Text = $"{saldo:C}";
            }
        }

        // ─────────────────────────────────────────────────────────────
        // ACTUALIZAR UI CON EL SALDO INICIAL
        // ─────────────────────────────────────────────────────────────
        private void ActualizarUI()
        {
            lblSaldo.Text = $"{cuenta.ObtenerSaldo():C}";
            lblSaldo.ForeColor = System.Drawing.Color.Green;
        }

        // ─────────────────────────────────────────────────────────────
        // AGREGAR MENSAJE AL LOG (thread-safe)
        // Igual que ActualizarSaldoUI, usamos Invoke para tocar la UI
        // desde los hilos secundarios de forma segura.
        // ─────────────────────────────────────────────────────────────
        private void AgregarLog(string mensaje)
        {
            // Añadimos la hora exacta a cada mensaje para ver el orden real
            string linea = $"[{DateTime.Now:HH:mm:ss.fff}] {mensaje}";

            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke((Action)(() =>
                {
                    txtLog.AppendText(linea + Environment.NewLine);

                    // Hacemos scroll automático para ver siempre el último mensaje
                    txtLog.ScrollToCaret();
                }));
            }
            else
            {
                txtLog.AppendText(linea + Environment.NewLine);
                txtLog.ScrollToCaret();
            }
        }

        // ─────────────────────────────────────────────────────────────
        // BOTÓN: LIMPIAR LOG
        // ─────────────────────────────────────────────────────────────
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
>>>>>>> 291eee6f95dd3218b95768c67933d7bf9cc3b88d
        }
    }
}