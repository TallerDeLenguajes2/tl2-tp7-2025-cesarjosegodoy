using System.Text.Json.Serialization;

namespace Presupuestos
{


    public class Presupuesto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nombreDestinatario")]
        public string? NombreDestinatario { get; set; }

        [JsonPropertyName("fechaCreacion")]
        public DateOnly FechaCreacion { get; set; }
        
       

    }

}