using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage; // Namespace for StorageAccounts
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;

namespace WEB_API
{
    public class AzureProductosRepository : IProductosRepository
    {
        private string azureConStr;
        public AzureProductosRepository()
        {
            azureConStr = "DefaultEndpointsProtocol=https;AccountName=s100ne2g2;AccountKey=IRr62TEFQHy4pnCbZypRae+rSD3e3kD7qE1WtqgMX7b6X2/UlVfKLGNnDkLDrDH+R/0tyiAJmfhbJZfDICZlUg==";
        }

        public async Task<bool> actualizarDatos(ProductoEntity producto)
        {
            var table = TablaAzure();
            var retriveOp = TableOperation.Retrieve<AzProductoEntity>(producto.Codigo.Substring(0, 3), producto.Codigo);
            var resultado = await table.ExecuteAsync(retriveOp);
            if (resultado != null)
            {
                var p = resultado.Result as AzProductoEntity;
                p.Precio = producto.Precio.ToString();
                p.Descripcion = producto.Descripcion;
                p.Categoria = producto.Categoria;

                var upOp = TableOperation.Replace(p);
                await table.ExecuteAsync(upOp);
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<bool> actualizarImagen(ProductoEntity producto, object Imagen)
        {
            var file = Imagen as IFormFile;
            if (file == null)
            {
                throw new ArgumentException(nameof(Imagen));
            }
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(azureConStr);
            // Create a blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Get a reference to a container named "my-new-container."
            CloudBlobContainer container = blobClient.GetContainerReference("productos");
            // Get a reference to a blob named "myblob".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(producto.Codigo + ".jpeg");

            using (var fileStream = file.OpenReadStream())
            {
                await blockBlob.UploadFromStreamAsync(fileStream);
            }        

            //Obtenemos la url de la foto ingresada al bloob
            var url = blockBlob.Uri.AbsoluteUri;

            //Ahora actualizamos
            var table = TablaAzure();
            var retriveOp = TableOperation.Retrieve<AzProductoEntity>(producto.Codigo.Substring(0, 3), producto.Codigo);
            var resultado = await table.ExecuteAsync(retriveOp);
            if (resultado != null)
            {
                var p = resultado.Result as AzProductoEntity;
                p.Imagen = url;

                var upOp = TableOperation.Replace(p);
                await table.ExecuteAsync(upOp);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> borrarProducto(string Codigo)
        {
            var table = TablaAzure();
            var retriveOp = TableOperation.Retrieve<AzProductoEntity>(Codigo.Substring(0, 3), Codigo);
            var resultado = await table.ExecuteAsync(retriveOp);
            if (resultado != null)
            {
                var p = resultado.Result as AzProductoEntity;
                var upOp = TableOperation.Delete(p);
                await table.ExecuteAsync(upOp);
                return true;
            }
            else
            {
                return false;
            }
        }

        private CloudTable TablaAzure()
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(azureConStr);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference("catalogo");
            return table;
        }

        public async Task<bool> crearProducto(ProductoEntity nuevo)
        {
            var table = TablaAzure();
            // Create the table if it doesn't exist.
            var creada = await table.CreateIfNotExistsAsync();

            var azEn = new AzProductoEntity(nuevo.Codigo);
            azEn.Categoria = nuevo.Categoria;
            azEn.Descripcion = nuevo.Descripcion;
            azEn.Precio = nuevo.Precio.ToString();
            // Create the TableOperation object that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(azEn);

            // Execute the insert operation.
            var x = await table.ExecuteAsync(insertOperation);
            return true;

        }

        public async Task<ProductoEntity> LeerProducto(string codigo)
        {
            var table = TablaAzure();
            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<AzProductoEntity>(codigo.Substring(0, 3), codigo);

            // Execute the retrieve operation.
            TableResult retrievedResult = await table.ExecuteAsync(retrieveOperation);

            // Print the phone number of the result.
            if (retrievedResult.Result != null)
            {
                var az = retrievedResult.Result as AzProductoEntity;
                return new ProductoEntity()
                {
                    Codigo = az.Codigo,
                    Descripcion = az.Descripcion,
                    Categoria = az.Categoria,
                    Precio = decimal.Parse(az.Precio),
                    Imagen = az.Imagen
                };
                //Console.WriteLine(((AzProductoEntity)retrievedResult.Result).PhoneNumber);
            }
            return null;
        }

        public async Task<List<ProductoEntity>> productosPorCatalogo(string categoria)
        {
            return new List<ProductoEntity>();
        }

        public async Task<List<ProductoEntity>> todosLosProductos()
        {
            var table = TablaAzure();

            // Construct the query operation for all customer entities where PartitionKey="Smith".
            TableQuery<AzProductoEntity> query = new TableQuery<AzProductoEntity>();
            //.Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Smith"));

            var token = new TableContinuationToken();
            var list = new List<ProductoEntity>();
            // Print the fields for each customer.
            foreach (AzProductoEntity entity in await table.ExecuteQuerySegmentedAsync(query, token))
            {
                list.Add(new ProductoEntity()
                {
                    Descripcion = entity.Descripcion,
                    Categoria = entity.Categoria,
                    Codigo = entity.Codigo,
                    Precio = Convert.ToDecimal(entity.Precio),
                    Imagen = entity.Imagen
                });
                /*Console.WriteLine("{0}, {1}\t{2}\t{3}", entity.PartitionKey, entity.RowKey,
                    entity.Email, entity.PhoneNumber);*/
            }
            return list;
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