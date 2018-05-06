using System;

namespace Model{
    public class PedidoModel{
        public Guid PedidoID {get;set;}
        public string Cliente { get; set; }
        public string Producto { get; set; }
        public string Estado { get; set; }
        
    }
}