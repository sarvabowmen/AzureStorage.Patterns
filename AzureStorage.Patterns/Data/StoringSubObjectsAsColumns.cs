using AzureStorage.Patterns.Models;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using static AzureStorage.Patterns.Data.DataAccess;

namespace AzureStorage.Patterns.Data
{
    public class StoringSubObjectsAsColumns
    {
        public async Task<Customer> GetCustomer(string id)
        {
            var table = await CreateTable("Customers");

            var result = await RetriveUsingRowAndPartitionKey(table, id, id.Substring(0));

            var intrests = JsonSerializer.Deserialize<Interests>(result.Interests);
            var groups = JsonSerializer.Deserialize<Groups>(result.Groups);

            var customerViewModel = new Customer
            {
                Id = result.RowKey,
                Name = result.Name,
                Groups = groups,
                Interests = intrests
            };
            return customerViewModel;

        }

        public async Task<Customer> AddCustomer(Customer cust)
        {
            var customerEntity = new CustomerEntity(cust.Id)
            {
                Name = cust.Name,
                Groups = JsonSerializer.Serialize(cust.Groups),
                Interests = JsonSerializer.Serialize(cust.Interests),
            };

            var table = await CreateTable("Customers");

            var addedRecord = await InsertOrMerge(table, customerEntity);
            return MapViewModel(addedRecord);
        }

        public async Task<Customer> UpdateCustomer(Customer cust)
        {
            var customerEntity = new CustomerEntity(cust.Id)
            {
                Name = cust.Name,
                Groups = JsonSerializer.Serialize(cust.Groups),
                Interests = JsonSerializer.Serialize(cust.Interests),
            };

            var table = await CreateTable("Customers");

            var addedRecord = await InsertOrMerge(table, customerEntity);
            return MapViewModel(addedRecord);
        }

        public async Task DeleteCustomer(string id)
        {
           
            var table = await CreateTable("Customers");
            var result = await RetriveUsingRowAndPartitionKey(table, id, id.Substring(0));

            await Delete(table, result);
        }



        private Customer MapViewModel(CustomerEntity cust)
        {
            return new Customer
            {
                Id = cust.RowKey,
                Name = cust.Name,
                Groups = JsonSerializer.Deserialize<Groups>(cust.Groups),
                Interests = JsonSerializer.Deserialize<Interests>(cust.Interests)
            };
        }

    }
}
