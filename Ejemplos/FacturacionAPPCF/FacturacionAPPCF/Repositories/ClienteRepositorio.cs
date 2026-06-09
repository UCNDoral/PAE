using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using FacturacionAPPCF.Models;
using FacturacionAPPCF.Data;
using System.Diagnostics.CodeAnalysis;

namespace FacturacionAPPCF.Repositories
{
    public class ClienteRepositorio
    {
        public List<Cliente> OtenerTodos()
        {
            using var context = new FacturacionContext();
            return context.Clientes
                .AsNoTracking()
                .Where(c => c.Activo)
                .OrderBy(c => c.Nombre)
                .ToList();
        }


        public Cliente OtenerPorId(int id)
        {
            using var context = new FacturacionContext();
            return context.Clientes
            .AsNoTracking()
                .Include(c => c.Facturas)
                    .ThenInclude(f => f.Detalles)
                        .ThenInclude(df => df.Producto)
                .FirstOrDefault(c => c.IdCliente == id && c.Activo);
        }



        public Cliente Crear(Cliente cliente)
        {

            using var context = new FacturacionContext();
            context.Clientes.Add(cliente);
            context.SaveChanges();
            return cliente;
        }


        public bool Actualizar(Cliente modificado)
        {

            using var context = new FacturacionContext();

            var clienteExistente = context.Clientes.Find(modificado.IdCliente);
            if (clienteExistente == null || !clienteExistente.Activo)
            {
                return false;
            }
            clienteExistente.Nombre = modificado.Nombre;
            clienteExistente.Apellido1 = modificado.Apellido1;
            clienteExistente.Apellido2 = modificado.Apellido2;
            clienteExistente.Telefono = modificado.Telefono;
            clienteExistente.Direccion = modificado.Direccion;
            clienteExistente.FechaModificacion = DateTime.Now;

            context.SaveChanges();
            return true;
        }

        public bool Eliminar(int id)
        {
            using var context = new FacturacionContext();

            var cliente = context.Clientes.Find(id);

            if (cliente == null || !cliente.Activo)
            {
                return false;
            }
            cliente.Activo = false;
            cliente.FechaModificacion = DateTime.Now;
            context.SaveChanges();
            return true;

        }
    }
}
