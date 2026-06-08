using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using FacturacionApp2.Data;

namespace FacturacionApp2
{
    internal static class Program
    {

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
        //leer configuracion desde el appsettings.json

        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();


        var connectionString = config.GetConnectionString("FacturacionDB");


        
        //Opciones de DbContext (LayoutSettings compartiremos de forma global)
        var optionsBuilder = new DbContextOptionsBuilder<FacturacionContext>();
        optionsBuilder.UseSqlServer(connectionString);

        
        AppConfig.DbOptions = optionsBuilder.Options;


            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}