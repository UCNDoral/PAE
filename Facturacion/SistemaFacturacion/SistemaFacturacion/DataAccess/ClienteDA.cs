using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.SqlClient;
using SistemaFacturacion.Models;

namespace SistemaFacturacion.DataAccess
{
    /// <summary>
    /// Data Access para la tabla Cliente.
    /// DA = Data Access (acceso a datos).
    /// Cada método realiza una operación CRUD directamente con SQL.
    /// </summary>
    public class ClienteDA
    {
        // ─────────────────────────────────────────
        // READ: Obtener TODOS los clientes
        // ─────────────────────────────────────────
        public List<Cliente> ObtenerTodos()
        {
            var lista = new List<Cliente>();

            // 'using' garantiza que la conexión se cierre aunque haya error
            using var con = Conexion.ObtenerConexion();

            var sql = "SELECT ClienteId, Nombre, Apellido, Email, Telefono, Direccion, FechaRegistro FROM Cliente ORDER BY Nombre";
            using var cmd = new SqlCommand(sql, con);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(MapearCliente(reader));
            }

            return lista;
        }

        // ─────────────────────────────────────────
        // READ: Obtener un cliente por ID
        // ─────────────────────────────────────────
        public Cliente? ObtenerPorId(int clienteId)
        {
            using var con = Conexion.ObtenerConexion();

            var sql = "SELECT ClienteId, Nombre, Apellido, Email, Telefono, Direccion, FechaRegistro FROM Cliente WHERE ClienteId = @Id";
            using var cmd = new SqlCommand(sql, con);

            // Parámetros: NUNCA concatenes strings → evita SQL Injection
            cmd.Parameters.AddWithValue("@Id", clienteId);

            using var reader = cmd.ExecuteReader();
            return reader.Read() ? MapearCliente(reader) : null;
        }

        // ─────────────────────────────────────────
        // CREATE: Insertar nuevo cliente
        // ─────────────────────────────────────────
        public bool Insertar(Cliente cliente)
        {
            using var con = Conexion.ObtenerConexion();

            var sql = @"INSERT INTO Cliente (Nombre, Apellido, Email, Telefono, Direccion)
                        VALUES (@Nombre, @Apellido, @Email, @Telefono, @Direccion)";

            using var cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre);
            cmd.Parameters.AddWithValue("@Apellido", cliente.Apellido);
            cmd.Parameters.AddWithValue("@Email", cliente.Email);
            cmd.Parameters.AddWithValue("@Telefono", cliente.Telefono);
            cmd.Parameters.AddWithValue("@Direccion", cliente.Direccion);

            // ExecuteNonQuery retorna las filas afectadas; > 0 significa éxito
            return cmd.ExecuteNonQuery() > 0;
        }

        // ─────────────────────────────────────────
        // UPDATE: Actualizar cliente existente
        // ─────────────────────────────────────────
        public bool Actualizar(Cliente cliente)
        {
            using var con = Conexion.ObtenerConexion();

            var sql = @"UPDATE Cliente 
                        SET Nombre    = @Nombre,
                            Apellido  = @Apellido,
                            Email     = @Email,
                            Telefono  = @Telefono,
                            Direccion = @Direccion
                        WHERE ClienteId = @Id";

            using var cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre);
            cmd.Parameters.AddWithValue("@Apellido", cliente.Apellido);
            cmd.Parameters.AddWithValue("@Email", cliente.Email);
            cmd.Parameters.AddWithValue("@Telefono", cliente.Telefono);
            cmd.Parameters.AddWithValue("@Direccion", cliente.Direccion);
            cmd.Parameters.AddWithValue("@Id", cliente.ClienteId);

            return cmd.ExecuteNonQuery() > 0;
        }

        // ─────────────────────────────────────────
        // DELETE: Eliminar cliente
        // ─────────────────────────────────────────
        public bool Eliminar(int clienteId)
        {
            using var con = Conexion.ObtenerConexion();

            var sql = "DELETE FROM Cliente WHERE ClienteId = @Id";
            using var cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Id", clienteId);

            return cmd.ExecuteNonQuery() > 0;
        }

        // ─────────────────────────────────────────
        // HELPER: Mapear fila del reader a objeto Cliente
        // Centralizado para no repetir código
        // ─────────────────────────────────────────
        private static Cliente MapearCliente(SqlDataReader r) => new()
        {
            ClienteId = r.GetInt32(r.GetOrdinal("ClienteId")),
            Nombre = r.GetString(r.GetOrdinal("Nombre")),
            Apellido = r.GetString(r.GetOrdinal("Apellido")),
            Email = r.IsDBNull(r.GetOrdinal("Email")) ? "" : r.GetString(r.GetOrdinal("Email")),
            Telefono = r.IsDBNull(r.GetOrdinal("Telefono")) ? "" : r.GetString(r.GetOrdinal("Telefono")),
            Direccion = r.IsDBNull(r.GetOrdinal("Direccion")) ? "" : r.GetString(r.GetOrdinal("Direccion")),
            FechaRegistro = r.GetDateTime(r.GetOrdinal("FechaRegistro"))
        };
    }
}
