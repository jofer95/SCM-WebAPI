using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace WEB_API.Controllers
{
    [Produces("application/json")]
    [Route("api/Pedidos")]
    public class PedidosController : Controller
    {
        IPedidosRepository pedidos;

        public PedidosController(){
            //pedidos = pedidosRepository;
            pedidos = new AzurePedidosRepository();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(DateTime fecha)
        {
            var resultados = await pedidos.LeerPedidosPorFecha(fecha);
            return Ok(resultados);
        }


        [HttpGet("{id}", Name = "GetTodo")]
        public async Task<IActionResult> GetById(string id)
        {
            var pedido = await pedidos.LeerPedido(id);
            return Ok(pedido);
        }

        //[HttpPost("{id}", Name = "Post")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PedidoModel nuevoPedido)
        {
            if(string.IsNullOrWhiteSpace(nuevoPedido.Cliente)){
                return this.BadRequest(StatusCodes.Status400BadRequest);
            }
            var id = await pedidos.IniciarPedido(nuevoPedido);
            nuevoPedido.PedidoID = id;
            return Ok(nuevoPedido);
        }

        [HttpPut("{id}/Avanzar", Name = "Put")]
        public async Task<IActionResult> Put(string id, [FromBody] PedidoModel nuevoPedido, string nuevoEstado)
        {
            var pedido = await pedidos.LeerPedido(id);
            if(pedido == null){
                return NotFound();
            }
            var update = await pedidos.AvanzarPedido(pedido,nuevoEstado);
            return Ok();
        }
        [HttpPut("{id}/Avanzar", Name = "Put")]
        public async Task<IActionResult> Put(string id, [FromBody] PedidoModel nuevoPedido)
        {
            var pedido = await pedidos.LeerPedido(id);
            if(pedido == null){
                return NotFound();
            }
            var update = await pedidos.CompletarPedido(pedido);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var pedido = await pedidos.LeerPedido(id);
            if(pedido == null){
                return NotFound();
            }
            var res = await pedidos.CancelarPedido(pedido);
            return Ok();
        }
    }
}