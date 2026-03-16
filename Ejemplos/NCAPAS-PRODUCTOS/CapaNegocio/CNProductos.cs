using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;


namespace CapaNegocio
{
    internal class CNProductos
    {
        private CDProductos objetoCD = new CDProductos();

        public DataTable MostrarProducto()
        {
            DataTable dt = new DataTable();
            dt = objetoCD.Mostrar();
            return dt;
        }
    }
}
