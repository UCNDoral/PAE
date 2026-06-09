using System;
using System.Collections.Generic;
using System.Text;


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FacturacionAPPCF.Models
{
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCliente { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Nombre { get; set; }

        public string? Apellido1 { get; set; }

        public string? Apellido2 { get; set; }

        [MaxLength(20)]
        public string? Telefono { get; set; }

        public string Direccion { get; set; }
        public  bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime FechaModificacion { get; set; }


        //propiedad de navegacion: no genera una columna en la base de datos, es para relacionar con otra tabla
        public ICollection<Factura> Facturas { get; set; } = new List<Factura>();
    }
}
