using Microsoft.AspNetCore.Mvc;
using Productos;
using Presupuestos;
using System.Collections.Generic;
using RepositoriesP;

namespace PresupuestoC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PresupuestoController : ControllerBase
    {
        private readonly PresupuestoRepository presupuestoRepository;

        public PresupuestoController()
        {
            presupuestoRepository = new PresupuestoRepository();
        }

        [HttpPost]
        public ActionResult CrearPresupuesto(Presupuesto nuevoPresupuesto)
        {
            presupuestoRepository.Crear(nuevoPresupuesto);
            return CreatedAtAction(nameof(ObtenerPresupuesto),
                new { id = nuevoPresupuesto.IdPresupuesto },
                nuevoPresupuesto);
        }

        [HttpGet]
        public ActionResult<List<Presupuesto>> ListarPresupuestos()
        {
            var lista = presupuestoRepository.Listar();
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public ActionResult<Presupuesto> ObtenerPresupuesto(int id)
        {
            var presupuesto = presupuestoRepository.ObtenerPorId(id);
            if (presupuesto is null)
                return NotFound($"No se encontró el presupuesto con ID {id}");
            return Ok(presupuesto);
        }

        [HttpPost("{id}/ProductoDetalle")]
        public ActionResult AgregarProductoAlPresupuesto(int id, [FromQuery] int idProducto, [FromQuery] int cantidad)
        {
            presupuestoRepository.AgregarProducto(id, idProducto, cantidad);
            return Ok($"Producto {idProducto} agregado al presupuesto {id} con cantidad {cantidad} ✅");
        }

        [HttpDelete("{id}")]
        public ActionResult EliminarPresupuesto(int id)
        {
            bool eliminado = presupuestoRepository.Eliminar(id);
            return eliminado ? NoContent() : NotFound($"No se encontró el presupuesto con ID {id}");
        }
    }
}