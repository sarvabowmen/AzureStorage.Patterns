using AzureStorage.Patterns.Models;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorage.Patterns
{
    public static class DataAccess
    {
        public static async Task Delete(CloudTable table, CustomerEntity customerEntity)
        {
            TableOperation operation = TableOperation.Delete(customerEntity);

            TableResult result = await table.ExecuteAsync(operation);

            Console.WriteLine("\t{0} Deleted Successfully", customerEntity.RowKey);

        }

        public static async Task<CustomerEntity> InsertOrMerge(CloudTable table, CustomerEntity customerEntity)
        {
            TableOperation operation = TableOperation.InsertOrMerge(customerEntity);

            TableResult result = await table.ExecuteAsync(operation);

            CustomerEntity customer = result.Result as CustomerEntity;
            if (customer != null)
            {
                Console.WriteLine("\t{0}\t{1}\t{2}\t{3}", customer.PartitionKey, customer.RowKey, customer.Groups, customer.Interests);
            }

            return customer;
        }

        public static async Task<CustomerEntity> RetriveUsingRowAndPartitionKey(CloudTable table, string partitionKey, string rowKey)
        {
            TableOperation operation = TableOperation.Retrieve<CustomerEntity>(partitionKey, rowKey);

            TableResult result = await table.ExecuteAsync(operation);

            CustomerEntity customer = result.Result as CustomerEntity;
            if (customer != null)
            {
                Console.WriteLine("\t{0}\t{1}\t{2}\t{3}", customer.PartitionKey, customer.RowKey, customer.Groups, customer.Interests);
            }

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
