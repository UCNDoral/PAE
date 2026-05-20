using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using FacturacionApp.Data;


namespace FacturacionApp
{
    internal static class Program
    {

        //leer la configuracion desde appsettings.json

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var confg = new ConfigurationBuilder()
                .SetBasePath(AppDomain
                .CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            //Configurar el DBContext con la cadena de conexion
            var ConnectionString = confg.GetConnectionString("FacturacionDB");

            //Opcines dbcontext
            var optionsBuilder = new DbContextOptionsBuilder<Factura>();
            optionsBuilder.UseSqlServer(ConnectionString);

            //guardar las opciones de una clase estatica para usarla en toda la aplicacion
            AppConfig.DbOption = optionsBuilder.Options;


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new FrmForm1());





            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}