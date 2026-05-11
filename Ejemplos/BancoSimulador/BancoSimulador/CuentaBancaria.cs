//Clase cuenta bancaria

using System.Threading;

namespace BancoSimulador
{
    // ─────────────────────────────────────────────────────────────────
    // Clase que representa una cuenta bancaria compartida entre hilos.
    // Tiene métodos CON y SIN sincronización para que puedas comparar
    // qué pasa en cada caso durante la simulación.
    // ─────────────────────────────────────────────────────────────────
    public class CuentaBancaria
    {
        // Campo privado que todos los hilos intentan modificar al mismo tiempo.
        // Este es el "recurso compartido" que debemos proteger.
        private int saldo;

        // Objeto usado como "candado" para el lock.
        // REGLA: siempre debe ser privado y readonly para evitar deadlocks.
        private readonly object _lock = new object();

        // Constructor: recibe el saldo inicial de la cuenta
        public CuentaBancaria(int saldoInicial)
        {
            this.saldo = saldoInicial;
        }

        // ═════════════════════════════════════════════════════════════
        // MÉTODOS SIN SINCRONIZACIÓN ⚠️
        // Demuestran la condición de carrera (race condition).
        // ═════════════════════════════════════════════════════════════

        // Deposita dinero SIN protección.
        // La operación saldo += cantidad NO es atómica, el CPU la divide en:
        //   PASO 1: Leer saldo desde memoria
        //   PASO 2: Sumar cantidad en un registro temporal
        //   PASO 3: Escribir el resultado de vuelta en saldo
        // Si otro hilo interrumpe entre el PASO 1 y el PASO 3,
        // se pierden operaciones y el saldo final es incorrecto.
        public void DepositarInseguro(int cantidad)
        {
            int temporal = saldo;   // PASO 1: lectura
            temporal += cantidad;   // PASO 2: modificación local
            Thread.Sleep(1);        // Simula tiempo de proceso
                                    // (aumenta la probabilidad de que otro
                                    //  hilo interrumpa justo aquí)
            saldo = temporal;       // PASO 3: escritura (puede pisar el valor
                                    //         que otro hilo ya escribió)
        }

        // Retira dinero SIN protección. Misma problemática que DepositarInseguro.
        public void RetirarInseguro(int cantidad)
        {
            int temporal = saldo;
            temporal -= cantidad;
            Thread.Sleep(1);
            saldo = temporal;
        }

        // ═════════════════════════════════════════════════════════════
        // MÉTODOS CON LOCK 🔒
        // Solo UN hilo puede ejecutar el bloque lock a la vez.
        // Los demás hilos quedan en espera hasta que el primero termine.
        // ═════════════════════════════════════════════════════════════

        // Deposita dinero de forma segura usando lock.
        // lock(_lock) garantiza que aunque 10 hilos lleguen al mismo tiempo,
        // solo uno entra al bloque, hace su trabajo, y sale antes de que
        // entre el siguiente.
        public void DepositarSeguro(int cantidad)
        {
            lock (_lock)  // ← Adquiere el candado
            {
                // ── SECCIÓN CRÍTICA ──────────────────────────────
                // Solo UN hilo llega aquí a la vez.
                // Los demás esperan afuera hasta que este salga.
                int temporal = saldo;
                temporal += cantidad;
                Thread.Sleep(1);    // El mismo retardo, pero ahora es seguro
                saldo = temporal;
                // ────────────────────────────────────────────────
            }             // ← Libera el candado automáticamente
        }

        // Retira dinero de forma segura usando lock.
        public void RetirarSeguro(int cantidad)
        {
            lock (_lock)
            {
                int temporal = saldo;
                temporal -= cantidad;
                Thread.Sleep(1);
                saldo = temporal;
            }
        }

        // ═════════════════════════════════════════════════════════════
        // MÉTODO CON MONITOR (equivalente a lock, más explícito)
        // lock es internamente un "shortcut" de Monitor.Enter/Exit.
        // Se usa cuando necesitas más control (ej: timeout).
        // ═════════════════════════════════════════════════════════════

        public void DepositarConMonitor(int cantidad)
        {
            bool adquirido = false;
            try
            {
                // Intenta adquirir el candado y registra si lo logró
                Monitor.Enter(_lock, ref adquirido);

                // Sección crítica (igual que con lock)
                saldo += cantidad;
            }
            finally
            {
                // El bloque finally garantiza que SIEMPRE liberamos el
                // candado, incluso si ocurre una excepción adentro.
                if (adquirido)
                    Monitor.Exit(_lock);
            }
        }

        // ═════════════════════════════════════════════════════════════
        // LECTURA DEL SALDO con Volatile.Read
        // Volatile garantiza que leemos el valor real desde la memoria
        // principal, no desde la caché local del procesador.
        // Sin Volatile, un hilo podría leer un valor desactualizado.
        // ═════════════════════════════════════════════════════════════
        public int ObtenerSaldo()
        {
            return Volatile.Read(ref saldo);
        }
    }
}