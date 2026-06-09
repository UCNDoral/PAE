namespace Semana_7
{
    internal class Program
    {
        class Cancion
        {
            public string Titulo { get; set; }
            public string Artista { get; set; }
            public string Album { get; set; }
            public int Duracion { get; set; } // Duración en segundos
            public Cancion(string titulo, string artista, string album, int duracion)
            {
                Titulo = titulo;
                Artista = artista;
                Album = album;
                Duracion = duracion;
            }
            public string ObtenerInfo()
            {
                return $"{Titulo} - {Artista} ({Album}) [{Duracion} segundos]";
            }
        }



        static void Main(string[] args)
        {

            //declarar la instancia de la lista
            //creamos una nueva lista 
            List<Cancion> miplayLists = new List<Cancion>();

            Console.WriteLine($"La lista esta vacia. cantidad: {miplayLists.Count}");



            //agregar elementos a la lista
            miplayLists.Add(new Cancion("Bohemian Rhapsody", "Queen", "A Night at the Opera", 354));
            miplayLists.Add(new Cancion("Amor Fritanguero", "La cuneta", "xd", 3));
            miplayLists.Add(new Cancion("La Camisa Negra", "Juanes", "Mi Sangre", 200));

            Console.WriteLine($"La lista tiene {miplayLists.Count} canciones.");


            //Recorrer la lista
            Console.WriteLine("\n MY PLAYLIST");

            //un ciclo Foreach
            foreach (var cancion in miplayLists)
            {
                Console.WriteLine(cancion.ObtenerInfo());
            }

            //eliminar elementos de la lista
            Console.WriteLine($"Eliminando la primera canción");
            miplayLists.RemoveAt(0); // Elimina la primera canción

            Console.WriteLine($"La lista tiene {miplayLists.Count} canciones.");

            Console.WriteLine("\n Playlist Actualizada");

            for (int i = 0; i < miplayLists.Count; i++)
            {
                Console.WriteLine($"Pista #{i + 1}: {miplayLists[i].Titulo}");
            }   

            Console.ReadLine();
        }
    }
}
