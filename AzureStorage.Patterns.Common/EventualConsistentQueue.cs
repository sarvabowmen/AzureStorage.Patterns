using System;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

namespace AzureStorage.Patterns.Common
{
    public class EventualConsistentQueue
    {

        private async Task<QueueClient> CreateQueueAsync(string name)
        {
            string connectionString = CloudConfigurationManager.Config.GetConnectionString("TableStorageConnection");

            QueueClient queueClient = new QueueClient(connectionString, name, new QueueClientOptions {  MessageEncoding = QueueMessageEncoding.Base64 });

            await queueClient.CreateIfNotExistsAsync();

            if (queueClient.Exists())
            {
                Console.WriteLine("Queue Exists");
            }
            else 
            {
                Console.WriteLine("Queue created");
            }

            return queueClient;

        }

        public async Task AddMessageAsync(string queueName, string newMessage)
        {
            var queue = await CreateQueueAsync(queueName);
       
            await queue.SendMessageAsync(newMessage);            
        }
    }
}
