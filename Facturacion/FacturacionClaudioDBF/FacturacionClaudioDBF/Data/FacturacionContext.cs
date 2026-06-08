using FacturacionClaudioDBF.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FacturacionClaudioDBF.Data
{
    public class FacturacionContext
    {
        // ── LAS 4 TABLAS ─────────────────────────────────────────────────
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<DetalleFactura> DetallesFactura { get; set; }
        public DbSet<Producto> Productos { get; set; }

        // ── CONEXIÓN ─────────────────────────────────────────────────────
        protected override void OnConfiguring(DbContextOptionsBuilder opt)
        {
            opt.UseSqlServer(
                "Server=(localdb)\\;" +
                "Database=FacturacionDB;" +
                "Trusted_Connection=True;" +
                "TrustServerCertificate=True;"
            );
        }

        // ── CONFIGURACIÓN ADICIONAL (OnModelCreating) ──────────────────
        // Aquí podemos configurar precisión de decimales para precios.
        protected override void OnModelCreating(ModelBuilder m)
        {
            // Precio con 10 dígitos en total y 2 decimales (ej: 99999999.99)
            m.Entity<DetalleFactura>().Property(d => d.Precio).HasPrecision(10, 2);
            m.Entity<Producto>().Property(p => p.Precio).HasPrecision(10, 2);
        }

    }
}
