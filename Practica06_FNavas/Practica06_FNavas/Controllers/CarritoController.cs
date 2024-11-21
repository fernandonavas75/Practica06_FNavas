using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practica06_FNavas.Models;

namespace Practica06_FNavas.Controllers
{
    [Route("api/Carritos")]
    [ApiController]
    public class CarritoController : ControllerBase
    {
        private readonly SupermarketContext context;

        public CarritoController(SupermarketContext context)
        {
            this.context = context;
        }

        // Obtener todos los carritos (Read - Listar)
        [HttpGet]
        [Route("Listar")]
        public ActionResult<List<Carrito>> Listar()
        {
            try
            {
                var carritos = context.Carritos
                                      .Include(c => c.Cliente)
                                      .Include(c => c.DetalleCarritos)
                                      .ThenInclude(dc => dc.Producto)
                                      .ToList();
                return Ok(carritos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener los carritos: {ex.Message}");
            }
        }

        // Obtener un carrito por ID (Read - Detalle)
        [HttpGet]
        [Route("Detalle/{id}")]
        public ActionResult<Carrito> Detalle(int id)
        {
            try
            {
                var carrito = context.Carritos
                                     .Include(c => c.Cliente)
                                     .Include(c => c.DetalleCarritos)
                                     .ThenInclude(dc => dc.Producto)
                                     .FirstOrDefault(c => c.CarritoId == id);

                if (carrito == null)
                {
                    return NotFound($"El carrito con ID {id} no existe.");
                }

                return Ok(carrito);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener el carrito: {ex.Message}");
            }
        }

        // Crear un nuevo carrito (Create)
        [HttpPost]
        [Route("Crear")]
        public ActionResult Crear([FromBody] Carrito nuevoCarrito)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    nuevoCarrito.FechaCreacion = DateTime.Now; // Asignar la fecha actual
                    context.Carritos.Add(nuevoCarrito);
                    context.SaveChanges();
                    return Ok("Carrito creado con éxito.");
                }
                return BadRequest("Datos inválidos.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al crear el carrito: {ex.Message}");
            }
        }

        // Actualizar un carrito existente (Update)
        [HttpPut]
        [Route("Actualizar/{id}")]
        public ActionResult Actualizar(int id, [FromBody] Carrito carritoActualizado)
        {
            try
            {
                var carrito = context.Carritos.Find(id);
                if (carrito == null)
                {
                    return NotFound($"El carrito con ID {id} no existe.");
                }

                carrito.ClienteId = carritoActualizado.ClienteId;
                carrito.Total = carritoActualizado.Total;

                context.SaveChanges();

                return Ok("Carrito actualizado con éxito.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al actualizar el carrito: {ex.Message}");
            }
        }

        // Eliminar un carrito (Delete)
        [HttpDelete]
        [Route("Eliminar/{id}")]
        public ActionResult Eliminar(int id)
        {
            try
            {
                var carrito = context.Carritos.Include(c => c.DetalleCarritos).FirstOrDefault(c => c.CarritoId == id);
                if (carrito == null)
                {
                    return NotFound($"El carrito con ID {id} no existe.");
                }

                // Eliminar detalles del carrito
                context.DetalleCarritos.RemoveRange(carrito.DetalleCarritos);

                // Eliminar el carrito
                context.Carritos.Remove(carrito);
                context.SaveChanges();

                return Ok("Carrito eliminado con éxito.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al eliminar el carrito: {ex.Message}");
            }
        }
    }
}
