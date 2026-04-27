using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_HistorialFactura
    {
        /// <summary>
        /// Obtiene el encabezado de todas las facturas con el total calculado desde BD
        /// </summary>
        public DataTable ObtenerHistorial()
        {
            DataTable tabla = new DataTable();
            string query = @"
                SELECT 
                    F.IdFactura, 
                    C.Nombre + ' ' + C.Apellido AS Cliente, 
                    F.Fecha,
                    ISNULL(SUM(D.Cantidad * D.Precio), 0) AS Total
                FROM Facturas F
                INNER JOIN Clientes C ON F.IdCliente = C.IdCliente
                LEFT JOIN DetalleFactura D ON F.IdFactura = D.IdFactura
                GROUP BY F.IdFactura, C.Nombre, C.Apellido, F.Fecha
                ORDER BY F.IdFactura DESC";

            //'using' garantiza cierre automático. Usa la versión corregida de CD_Conexion
            using (SqlConnection conn = new CD_Conexion().ObtenerConexion())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                    tabla.Load(reader);
            }
            return tabla;
        }

        /// <summary>
        /// Obtiene los productos vendidos en una factura específica
        /// </summary>
        public DataTable ObtenerDetalles(int idFactura)
        {
            DataTable tabla = new DataTable();
            string query = @"
                SELECT 
                    P.Nombre AS Producto,
                    D.Cantidad,
                    D.Precio,
                    (D.Cantidad * D.Precio) AS Subtotal
                FROM DetalleFactura D
                INNER JOIN Productos P ON D.IdProducto = P.IdProducto
                WHERE D.IdFactura = @IdFactura";

            using (SqlConnection conn = new CD_Conexion().ObtenerConexion())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@IdFactura", SqlDbType.Int).Value = idFactura;
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                    tabla.Load(reader);
            }
            return tabla;
        }
    }
}
