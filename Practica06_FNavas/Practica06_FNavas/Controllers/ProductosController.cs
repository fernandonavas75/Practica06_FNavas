using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Practica06_FNavas.Models;
using Microsoft.EntityFrameworkCore;

namespace Practica06_FNavas.Controllers
{
    [Route("api/Productos")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly SupermarketContext context;

        public ProductoController(SupermarketContext context)
        {
            this.context = context;
        }

        // Obtener todos los productos (Read - Listar)
        [HttpGet]
        [Route("Listar")]
        public ActionResult<List<Producto>> Listar()
        {
            try
            {
                var productos = context.Productos.Include(p => p.Categoria).ToList();
                return Ok(productos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener los productos: {ex.Message}");
            }
        }

        // Obtener un producto por ID (Read - Detalle)
        [HttpGet]
        [Route("Detalle/{id}")]
        public ActionResult<Producto> Detalle(int id)
        {
            try
            {
                var producto = context.Productos.Include(p => p.Categoria).FirstOrDefault(p => p.ProductoId == id);
                if (producto == null)
                {
                    return NotFound($"El producto con ID {id} no existe.");
                }
                return Ok(producto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener el producto: {ex.Message}");
            }
        }

        // Crear un nuevo producto (Create)
        [HttpPost]
        [Route("Crear")]
        public ActionResult Crear([FromBody] Producto nuevoProducto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    context.Productos.Add(nuevoProducto);
                    context.SaveChanges();
                    return Ok("Producto creado con éxito.");
                }
                return BadRequest("Datos inválidos.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al crear el producto: {ex.Message}");
            }
        }

        // Actualizar un producto existente (Update)
        [HttpPut]
        [Route("Actualizar/{id}")]
        public ActionResult Actualizar(int id, [FromBody] Producto productoActualizado)
        {
            try
            {
                var producto = context.Productos.Find(id);
                if (producto == null)
                {
                    return NotFound($"El producto con ID {id} no existe.");
                }

                producto.Nombre = productoActualizado.Nombre;
                producto.Descripcion = productoActualizado.Descripcion;
                producto.Precio = productoActualizado.Precio;
                producto.Stock = productoActualizado.Stock;
                producto.CategoriaId = productoActualizado.CategoriaId;

                context.SaveChanges();

                return Ok("Producto actualizado con éxito.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al actualizar el producto: {ex.Message}");
            }
        }

        // Eliminar un producto (Delete)
        [HttpDelete]
        [Route("Eliminar/{id}")]
        public ActionResult Eliminar(int id)
        {
            try
            {
                var producto = context.Productos.Find(id);
                if (producto == null)
                {
                    return NotFound($"El producto con ID {id} no existe.");
                }

                context.Productos.Remove(producto);
                context.SaveChanges();

                return Ok("Producto eliminado con éxito.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al eliminar el producto: {ex.Message}");
            }
        }
        [HttpGet]
        [Route("BusquedaXNombre")]
        public List<Producto> BuscarXNombre(String criterio) { 
            var productos = context.Productos
            .Where(p => p.Nombre.Contains(criterio))
            .ToList();
            return productos;
        }
    }
}
