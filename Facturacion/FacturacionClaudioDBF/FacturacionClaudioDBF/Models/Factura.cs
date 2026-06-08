using System;
using System.Collections.Generic;
using System.Text;


namespace FacturacionClaudioDBF.Models
{
    public class Factura
    {
        public int IdFactura { get; set; }

        // ── CLAVE FORÁNEA (Foreign Key) ───────────────────
        // Esta propiedad guarda el Id del cliente al que pertenece.
        // EF la usa para crear la relación en SQL Server:
        //   FOREIGN KEY (IdCliente) REFERENCES Clientes(IdCliente)
        public int IdCliente { get; set; }

        public DateTime Fecha { get; set; }

        // ── NAVIGATION PROPERTY → PADRE (N:1) ────────────
        // Accede al objeto Cliente completo desde una factura.
        // Ejemplo: factura.Cliente.Nombre
        public Cliente Cliente { get; set; } = null!;

        // ── NAVIGATION PROPERTY → HIJOS (1:N) ────────────
        // Lista de renglones/detalles de esta factura.
        public ICollection<DetalleFactura> Detalles { get; set; } = new List<DetalleFactura>();

    }
}
