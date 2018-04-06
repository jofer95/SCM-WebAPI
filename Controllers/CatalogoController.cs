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
        // GET: Catalogo
        public ActionResult Index()
        {
            var model = new List<ProductoModel>();
            model.Add(new ProductoModel(){
                Descripcion = "producto",
                Precio = 100
            });
            model.Add(new ProductoModel(){
                Descripcion = "producto 2",
                Precio = 140
            });
            return View(model);
        }

        // GET: Catalogo/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Catalogo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Catalogo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Catalogo/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Catalogo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Catalogo/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Catalogo/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}