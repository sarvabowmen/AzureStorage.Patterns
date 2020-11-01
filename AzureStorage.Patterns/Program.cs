﻿using AzureStorage.Patterns.Data;
using AzureStorage.Patterns.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace AzureStorage.Patterns.One
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json").AddEnvironmentVariables().Build();

            CloudConfigurationManager.Config = config;

            var id = Guid.NewGuid().ToString();
            Console.WriteLine("{0} This is autogenerated customer id", id);
            var customer = new Customer
            {
                Name = "Cust1",
                Groups = new Groups { Id = 1, Value = "Group1" },
                Interests = new Interests { Id = 1, Value = "Interests1" },
                Id = id
            };

            StoringSubObjectsAsColumns store = new StoringSubObjectsAsColumns();

            Console.WriteLine("Enter one of the operations you want to perform INSERT, UPDATE, READ, DELETE");

            while (true) // Loop indefinitely
            {
                var operation = Console.ReadLine();
                var cust = default(Customer);
                if (operation.Trim() == "EXIT")
                {
                    break;
                }
                switch (operation)
                {
                    case "INSERT":
                        cust = await store.AddCustomer(customer);
                        break;
                    case "UPDATE":
                        var getcustomer = await store.GetCustomer(id);
                        getcustomer.Name = "CustomerNameUpdated";
                        cust = await store.UpdateCustomer(getcustomer);
                        break;
                    case "READ":
                        cust = await store.GetCustomer(id);
                        break;
                    case "DELETE":
                        cust = await store.GetCustomer(id);
                        await store.DeleteCustomer(id);
                        break;
                    default:
                        break;
                }

                Console.WriteLine("Customer {1} {0} Successfully {2}", operation, id, JsonSerializer.Serialize(cust));
                Console.WriteLine("End");
            }
        }
    }
}
