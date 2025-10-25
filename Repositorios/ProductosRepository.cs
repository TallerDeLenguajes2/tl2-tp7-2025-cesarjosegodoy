using Microsoft.Data.Sqlite;
using Productos;

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


    }
}






