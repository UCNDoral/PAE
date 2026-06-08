using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaFacturacion.Models
{
    /// <summary>
    /// Cada línea de producto dentro de una factura.
    /// </summary>
    public class DetalleFactura
    {
        public int DetalleId { get; set; }
        public int FacturaId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        // Calculado igual que la BD: Cantidad * PrecioUnitario
        public decimal Subtotal => Cantidad * PrecioUnitario;

        // Navegación: nombre del producto (para mostrar en grilla)
        public Producto? Producto { get; set; }
    }
}
