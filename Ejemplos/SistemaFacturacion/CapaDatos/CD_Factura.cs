using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    // Clase auxiliar para transportar los detalles de la factura (DTO)
    // Se coloca en CapaDatos para mantener estrictamente 3 proyectos
    public class DetalleFacturaItem
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Subtotal => Cantidad * Precio; // Calculado en memoria
    }

    public class CD_Factura
    {
        /// <summary>
        /// Crea la factura y sus detalles dentro de una transacción SQL.
        /// Garantiza atomicidad: si algo falla, se deshace todo.
        /// </summary>
        public int CrearFactura(int idCliente, DateTime fecha, List<DetalleFacturaItem> detalles)
        {
            int idFacturaGenerada = 0;

            // Conexión nueva por operación (usa el pool interno de ADO.NET)
            using (SqlConnection conn = new CD_Conexion().ObtenerConexion())
            {
                conn.Open();
                // Inicio de transacción explícita
                using (SqlTransaction transaccion = conn.BeginTransaction())
                {
                    try
                    {
                        //Insertar encabezado de factura y obtener el ID generado
                        using (SqlCommand cmdFact = new SqlCommand(
                            "INSERT INTO Facturas (IdCliente, Fecha) VALUES (@IdCliente, @Fecha); SELECT SCOPE_IDENTITY();",
                            conn, transaccion))
                        {
                            cmdFact.Parameters.Add("@IdCliente", SqlDbType.Int).Value = idCliente;
                            cmdFact.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = fecha;
                            idFacturaGenerada = Convert.ToInt32(cmdFact.ExecuteScalar());
                        }

                        // Insertar cada detalle y descontar stock
                        foreach (var det in detalles)
                        {
                            using (SqlCommand cmdDet = new SqlCommand(
                                "INSERT INTO DetalleFactura (IdFactura, IdProducto, Cantidad, Precio) VALUES (@IdFactura, @IdProducto, @Cantidad, @Precio); " +
                                "UPDATE Productos SET Stock = Stock - @Cantidad WHERE IdProducto = @IdProducto;",
                                conn, transaccion))
                            {
                                cmdDet.Parameters.Add("@IdFactura", SqlDbType.Int).Value = idFacturaGenerada;
                                cmdDet.Parameters.Add("@IdProducto", SqlDbType.Int).Value = det.IdProducto;
                                cmdDet.Parameters.Add("@Cantidad", SqlDbType.Int).Value = det.Cantidad;
                                cmdDet.Parameters.Add("@Precio", SqlDbType.Decimal).Value = det.Precio;
                                cmdDet.ExecuteNonQuery();
                            }
                        }

                        // Si todo sale bien, confirmar cambios
                        transaccion.Commit();
                    }
                    catch
                    {
                        // Si falla CUALQUIER detalle o la actualización de stock, revertir TODO
                        transaccion.Rollback();
                        throw; // Re-lanzar para que la capa de negocio lo capture
                    }
                }
            }

            return idFacturaGenerada;
        }
    }
}
