using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model;

namespace WEB_API{
    public interface IPedidosRepository
    {
        Task<PedidoModel> LeerPedido(string id);
        Task<string> IniciarPedido(PedidoModel nuevoPedido);
        Task<bool> AvanzarPedido(PedidoModel pedido, string nuevoEstado);
        Task<bool> CancelarPedido(PedidoModel pedido);
        Task<bool> CompletarPedido(PedidoModel pedido);
        Task<List<PedidoModel>> LeerPedidosPorFecha(DateTime fecha);
        
    }
}