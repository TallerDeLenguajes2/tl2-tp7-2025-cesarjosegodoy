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




}
}






