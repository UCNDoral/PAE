using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FacturacionAPPCF.Models
{
    public class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdProducto { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }


        [Required]
        public decimal Precio { get; set; } = decimal.Zero;

        [Required]
        public int Stock { get; set; }

        [Required]
        public bool Activo { get; set; } = true;

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        //esta es la navegacion hacia los detalles de la factura, un producto puede estar en muchos detalles de factura, pero un detalle de factura solo tiene un producto
        public ICollection<DetalleFactura> Detalles { get; set; } = new List<DetalleFactura>();


    }
}
