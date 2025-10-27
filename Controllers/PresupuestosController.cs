using Microsoft.AspNetCore.Mvc;
using Productos;
using Presupuestos;
using System.Collections.Generic;
using RepositoriesP;

namespace PresupuestoC.controllers
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

        // POST /api/Presupuesto
        [HttpPost]
        public ActionResult CrearPresupuesto(Presupuesto nuevoPresupuesto)
        {
            presupuestoRepository.Crear(nuevoPresupuesto);
            return Ok("Presupuesto creado correctamente ✅");
        }

        // GET /api/Presupuesto
        [HttpGet]
        public ActionResult<List<Presupuesto>> ListarPresupuestos()
        {
            var lista = presupuestoRepository.Listar();
            return Ok(lista);
        }

        // GET /api/Presupuesto/{id}
        [HttpGet("{id}")]
        public ActionResult<Presupuesto> ObtenerPresupuesto(int id)
        {
            var presupuesto = presupuestoRepository.ObtenerPorId(id);
            if (presupuesto == null)
                return NotFound($"No se encontró el presupuesto con ID {id}");
            return Ok(presupuesto);
        }

        // POST /api/Presupuesto/{id}/ProductoDetalle
        [HttpPost("{id}/ProductoDetalle")]
        public ActionResult AgregarProductoAlPresupuesto(int id, int idProducto, int cantidad)
        {
            presupuestoRepository.AgregarProducto(id, idProducto, cantidad);
            return Ok($"Producto {idProducto} agregado al presupuesto {id} con cantidad {cantidad} ✅");
        }

        // DELETE /api/Presupuesto/{id}
        [HttpDelete("{id}")]
        public ActionResult EliminarPresupuesto(int id)
        {
            bool eliminado = presupuestoRepository.Eliminar(id);
            if (eliminado)
                return NoContent();
            else
                return NotFound($"No se encontró el presupuesto con ID {id}");
        }
    }
}