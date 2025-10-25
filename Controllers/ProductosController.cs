using Microsoft.AspNetCore.Mvc;
using ProductoRepotitorys;
using Productos;

[ApiController]
[Route("[controller]")]
public class ProductosController : ControllerBase
{
    private ProductoRepository _productoRepository;
    public ProductosController()
    {
        _productoRepository = new ProductoRepository();
    }

    [HttpPost("AltaProducto")]
    public ActionResult<string> AltaProducto(Producto nuevoProducto)
    {
        _productoRepository.Alta(nuevoProducto);
        return Ok("Producto dado de alta exitosamente");
    }

    [HttpGet("products")]
    public IActionResult GetAll()
    {
        var productos = _productoRepository.GetAll();
        return Ok(productos);
    }









}