using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WEB_API.Controllers
{
    [Produces("application/json")]
    [Route("api/Pedidos")]
    public class PedidosController : Controller
    {
        [HttpGet("{id}", Name = "GetPedido")]
        public async Task<IActionResult> GetById(string id)
        {
            return null;
        }
    }
}