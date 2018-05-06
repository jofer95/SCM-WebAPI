using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using WEB_API.Models;

namespace WEB_API.Controllers
{
    [Produces("application/json")]
    [Route("api/Productos")]
    public class ProductosController : Controller
    {
        IProductosRepository productos;
        IPedidosRepository pedidos;
        public ProductosController()
        {
            productos = new AzureProductosRepository();
            //Implementar uno en azure
            pedidos = null;
        }
        [HttpGet]
        public async Task<IEnumerable<ProductoModel>> GetAll()
        {
            /*var model = (await productos.todosLosProductos())
            .Select( p => new ProductoModel(){
                    Codigo = p.Codigo,
                    Descripcion = p.Descripcion,
                    Categoria = p.Categoria,
                    Precio = p.Precio
                });
            return model;*/
            return null;
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var pedido = pedidos.LeerPedido(id);
            return Ok(pedido);
            /*var model = await productos.LeerProducto(id);
            return new ObjectResult(model);*/
        }

        public async Task<ActionResult> Post([FromBody] PedidoModel nuevoPedido){
            if(String.IsNullOrWhiteSpace(nuevoPedido.Cliente)){
                return this.BadRequest(StatusCodes.Status400BadRequest);

            }
            var id = await pedidos.IniciarPedido(nuevoPedido);
            nuevoPedido.PedidoID = id;
            return Ok(nuevoPedido);

        }

        [HttpGet("{id}", Name = "Put")]
        public async Task<ActionResult> Put(Guid id,[FromBody] PedidoModel nuevoPedido){
            var pedido = await pedidos.LeerPedido(id);
            if(pedido == null){
                return NotFound();
            }
            var update = await pedidos.CompletarPedido(pedido);
            return Ok();
        }

        [HttpGet("{id}/{nuevoEstado}", Name = "Put")]
        public async Task<ActionResult> Put(Guid id,[FromBody] PedidoModel nuevoPedido, string nuevoEstado){
            var pedido = await pedidos.LeerPedido(id);
            if(pedido == null){
                return NotFound();
            }
            var update = await pedidos.CompletarPedido(pedido);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Delete(Guid id){
            var pedido = await pedidos.LeerPedido(id);
            if(pedido == null){
                return NotFound();
            }
            var res = await pedidos.CancelarPedido(pedido);
            return Ok();
        }
    }
}