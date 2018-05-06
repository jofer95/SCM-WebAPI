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
            productos = new AzureProductosRepository();
        }
        // GET: Catalogo
        public async Task<ActionResult> Index()
        {            
            var model = (await productos.todosLosProductos())
            .Select(p => new ProductoModel()
            {
                Codigo = p.Codigo,
                Descripcion = p.Descripcion,
                Categoria = p.Categoria,
                Precio = p.Precio,
                Imagen = p.Imagen
            });
            return View(model);
        }

        // GET: Catalogo/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var model = await productos.LeerProducto(id);
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
        public async Task<ActionResult> Create(ProductoCrearModelo model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add insert logic here
                    await productos.crearProducto(new ProductoEntity()
                    {
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
        public async Task<ActionResult> Edit(string id)
        {
            var model = await productos.LeerProducto(id);
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
        public async Task<ActionResult> Edit(string id, ProductoEditarModelo model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await productos.actualizarDatos(new ProductoEntity()
                    {
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
        public async Task<ActionResult> Delete(string id)
        {
            var producto = await productos.LeerProducto(id);
            return View(new ProductoBorrarModelo()
            {
                Codigo = producto.Codigo,
                Descripcion = producto.Descripcion
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
                await productos.borrarProducto(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> CambiarImagen(String id)
        {
            var producto = await productos.LeerProducto(id);
            return View(new ProductoCambiarImagenModelo()
            {
                Codigo = producto.Codigo,
                Descripcion = producto.Descripcion,
                Imagen = producto.Imagen,
            });
        }

        [HttpPost]
        public async Task<ActionResult> CambiarImagen(ProductoCambiarImagenModelo model)
        {
            var p = await productos.LeerProducto(model.Codigo);
            if (p == null)
            {
                return NotFound();
            }
            /*if (!ModelState.IsValid)
            {
                return View(model);
            }*/
            if (model.NuevaImagen.ContentType != "image/jpeg")
            {
                ModelState.AddModelError("NuevaImagen", "Solo se aceptan archivos jpeg");
            }
            if (model.NuevaImagen.Length > 10 * 1024 * 1024)
            {
                ModelState.AddModelError("NuevaImagen", "Tamaño de la imagen es muy grande, mas de 10mb");
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var res = await productos.actualizarImagen(p, model.NuevaImagen);
            return RedirectToAction("Index");
        }
    }
}