using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage; // Namespace for StorageAccounts
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

        private CloudTable TablaAzure(){
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(azureConStr);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference("catalogo");
            return table;
        }

        public void crearProducto(ProductoEntity nuevo)
        {            
            var table = TablaAzure();
            // Create the table if it doesn't exist.
            var creada = table.CreateIfNotExistsAsync().Result;

            var azEn = new AzProductoEntity(nuevo.Codigo);
            azEn.Categoria = nuevo.Categoria;
            azEn.Descripcion = nuevo.Descripcion;
            azEn.Precio = nuevo.Precio.ToString();
            // Create the TableOperation object that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(azEn);

            // Execute the insert operation.
            var x = table.ExecuteAsync(insertOperation).Result;

        }

        public ProductoEntity LeerProducto(string codigo)
        {
            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<CustomerEntity>("Smith", "Ben");

            // Execute the retrieve operation.
            TableResult retrievedResult = table.Execute(retrieveOperation);

            // Print the phone number of the result.
            if (retrievedResult.Result != null)
            {
                Console.WriteLine(((CustomerEntity)retrievedResult.Result).PhoneNumber);
            }             
        }

        public List<ProductoEntity> productosPorCatalogo(string categoria)
        {
            return new List<ProductoEntity>();
        }

        public List<ProductoEntity> todosLosProductos()
        {
            var table = TablaAzure();

            // Construct the query operation for all customer entities where PartitionKey="Smith".
            TableQuery<AzProductoEntity> query = new TableQuery<AzProductoEntity>();
            //.Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Smith"));

            var token = new TableContinuationToken();
            var list = new List<ProductoEntity>();
            // Print the fields for each customer.
            foreach (AzProductoEntity entity in table.ExecuteQuerySegmentedAsync(query, token).Result)
            {
                list.Add(new ProductoEntity()
                {
                    Descripcion = entity.Descripcion,
                    Categoria = entity.Categoria,
                    Codigo = entity.Codigo,
                    //Precio = Convert.ToDecimal(entity.Precio)
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