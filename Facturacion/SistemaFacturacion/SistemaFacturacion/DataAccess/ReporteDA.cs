using System.Data;
using Microsoft.Data.SqlClient;

namespace SistemaFacturacion.DataAccess
{
    /// <summary>
    /// Data Access exclusivo para reportes.
    /// Devuelve DataTable porque los reportes son consultas de lectura y cada uno
    /// tiene columnas distintas; asi el formulario puede enlazar cualquier resultado
    /// directamente al DataGridView sin crear un modelo por cada reporte.
    /// </summary>
    public class ReporteDA
    {
        /// <summary>
        /// Agrupa las ventas activas por dia.
        /// Es util para ver el comportamiento diario de ingresos dentro del rango.
        /// </summary>
        public DataTable ObtenerVentasPorFecha(DateTime desde, DateTime hasta)
        {
            var sql = @"
                SELECT
                    CONVERT(date, f.FechaEmision) AS Fecha,
                    COUNT(*) AS Facturas,
                    SUM(f.Subtotal) AS Subtotal,
                    SUM(f.IVA) AS IVA,
                    SUM(f.Total) AS Total
                FROM Factura f
                WHERE f.Estado = 'ACTIVA'
                  AND f.FechaEmision >= @Desde
                  AND f.FechaEmision < @Hasta
                GROUP BY CONVERT(date, f.FechaEmision)
                ORDER BY Fecha";

            return EjecutarConsulta(sql, desde, hasta);
        }

        /// <summary>
        /// Lista las facturas emitidas en el rango, incluyendo activas y anuladas.
        /// Se incluyen anuladas porque este reporte sirve para auditoria del periodo.
        /// </summary>
        public DataTable ObtenerFacturas(DateTime desde, DateTime hasta)
        {
            var sql = @"
                SELECT
                    f.NumeroFactura AS Factura,
                    f.FechaEmision AS Fecha,
                    c.Nombre + ' ' + c.Apellido AS Cliente,
                    f.Subtotal,
                    f.IVA,
                    f.Total,
                    f.Estado
                FROM Factura f
                INNER JOIN Cliente c ON c.ClienteId = f.ClienteId
                WHERE f.FechaEmision >= @Desde
                  AND f.FechaEmision < @Hasta
                ORDER BY f.FechaEmision DESC";

            return EjecutarConsulta(sql, desde, hasta);
        }

        /// <summary>
        /// Suma cantidades e importes por producto tomando solo facturas activas.
        /// Las facturas anuladas no cuentan como venta real.
        /// </summary>
        public DataTable ObtenerProductosMasVendidos(DateTime desde, DateTime hasta)
        {
            var sql = @"
                SELECT
                    p.Codigo,
                    p.Nombre AS Producto,
                    SUM(d.Cantidad) AS CantidadVendida,
                    SUM(d.Cantidad * d.PrecioUnitario) AS TotalVendido
                FROM DetalleFactura d
                INNER JOIN Factura f ON f.FacturaId = d.FacturaId
                INNER JOIN Producto p ON p.ProductoId = d.ProductoId
                WHERE f.Estado = 'ACTIVA'
                  AND f.FechaEmision >= @Desde
                  AND f.FechaEmision < @Hasta
                GROUP BY p.Codigo, p.Nombre
                ORDER BY CantidadVendida DESC, TotalVendido DESC";

            return EjecutarConsulta(sql, desde, hasta);
        }

        /// <summary>
        /// Agrupa la facturacion activa por cliente para identificar los clientes
        /// con mayor monto comprado en el periodo.
        /// </summary>
        public DataTable ObtenerClientesConMayorCompra(DateTime desde, DateTime hasta)
        {
            var sql = @"
                SELECT
                    c.Nombre + ' ' + c.Apellido AS Cliente,
                    COUNT(f.FacturaId) AS Facturas,
                    SUM(f.Total) AS TotalComprado
                FROM Factura f
                INNER JOIN Cliente c ON c.ClienteId = f.ClienteId
                WHERE f.Estado = 'ACTIVA'
                  AND f.FechaEmision >= @Desde
                  AND f.FechaEmision < @Hasta
                GROUP BY c.ClienteId, c.Nombre, c.Apellido
                ORDER BY TotalComprado DESC";

            return EjecutarConsulta(sql, desde, hasta);
        }

        /// <summary>
        /// Calcula los totales que se muestran como resumen superior.
        /// Cuenta activas y anuladas, pero los montos solo suman facturas activas.
        /// </summary>
        public DataTable ObtenerResumen(DateTime desde, DateTime hasta)
        {
            var sql = @"
                SELECT
                    COUNT(CASE WHEN Estado = 'ACTIVA' THEN 1 END) AS FacturasActivas,
                    COUNT(CASE WHEN Estado = 'ANULADA' THEN 1 END) AS FacturasAnuladas,
                    ISNULL(SUM(CASE WHEN Estado = 'ACTIVA' THEN Subtotal ELSE 0 END), 0) AS Subtotal,
                    ISNULL(SUM(CASE WHEN Estado = 'ACTIVA' THEN IVA ELSE 0 END), 0) AS IVA,
                    ISNULL(SUM(CASE WHEN Estado = 'ACTIVA' THEN Total ELSE 0 END), 0) AS Total
                FROM Factura
                WHERE FechaEmision >= @Desde
                  AND FechaEmision < @Hasta";

            return EjecutarConsulta(sql, desde, hasta);
        }

        /// <summary>
        /// Ejecuta cualquier consulta de reporte con parametros de fecha seguros.
        /// @Hasta se recibe como fecha exclusiva; por eso se suma un dia para incluir
        /// todas las facturas del dia final hasta las 23:59:59.
        /// </summary>
        private static DataTable EjecutarConsulta(string sql, DateTime desde, DateTime hasta)
        {
            using var con = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Desde", desde.Date);
            cmd.Parameters.AddWithValue("@Hasta", hasta.Date.AddDays(1));

            using var adapter = new SqlDataAdapter(cmd);
            var tabla = new DataTable();
            adapter.Fill(tabla);
            return tabla;
        }
    }
}
