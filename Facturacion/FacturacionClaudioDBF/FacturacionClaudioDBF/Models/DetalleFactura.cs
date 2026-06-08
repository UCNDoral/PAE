using System;
using System.Collections.Generic;
using System.Text;


namespace FacturacionClaudioDBF.Models
{
    public class DetalleFactura
    {
        public int IdDetalle { get; set; }

        // ── CLAVES FORÁNEAS ───────────────────────────────
        // A qué factura pertenece este renglón
        public int IdFactura { get; set; }

        // Qué producto se está vendiendo en este renglón
        public int IdProducto { get; set; }

        // ── DATOS DEL RENGLÓN ──────────────────────────────
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }   // precio al momento de la venta

        // ── NAVIGATION PROPERTIES ──────────────────────────
        // Acceder a la factura padre: detalle.Factura.Fecha
        public Factura Factura { get; set; } = null!;

        // Acceder al producto: detalle.Producto.Nombre
        public Producto Producto { get; set; } = null!;

    }
}
