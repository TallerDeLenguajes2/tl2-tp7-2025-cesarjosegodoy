using System.Text.Json.Serialization;
using PresupuestoDetalles;

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

        public List<PresupuestoDetalle> Detalle { get; set; } = new List<PresupuestoDetalle>();


        // Calcula el monto total (sin IVA)
        public decimal MontoPresupuesto()
        {
            return Detalle.Sum(d => d.Producto.Precio * d.Cantidad);
        }

        // Calcula el monto total con IVA del 21%
        public decimal MontoPresupuestoConIva()
        {
            decimal monto = MontoPresupuesto();
            return monto * 1.21m;
        }

        // Cuenta el total de productos (sumando las cantidades)
        public int CantidadProductos()
        {
            return Detalle.Sum(d => d.Cantidad);
        }






    }

}