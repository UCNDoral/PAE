using System;
using System.Collections.Generic;

namespace FacturacionApp.Model;

public partial class DetalleFactura
{
    public int IdDetalle { get; set; }

    public int IdFactura { get; set; }

    public int IdProducto { get; set; }

    public int Cantidad { get; set; }

    public decimal Precio { get; set; }

    public virtual Factura IdFacturaNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
