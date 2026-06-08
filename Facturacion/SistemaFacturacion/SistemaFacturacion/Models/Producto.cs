using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaFacturacion.Models
{
    /// <summary>
    /// Representa un producto del catálogo.
    /// </summary>
    public class Producto
    {
        public int ProductoId { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal PrecioUnitario { get; set; }
        public int Stock { get; set; }
        public bool Activo { get; set; } = true;

        // Para mostrar en ComboBox o grillas
        public override string ToString() => $"[{Codigo}] {Nombre}";
    }
}
