// =====================================================
// ARCHIVO: Producto.cs
// DESCRIPCIÓN: Entidad que representa un producto en inventario
// TABLA EN BD: Productos
// =====================================================

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FacturacionDBQwen.Models
{
    /// <summary>
    /// Clase Producto - Catálogo de productos disponibles para venta
    /// </summary>
    [Table("Productos")]
    public class Producto
    {
        // =====================================================
        // PROPIEDADES PRINCIPALES
        // =====================================================

        /// <summary>
        /// Identificador único del producto (Clave Primaria)
        /// Autoincremental en la base de datos
        /// </summary>
        [Key]
        [Column("IdProducto")]
        public int IdProducto { get; set; }

        /// <summary>
        /// Nombre o descripción del producto
        /// Campo obligatorio, máximo 150 caracteres
        /// Ejemplo: "Laptop Dell Inspiron 15"
        /// </summary>
        [Required(ErrorMessage = "El nombre del producto es obligatorio")]
        [StringLength(150, ErrorMessage = "El nombre no puede exceder 150 caracteres")]
        [Column("Nombre")]
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Precio de venta del producto
        /// Tipo decimal para precisión monetaria (no usar double para dinero)
        /// Debe ser mayor a 0
        /// </summary>
        [Column("Precio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }

        /// <summary>
        /// Cantidad de unidades disponibles en inventario
        /// Controla el stock disponible para venta
        /// No puede ser negativo
        /// </summary>
        [Column("Stock")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }

        /// <summary>
        /// Estado del producto en el sistema
        /// true = Producto activo (se puede vender)
        /// false = Producto descontinuado/inactivo
        /// </summary>
        [Column("Activo")]
        public bool Activo { get; set; } = true;

        /// <summary>
        /// Fecha de registro del producto en el sistema
        /// </summary>
        [Column("FechaRegistro")]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // =====================================================
        // PROPIEDADES DE NAVEGACIÓN
        // =====================================================

        /// <summary>
        /// Colección de detalles de factura que incluyen este producto
        /// Relación: Un producto puede aparecer en MUCHAS facturas (1:N)
        /// </summary>
        public virtual ICollection<DetalleFactura>? DetallesFactura { get; set; }

        // =====================================================
        // MÉTODOS
        // =====================================================

        /// <summary>
        /// Verifica si hay stock disponible del producto
        /// </summary>
        /// <returns>true si hay stock > 0, false en caso contrario</returns>
        public bool HayStockDisponible()
        {
            return this.Stock > 0;
        }

        /// <summary>
        /// Calcula el valor total del inventario de este producto
        /// (Precio unitario × Cantidad en stock)
        /// </summary>
        /// <returns>Valor total en inventario</returns>
        [NotMapped]
        public decimal ValorInventario => this.Precio * this.Stock;

        /// <summary>
        /// Representación en texto del producto
        /// Se muestra en combos y listas
        /// </summary>
        public override string ToString()
        {
            return $"{IdProducto} - {Nombre} (${Precio:N2} - Stock: {Stock})";
        }
    }
}
