using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaFacturacion.Models
{
    /// <summary>
    /// Clase plana para el reporte de ventas por período.
    /// Cada instancia = una factura con su resumen.
    /// </summary>
    public class RptVentaItem
    {
        public string NumeroFactura { get; set; } = "";
        public string FechaEmision { get; set; } = "";
        public string Cliente { get; set; } = "";
        public decimal Subtotal { get; set; }
        public decimal IVA { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; } = "";
        public int CantLineas { get; set; }
    }
}
