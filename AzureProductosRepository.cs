using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage; // Namespace for StorageAccounts
using Microsoft.WindowsAzure.Storage.Table ;

namespace WEB_API
{
    public class AzureProductosRepository : IProductosRepository
    {
        private string azureConStr;
        public AzureProductosRepository()
        {
            azureConStr = "@DefaultEndpointsProtocol=https;AccountName=s100ne2g2;AccountKey=Rr62TEFQHy4pnCbZypRae+rSD3e3kD7qE1WtqgMX7b6X2/UlVfKLGNnDkLDrDH+R/0tyiAJmfhbJZfDICZlUg==;EndpointSuffix=core.windows.net";
        }

        public void actualizarDatos(ProductoEntity producto)
        {
            throw new NotImplementedException();
        }

        public void actualizarImagen(ProductoEntity producto, object Imagen)
        {
            throw new NotImplementedException();
        }

        public void borrarProducto(string codigo)
        {
            throw new NotImplementedException();
        }

        public void crearProducto(ProductoEntity nuevo)
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(azureConStr);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference("catalogo");

            // Create the table if it doesn't exist.
            var creada = table.CreateIfNotExistsAsync().Result;

            var azEn = new AzProductoEntity(nuevo.Codigo);
            azEn.Categoria = nuevo.Categoria;
            azEn.Descripcion = nuevo.Descripcion;
            azEn.Precio = nuevo.Precio.ToString("C ");
            // Create the TableOperation object that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(azEn);

            // Execute the insert operation.
            var x =  table.ExecuteAsync(insertOperation).Result;

        }

        public ProductoEntity LeerProducto(string codigo)
        {
            return new ProductoEntity();
        }

        public List<ProductoEntity> productosPorCatalogo(string categoria)
        {
            return new List<ProductoEntity>();
        }

        public List<ProductoEntity> todosLosProductos()
        {
            return new List<ProductoEntity>();
        }
    }

    public class AzProductoEntity : TableEntity
    {
        public AzProductoEntity()
        {
        }

        public AzProductoEntity(String codigo)
        {
            this.PartitionKey = codigo.Substring(0, 3);
            this.RowKey = codigo;
            this.Codigo = codigo;
        }

        public string Codigo { get; set; }
        public string Categoria { get; set; }
        public string Descripcion { get; set; }
        public string Precio { get; set; }
        public string Imagen { get; set; }
    }
}