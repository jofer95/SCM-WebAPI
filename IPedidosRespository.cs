using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model;

namespace WEB_API{
    public interface IPedidosRepository
    {
        Task<List<PedidoModel>> todosLosPedidos();
        Task<PedidoModel> LeerPedido(string id);
        Task<string> IniciarPedido(PedidoModel nuevoPedido);
        Task<bool> AvanzarPedido(PedidoModel pedido, string nuevoEstado);
        Task<bool> actualizarDatos(PedidoModel producto);
        Task<bool> CancelarPedido(PedidoModel pedido);
        Task<bool> CompletarPedido(PedidoModel pedido);
        Task<List<PedidoModel>> LeerPedidosPorFecha(DateTime fecha);
        Task<List<PedidoModel>> LeerPedidosPorTelefono(string telefono);
        Task<List<PedidoModel>> LeerPedidosPorEstado(string estado);
        Task<bool> borrarPedido(string codigo);
    }
}