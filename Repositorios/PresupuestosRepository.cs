using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Productos;
using Presupuestos;
using PresupuestoDetalles;

namespace RepositoriesP
{

    public class PresupuestoRepository
    {
        private string _cadenaConexion = "Data Source=Db/Tienda.db;"; // base de datos

        // Crear nuevo presupuesto
        public void Crear(Presupuesto p)
        {
            using (SqliteConnection conexion = new SqliteConnection(_cadenaConexion))
            {
                conexion.Open();
                string sql = "INSERT INTO Presupuestos (nombreDestinatario, FechaCreacion) VALUES (@nombreDestinatario, @fecha)";

                using (var cmd = new SqliteCommand(sql, conexion))
                {
                    cmd.Parameters.AddWithValue("@nombreDestinatario", p.NombreDestinatario);
                    cmd.Parameters.AddWithValue("@fecha", DateTime.Now.ToString("yyyy-MM-dd"));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Listar todos los presupuestos
        public List<Presupuesto> Listar()
        {
            List<Presupuesto> lista = new List<Presupuesto>();

            using (SqliteConnection conexion = new SqliteConnection(_cadenaConexion))
            {
                conexion.Open();
                string sql = "SELECT * FROM Presupuestos";

                using (var cmd = new SqliteCommand(sql, conexion))

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Presupuesto p = new Presupuesto()
                        {
                            IdPresupuesto = reader.GetInt32(0),
                            NombreDestinatario = reader.GetString(1),
                            FechaCreacion = DateOnly.Parse(reader.GetString(2)),//reader.GetString(2),//DateTime.Parse(reader.GetString(2))
                            Detalle = new List<PresupuestoDetalle>()
                        };
                        lista.Add(p);
                    }
                }
            }
            return lista;
        }

        // Obtener un presupuesto con su detalle (productos + cantidad)
        public Presupuesto ObtenerPorId(int id)
        {
            Presupuesto p = null;

            using (SqliteConnection conexion = new SqliteConnection(_cadenaConexion))
            {
                conexion.Open();

                // Obtener datos del presupuesto
                string sqlPres = "SELECT * FROM Presupuestos WHERE IdPresupuesto = @id";

                using (var cmd = new SqliteCommand(sqlPres, conexion))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            p = new Presupuesto()
                            {
                                IdPresupuesto = reader.GetInt32(0),
                                NombreDestinatario = reader.GetString(1),
                                FechaCreacion = DateOnly.Parse(reader.GetString(2)),//DateTime.Parse(reader.GetString(2))
                                Detalle = new List<PresupuestoDetalle>()
                            };
                        }
                    }
                }

                if (p == null) return null;

                // Obtener los productos del presupuesto
                string sqlDet = @"SELECT pd.IdProducto, pr.descripcion, pr.precio, pd.cantidad
                                  FROM PresupuestosDetalle pd
                                  JOIN Productos pr ON pd.IdProducto = pr.idProducto
                                  WHERE pd.IdPresupuesto = @id";

                using (var cmd = new SqliteCommand(sqlDet, conexion))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Producto prod = new Producto()
                            {
                                IdProducto = reader.GetInt32(0),
                                Descripcion = reader.GetString(1),
                                Precio = reader.GetInt32(2)
                            };

                            PresupuestoDetalle det = new PresupuestoDetalle()
                            {
                                Producto = prod,
                                Cantidad = reader.GetInt32(3)

                            };
                            p.Detalle.Add(det);
                        }
                    }
                }
            }
            return p;
        }

        // Agregar un producto a un presupuesto existente
        public void AgregarProducto(int idPresupuesto, int idProducto, int cantidad)
        {
            using (SqliteConnection conexion = new SqliteConnection(_cadenaConexion))
            {
                conexion.Open();
                string sql = "INSERT INTO PresupuestosDetalle (IdPresupuesto, IdProducto, cantidad) VALUES (@idPres, @idProd, @cant)";

                using (var cmd = new SqliteCommand(sql, conexion))
                {
                    cmd.Parameters.AddWithValue("@idPres", idPresupuesto);
                    cmd.Parameters.AddWithValue("@idProd", idProducto);
                    cmd.Parameters.AddWithValue("@cant", cantidad);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Eliminar un presupuesto (y su detalle)
        public bool Eliminar(int id)
        {
            using (SqliteConnection conexion = new SqliteConnection(_cadenaConexion))
            {
                conexion.Open();

                // Eliminar detalles asociados
                string sqlDet = "DELETE FROM PresupuestosDetalle WHERE IdPresupuesto = @id";

                using (var cmd = new SqliteCommand(sqlDet, conexion))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }

                // Eliminar presupuesto
                string sqlPres = "DELETE FROM Presupuestos WHERE IdPresupuesto = @id";

                using (var cmd = new SqliteCommand(sqlPres, conexion))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    int filas = cmd.ExecuteNonQuery();
                    return filas > 0;
                }
            }
        }
    }



}