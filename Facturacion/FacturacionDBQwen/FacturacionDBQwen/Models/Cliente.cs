// =====================================================
// ARCHIVO: Cliente.cs
// DESCRIPCIÓN: Entidad que representa un cliente del sistema
// TABLA EN BD: Clientes
// =====================================================

using System;                    // Namespace para tipos básicos como DateTime
using System.Collections.Generic; // Namespace para colecciones como List<T>
using System.ComponentModel.DataAnnotations; // Validaciones de datos
using System.ComponentModel.DataAnnotations.Schema; // Atributos de mapeo a BD

namespace FacturacionDBQwen.Models
{
  
    /// <summary>
    /// Clase Cliente - Representa un cliente en el sistema
    /// Esta clase se mapeará a la tabla "Clientes" en la base de datos
    /// </summary>
    [Table("Clientes")] // Atributo que especifica el nombre exacto de la tabla en SQL Server
    public class Cliente
    {
        // =====================================================
        // PROPIEDADES DE LA ENTIDAD CLIENTE
        // Cada propiedad representa una columna en la tabla
        // =====================================================

        /// <summary>
        /// Identificador único del cliente (Clave Primaria)
        /// Se genera automáticamente en la base de datos (IDENTITY)
        /// </summary>
        [Key] // Indica que esta propiedad es la clave primaria
        [Column("IdCliente")] // Nombre exacto de la columna en la BD
        public int IdCliente { get; set; }

        /// <summary>
        /// Nombre del cliente
        /// Campo obligatorio, máximo 100 caracteres
        /// </summary>
        [Required(ErrorMessage = "El campo Nombre es obligatorio")] // Validación de campo requerido
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres")] // Longitud máxima
        [Column("Nombre")] // Mapeo a columna "Nombre"
        public string Nombre { get; set; } = string.Empty; // Inicialización para evitar null

        /// <summary>
        /// Apellido del cliente
        /// Campo obligatorio, máximo 100 caracteres
        /// </summary>
        [Required(ErrorMessage = "El campo Apellido es obligatorio")]
        [StringLength(100, ErrorMessage = "El apellido no puede tener más de 100 caracteres")]
        [Column("Apellido")]
        public string Apellido { get; set; } = string.Empty;

        /// <summary>
        /// Número de teléfono del cliente
        /// Campo opcional (puede ser null)
        /// Formato de teléfono válido
        /// </summary>
        [Column("Telefono")]
        [Phone(ErrorMessage = "El formato del teléfono no es válido")] // Validación de formato telefónico
        public string? Telefono { get; set; } // El ? indica que puede ser null

        /// <summary>
        /// Dirección física del cliente
        /// Campo opcional, máximo 200 caracteres
        /// </summary>
        [Column("Direccion")]
        [StringLength(200, ErrorMessage = "La dirección no puede tener más de 200 caracteres")]
        public string? Direccion { get; set; }

        /// <summary>
        /// Estado del cliente
        /// true = Cliente activo (puede realizar compras)
        /// false = Cliente inactivo (no puede realizar compras)
        /// Valor por defecto: true (activo)
        /// </summary>
        [Column("Activo")]
        public bool Activo { get; set; } = true; // Valor por defecto

        /// <summary>
        /// Fecha y hora de registro del cliente
        /// Se asigna automáticamente al crear el registro
        /// </summary>
        [Column("FechaRegistro")]
        public DateTime FechaRegistro { get; set; } = DateTime.Now; // Fecha actual por defecto

        /// <summary>
        /// Fecha y hora de la última modificación del registro
        /// Se actualiza automáticamente cada vez que se modifica el cliente
        /// Nullable porque al crear el registro aún no hay modificación
        /// </summary>
        [Column("FechaModificacion")]
        public DateTime? FechaModificacion { get; set; } // Nullable (?)

        // =====================================================
        // PROPIEDADES DE NAVEGACIÓN (RELACIONES)
        // Permiten acceder a datos relacionados sin hacer joins manuales
        // =====================================================

        /// <summary>
        /// Colección de facturas asociadas a este cliente
        /// Relación: Un cliente puede tener MUCHAS facturas (1:N)
        /// virtual permite Lazy Loading (carga diferida)
        /// </summary>
        public virtual ICollection<Factura>? Facturas { get; set; }

        // =====================================================
        // MÉTODOS DE LA CLASE
        // =====================================================

        /// <summary>
        /// Obtiene el nombre completo del cliente (Nombre + Apellido)
        /// Propiedad calculada (no se guarda en BD)
        /// </summary>
        /// <returns>Nombre completo formateado</returns>
        [NotMapped] // Indica que esta propiedad NO se mapea a la base de datos
        public string NombreCompleto => $"{Nombre} {Apellido}";

        /// <summary>
        /// Representación en texto del objeto Cliente
        /// Se usa para mostrar el cliente en combos, listas, etc.
        /// </summary>
        /// <returns>String con nombre completo e ID</returns>
        public override string ToString()
        {
            return $"{IdCliente} - {NombreCompleto}";
        }
    }
}

