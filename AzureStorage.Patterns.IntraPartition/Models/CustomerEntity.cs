using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureStorage.Patterns.IntraPartition.Models
{
    public class CustomerEntity : TableEntity
    {
        public CustomerEntity()
        {

        }
        public CustomerEntity(string type, string id, string indexType)
        {
            PartitionKey = type;
            RowKey = indexType + id;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }
    }
}
