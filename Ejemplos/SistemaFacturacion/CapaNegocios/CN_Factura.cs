using CapaDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocios
{
    public class CN_Factura
    {
        private readonly CD_Factura objetoCD = new CD_Factura();

        public int RegistrarFactura(int idCliente, DateTime fecha, List<DetalleFacturaItem> detalles)
        {
            //Validaciones de negocio antes de tocar la BD
            if (idCliente <= 0) throw new ArgumentException("Debe seleccionar un cliente válido.");
            if (detalles == null || detalles.Count == 0) throw new ArgumentException("La factura debe contener al menos un producto.");

            foreach (var det in detalles)
            {
                if (det.Cantidad <= 0) throw new ArgumentException("La cantidad debe ser mayor a cero.");
                if (det.Precio <= 0) throw new ArgumentException("El precio unitario debe ser mayor a cero.");
            }

            // Delegar a capa de datos (que maneja la transacción)
            return objetoCD.CrearFactura(idCliente, fecha, detalles);
        }
    }
}
