using System;
using System.Collections.Generic;
using System.Text;

// =====================================================
// ARCHIVO: DetalleFactura.cs
// DESCRIPCIÓN: Entidad que representa un ítem/detalle de factura
// TABLA EN BD: DetalleFactura
// =====================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FacturacionDBQwen.Models
{
    /// <summary>
    /// Clase DetalleFactura - Línea individual de una factura
    /// Representa cada producto vendido en una factura
    /// </summary>
    [Table("DetalleFactura")]
    public class DetalleFactura
    {
        // =====================================================
        // PROPIEDADES PRINCIPALES
        // =====================================================

        /// <summary>
        /// Identificador único del detalle (Clave Primaria)
        /// </summary>
        [Key]
        [Column("IdDetalle")]
        public int IdDetalle { get; set; }

        /// <summary>
        /// Identificador de la factura a la que pertenece (Clave Foránea)
        /// Referencia a la tabla Facturas
        /// </summary>
        [Column("IdFactura")]
        [ForeignKey("Factura")]
        public int IdFactura { get; set; }

        /// <summary>
        /// Identificador del producto vendido (Clave Foránea)
        /// Referencia a la tabla Productos
        /// </summary>
        [Column("IdProducto")]
        [ForeignKey("Producto")]
        public int IdProducto { get; set; }

        /// <summary>
        /// Cantidad de unidades del producto vendidas
        /// Debe ser al menos 1
        /// </summary>
        [Column("Cantidad")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1")]
        public int Cantidad { get; set; }

        /// <summary>
        /// Precio unitario del producto al momento de la venta
        /// Se guarda el precio histórico (puede cambiar en el catálogo)
        /// </summary>
        [Column("Precio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }

        // =====================================================
        // PROPIEDADES DE NAVEGACIÓN
        // =====================================================

        /// <summary>
        /// Factura a la que pertenece este detalle
        /// Relación: Muchos detalles pertenecen a UNA factura (N:1)
        /// </summary>
        public virtual Factura? Factura { get; set; }

        /// <summary>
        /// Producto vendido en este detalle
        /// Relación: Muchos detalles pueden ser de UN producto (N:1)
        /// </summary>
        public virtual Producto? Producto { get; set; }

        // =====================================================
        // PROPIEDADES CALCULADAS
        // =====================================================

        /// <summary>
        /// Calcula el subtotal de este detalle
        /// (Cantidad × Precio unitario)
        /// </summary>
        [NotMapped]
        public decimal Subtotal => this.Cantidad * this.Precio;

        // =====================================================
        // MÉTODOS
        // =====================================================

        /// <summary>
        /// Representación en texto del detalle
        /// </summary>
        public override string ToString()
        {
            string nombreProducto = Producto?.Nombre ?? $"Producto #{IdProducto}";
            return $"{Cantidad} x {nombreProducto} @ ${Precio:N2} = ${Subtotal:N2}";
        }
    }
}

