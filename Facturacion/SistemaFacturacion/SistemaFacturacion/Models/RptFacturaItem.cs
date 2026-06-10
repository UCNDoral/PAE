using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaFacturacion.Models
{
    /// <summary>
    /// Clase plana para el reporte de factura individual.
    /// Cada instancia = una línea de detalle de la factura.
    /// Los datos del encabezado se repiten en cada fila.
    /// </summary>
    public class RptFacturaItem
    {
        // ── Datos del encabezado (iguales en todas las filas) ──
        public string NumeroFactura { get; set; } = "";
        public string FechaEmision { get; set; } = "";
        public string ClienteNombre { get; set; } = "";
        public string ClienteEmail { get; set; } = "";
        public string ClienteTelefono { get; set; } = "";
        public string ClienteDireccion { get; set; } = "";
        public string Estado { get; set; } = "";

        // ── Datos de cada línea de producto ──
        public string CodigoProducto { get; set; } = "";
        public string NombreProducto { get; set; } = "";
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }

        // ── Totales generales de la factura ──
        public decimal TotalSubtotal { get; set; }
        public decimal TotalIVA { get; set; }
        public decimal TotalFactura { get; set; }
    }
}
