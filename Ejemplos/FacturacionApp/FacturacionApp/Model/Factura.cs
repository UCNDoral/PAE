using System;
using System.Collections.Generic;

namespace FacturacionApp.Model;

public partial class Factura
{
    public int IdFactura { get; set; }

    public int IdCliente { get; set; }

    public DateTime? Fecha { get; set; }

    public virtual ICollection<DetalleFactura> DetalleFacturas { get; set; } = new List<DetalleFactura>();

    public virtual Cliente IdClienteNavigation { get; set; } = null!;
}
