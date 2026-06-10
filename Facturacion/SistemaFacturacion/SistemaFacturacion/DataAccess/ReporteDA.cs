using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Data.SqlClient;
using SistemaFacturacion.Models;

namespace SistemaFacturacion.DataAccess
{
    /// <summary>
    /// Consultas SQL para alimentar los reportes.
    /// Usa los mismos patrones que ClienteDA y ProductoDA.
    /// </summary>
    public class ReporteDA
    {
        // ─────────────────────────────────────────────────
        // REPORTE 1: Datos de una factura individual
        // Retorna una fila por cada producto en la factura
        // ─────────────────────────────────────────────────
        public List<RptFacturaItem> ObtenerDatosFactura(int facturaId)
        {
            var lista = new List<RptFacturaItem>();
            using var con = Conexion.ObtenerConexion();

            var sql = @"
                SELECT
                    f.NumeroFactura,
                    CONVERT(VARCHAR, f.FechaEmision, 103) AS FechaEmision,
                    c.Nombre + ' ' + c.Apellido           AS ClienteNombre,
                    ISNULL(c.Email,     '')                AS ClienteEmail,
                    ISNULL(c.Telefono,  '')                AS ClienteTelefono,
                    ISNULL(c.Direccion, '')                AS ClienteDireccion,
                    f.Estado,
                    p.Codigo                               AS CodigoProducto,
                    p.Nombre                               AS NombreProducto,
                    d.Cantidad,
                    d.PrecioUnitario,
                    d.Subtotal,
                    f.Subtotal  AS TotalSubtotal,
                    f.IVA       AS TotalIVA,
                    f.Total     AS TotalFactura
                FROM DetalleFactura d
                INNER JOIN Factura  f ON f.FacturaId  = d.FacturaId
                INNER JOIN Cliente  c ON c.ClienteId  = f.ClienteId
                INNER JOIN Producto p ON p.ProductoId = d.ProductoId
                WHERE f.FacturaId = @FacturaId
                ORDER BY d.DetalleId";

            using var cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@FacturaId", facturaId);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new RptFacturaItem
                {
                    NumeroFactura = reader.GetString(reader.GetOrdinal("NumeroFactura")),
                    FechaEmision = reader.GetString(reader.GetOrdinal("FechaEmision")),
                    ClienteNombre = reader.GetString(reader.GetOrdinal("ClienteNombre")),
                    ClienteEmail = reader.GetString(reader.GetOrdinal("ClienteEmail")),
                    ClienteTelefono = reader.GetString(reader.GetOrdinal("ClienteTelefono")),
                    ClienteDireccion = reader.GetString(reader.GetOrdinal("ClienteDireccion")),
                    Estado = reader.GetString(reader.GetOrdinal("Estado")),
                    CodigoProducto = reader.GetString(reader.GetOrdinal("CodigoProducto")),
                    NombreProducto = reader.GetString(reader.GetOrdinal("NombreProducto")),
                    Cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad")),
                    PrecioUnitario = reader.GetDecimal(reader.GetOrdinal("PrecioUnitario")),
                    Subtotal = reader.GetDecimal(reader.GetOrdinal("Subtotal")),
                    TotalSubtotal = reader.GetDecimal(reader.GetOrdinal("TotalSubtotal")),
                    TotalIVA = reader.GetDecimal(reader.GetOrdinal("TotalIVA")),
                    TotalFactura = reader.GetDecimal(reader.GetOrdinal("TotalFactura")),
                });
            }
            return lista;
        }

        // ─────────────────────────────────────────────────
        // REPORTE 2: Ventas por período
        // Retorna una fila por factura entre dos fechas
        // ─────────────────────────────────────────────────
        public List<RptVentaItem> ObtenerVentasPorPeriodo(
            DateTime fechaInicio, DateTime fechaFin)
        {
            var lista = new List<RptVentaItem>();
            using var con = Conexion.ObtenerConexion();

            var sql = @"
                SELECT
                    f.NumeroFactura,
                    CONVERT(VARCHAR, f.FechaEmision, 103) AS FechaEmision,
                    c.Nombre + ' ' + c.Apellido           AS Cliente,
                    f.Subtotal,
                    f.IVA,
                    f.Total,
                    f.Estado,
                    (SELECT COUNT(*) FROM DetalleFactura d
                     WHERE d.FacturaId = f.FacturaId)     AS CantLineas
                FROM Factura f
                INNER JOIN Cliente c ON c.ClienteId = f.ClienteId
                WHERE CAST(f.FechaEmision AS DATE)
                      BETWEEN @FechaInicio AND @FechaFin
                ORDER BY f.FechaEmision DESC";

            using var cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio.Date);
            cmd.Parameters.AddWithValue("@FechaFin", fechaFin.Date);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new RptVentaItem
                {
                    NumeroFactura = reader.GetString(reader.GetOrdinal("NumeroFactura")),
                    FechaEmision = reader.GetString(reader.GetOrdinal("FechaEmision")),
                    Cliente = reader.GetString(reader.GetOrdinal("Cliente")),
                    Subtotal = reader.GetDecimal(reader.GetOrdinal("Subtotal")),
                    IVA = reader.GetDecimal(reader.GetOrdinal("IVA")),
                    Total = reader.GetDecimal(reader.GetOrdinal("Total")),
                    Estado = reader.GetString(reader.GetOrdinal("Estado")),
                    CantLineas = reader.GetInt32(reader.GetOrdinal("CantLineas")),
                });
            }
            return lista;
        }
    }
}
