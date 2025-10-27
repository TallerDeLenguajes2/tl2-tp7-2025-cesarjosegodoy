using System.Text.Json.Serialization;
using PresupuestoDetalles;


namespace Presupuestos
{


    public class Presupuesto
    {
        [JsonPropertyName("idPresupuesto")]
        public int IdPresupuesto { get; set; }

        [JsonPropertyName("nombreDestinatario")]
        public string NombreDestinatario { get; set; }

        [JsonPropertyName("fechaCreacion")]
        public DateOnly FechaCreacion { get; set; }

        [JsonPropertyName("detalle")]
        public List<PresupuestoDetalle> Detalle { get; set; } = new();


       public double MontoPresupuesto()
        {
            return Detalle.Sum(d => d.Producto.Precio * d.Cantidad);
        }

        public double MontoPresupuestoConIva()
        {
            return MontoPresupuesto() * 1.21;
        }

        public int CantidadProductos()
        {
            return Detalle.Sum(d => d.Cantidad);
        }


    }

}