using Microsoft.AspNetCore.Mvc;
using ProductoRepotitorys;
using Productos;

[ApiController]
[Route("[controller]")]
public class ProductosController : ControllerBase
{
    private ProductoRepository productoRepository;
    public ProductosController()
    {
        productoRepository = new ProductoRepository();
    }

    [HttpPost("AltaProducto")]
    public ActionResult<string> AltaProducto(Producto nuevoProducto)
    {
        productoRepository.Alta(nuevoProducto);
        return Ok("Producto dado de alta exitosamente");
    }


}