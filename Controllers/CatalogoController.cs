using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB_API.Models;

namespace WEB_API.Controllers
{
    public class CatalogoController : Controller
    {
        IProductosRepository productos;
        public CatalogoController()
        {
            productos = new MemoryProductosRepository();
        }
        // GET: Catalogo
        public ActionResult Index()
        {
            var model = productos.todosLosProductos()
            .Select( p => new ProductoModel(){
                    Codigo = p.Codigo,
                    Descripcion = p.Descripcion,
                    Categoria = p.Categoria,
                    Precio = p.Precio
                });
            /*var model = new List<ProductoModel>();
            model.Add(new ProductoModel(){
                Descripcion = "producto",
                Precio = 100
            });
            model.Add(new ProductoModel(){
                Descripcion = "producto 2",
                Precio = 140
            });*/
            return View(model);
        }

        // GET: Catalogo/Details/5
        public ActionResult Details(string id)
        {
            var model = productos.LeerProducto(id);
            return View(model);
        }

        // GET: Catalogo/Create
        public ActionResult Create()
        {
            var model = new ProductoCrearModelo();
            return View(model);
        }

        // POST: Catalogo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductoCrearModelo model)
        {
            if(ModelState.IsValid){

            
            try
            {
                // TODO: Add insert logic here
                productos.crearProducto(new ProductoEntity(){
                    Codigo = model.Codigo,
                    Descripcion = model.Descripcion,
                    Precio = model.Precio,
                    Categoria = model.Categoria
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
        public ActionResult Edit(string id)
        {
            var model = productos.LeerProducto(id);
            ProductoEditarModelo edit = new ProductoEditarModelo();
            edit.Codigo = model.Codigo;
            edit.Descripcion = model.Descripcion;
            edit.Categoria = model.Categoria;
            edit.Precio = model.Precio;
            return View(edit);
        }

        // POST: Catalogo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, ProductoEditarModelo model)
        {
            if(ModelState.IsValid){
                try
            {
                productos.actualizarDatos(new ProductoEntity(){
                    Codigo = model.Codigo,
                    Descripcion = model.Descripcion,
                    Categoria = model.Categoria,
                    Precio = model.Precio
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
        public ActionResult Delete(string id)
        {
            var producto = productos.LeerProducto(id);
            return View(new ProductoBorrarModelo(){
                Codigo = producto.Codigo,
                Descripcion = producto.Descripcion
            });
        }

        // POST: Catalogo/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                productos.borrarProducto(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}