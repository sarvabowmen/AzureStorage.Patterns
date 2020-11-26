using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using AzureStorage.Patterns.Common;
using AzureStorage.Patterns.Common.Models;
using static AzureStorage.Patterns.Common.DataAccess;
namespace AzureStoragePatterns.Worker
{
    public static class ConsistentInterPartitionQueueFunction
    {
        [FunctionName("ConsistentInterPartitionQueueFunction")]
        public static async Task Run([QueueTrigger("interpartition-queue", Connection = "Connection")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
            var userId = myQueueItem.Split(':')[0];
            var type = myQueueItem.Split(':')[1];
            await InsertCustomerByEmailAsync(userId, type);
        }
        private static async Task InsertCustomerByEmailAsync(string userId, string type)
        {
            var table = await CreateTable("CustomersInterPartition");
            var customer = await RetriveUsingRowAndPartitionKey<CustomerEntity>(table, UserIdIndexPrefix + type, userId);
            var customerEntityEmail = new CustomerEntity(EmailIdIndexPrefix + customer.Type, customer.Id)
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Age = customer.Age,
                Type = customer.Type
            };
            var instertedCust = await InsertOrMerge(table, customerEntityEmail);
            if (instertedCust != null)
            {
                Console.WriteLine("\t{0}\t{1}\t{2}\t{3}\t{4}", customer.Id, customer.Name, customer.Age, customer.Email, customer.Type);
            }
        }
    }
}
