using Microsoft.Azure.Cosmos.Table;

namespace AzureStorage.Patterns.Models
{
    public class CustomerEntity: TableEntity
    {
        public CustomerEntity(string id)
        {
            PartitionKey = id.Substring(0);
            RowKey = id;
        }

        public string Name { get; set; }
        public string Interests { get; set; }
        public string Groups { get; set; }
    }
}
