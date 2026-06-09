namespace Semana70
{
    internal class Program
    {

        class Producto
        {
            public string Nombre { get; set; }
            public decimal Precio { get; set; }

            public Producto(string nombre, decimal precio)
            {
                Nombre = nombre;
                Precio = precio;
            }

            public string ObtenerInformacion()
             {
                 return $"{Nombre} - ${Precio}";
            }
        }
        static void Main(string[] args)
        {
            List<Producto> productos = new List<Producto>
            {
                new Producto("Laptop", 999.99m),
                new Producto("Smartphone", 499.99m),
                new Producto("Tablet", 299.99m)
            };

            productos.Add(new Producto("Smartwatch", 199.99m));

            foreach (var producto in productos)
            {
                Console.WriteLine(producto.ObtenerInformacion());
            }

            productos.RemoveAt(0);


            foreach (var producto in productos)
            {
                Console.WriteLine(producto.ObtenerInformacion());
            }



            var produc = productos.Find(p => p.Nombre == "Laptop");

            if (produc != null)
            {
                Console.WriteLine($"Producto encontrado: {produc.ObtenerInformacion()}");
            }
            else
            {
                Console.WriteLine("Producto no encontrado." );
            }
        }
    }
}
