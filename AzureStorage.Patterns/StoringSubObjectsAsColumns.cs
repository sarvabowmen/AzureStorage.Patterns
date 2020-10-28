using AzureStorage.Patterns.Models;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AzureStorage.Patterns
{
    public class StoringSubObjectsAsColumns
    {
        public async Task<Customer> GetCustomer(string id)
        {
            var table = await CreateTable("Customers");

            var result = await RetriveUsingRowAndPartitionKey(table, id, id.Substring(0));

            var intrests = JsonSerializer.Deserialize<Interests>(result.Interests);
            var groups = JsonSerializer.Deserialize<Groups>(result.Groups);

            var customerViewModel = new Customer
            {
                Id = result.PartitionKey,
                Name = result.Name,
                Groups = groups,
                Interests = intrests
            };
            return customerViewModel;

        }

        public Task<Customer> AddCustomer(string id)
        {
            return Task.FromResult<Customer>(new Customer());
        }

        private async Task<CustomerEntity> RetriveUsingRowAndPartitionKey(CloudTable table, string partitionKey, string rowKey)
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

        private CloudStorageAccount CreateTableStorage(string connectionString)
        {
            CloudStorageAccount cloudStorage;
            cloudStorage = CloudStorageAccount.Parse(connectionString);

            return cloudStorage;
        }

        private async Task<CloudTable> CreateTable(string tableName)
        {

            string connectionString = CloudConfigurationManager.Config.GetConnectionString("StorageConnectionString");

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
