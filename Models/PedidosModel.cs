using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace WEB_API.Models{
    /*public class PedidoModel
    {
        public string PedidoID { get; set; }
        public string Descripcion { get; set; }
        public string Categoria { get; set; }
        public decimal Precio { get; set; }
        public string Imagen { get; set; }
    }*/

    public class PedidoEditarModelo
    {
        [Required]
        public string PedidoID {get;set;}
        public string Cliente { get; set; }
        [Required]
        public string Telefono {get;set;}
        public string Producto { get; set; }
        [Required]
        public DateTime FechaPedido {get;set;}
        [Required]
        public decimal Precio { get; set; }
        [Required]
        public string DescripcionProducto { get; set; }
        [Required]
        public string Estado { get; set; }
        public string Latitud {get;set;}
        public string Longitud { get; set; }
    }

    public class PedidoBorrarModelo
    {
        public string PedidoID {get;set;}
        public string Cliente { get; set; }
        public string Telefono {get;set;}
        public string Producto { get; set; }
        public DateTime FechaPedido {get;set;}
        public decimal Precio { get; set; }
        public string DescripcionProducto { get; set; }
        public string Estado { get; set; }
        public string Latitud {get;set;}
        public string Longitud { get; set; }
    }

    public class PedidoCrearModelo
    {
        [Required]
        public string PedidoID {get;set;}
        public string Cliente { get; set; }
        [Required]
        public string Telefono {get;set;}
        public string Producto { get; set; }
        [Required]
        public DateTime FechaPedido {get;set;}
        [Required]
        public decimal Precio { get; set; }
        [Required]
        public string DescripcionProducto { get; set; }
        [Required]
        public string Estado { get; set; }
        public string Latitud {get;set;}
        public string Longitud { get; set; }
    }
}