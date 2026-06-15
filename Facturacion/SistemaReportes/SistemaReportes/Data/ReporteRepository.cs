using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using Microsoft.Data.SqlClient;

using SistemaReportes.Model;
namespace SistemaReportes.Data
{
    public class ReporteRepository
    {
        // Obtiene ventas en un rango de fechas.
        // Usa @fechaInicio y @fechaFin como parámetros seguros.
        public List<VentaReporte> ObtenerVentasPorFecha(
            DateTime fechaInicio, DateTime fechaFin)
        {
            var lista = new List<VentaReporte>();

            // 'using' cierra la conexión automáticamente al terminar
            using var conexion = ConexionDB.ObtenerConexion();

            string sql = @"
                SELECT v.IdVenta, c.Nombre AS Cliente,
                       p.Descripcion AS Producto, d.Cantidad,
                       d.PrecioUnitario,
                       (d.Cantidad * d.PrecioUnitario) AS Total,
                       v.Fecha
                FROM Ventas v
                INNER JOIN Clientes c     ON c.IdCliente  = v.IdCliente
                INNER JOIN DetalleVenta d ON d.IdVenta    = v.IdVenta
                INNER JOIN Productos p    ON p.IdProducto = d.IdProducto
                WHERE v.Fecha BETWEEN @fechaInicio AND @fechaFin
                ORDER BY v.Fecha DESC";

            using var cmd = new SqlCommand(sql, conexion);

            // Parámetros tipados: más seguros y eficientes
            cmd.Parameters.Add("@fechaInicio", SqlDbType.DateTime).Value = fechaInicio;
            cmd.Parameters.Add("@fechaFin", SqlDbType.DateTime).Value = fechaFin;

            // SqlDataReader lee fila por fila sin cargar todo en memoria
            using var lector = cmd.ExecuteReader();

            while (lector.Read())
            {
                lista.Add(new VentaReporte
                {
                    IdVenta = lector.GetInt32(lector.GetOrdinal("IdVenta")),
                    Cliente = lector.GetString(lector.GetOrdinal("Cliente")),
                    Producto = lector.GetString(lector.GetOrdinal("Producto")),
                    Cantidad = lector.GetInt32(lector.GetOrdinal("Cantidad")),
                    PrecioUnit = lector.GetDecimal(lector.GetOrdinal("PrecioUnitario")),
                    Total = lector.GetDecimal(lector.GetOrdinal("Total")),
                    FechaVenta = lector.GetDateTime(lector.GetOrdinal("Fecha"))
                });
            }

            return lista;
        }
    }
}
