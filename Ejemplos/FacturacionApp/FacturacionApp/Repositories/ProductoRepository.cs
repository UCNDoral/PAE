using System;
using System.Collections.Generic;
using System.Text;
using FacturacionApp.Data;
using FacturacionApp.Model;
using Microsoft.EntityFrameworkCore;

namespace FacturacionApp.Repositories
{
    internal class ProductoRepository
    {

        private FacturacionApp.Data.Factura GetContext()
            => new FacturacionApp.Data.Factura(AppConfig.DbOption);


        //Obtener los productos
        public List<Producto> ObtenerTodos()
        {
            using var context = GetContext();
            return context.Productos.AsNoTracking().ToList();
        }

        //Obtener producto por ID
        public Producto ObtenerPorId(int id)
        {
            using var context = GetContext();
            return context.Productos.AsNoTracking().FirstOrDefault(p => p.IdProducto == id);
        }

        //Buscar por nombre
        public List<Producto> BuscarPorNombre(string nombre)
        {
            using var context = GetContext();

            return context.Productos.AsNoTracking()
                .Where(p => p.Nombre.Contains(nombre))
                .ToList();
        }


        //CREATE: Agregar producto

        public void Agregar(Producto producto)
        {
            using var context = GetContext();
            context.Productos.Add(producto);
            context.SaveChanges();
        }

        //UPDATE: Actualizar producto
        public void Actualizar(Producto producto)
        {
            using var context = GetContext();
            context.Productos.Update(producto);
            context.SaveChanges();
        }

        //Delete: Eliminar producto

        public void Eliminar(int id)
        {
            using var context = GetContext();
            var producto = context.Productos.Find(id);
            if (producto != null)
            {
                context.Productos.Remove(producto);
                context.SaveChanges();
            }
        }




    }
}
