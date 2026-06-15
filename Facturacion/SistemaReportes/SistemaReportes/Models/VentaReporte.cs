using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaReportes.Model
{
    // Esta clase representa UNA fila del reporte de ventas.
    // Cada propiedad corresponde a una columna del resultado SQL.
    public class VentaReporte
    {
        public int IdVenta { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public string Producto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnit { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaVenta { get; set; }
    }
}
