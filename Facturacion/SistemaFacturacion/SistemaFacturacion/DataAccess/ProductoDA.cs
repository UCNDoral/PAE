using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.SqlClient;
using SistemaFacturacion.Models;

namespace SistemaFacturacion.DataAccess
{
    /// <summary>
    /// Data Access para la tabla Producto.
    /// Misma estructura que ClienteDA — patrón consistente.
    /// </summary>
    public class ProductoDA
    {
        public List<Producto> ObtenerTodos()
        {
            var lista = new List<Producto>();
            using var con = Conexion.ObtenerConexion();

            var sql = "SELECT ProductoId, Codigo, Nombre, Descripcion, PrecioUnitario, Stock, Activo FROM Producto ORDER BY Nombre";
            using var cmd = new SqlCommand(sql, con);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
                lista.Add(MapearProducto(reader));

            return lista;
        }

        public List<Producto> ObtenerActivos()
        {
            var lista = new List<Producto>();
            using var con = Conexion.ObtenerConexion();

            // Solo productos activos con stock > 0 para la factura
            var sql = "SELECT ProductoId, Codigo, Nombre, Descripcion, PrecioUnitario, Stock, Activo FROM Producto WHERE Activo = 1 ORDER BY Nombre";
            using var cmd = new SqlCommand(sql, con);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
                lista.Add(MapearProducto(reader));

            return lista;
        }

        public Producto? ObtenerPorId(int productoId)
        {
            using var con = Conexion.ObtenerConexion();
            var sql = "SELECT ProductoId, Codigo, Nombre, Descripcion, PrecioUnitario, Stock, Activo FROM Producto WHERE ProductoId = @Id";
            using var cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Id", productoId);
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? MapearProducto(reader) : null;
        }

        public bool Insertar(Producto p)
        {
            using var con = Conexion.ObtenerConexion();
            var sql = @"INSERT INTO Producto (Codigo, Nombre, Descripcion, PrecioUnitario, Stock, Activo)
                        VALUES (@Codigo, @Nombre, @Descripcion, @Precio, @Stock, @Activo)";
            using var cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Codigo", p.Codigo);
            cmd.Parameters.AddWithValue("@Nombre", p.Nombre);
            cmd.Parameters.AddWithValue("@Descripcion", p.Descripcion);
            cmd.Parameters.AddWithValue("@Precio", p.PrecioUnitario);
            cmd.Parameters.AddWithValue("@Stock", p.Stock);
            cmd.Parameters.AddWithValue("@Activo", p.Activo);
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Actualizar(Producto p)
        {
            using var con = Conexion.ObtenerConexion();
            var sql = @"UPDATE Producto 
                        SET Codigo         = @Codigo,
                            Nombre         = @Nombre,
                            Descripcion    = @Descripcion,
                            PrecioUnitario = @Precio,
                            Stock          = @Stock,
                            Activo         = @Activo
                        WHERE ProductoId = @Id";
            using var cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Codigo", p.Codigo);
            cmd.Parameters.AddWithValue("@Nombre", p.Nombre);
            cmd.Parameters.AddWithValue("@Descripcion", p.Descripcion);
            cmd.Parameters.AddWithValue("@Precio", p.PrecioUnitario);
            cmd.Parameters.AddWithValue("@Stock", p.Stock);
            cmd.Parameters.AddWithValue("@Activo", p.Activo);
            cmd.Parameters.AddWithValue("@Id", p.ProductoId);
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Eliminar(int productoId)
        {
            using var con = Conexion.ObtenerConexion();
            var sql = "DELETE FROM Producto WHERE ProductoId = @Id";
            using var cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Id", productoId);
            return cmd.ExecuteNonQuery() > 0;
        }

        private static Producto MapearProducto(SqlDataReader r) => new()
        {
            ProductoId = r.GetInt32(r.GetOrdinal("ProductoId")),
            Codigo = r.GetString(r.GetOrdinal("Codigo")),
            Nombre = r.GetString(r.GetOrdinal("Nombre")),
            Descripcion = r.IsDBNull(r.GetOrdinal("Descripcion")) ? "" : r.GetString(r.GetOrdinal("Descripcion")),
            PrecioUnitario = r.GetDecimal(r.GetOrdinal("PrecioUnitario")),
            Stock = r.GetInt32(r.GetOrdinal("Stock")),
            Activo = r.GetBoolean(r.GetOrdinal("Activo"))
        };
    }
}
