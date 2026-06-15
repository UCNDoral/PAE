using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.SqlClient;

namespace SistemaReportes.Data
{
    public static class ConexionDB
    {
        // Cadena de conexión. Ajusta 'Server' si tu instancia tiene otro nombre.
        // .\SQLEXPRESS es el nombre por defecto de SQL Server Express.
        private const string CadenaConexion =
            //"Server=DESKTOP-LARS505;Database=SistemaFacturacionReportes;Trusted_Connection=True;";
            "Data Source=DESKTOP-LARS505;Initial Catalog=SistemaFacturacionReportes;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";

        // Devuelve una conexión ya abierta.
        // El que la usa debe cerrarla (con 'using' es automático).
        public static SqlConnection ObtenerConexion()
        {
            var conexion = new SqlConnection(CadenaConexion);
            conexion.Open();
            return conexion;
        }
    }
}
