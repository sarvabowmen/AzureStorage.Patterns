using System;
using System.Collections.Generic;
using System.Text;

namespace AzureStorage.Patterns.Models
{
    public class Customer
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public Interests Interests { get; set; }
        public Groups Groups { get; set; }
    }

    public class Interests
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }

    public class Groups
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
}
