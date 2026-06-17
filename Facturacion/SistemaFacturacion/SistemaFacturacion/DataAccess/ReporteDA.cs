using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using System.Data;
using Microsoft.Data.SqlClient;


namespace SistemaFacturacion.DataAccess
{
    // Clase exclusiva para reportes.
    // No hereda ni implementa nada — solo métodos de lectura.
    // Usa ADO.NET puro, igual que el resto del proyecto.

    public class ReporteDA
    {
        public DataTable ObtenerVentasPorFecha(DateTime desde, DateTime hasta)
        {
            var sql = @"
                SELECT
                    CONVERT(date, f.FechaEmision) AS Fecha, -- Trunca la hora
                    COUNT(*)          AS Facturas,           -- Conteo por día
                    SUM(f.Subtotal)   AS Subtotal,
                    SUM(f.IVA)        AS IVA,
                    SUM(f.Total)      AS Total
                FROM Factura f
                WHERE f.Estado = 'ACTIVA'   -- Excluye anuladas del ingreso real
                  AND f.FechaEmision >= @Desde
                  AND f.FechaEmision <  @Hasta -- < no <= (ver EjecutarConsulta)
                GROUP BY CONVERT(date, f.FechaEmision)
                ORDER BY Fecha";

            return EjecutarConsulta(sql, desde, hasta);
        }


        public DataTable ObtenerFacturas(DateTime desde, DateTime hasta)
        {
            var sql = @"
                SELECT
                    f.NumeroFactura              AS Factura,
                    f.FechaEmision               AS Fecha,
                    c.Nombre + ' ' + c.Apellido  AS Cliente, -- JOIN para nombre legible
                    f.Subtotal, f.IVA, f.Total,
                    f.Estado                              -- Muestra ACTIVA / ANULADA
                FROM Factura f
                INNER JOIN Cliente c ON c.ClienteId = f.ClienteId
                WHERE f.FechaEmision >= @Desde
                  AND f.FechaEmision <  @Hasta
                ORDER BY f.FechaEmision DESC";  //--Más recientes primero

            return EjecutarConsulta(sql, desde, hasta);
        }

        public DataTable ObtenerProductosMasVendidos(DateTime desde, DateTime hasta)
        {
            var sql = @"
                SELECT
                    p.Codigo,
                    p.Nombre                              AS Producto,
                    SUM(d.Cantidad)                       AS CantidadVendida,
                    SUM(d.Cantidad * d.PrecioUnitario)    AS TotalVendido
                FROM DetalleFactura d
                INNER JOIN Factura  f ON f.FacturaId  = d.FacturaId
                INNER JOIN Producto p ON p.ProductoId = d.ProductoId
                WHERE f.Estado = 'ACTIVA'   -- Anuladas no son venta real
                  AND f.FechaEmision >= @Desde
                  AND f.FechaEmision <  @Hasta
                GROUP BY p.Codigo, p.Nombre
                ORDER BY CantidadVendida DESC, TotalVendido DESC";

            return EjecutarConsulta(sql, desde, hasta);
        }

        public DataTable ObtenerClientesConMayorCompra(DateTime desde, DateTime hasta)
        {
            var sql = @"
                SELECT
                    c.Nombre + ' ' + c.Apellido  AS Cliente,
                    COUNT(f.FacturaId)            AS Facturas,
                    SUM(f.Total)                  AS TotalComprado
                FROM Factura f
                INNER JOIN Cliente c ON c.ClienteId = f.ClienteId
                WHERE f.Estado = 'ACTIVA'
                  AND f.FechaEmision >= @Desde
                  AND f.FechaEmision <  @Hasta
                GROUP BY c.ClienteId, c.Nombre, c.Apellido
                -- Se agrupa también por ClienteId para evitar
                -- duplicados si dos clientes se llaman igual
                ORDER BY TotalComprado DESC";

            return EjecutarConsulta(sql, desde, hasta);
        }

        public DataTable ObtenerResumen(DateTime desde, DateTime hasta)
        {
            var sql = @"
                SELECT
                    -- CASE WHEN dentro de COUNT: cuenta solo las que cumplen
                    COUNT(CASE WHEN Estado = 'ACTIVA'  THEN 1 END) AS FacturasActivas,
                    COUNT(CASE WHEN Estado = 'ANULADA' THEN 1 END) AS FacturasAnuladas,
                    -- ISNULL evita NULL si no hay facturas en el período
                    ISNULL(SUM(CASE WHEN Estado = 'ACTIVA' THEN Subtotal ELSE 0 END), 0) AS Subtotal,
                    ISNULL(SUM(CASE WHEN Estado = 'ACTIVA' THEN IVA      ELSE 0 END), 0) AS IVA,
                    ISNULL(SUM(CASE WHEN Estado = 'ACTIVA' THEN Total    ELSE 0 END), 0) AS Total
                FROM Factura
                WHERE FechaEmision >= @Desde
                  AND FechaEmision <  @Hasta";

            return EjecutarConsulta(sql, desde, hasta);
        }

        private static DataTable EjecutarConsulta(
            string sql, DateTime desde, DateTime hasta)
        {
            // using garantiza que la conexión se cierra aunque haya excepción
            using var con = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand(sql, con);

            // Fecha inicio: 00:00:00 del día inicial
            cmd.Parameters.AddWithValue("@Desde", desde.Date);

            // Fecha fin: 00:00:00 del día SIGUIENTE
            // Ejemplo: hasta = 10/06 → @Hasta = 11/06 00:00:00
            // Así se incluyen facturas de las 23:59 del día 10
            cmd.Parameters.AddWithValue("@Hasta", hasta.Date.AddDays(1));

            using var adapter = new SqlDataAdapter(cmd);
            var tabla = new DataTable();
            // Fill abre la conexión, ejecuta y la cierra automáticamente
            adapter.Fill(tabla);
            return tabla;
        }
    }
}


