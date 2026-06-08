using System;
using System.Collections.Generic;
using System.Text;

namespace FacturacionClaudioDBF.Models
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaRegistro { get; set; }

        // ── NAVIGATION PROPERTY ──────────────────────────────
        // Lista de todos los detalles donde aparece este producto.
        public ICollection<DetalleFactura> Detalles { get; set; } = new List<DetalleFactura>();

    }
}
