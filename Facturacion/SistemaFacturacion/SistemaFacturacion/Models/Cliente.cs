using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaFacturacion.Models
{
    /// <summary>
    /// Representa un cliente del sistema.
    /// Mapea 1 a 1 con la tabla [Cliente] de la BD.
    /// </summary>
    public class Cliente
    {
        public int ClienteId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }

        // Propiedad calculada para mostrar nombre completo en listas
        public string NombreCompleto => $"{Nombre} {Apellido}";
     
    }
}
