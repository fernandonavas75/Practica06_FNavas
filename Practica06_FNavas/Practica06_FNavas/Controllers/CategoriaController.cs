using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Practica06_FNavas.Models;

namespace Practica06_FNavas.Controllers
{
    [Route("api/Categorias")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        SupermarketContext context = new SupermarketContext();
        [HttpGet]
        [Route("Listar")]
        public List<Categorium> Listado() { 
            return context.Categoria.ToList();
        }
    }
}
