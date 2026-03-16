using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;


namespace CapaNegocio
{
    public class CNProductos
    {
        private CDProductos objetoCD = new CDProductos();

        public DataTable MostrarProducto()
        {
            DataTable dt = new DataTable();
            dt = objetoCD.Mostrar();
            return dt;
        }


        public void InsertarProducto(string nombre, string descripcion, string marca, float precio, int stock)
        {
            objetoCD.Insertar(nombre, descripcion, marca, precio, stock);
        }
    }
}
