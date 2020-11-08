using System;
using System.Collections.Generic;
using System.Text;

namespace AzureStorage.Patterns.IntraPartition.Models
{
    public class CustomerDetails
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }
    }
}
