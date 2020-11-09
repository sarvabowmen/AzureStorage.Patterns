using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AzureStorage.Patterns.Common
{
    public static class  DataAccess
    {
        public static async Task Delete(CloudTable table, ITableEntity customerEntity)
        {
            TableOperation operation = TableOperation.Delete(customerEntity);

            TableResult result = await table.ExecuteAsync(operation);

            Console.WriteLine("\t{0} Deleted Successfully", customerEntity.RowKey);

        }

        public static async Task<ITableEntity> InsertOrMerge(CloudTable table, ITableEntity customerEntity)
        {
            TableOperation operation = TableOperation.InsertOrMerge(customerEntity);

            TableResult result = await table.ExecuteAsync(operation);

            ITableEntity customer = result.Result as ITableEntity;
            //if (customer != null)
            //{
            //    Console.WriteLine("\t{0}", JsonSerializer.Deserialize<T>);
            //}

            return customer;
        }

        public static async Task ExecuteBatchAsync(CloudTable table, IEnumerable<ITableEntity> customerEntities)
        {
            TableBatchOperation operation = new TableBatchOperation();

            foreach (var item in customerEntities)
            {
                operation.InsertOrMerge(item); 
            }

           var s =  await table.ExecuteBatchAsync(operation);
            //if (customer != null)
            //{
            //    Console.WriteLine("\t{0}", JsonSerializer.Deserialize<T>);
            //}
        }

        public static async Task ExecuteBatchDeleteAsync(CloudTable table, IEnumerable<ITableEntity> customerEntities)
        {
            TableBatchOperation operation = new TableBatchOperation();

            foreach (var item in customerEntities)
            {
                operation.Delete(item);
            }

            await table.ExecuteBatchAsync(operation);
            //if (customer != null)
            //{
            //    Console.WriteLine("\t{0}", JsonSerializer.Deserialize<T>);
            //}
        }

        public static async Task<T> RetriveUsingRowAndPartitionKey<T>(CloudTable table, string partitionKey, string rowKey) where T : TableEntity, new()
        {
            TableOperation operation = TableOperation.Retrieve<T>(partitionKey, rowKey);

            TableResult result = await table.ExecuteAsync(operation);

            T customer = result.Result as T;
            //if (customer != null)
            //{
            //    Console.WriteLine("\t{0}\t{1}\t{2}\t{3}", customer.PartitionKey, customer.RowKey, customer.Groups, customer.Interests);
            //}

            return customer;
        }

        private static CloudStorageAccount CreateTableStorage(string connectionString)
        {
            CloudStorageAccount cloudStorage;
            cloudStorage = CloudStorageAccount.Parse(connectionString);

            return cloudStorage;
        }

        public static async Task<CloudTable> CreateTable(string tableName)
        {

            string connectionString = CloudConfigurationManager.Config.GetConnectionString("TableStorageConnection");

            var cloudStorage = CreateTableStorage(connectionString);
            var tableClient = cloudStorage.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference(tableName);

            try
            {

                if (await table.CreateIfNotExistsAsync())
                {

                    Console.WriteLine("Created Table named: {0}", tableName);
                }
                else
                {
                    Console.WriteLine("Already Table exists: {0}", tableName);
                }


            }
            catch (System.Exception)
            {

                throw;
            }
            return table;
        }
    }
}
