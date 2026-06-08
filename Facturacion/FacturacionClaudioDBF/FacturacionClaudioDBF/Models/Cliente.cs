using System;
using System.Collections.Generic;
using System.Text;

namespace FacturacionClaudioDBF.Models
{
    public class Cliente
    {
        // ── CLAVE PRIMARIA ──────────────────────────────────
        public int IdCliente { get; set; }

        // ── COLUMNAS ────────────────────────────────────────
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }

        // ── NAVIGATION PROPERTY (relación 1:N) ────────────
        // Un cliente TIENE MUCHAS facturas.
        // EF crea el JOIN automáticamente con Include().
        // ICollection porque es una colección (0 o más elementos).
        public ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    }
}
