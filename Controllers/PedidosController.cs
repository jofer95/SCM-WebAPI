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
    //[Route("api/Pedidos")]
    [Route("api/Pedidos/[action]")]
    public class PedidosController : Controller
    {
        IPedidosRepository pedidos;

        public PedidosController()
        {

            //pedidos = pedidosRepository;
            pedidos = new AzurePedidosRepository();
        }
        public async Task<ActionResult> Index()
        {
            var model = (await pedidos.todosLosPedidos())
            .Select(p => new PedidoModel()
            {
                PedidoID = p.PedidoID,
                DescripcionProducto = p.DescripcionProducto,
                Cliente = p.Cliente,
                Precio = p.Precio,
                Telefono = p.Telefono,
                Producto = p.Producto,
                FechaPedido = p.FechaPedido,
                Estado = p.Estado,
                Latitud = p.Latitud,
                Longitud = p.Latitud
            });
            return View(model);
        }
        // GET: Catalogo/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var model = await pedidos.LeerPedido(id);
            return View(model);
        }

        // GET: Catalogo/Create
        public ActionResult Create()
        {
            var model = new PedidoCrearModelo();
            return View(model);
        }

        // POST: Catalogo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PedidoCrearModelo p)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add insert logic here
                    await pedidos.IniciarPedido(new PedidoModel()
                    {
                        PedidoID = p.PedidoID,
                        DescripcionProducto = p.DescripcionProducto,
                        Cliente = p.Cliente,
                        Precio = p.Precio,
                        Telefono = p.Telefono,
                        Producto = p.Producto,
                        FechaPedido = p.FechaPedido,
                        Estado = p.Estado,
                        Latitud = p.Latitud,
                        Longitud = p.Latitud
                    });
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }

        // GET: Catalogo/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var p = await pedidos.LeerPedido(id);
            PedidoModel edit = new PedidoModel();
            edit.PedidoID = p.PedidoID;
                edit.DescripcionProducto = p.DescripcionProducto;
                edit.Cliente = p.Cliente;
                edit.Precio = p.Precio;
                edit.Telefono = p.Telefono;
                edit.Producto = p.Producto;
                edit.FechaPedido = p.FechaPedido;
                edit.Estado = p.Estado;
                edit.Latitud = p.Latitud;
                edit.Longitud = p.Latitud;
            return View(edit);
        }

        // POST: Catalogo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, PedidoEditarModelo model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await pedidos.actualizarDatos(new PedidoModel()
                    {
                    Telefono = model.Telefono,
                    Cliente = model.Cliente,
                    FechaPedido = model.FechaPedido,
                    Precio = model.Precio,
                    DescripcionProducto = model.DescripcionProducto,
                    Latitud = model.Latitud,
                    Longitud = model.Longitud,
                    Producto = model.Producto
                    });

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }

        // GET: Catalogo/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            var producto = await pedidos.LeerPedido(id);
            return View(new PedidoBorrarModelo()
            {
                PedidoID = producto.PedidoID,
                DescripcionProducto = producto.DescripcionProducto
            });
        }

        // POST: Catalogo/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                await pedidos.borrarPedido(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        [ActionName("ObtenerTodosLosPedidos")]
        public async Task<IActionResult> ObtenerTodosLosPedidos()
        {
            var resultados = await pedidos.todosLosPedidos();
            return Ok(resultados);
        }

        /*
        Metodo para actualizar el estado del pedido mandando su RowID y su estado a actualizar.
         */
        [HttpPost]
        [ActionName("ActualizarEstadoPedido")]
        public async Task<IActionResult> ActualizarEstadoPedido([FromBody]PedidoModel pedido)
        {
            var resultados = await pedidos.AvanzarPedido(pedido, pedido.Estado);
            return Ok(resultados);
        }
        [HttpPost]
        [ActionName("PedidoPorFecha")]
        public async Task<IActionResult> PedidoPorFecha([FromBody]PedidoModel pedido)
        {
            var resultados = await pedidos.LeerPedidosPorFecha(pedido.FechaPedido);
            return Ok(resultados);
        }
        [HttpPost]
        [ActionName("PedidosPorTelefono")]
        public async Task<IActionResult> PedidosPorTelefono([FromBody]PedidoModel pedido)
        {
            var resultados = await pedidos.LeerPedidosPorTelefono(pedido.Telefono);
            return Ok(resultados);
        }
        [HttpPost]
        [ActionName("PedidosPorEstado")]
        public async Task<IActionResult> PedidosPorEstado([FromBody]PedidoModel pedido)
        {
            var resultados = await pedidos.LeerPedidosPorEstado(pedido.Estado);
            return Ok(resultados);
        }

        [HttpPost]
        [ActionName("PedidoPorId")]
        public async Task<IActionResult> PedidoPorId([FromBody]PedidoModel pedido)
        {
            var resultado = await pedidos.LeerPedido(pedido.PedidoID);
            return Ok(resultado);
        }

        //[HttpPost("{id}", Name = "Post")]
        [HttpPost]
        [ActionName("insertarPedido")]
        public async Task<IActionResult> insertarPedido([FromBody] PedidoModel nuevoPedido)
        {
            if (string.IsNullOrWhiteSpace(nuevoPedido.Cliente))
            {
                return this.BadRequest(StatusCodes.Status400BadRequest);
            }
            var id = await pedidos.IniciarPedido(nuevoPedido);
            nuevoPedido.PedidoID = id;
            return Ok(nuevoPedido);
        }
        [HttpPost]
        [ActionName("BorrarPedido")]
        public async Task<IActionResult> BorrarPedido([FromBody]PedidoModel pedido)
        {
            var resultados = await pedidos.borrarPedido(pedido.PedidoID);
            return Ok(resultados);
        }

        [HttpPut("{id}/Avanzar", Name = "Put")]
        public async Task<IActionResult> Put(string id, [FromBody] PedidoModel nuevoPedido, string nuevoEstado)
        {
            var pedido = await pedidos.LeerPedido(id);
            if (pedido == null)
            {
                return NotFound();
            }
            var update = await pedidos.AvanzarPedido(pedido, nuevoEstado);
            return Ok();
        }
        [HttpPut("{id}/Avanzar", Name = "Put")]
        public async Task<IActionResult> Put(string id, [FromBody] PedidoModel nuevoPedido)
        {
            var pedido = await pedidos.LeerPedido(id);
            if (pedido == null)
            {
                return NotFound();
            }
            var update = await pedidos.CompletarPedido(pedido);
            return Ok();
        }

        /*[HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var pedido = await pedidos.LeerPedido(id);
            if (pedido == null)
            {
                return NotFound();
            }
            var res = await pedidos.CancelarPedido(pedido);
            return Ok();
        }*/
    }
}