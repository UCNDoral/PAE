using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FacturacionAPPCF.Models
{
    public class Factura
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdFactura { get; set; }

        [Required]
        public int IdCliente { get; set; } // llave foranea

        public DateTime Fecha { get; set; } = DateTime.Now;

        [ForeignKey("IdCliente")]
        public Cliente Cliente { get; set; } // propiedad de navegacion

        //Navegacion hacia los detalles de la factura, una factura puede tener muchos detalles, pero un detalle solo pertenece a una factura
        public ICollection<DetalleFactura> Detalles { get; set; } = new List<DetalleFactura>();


        public decimal Total => Detalles.Sum(d => d.Precio * d.Cantidad);
    }


    public class DetalleFactura
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdDetalle { get; set; }
        [Required]
        public int IdFactura { get; set; } // llave foranea
        [Required]
        public int IdProducto { get; set; } // llave foranea
        public string? Descripcion { get; set; }
        [Required]
        public decimal Precio { get; set; }
        [Required]
        public int Cantidad { get; set; }

        [ForeignKey("IdFactura")]
        public Factura Factura { get; set; } // propiedad de navegacion

        [ForeignKey("IdProducto")]
        public Producto Producto { get; set; } // propiedad de navegacion
    }
}
