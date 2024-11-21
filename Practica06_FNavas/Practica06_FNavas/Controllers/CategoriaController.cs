using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Practica06_FNavas.Models;

namespace Practica06_FNavas.Controllers
{
    [Route("api/Categorias")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly SupermarketContext context;

        public CategoriaController(SupermarketContext context)
        {
            this.context = context;
        }

        // Obtener todas las categorías (Read - Listar)
        [HttpGet]
        [Route("Listar")]
        public ActionResult<List<Categorium>> Listar()
        {
            try
            {
                return Ok(context.Categoria.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener las categorías: {ex.Message}");
            }
        }

        // Obtener una categoría por ID (Read - Detalle)
        [HttpGet]
        [Route("Detalle/{id}")]
        public ActionResult<Categorium> Detalle(int id)
        {
            try
            {
                var categoria = context.Categoria.Find(id);
                if (categoria == null)
                {
                    return NotFound($"La categoría con ID {id} no existe.");
                }
                return Ok(categoria);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener la categoría: {ex.Message}");
            }
        }

        // Crear una nueva categoría (Create)
        [HttpPost]
        [Route("Crear")]
        public ActionResult Crear([FromBody] Categorium nuevaCategoria)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    context.Categoria.Add(nuevaCategoria);
                    context.SaveChanges();
                    return Ok("Categoría creada con éxito.");
                }
                return BadRequest("Datos inválidos.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al crear la categoría: {ex.Message}");
            }
        }

        // Actualizar una categoría existente (Update)
        [HttpPut]
        [Route("Actualizar/{id}")]
        public ActionResult Actualizar(int id, [FromBody] Categorium categoriaActualizada)
        {
            try
            {
                var categoria = context.Categoria.Find(id);
                if (categoria == null)
                {
                    return NotFound($"La categoría con ID {id} no existe.");
                }

                categoria.Nombre = categoriaActualizada.Nombre;
                categoria.Descripcion = categoriaActualizada.Descripcion;
                context.SaveChanges();
                return Ok("Categoría actualizada con éxito.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al actualizar la categoría: {ex.Message}");
            }
        }

        // Eliminar una categoría (Delete)
        [HttpDelete]
        [Route("Eliminar/{id}")]
        public ActionResult Eliminar(int id)
        {
            try
            {
                var categoria = context.Categoria.Find(id);
                if (categoria == null)
                {
                    return NotFound($"La categoría con ID {id} no existe.");
                }

                context.Categoria.Remove(categoria);
                context.SaveChanges();
                return Ok("Categoría eliminada con éxito.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al eliminar la categoría: {ex.Message}");
            }
        }
    }
}
