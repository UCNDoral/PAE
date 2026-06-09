using CapaDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CNCliente
    {

        private readonly CDCliente _dal = new CDCliente();

        public DataTable ObtenerClientes() => _dal.Listar();

        public bool RegistrarCliente(string nombre, string telefono, string email)
        {
            if (string.IsNullOrWhiteSpace(nombre)) throw new ArgumentException("El nombre es obligatorio.");
            if (string.IsNullOrWhiteSpace(telefono)) throw new ArgumentException("El teléfono es obligatorio.");
            return _dal.Insertar(nombre, telefono, email) > 0;
        }

        public bool ActualizarCliente(int id, string nombre, string telefono, string email)
        {
            if (string.IsNullOrWhiteSpace(nombre)) throw new ArgumentException("El nombre es obligatorio.");
            if (string.IsNullOrWhiteSpace(telefono)) throw new ArgumentException("El teléfono es obligatorio.");
            return _dal.Actualizar(id, nombre, telefono, email) > 0;
        }

        public bool EliminarCliente(int id)
        {
            if (id <= 0) throw new ArgumentException("ID de cliente inválido.");
            return _dal.Eliminar(id) > 0;
        }



    }
}
