using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Productos;
using Presupuestos;

namespace Repositories{

public class PresupuestoRepository
    {
        private string cadenaConexion = "Data Source=tienda.db;";

        // Crear nuevo presupuesto
        public void Crear(Presupuesto presupuesto)
        {
            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();

            string sql = "INSERT INTO Presupuestos (nombreDestinatario, fechaCreacion) VALUES (@nombre, @fecha)";
            using var comando = new SqliteCommand(sql, conexion);
            comando.Parameters.AddWithValue("@nombre", presupuesto.NombreDestinatario);
            comando.Parameters.AddWithValue("@fecha", presupuesto.FechaCreacion);
            comando.ExecuteNonQuery();
        }

        // Listar todos los presupuestos
        public List<Presupuesto> Listar()
        {
            var lista = new List<Presupuesto>();
            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();

            string sql = "SELECT idPresupuesto, nombreDestinatario, fechaCreacion FROM Presupuestos";
            using var comando = new SqliteCommand(sql, conexion);
            using var lector = comando.ExecuteReader();

            while (lector.Read())
            {
                var p = new Presupuesto
                {
                    IdPresupuesto = lector.GetInt32(0),
                    NombreDestinatario = lector.GetString(1),
                    FechaCreacion = DateTime.Parse(lector.GetString(2))
                };
                lista.Add(p);
            }

            return lista;
        }

        // Obtener presupuesto con sus productos
        public Presupuesto? ObtenerPorId(int id)
        {
            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();

            // Datos del presupuesto
            string sqlPresupuesto = "SELECT idPresupuesto, nombreDestinatario, fechaCreacion FROM Presupuestos WHERE idPresupuesto = @id";
            using var cmdPres = new SqliteCommand(sqlPresupuesto, conexion);
            cmdPres.Parameters.AddWithValue("@id", id);
            using var lector = cmdPres.ExecuteReader();

            if (!lector.Read()) return null;

            var presupuesto = new Presupuesto
            {
                IdPresupuesto = lector.GetInt32(0),
                NombreDestinatario = lector.GetString(1),
                FechaCreacion = DateTime.Parse(lector.GetString(2))
            };

            lector.Close();

            // Cargar los detalles
            string sqlDetalle = @"SELECT pd.cantidad, pr.idProducto, pr.descripcion, pr.precio
                                  FROM PresupuestoDetalle pd
                                  INNER JOIN Productos pr ON pr.idProducto = pd.idProducto
                                  WHERE pd.idPresupuesto = @id";

            using var cmdDet = new SqliteCommand(sqlDetalle, conexion);
            cmdDet.Parameters.AddWithValue("@id", id);
            using var lectorDet = cmdDet.ExecuteReader();

            while (lectorDet.Read())
            {
                var detalle = new PresupuestoDetalle
                {
                    Producto = new Producto
                    {
                        IdProducto = lectorDet.GetInt32(1),
                        Descripcion = lectorDet.GetString(2),
                        Precio = lectorDet.GetInt32(3)
                    },
                    Cantidad = lectorDet.GetInt32(0)
                };
                presupuesto.Detalle.Add(detalle);
            }

            return presupuesto;
        }

        // Agregar un producto a un presupuesto
        public void AgregarProducto(int idPresupuesto, int idProducto, int cantidad)
        {
            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();

            string sql = "INSERT INTO PresupuestoDetalle (idPresupuesto, idProducto, cantidad) VALUES (@pres, @prod, @cant)";
            using var comando = new SqliteCommand(sql, conexion);
            comando.Parameters.AddWithValue("@pres", idPresupuesto);
            comando.Parameters.AddWithValue("@prod", idProducto);
            comando.Parameters.AddWithValue("@cant", cantidad);
            comando.ExecuteNonQuery();
        }

        // Eliminar presupuesto
        public bool Eliminar(int id)
        {
            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();

            // Eliminar detalles primero
            string sqlDetalle = "DELETE FROM PresupuestoDetalle WHERE idPresupuesto = @id";
            using var cmdDetalle = new SqliteCommand(sqlDetalle, conexion);
            cmdDetalle.Parameters.AddWithValue("@id", id);
            cmdDetalle.ExecuteNonQuery();

            // Luego el presupuesto
            string sqlPres = "DELETE FROM Presupuestos WHERE idPresupuesto = @id";
            using var cmdPres = new SqliteCommand(sqlPres, conexion);
            cmdPres.Parameters.AddWithValue("@id", id);
            int filas = cmdPres.ExecuteNonQuery();

            return filas > 0;
        }
    }


}