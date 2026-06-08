using Microsoft.EntityFrameworkCore;
using FacturacionApp.Data;

namespace FacturacionApp
{
    internal static class AppConfig
    {
        // Se asigna en Program.Main antes de su uso.
        public static DbContextOptions<Factura>? DbOption { get; set; }
    }
}
