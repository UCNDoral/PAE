using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

using FacturacionApp2.Data;
using FacturacionApp2.Models;


namespace FacturacionApp2.Repositories
{
    public class ProductoRespository
    {
        //metodo privado para obtner una instacia del context

        private FacturacionContext GetContext() => new FacturacionContext(AppConfig.DbOptions);


        //CRUD
        //---READ: obtiene los datos

        public List<Producto> ObtenerProductos()
        {
            using var context = GetContext();
            //AsNoTracking -> Mejora el rendimiento en consultas de solo lectura
            return context.Productos.AsNoTracking().ToList();
        }


        //READ: obtiene un producto por su id

        public Producto ObtenerPorId(int id)
        {
            using var context = GetContext();
            return context.Productos.AsNoTracking()
                .FirstOrDefault(p => p.IdProducto == id);
        }

        //READ: obtiene un producto por su nombre
        public List<Producto> ObtenerPorNombre(string nombre)
        {
            using var context = GetContext();
            return context.Productos
                .AsNoTracking()
                .Where(p => p.Nombre.Contains(nombre))
                .ToList();
        }

        //CREATE:

        public void Agregar(Producto producto)
        {
            using var context = GetContext();
            context.Productos.Add(producto);
            context.SaveChanges();
        }


        //UPDATE:

        public void Actualizar(Producto producto)
        {
            using var context = GetContext();
            context.Productos.Update(producto);
            context.SaveChanges();
        }



        //DELETE:
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
