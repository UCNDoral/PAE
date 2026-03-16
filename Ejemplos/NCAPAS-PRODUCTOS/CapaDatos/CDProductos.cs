using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Data;

namespace CapaDatos
{
    public class CDProductos
    {
        private CD_Conexion conexion = new CD_Conexion();

        SqlDataReader leer;
        DataTable tabla = new DataTable();
        SqlCommand comando = new SqlCommand();


        public DataTable Mostrar()
        {
            comando.Connection = conexion.AbrirConexion();
            comando.CommandText = "SELECT Id, Nombre, Descripcion, Marca, Precio, Stock FROM Productos";
            leer = comando.ExecuteReader();
            tabla.Load(leer);
            return tabla;
        }

        public void Insertar(string nombre, string descripcion, string marca, float precio, int stock)
        {
            comando.Connection = conexion.AbrirConexion();
            comando.CommandText = "INSERT INTO Productos (Nombre, Descripcion, Marca, Precio, Stock) VALUES (@Nombre, @Descripcion, @Marca, @Precio, @Stock)";
            comando.Parameters.AddWithValue("@Nombre", nombre);
            comando.Parameters.AddWithValue("@Descripcion", descripcion);
            comando.Parameters.AddWithValue("@Marca", marca);
            comando.Parameters.AddWithValue("@Precio", precio);
            comando.Parameters.AddWithValue("@Stock", stock);
            comando.ExecuteNonQuery();
            comando.Parameters.Clear();
        }








    }
}
