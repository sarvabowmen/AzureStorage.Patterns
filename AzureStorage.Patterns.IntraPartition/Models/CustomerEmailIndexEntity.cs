using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureStorage.Patterns.IntraPartition.Models
{
    public class CustomerEmailIndexEntity : TableEntity
    {
        public CustomerEmailIndexEntity()
        {

        }
        public CustomerEmailIndexEntity(string type, string id)
        {
            PartitionKey = type;
            RowKey = id;
        }

        public string Id { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }
        public string Type { get; set; }
    }
}
