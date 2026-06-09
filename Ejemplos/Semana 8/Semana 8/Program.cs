namespace Semana_8
{
    internal class Program
    {
        static int contador = 0;
        static object candado = new object();
        static int INCREMENTOS = 1000000;

        static void Main(string[] args)
        {
            //Libreta compartida 
            Console.WriteLine("Simulación de Hilos");

            //Escenario 1: Sin Proteccion
            EjecutarSimulacion(false);



            //Escenario 2: Con Proteccion
            EjecutarSimulacion(true);


        }


        static void EjecutarSimulacion(bool usarLok)
        {
            Console.WriteLine($"\nIniciando con lock = {usarLok}");

            //creamois dos hilos que hataran la misma tarea
            Thread hilo1 = new Thread( () => RealizarTrabajo(usarLok));
            Thread hilo2 = new Thread(() => RealizarTrabajo(usarLok));

            hilo1.Start();
            hilo2.Start();

            //esperamos a que ambos teminen
            hilo1.Join();
            hilo2.Join();


            int esperado = INCREMENTOS * 2;
            Console.WriteLine($"Resultado final: {contador}");
            Console.WriteLine($"Resultado esperado: {esperado}");

            if (contador == esperado)
                Console.WriteLine(" ✅ los datos son correctos");
            else
                Console.WriteLine($" Error! se perdieron {esperado - contador} incrementos");
            

        }

        static void RealizarTrabajo(bool usarLok) 
        {
            for (int i = 0; i < INCREMENTOS; i++) 
            {
                if (usarLok)
                {
                    lock (candado)
                    {
                        contador++;
                    }

                }
                else
                {
                    contador++; //operacio insegura, puede generar condiciones de carrera
                }
            }
        }
    }
}
