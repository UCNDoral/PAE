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
    /// Data Access para Factura y DetalleFactura.
    /// La operación de guardar maneja ambas tablas en una TRANSACCIÓN,
    /// garantizando que si falla el detalle, no quede el encabezado huérfano.
    /// </summary>
    public class FacturaDA
    {
        // ─────────────────────────────────────────
        // READ: Lista de todas las facturas con datos del cliente
        // ─────────────────────────────────────────
        public List<Factura> ObtenerTodas()
        {
            var lista = new List<Factura>();
            using var con = Conexion.ObtenerConexion();

            // JOIN para traer el nombre del cliente junto a la factura
            var sql = @"
                SELECT f.FacturaId, f.NumeroFactura, f.ClienteId,
                       f.FechaEmision, f.Subtotal, f.IVA, f.Total, f.Estado,
                       c.Nombre + ' ' + c.Apellido AS NombreCliente
                FROM Factura f
                INNER JOIN Cliente c ON c.ClienteId = f.ClienteId
                ORDER BY f.FechaEmision DESC";

            using var cmd = new SqlCommand(sql, con);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Factura
                {
                    FacturaId = reader.GetInt32(reader.GetOrdinal("FacturaId")),
                    NumeroFactura = reader.GetString(reader.GetOrdinal("NumeroFactura")),
                    ClienteId = reader.GetInt32(reader.GetOrdinal("ClienteId")),
                    FechaEmision = reader.GetDateTime(reader.GetOrdinal("FechaEmision")),
                    Subtotal = reader.GetDecimal(reader.GetOrdinal("Subtotal")),
                    IVA = reader.GetDecimal(reader.GetOrdinal("IVA")),
                    Total = reader.GetDecimal(reader.GetOrdinal("Total")),
                    Estado = reader.GetString(reader.GetOrdinal("Estado")),
                    Cliente = new Cliente
                    {
                        Nombre = reader.GetString(reader.GetOrdinal("NombreCliente"))
                    }
                });
            }
            return lista;
        }

        // ─────────────────────────────────────────
        // READ: Factura completa con sus detalles
        // ─────────────────────────────────────────
        public Factura? ObtenerPorId(int facturaId)
        {
            using var con = Conexion.ObtenerConexion();

            // Primero traemos el encabezado
            var sqlEncabezado = @"
                SELECT f.*, c.Nombre + ' ' + c.Apellido AS NombreCliente
                FROM Factura f
                INNER JOIN Cliente c ON c.ClienteId = f.ClienteId
                WHERE f.FacturaId = @Id";

            using var cmdE = new SqlCommand(sqlEncabezado, con);
            cmdE.Parameters.AddWithValue("@Id", facturaId);
            using var readerE = cmdE.ExecuteReader();

            if (!readerE.Read()) return null;

            var factura = new Factura
            {
                FacturaId = readerE.GetInt32(readerE.GetOrdinal("FacturaId")),
                NumeroFactura = readerE.GetString(readerE.GetOrdinal("NumeroFactura")),
                ClienteId = readerE.GetInt32(readerE.GetOrdinal("ClienteId")),
                FechaEmision = readerE.GetDateTime(readerE.GetOrdinal("FechaEmision")),
                Subtotal = readerE.GetDecimal(readerE.GetOrdinal("Subtotal")),
                IVA = readerE.GetDecimal(readerE.GetOrdinal("IVA")),
                Total = readerE.GetDecimal(readerE.GetOrdinal("Total")),
                Estado = readerE.GetString(readerE.GetOrdinal("Estado")),
                Cliente = new Cliente { Nombre = readerE.GetString(readerE.GetOrdinal("NombreCliente")) }
            };
            readerE.Close(); // Cerramos para reusar la misma conexión

            // Luego traemos los detalles
            var sqlDetalle = @"
                SELECT d.DetalleId, d.FacturaId, d.ProductoId,
                       d.Cantidad, d.PrecioUnitario, d.Subtotal,
                       p.Nombre AS NombreProducto, p.Codigo
                FROM DetalleFactura d
                INNER JOIN Producto p ON p.ProductoId = d.ProductoId
                WHERE d.FacturaId = @Id";

            using var cmdD = new SqlCommand(sqlDetalle, con);
            cmdD.Parameters.AddWithValue("@Id", facturaId);
            using var readerD = cmdD.ExecuteReader();

            while (readerD.Read())
            {
                factura.Detalles.Add(new DetalleFactura
                {
                    DetalleId = readerD.GetInt32(readerD.GetOrdinal("DetalleId")),
                    FacturaId = facturaId,
                    ProductoId = readerD.GetInt32(readerD.GetOrdinal("ProductoId")),
                    Cantidad = readerD.GetInt32(readerD.GetOrdinal("Cantidad")),
                    PrecioUnitario = readerD.GetDecimal(readerD.GetOrdinal("PrecioUnitario")),
                    Producto = new Producto
                    {
                        Nombre = readerD.GetString(readerD.GetOrdinal("NombreProducto")),
                        Codigo = readerD.GetString(readerD.GetOrdinal("Codigo"))
                    }
                });
            }
            return factura;
        }

        // ─────────────────────────────────────────
        // CREATE: Guardar factura + detalles en TRANSACCIÓN
        // Si cualquier paso falla → se revierte TODO (ROLLBACK)
        // ─────────────────────────────────────────
        public bool Guardar(Factura factura)
        {
            using var con = Conexion.ObtenerConexion();
            using var transaccion = con.BeginTransaction(); // ← inicio de transacción

            try
            {
                // 1) Insertar encabezado y recuperar el ID generado
                var sqlFactura = @"
                    INSERT INTO Factura (NumeroFactura, ClienteId, FechaEmision, Subtotal, IVA, Total, Estado)
                    VALUES (@Numero, @ClienteId, @Fecha, @Subtotal, @IVA, @Total, @Estado);
                    SELECT SCOPE_IDENTITY();"; // Retorna el nuevo FacturaId

                using var cmdF = new SqlCommand(sqlFactura, con, transaccion);
                cmdF.Parameters.AddWithValue("@Numero", factura.NumeroFactura);
                cmdF.Parameters.AddWithValue("@ClienteId", factura.ClienteId);
                cmdF.Parameters.AddWithValue("@Fecha", factura.FechaEmision);
                cmdF.Parameters.AddWithValue("@Subtotal", factura.Subtotal);
                cmdF.Parameters.AddWithValue("@IVA", factura.IVA);
                cmdF.Parameters.AddWithValue("@Total", factura.Total);
                cmdF.Parameters.AddWithValue("@Estado", factura.Estado);

                // ExecuteScalar retorna el primer valor de la primera fila
                var nuevoId = Convert.ToInt32(cmdF.ExecuteScalar());
                factura.FacturaId = nuevoId;

                // 2) Insertar cada línea del detalle
                foreach (var detalle in factura.Detalles)
                {
                    var sqlDetalle = @"
                        INSERT INTO DetalleFactura (FacturaId, ProductoId, Cantidad, PrecioUnitario)
                        VALUES (@FacturaId, @ProductoId, @Cantidad, @Precio)";

                    using var cmdD = new SqlCommand(sqlDetalle, con, transaccion);
                    cmdD.Parameters.AddWithValue("@FacturaId", nuevoId);
                    cmdD.Parameters.AddWithValue("@ProductoId", detalle.ProductoId);
                    cmdD.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                    cmdD.Parameters.AddWithValue("@Precio", detalle.PrecioUnitario);
                    cmdD.ExecuteNonQuery();
                }

                // 3) Todo bien → confirmar la transacción
                transaccion.Commit();
                return true;
            }
            catch
            {
                // Algo falló → deshacer TODO lo que se hizo en esta transacción
                transaccion.Rollback();
                throw; // Re-lanzamos para que la UI muestre el error
            }
        }

        // ─────────────────────────────────────────
        // UPDATE: Anular factura (no eliminamos físicamente)
        // ─────────────────────────────────────────
        public bool Anular(int facturaId)
        {
            using var con = Conexion.ObtenerConexion();
            var sql = "UPDATE Factura SET Estado = 'ANULADA' WHERE FacturaId = @Id";
            using var cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Id", facturaId);
            return cmd.ExecuteNonQuery() > 0;
        }

        // ─────────────────────────────────────────
        // HELPER: Generar número de factura correlativo
        // Formato: FAC-000001
        // ─────────────────────────────────────────
        public string GenerarNumeroFactura()
        {
            using var con = Conexion.ObtenerConexion();
            var sql = "SELECT ISNULL(MAX(FacturaId), 0) + 1 FROM Factura";
            using var cmd = new SqlCommand(sql, con);
            var numero = Convert.ToInt32(cmd.ExecuteScalar());
            return $"FAC-{numero:D6}"; // FAC-000001, FAC-000002, etc.
        }
    }
}
