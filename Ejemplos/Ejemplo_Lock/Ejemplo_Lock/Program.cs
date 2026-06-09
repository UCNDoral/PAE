using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace Ejemplo_Lock
{
    internal class Program
    {

        static  int contador = 0;
        //objeto candado (Lock)  es la llave del baño
        //solo permite que solo un hilo pueda teter la llave a la vez, evitando que ambos hilos entren al baño al mismo tiempo y se peleen por el papel higienico (recurso compartido).
        //REGLAS: siempre debe de ser private y readonly, para evitar que otros objetos puedan accederlo o modificarlo, lo que podria causar problemas de sincronizacion.
        static readonly object _lock = new object();
        static void Main(string[] args)
        {



            Thread hilo1 = new Thread(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    //lock(_lock) significa
                    //quiero entrar. si alguien mas esta adentro, espero.
                    //Cuando el hilo anterior sale, este entra y cierra puerta

                    lock (_lock) 
                    {
                        contador++;
                    }
                    //al salir del bloqueo, el candado se libera y el otro puede entrar  
                }
                Console.WriteLine($" ➡️ hilo 1 termino. Contador en este momento: {contador}");
            });



            Thread hilo2 = new Thread(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    lock (_lock)
                    {
                        int temporal = contador;
                        Thread.Sleep(1); // Simulamos que el hilo se demora un poco entre leer y escribir, aumentando la probabilidad de que el otro hilo interrumpa.
                        contador = temporal + 1;
                    }
                }
                Console.WriteLine($" ➡️ hilo 2 termino. Contador en este momento: {contador}");
            });

            //medir medir el tiempo para ver cuanto tarda lock
            var cronometro = Stopwatch.StartNew();


            //Arrancamos ambos hilos AL MISMO TIEMPO
            hilo1.Start();
            hilo2.Start();

            //JoinBlock hace que el Main espere a que ambos hilos terminen
            //antes de continuar. sin esto, el Main podria imprimir el resultado antes que los hilos terminen, mostrando un resultado incorrecto.
            hilo1.Join();
            hilo2.Join();

            cronometro.Stop();

            Console.WriteLine();
            Console.WriteLine($" Resultado esperado: 2000");
            Console.WriteLine($" Resultado obtenido: ${contador}");
            Console.WriteLine($" Tiempo Trascurrido: ${cronometro.ElapsedMilliseconds} ms");

            Console.WriteLine();

            if (contador == 2000)
            {
                Console.WriteLine("Correcto (tuvieron suerte!");
            }
            else
            {
                Console.WriteLine($"Incorrecto (condicion de carrera) - se perdieron {200 - contador} incrementos");
            }
        }
    }
}

