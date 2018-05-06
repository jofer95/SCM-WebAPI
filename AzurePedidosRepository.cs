using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage; // Namespace for StorageAccounts
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using Model;

namespace WEB_API
{
    public class AzurePedidosRepository : IPedidosRepository
    {
        private string azureConStr;
        public AzurePedidosRepository()
        {
            azureConStr = "DefaultEndpointsProtocol=https;AccountName=s100ne2g2;AccountKey=IRr62TEFQHy4pnCbZypRae+rSD3e3kD7qE1WtqgMX7b6X2/UlVfKLGNnDkLDrDH+R/0tyiAJmfhbJZfDICZlUg==";
        }
        public async Task<bool> AvanzarPedido(PedidoModel pedido, string nuevoEstado)
        {
            var table = TablaAzure();
            var retriveOp = TableOperation.Retrieve<PedidoAzureEntity>("P" + pedido.FechaPedido.ToString("YYYYMMDD"), pedido.PedidoID.ToString());
            var resultado = await table.ExecuteAsync(retriveOp);
            if (resultado != null)
            {
                var p = resultado.Result as PedidoAzureEntity;
                p.Estado = nuevoEstado;
                var upOp = TableOperation.Replace(p);
                await table.ExecuteAsync(upOp);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CancelarPedido(PedidoModel pedido)
        {
            return await AvanzarPedido(pedido, "CA");
        }

        public async Task<bool> CompletarPedido(PedidoModel pedido)
        {
            return await AvanzarPedido(pedido, "CO");
        }

        public async Task<string> IniciarPedido(PedidoModel nuevoPedido)
        {
            var table = TablaAzure();
            // Create the table if it doesn't exist.
            var creada = await table.CreateIfNotExistsAsync();

            var azEn = new PedidoAzureEntity(nuevoPedido.FechaPedido, nuevoPedido.Telefono);
            azEn.Cliente = nuevoPedido.Cliente;
            azEn.Telefono = nuevoPedido.Telefono;
            azEn.DescripcionProducto = nuevoPedido.DescripcionProducto;
            azEn.Estado = "N";
            azEn.Producto = nuevoPedido.Producto;
            azEn.Precio = nuevoPedido.Precio.ToString();
            // Create the TableOperation object that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(azEn);

            // Execute the insert operation.
            var x = await table.ExecuteAsync(insertOperation);
            return azEn.RowKey;
        }

        public async Task<PedidoModel> LeerPedido(string id)
        {
            var table = TablaAzure();
            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation
            .Retrieve<PedidoAzureEntity>(PedidoAzureEntity.PartitionFromRowID(id), id);

            // Execute the retrieve operation.
            TableResult retrievedResult = await table.ExecuteAsync(retrieveOperation);

            // Print the phone number of the result.
            if (retrievedResult.Result != null)
            {
                var az = retrievedResult.Result as PedidoAzureEntity;
                return new PedidoModel()
                {
                    PedidoID = az.PedidoID,
                    Telefono = az.Telefono,
                    Cliente = az.Cliente,
                    FechaPedido = az.FechaPedido,
                    Precio = decimal.Parse(az.Precio)
                };
                //Console.WriteLine(((AzProductoEntity)retrievedResult.Result).PhoneNumber);
            }
            return null;
        }

        public async Task<List<PedidoModel>> LeerPedidosPorFecha(DateTime fecha)
        {
            var tabla = TablaAzure();
            var particion = PedidoAzureEntity.PartitionKeyFromFecha(fecha);
            TableQuery<PedidoAzureEntity> query = new TableQuery<PedidoAzureEntity>()
            .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, particion));

            var token = new TableContinuationToken();
            var list = new List<PedidoModel>();
            foreach (PedidoAzureEntity az in await tabla.ExecuteQuerySegmentedAsync(query, token))
            {
                list.Add(new PedidoModel()
                {
                    PedidoID = az.PedidoID,
                    Telefono = az.Telefono,
                    Cliente = az.Cliente,
                    FechaPedido = az.FechaPedido,
                    Precio = decimal.Parse(az.Precio)
                });
            }
            return list;
        }

        private CloudTable TablaAzure()
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(azureConStr);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference("pedidos");
            return table;
        }
    }

    public class PedidoAzureEntity : TableEntity
    {
        public static string PartitionFromRowID(string id)
        {
            if (!string.IsNullOrWhiteSpace(id) || id.Length < 9)
            {
                return string.Empty;
            }
            return id.Substring(0, 9);
        }

        public static string RowKeyFromFechaTelefono(DateTime fecha, string telefono) => PartitionKeyFromFecha(fecha) + "_" + fecha.ToString("hh_mm:ss") + "_" + telefono;

        public static string PartitionKeyFromFecha(DateTime fecha) => "P" + fecha.ToString("yyyMMdd");
        public PedidoAzureEntity()
        {

        }
        public PedidoAzureEntity(DateTime fechaPedido, string telefono)
        {
            PartitionKey = PartitionKeyFromFecha(fechaPedido);
            RowKey = RowKeyFromFechaTelefono(fechaPedido, telefono);
            FechaPedido = fechaPedido;
        }
        public string PedidoID { get { return RowKey; } }
        public string Cliente { get; set; }
        public string Producto { get; set; }
        public DateTime FechaPedido { get; set; }
        public string Precio { get; set; }
        public string DescripcionProducto { get; set; }
        public string Estado { get; set; }
        public string Telefono { get; set; }
    }
}