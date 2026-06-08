using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaFacturacion.Models
{
    /// <summary>
    /// Encabezado de la factura. 
    /// Una factura tiene UNO o MUCHOS DetalleFactura.
    /// </summary>
    public class Factura
    {
        public int FacturaId { get; set; }
        public string NumeroFactura { get; set; } = string.Empty;
        public int ClienteId { get; set; }
        public DateTime FechaEmision { get; set; }
        public decimal Subtotal { get; set; }
        public decimal IVA { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; } = "ACTIVA";

        // Navegación: cliente asociado (se llena desde el DA)
        public Cliente? Cliente { get; set; }

        // Navegación: líneas de detalle
        public List<DetalleFactura> Detalles { get; set; } = new();
    }
}
