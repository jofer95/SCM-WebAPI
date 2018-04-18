using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB_API.Models;

namespace WEB_API.Controllers
{
    [Produces("application/json")]
    [Route("api/Productos")]
    public class ProductosController : Controller
    {
        IProductosRepository productos;
        public ProductosController()
        {
            productos = new AzureProductosRepository();
        }
        [HttpGet]
        public IEnumerable<ProductoModel> GetAll()
        {
            var model = productos.todosLosProductos()
            .Select( p => new ProductoModel(){
                    Codigo = p.Codigo,
                    Descripcion = p.Descripcion,
                    Categoria = p.Categoria,
                    Precio = p.Precio
                });
            return model;
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(string id)
        {
            var model = productos.LeerProducto(id);
            return new ObjectResult(model);
        }
    }
}