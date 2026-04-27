using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace CapaDatos
{
    public class CD_Conexion
    {
        // Obtener cadena desde App.config (nunca hardcodeada)
        public static string ObtenerCadena() =>
            ConfigurationManager.ConnectionStrings["FacturacionDB"].ConnectionString;

        //Devolver una conexión NUEVA por cada uso.
        // El pool interno de ADO.NET maneja la reutilización eficientemente.
        public SqlConnection ObtenerConexion() => new SqlConnection(ObtenerCadena());
    }
}
