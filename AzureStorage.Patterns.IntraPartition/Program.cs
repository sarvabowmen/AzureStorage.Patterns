﻿using System;
using AzureStorage.Patterns.IntraPartition.Models;
using Microsoft.Extensions.Configuration;
using AzureStorage.Patterns.Common;
using System.Threading.Tasks;
using AzureStorage.Patterns.IntraPartition.Data;
using System.Text.Json;

namespace AzureStorage.Patterns.IntraPartition
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

            StoreData store = new StoreData();

            Console.WriteLine("Enter one of the operations you want to perform INSERT, UPDATE, READ, DELETE");

            while (true) // Loop indefinitely
            {
                var operation = Console.ReadLine();
                var cust = default(CustomerDetails);
                if (operation.Trim() == "EXIT")
                {
                    break;
                }
                switch (operation)
                {
                    case "INSERT":
                        Console.WriteLine("Enter Customer Name:");
                        var customerNameToAdd = Console.ReadLine();
                        Console.WriteLine("Enter Customer Age:");
                        var customerAge = Console.ReadLine();
                        Console.WriteLine("Enter Customer Email:");
                        var customerEmail = Console.ReadLine();
                        Console.WriteLine("Enter Customer Type (Seller/Agent/Buyer/ThirdParty):");
                        var customerType = Console.ReadLine();

                        var customerToAdd = new CustomerDetails
                        {
                            Name = customerNameToAdd,
                            Id = id,
                            Age = Convert.ToInt32(customerAge),
                            Email = customerEmail,
                            Type = customerType,
                        };
                        cust = await store.AddCustomer(customerToAdd);
                        break;
                    case "UPDATE":
                        Console.WriteLine("Enter Customer Type (Seller/Agent/Buyer/ThirdParty):");
                        var customerTypeToUpdate = Console.ReadLine();
                        Console.WriteLine("Enter Customer Email:");
                        var customerEmailToUpdate = Console.ReadLine();
                        Console.WriteLine("Enter New name:");
                        var customerName = Console.ReadLine();
                        var getcustomer = await store.GetCustomerByEmail(customerTypeToUpdate, customerEmailToUpdate);
                        getcustomer.Name = customerName;
                        cust = await store.UpdateCustomer(getcustomer);
                        break;
                    case "READ":
                        Console.WriteLine("Enter Customer Type (Seller/Agent/Buyer/ThirdParty):");
                        var customerTypeToRead = Console.ReadLine();
                        Console.WriteLine("Enter Customer Email:");
                        var customerEmailToRead = Console.ReadLine();
                        cust = await store.GetCustomerByEmail(customerTypeToRead, customerEmailToRead);
                        break;
                    case "DELETE":
                        Console.WriteLine("Enter Customer Type (Seller/Agent/Buyer/ThirdParty):");
                        var customerTypeToDelete = Console.ReadLine();
                        Console.WriteLine("Enter Customer email:");
                        var customerEmailToDelete = Console.ReadLine();
                        cust = await store.GetCustomerById(customerTypeToDelete, customerEmailToDelete);
                        await store.DeleteCustomer(customerTypeToDelete, customerEmailToDelete);
                        break;
                    default:
                        break;
                }

                if (cust == null)
                    Console.WriteLine("Customer Doesn't Exists");
                else
                    Console.WriteLine("Customer {1} {0} Successfully {2}", operation, id, JsonSerializer.Serialize(cust, new JsonSerializerOptions { WriteIndented = true })); 
                
                Console.WriteLine("End");
            }
        }
    }
}
