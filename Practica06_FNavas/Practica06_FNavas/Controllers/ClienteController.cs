using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Practica06_FNavas.Models;

namespace Practica06_FNavas.Controllers
{
    [Route("api/Clientes")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly SupermarketContext context;

        public ClienteController(SupermarketContext context)
        {
            this.context = context;
        }

        // Obtener todos los clientes (Read - Listar)
        [HttpGet]
        [Route("Listar")]
        public ActionResult<List<Cliente>> Listar()
        {
            try
            {
                return Ok(context.Clientes.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener los clientes: {ex.Message}");
            }
        }

        // Obtener un cliente por ID (Read - Detalle)
        [HttpGet]
        [Route("Detalle/{id}")]
        public ActionResult<Cliente> Detalle(int id)
        {
            try
            {
                var cliente = context.Clientes.Find(id);
                if (cliente == null)
                {
                    return NotFound($"El cliente con ID {id} no existe.");
                }
                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener el cliente: {ex.Message}");
            }
        }

        // Crear un nuevo cliente (Create)
        [HttpPost]
        [Route("Crear")]
        public ActionResult Crear([FromBody] Cliente nuevoCliente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    context.Clientes.Add(nuevoCliente);
                    context.SaveChanges();
                    return Ok("Cliente creado con éxito.");
                }
                return BadRequest("Datos inválidos.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al crear el cliente: {ex.Message}");
            }
        }

        // Actualizar un cliente existente (Update)
        [HttpPut]
        [Route("Actualizar/{id}")]
        public ActionResult Actualizar(int id, [FromBody] Cliente clienteActualizado)
        {
            try
            {
                var cliente = context.Clientes.Find(id);
                if (cliente == null)
                {
                    return NotFound($"El cliente con ID {id} no existe.");
                }

                cliente.Nombre = clienteActualizado.Nombre;
                cliente.Apellido = clienteActualizado.Apellido;
                cliente.Email = clienteActualizado.Email;
                cliente.Telefono = clienteActualizado.Telefono;
                cliente.Activo = clienteActualizado.Activo;
                context.SaveChanges();

                return Ok("Cliente actualizado con éxito.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al actualizar el cliente: {ex.Message}");
            }
        }

        // Eliminar un cliente (Delete)
        [HttpDelete]
        [Route("Eliminar/{id}")]
        public ActionResult Eliminar(int id)
        {
            try
            {
                var cliente = context.Clientes.Find(id);
                if (cliente == null)
                {
                    return NotFound($"El cliente con ID {id} no existe.");
                }

                context.Clientes.Remove(cliente);
                context.SaveChanges();

                return Ok("Cliente eliminado con éxito.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al eliminar el cliente: {ex.Message}");
            }
        }
    }
}
