using Microsoft.Data.Sqlite;
using Productos;
using Presupuestos;

namespace ProductoRepotitorys
{


    public class ProductoRepository
    {
        private string _connectionString = "Data Source=Db/Tienda.db;";

        /// <summary>
        /// Metodo : Alta (crear)   
        /// </summary>
        /// <param name="producto"></param>
        /// <returns></returns>
        public int Alta(Producto producto)
        {

            int nuevoId = 0;

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO Productos (Descripcion, Precio) VALUES (@desc, @prec); SELECT last_insert_rowid();";


                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@desc", producto.Descripcion);
                    command.Parameters.AddWithValue("@prec", producto.Precio);
                    nuevoId = Convert.ToInt32(command.ExecuteScalar());
                }

            }

            return nuevoId;


        }
        /// <summary>
        /// Metodo : Listar (Todo)   
        /// </summary>
        /// <param name="producto"></param>
        /// <returns></returns>
        public List<Producto> GetAll()
        {
            string query = "SELECT * FROM productos";
            List<Producto> productos = [];
            using var Connection = new SqliteConnection(_connectionString);
            Connection.Open();

            var command = new SqliteCommand(query, Connection);

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var producto = new Producto
                    {
                        //Id = Convert.ToInt32(reader["idProducto"]),
                        Descripcion = reader["Descripcion"].ToString(),
                        Precio = Convert.ToInt32(reader["Precio"])
                    };
                    productos.Add(producto);
                }
            }
            Connection.Close();
            return productos;
        }


        // Obtener presupuesto con sus productos
        public Presupuesto? ObtenerPorId(int id)
        {
            using var conexion = new SqliteConnection(_connectionString);
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

        }

         // Eliminar presupuesto
        public bool Eliminar(int id)
        {
            using var conexion = new SqliteConnection(_connectionString);
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






