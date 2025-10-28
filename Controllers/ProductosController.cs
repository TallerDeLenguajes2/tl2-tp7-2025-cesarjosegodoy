using Microsoft.AspNetCore.Mvc;
using ProductoRepotitorys;
using Productos;

[ApiController]
[Route("[controller]")]
public class ProductosController : ControllerBase
{
    private readonly ProductoRepository _productoRepository;

    public ProductosController()
    {
        _productoRepository = new ProductoRepository();
    }

    // POST /Productos/AltaProducto
    [HttpPost("AltaProducto")]
    public ActionResult AltaProducto([FromBody] Producto nuevoProducto)
    {
        if (string.IsNullOrWhiteSpace(nuevoProducto.Descripcion))
            return BadRequest("La descripción es obligatoria.");

        if (nuevoProducto.Precio <= 0)
            return BadRequest("El precio debe ser mayor que cero.");

        int id = _productoRepository.Alta(nuevoProducto);

        return CreatedAtAction(nameof(GetById), new { id = id }, nuevoProducto);
    }

    // GET /Productos/products
    [HttpGet("products")]
    public IActionResult GetAll()
    {
        var productos = _productoRepository.GetAll();
        return Ok(productos);
    }

    // GET /Productos/5
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var producto = _productoRepository.GetById(id);
        if (producto == null)
            return NotFound("Producto no encontrado.");
        return Ok(producto);
    }

    // PUT /Productos/5
    [HttpPut("{id}")]
    public IActionResult Modificar(int id, [FromBody] Producto producto)
    {
        bool actualizado = _productoRepository.Modificar(id, producto);
        if (!actualizado)
            return NotFound("No se pudo actualizar el producto.");
        return Ok("Producto actualizado correctamente.");
    }

    // DELETE /Productos/5
    [HttpDelete("{id}")]
    public IActionResult Eliminar(int id)
    {
        bool eliminado = _productoRepository.Eliminar(id);
        if (!eliminado)
            return NotFound("No se pudo eliminar el producto.");
        return Ok("Producto eliminado correctamente.");
    }
}