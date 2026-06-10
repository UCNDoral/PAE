using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.SqlClient;

namespace SistemaFacturacion.DataAccess
{
    /// <summary>
    /// Clase centralizada para manejar la conexión a SQL Server.
    /// Un solo lugar donde cambiar la cadena de conexión.
    /// </summary>
    public class Conexion
    {
        // Ajusta Server, Database, User y Password a tu entorno
        private const string CadenaConexion =
            "Data Source=D-DTCING\\SQLSERVER2025;Initial Catalog=FacturacionDBEjemplo;Integrated Security=True;Trust Server Certificate=True";

        /// <summary>
        /// Retorna una conexión ABIERTA lista para usar.
        /// El llamador es responsable de cerrarla (usando 'using').
        /// </summary>
        public static SqlConnection ObtenerConexion()
        {
            var conexion = new SqlConnection(CadenaConexion);
            conexion.Open();
            return conexion;
        }
    }
}
