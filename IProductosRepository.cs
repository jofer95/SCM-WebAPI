using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB_API{
    public interface IProductosRepository
    {
        List<ProductoEntity> todosLosProductos();
        List<ProductoEntity> productosPorCatalogo(string categoria);
        ProductoEntity LeerProducto(string codigo);
        void crearProducto(ProductoEntity nuevo);
        void actualizarDatos(ProductoEntity producto);
        void actualizarImagen(ProductoEntity producto, object Imagen);
        void borrarProducto(string codigo);
    }

    public class MemoryProductosRepository : IProductosRepository
    {
        private static List<ProductoEntity> db;
        static MemoryProductosRepository(){
            db = new List<ProductoEntity>();
            db.Add(new ProductoEntity(){
                Codigo = "001",
                Descripcion = "producto 1",
                Categoria = "Categoria 1", 
                Precio = 100,
                Imagen = string.Empty
            });
        }
        public void actualizarDatos(ProductoEntity producto)
        {
            var existe = db.FirstOrDefault(s => s.Codigo == producto.Codigo);
            if(existe != null){
                existe.Categoria = producto.Categoria;
                existe.Precio = producto.Precio;
                existe.Descripcion = producto.Descripcion;
            }
        }

        public void actualizarImagen(ProductoEntity producto, object Imagen)
        {
            throw new NotImplementedException();
        }

        public void borrarProducto(string codigo)
        {
            var existe = db.FirstOrDefault(s => s.Codigo == codigo);
            if(existe != null){
                db.Remove(existe);
            }
        }

        public void crearProducto(ProductoEntity nuevo)
        {
            db.Add(nuevo); 
        }

        public ProductoEntity LeerProducto(string codigo)
        {
            return db.FirstOrDefault(p => p.Codigo == codigo);
        }

        public List<ProductoEntity> productosPorCatalogo(string categoria)
        {
            return db.Where(s => s.Categoria == categoria).ToList();
        }

        public List<ProductoEntity> todosLosProductos()
        {
            return db.ToList();
        }
    }

    public class ProductoEntity{
        public string Codigo { get; set; }
        public string Categoria { get; set; }
        public string Descripcion  { get; set; }
        public decimal Precio { get; set; }
        public string Imagen { get; set; }
    }
}