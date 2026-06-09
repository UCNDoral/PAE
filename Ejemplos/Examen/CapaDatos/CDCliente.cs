using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CDCliente
    {
        public DataTable Listar()
        {
            using (SqlConnection cn = CD_Conexion.ObtenerConexion())
            using (SqlCommand cmd = new SqlCommand("SELECT IdCliente, Nombre, Telefono, Email FROM Clientes", cn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

            public int Insertar(string nombre, string telefono, string email)
        {
            using (SqlConnection cn = CD_Conexion.ObtenerConexion())
            using (SqlCommand cmd = new SqlCommand("INSERT INTO Clientes (Nombre, Telefono, Email) VALUES (@Nombre, @Telefono, @Email)", cn))
            {
                cmd.Parameters.AddWithValue("@Nombre", nombre.Trim());
                cmd.Parameters.AddWithValue("@Telefono", telefono.Trim());
                cmd.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(email) ? (object)DBNull.Value : email.Trim());
                cn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public int Actualizar(int id, string nombre, string telefono, string email)
        {
            using (SqlConnection cn = CD_Conexion.ObtenerConexion())
            using (SqlCommand cmd = new SqlCommand("UPDATE Clientes SET Nombre = @Nombre, Telefono = @Telefono, Email = @Email WHERE IdCliente = @Id", cn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Nombre", nombre.Trim());
                cmd.Parameters.AddWithValue("@Telefono", telefono.Trim());
                cmd.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(email) ? (object)DBNull.Value : email.Trim());
                cn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public int Eliminar(int id)
        {
            using (SqlConnection cn = CD_Conexion.ObtenerConexion())
            using (SqlCommand cmd = new SqlCommand("DELETE FROM Clientes WHERE IdCliente = @Id", cn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                cn.Open();
                return cmd.ExecuteNonQuery();
            }
        }





    }
}
