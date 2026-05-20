using System;
using System.Collections.Generic;

namespace FacturacionApp.Model;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Apellido1 { get; set; }

    public string? Apellido2 { get; set; }

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public string? Email { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();
}
