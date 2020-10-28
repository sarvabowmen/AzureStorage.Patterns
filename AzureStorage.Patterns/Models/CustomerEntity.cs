using Microsoft.Azure.Cosmos.Table;

namespace AzureStorage.Patterns.Models
{
    public class CustomerEntity: TableEntity
    {
        public CustomerEntity(string id)
        {
            PartitionKey = id;
            RowKey = id.Substring(0);
        }

        public string Name { get; set; }
        public string Interests { get; set; }
        public string Groups { get; set; }
    }
}
