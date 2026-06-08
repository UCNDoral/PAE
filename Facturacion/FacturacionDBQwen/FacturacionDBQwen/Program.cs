// =====================================================
// ARCHIVO: Program.cs
// DESCRIPCIÓN: Punto de entrada de la aplicación Windows Forms
// Configura dependencias, inyección y formulario principal
// =====================================================

using Microsoft.EntityFrameworkCore; // Para DbContext
using Microsoft.Extensions.Configuration; // Para leer appsettings.json
using Microsoft.Extensions.DependencyInjection; // Para inyección de dependencias
using FacturacionDBQwen.Data; // Namespace del DbContext
using FacturacionDBQwen.Forms;// Namespace de los formularios

namespace FacturacionDBQwen
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}