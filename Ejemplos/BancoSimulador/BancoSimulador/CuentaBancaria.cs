using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms.Design.Behavior;

namespace BancoSimulador
{

    public class CuentaBancaria
    {

        // Campo privado  sera el que todos los hilos intenten modificar al mismo tiempo
        //Este es el recurso compartido que debemos proteger
        private int saldo;

        private readonly object _lock = new object(); // Objeto para sincronización
        public CuentaBancaria( int saldoInicial)
        {
            saldo = saldoInicial;
        }


        //METODOS SIN SINCRONIZACION
        public void DepositarInseguro( int cantidad)
        {
            int temporal = saldo;
            temporal = temporal + cantidad;
            Thread.Sleep(1);
            saldo = temporal;
        }

        public void RetirarInseguro( int cantidad)
        {
            int temporal = saldo;
            temporal = temporal - cantidad;
            Thread.Sleep(1);
            saldo = temporal;
        }

        //METODOS CON LOCK
        //Solo un hilo puede ejecutar el bloque de lock a la vez
        public void DepositarSeguro( int cantidad)
        {
            lock (_lock)
            {
                int temporal = saldo;
                temporal = temporal + cantidad;
                Thread.Sleep(1);
                saldo = temporal;
            }
        }

        //retir dineroo seguro

        public void RetirarSeguro( int cantidad)
        {
            lock (_lock)
            {
                int temporal = saldo;
                temporal = temporal - cantidad;
                Thread.Sleep(1);
                saldo = temporal;
            }
        }


        //METODO CON MONITOR este equivale a lock, pero mas explicto

        public void DepositarConMonitor( int cantidad)
        {
            bool adquirido = false;
         
            try
            {
                //Intentar adquirir el candado  y registrar si lo logro
                Monitor.Enter(_lock, ref adquirido);

                saldo = saldo + cantidad;
            }
            finally
            {
                //el blocque finally garantiza que siempre liberemos el candado
                if (adquirido)
                    Monitor.Exit(_lock);
            }
        }

        //LECTURA DEL SALDO con Volatile.Read
        //para asegurar que leemos el valor mas reciente del saldo

        public int ObtenerSaldo()
        {
            return Volatile.Read(ref saldo);
        }



    }


}
