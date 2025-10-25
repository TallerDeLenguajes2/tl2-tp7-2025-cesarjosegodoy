using System.Text.Json.Serialization;

namespace Productos
{


    public class Producto
    {
        [JsonPropertyName("id")]
        public int IdProducto { get; set; }

        [JsonPropertyName("descripcion")]
        public string? Descripcion { get; set; }

        [JsonPropertyName("precio")]
        public int Precio { get; set; }
    }

}