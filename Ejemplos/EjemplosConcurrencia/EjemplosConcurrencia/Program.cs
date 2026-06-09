using System.Threading.Tasks.Dataflow;

class program
{
    //Ejemplo 1: Condicion de carrera (RAce condition)
    //Dos hilos incrementan un contador compartido al mismo tiempo / no le vamos a agregar ninguna proteccion


    static int contador = 0;

    static void Main(string[] args)
    {

        Thread hilo1 = new Thread (() =>
        {
            for (int i = 0; i < 100; i++)
            {
                //Esta operacion no es atomica.
                //El CPU la divide en 3 pasos:
                //1. Leer el contador desde la memoria.
                //2. suma 1 al contador.
                //3. Escribe el resultado
                //Otro hilo puede interrumpir en el paso 1 y el 3,
                //se pierde el incremento y el resultado final es incorrecto.

                int temporal = contador;
                Thread.Sleep(1); // Simulamos que el hilo se demora un poco entre leer y escribir, aumentando la probabilidad de que el otro hilo interrumpa.
                contador = temporal + 1;
            }
            Console.WriteLine($" ➡️ hilo 1 termino. Contador en este momento: {contador}");
        });

        Thread hilo2 = new Thread(() =>
        {
            for (int i = 0; i < 100; i++)
            {
                int temporal = contador;
                Thread.Sleep(1); // Simulamos que el hilo se demora un poco entre leer y escribir, aumentando la probabilidad de que el otro hilo interrumpa.
                contador = temporal + 1;
            }
            Console.WriteLine($" ➡️ hilo 2 termino. Contador en este momento: {contador}");
        });


        //Arrancamos ambos hilos AL MISMO TIEMPO
        hilo1.Start();
        hilo2.Start();

        //JoinBlock hace que el Main espere a que ambos hilos terminen
        //antes de continuar. sin esto, el Main podria imprimir el resultado antes que los hilos terminen, mostrando un resultado incorrecto.
        hilo1.Join();
        hilo2.Join();

        Console.WriteLine();
        Console.WriteLine($" Resultado esperado: 2000");
        Console.WriteLine($" Resultado obtenido: ${contador}");
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
