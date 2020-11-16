using AzureStorage.Patterns.InterPartition.Models;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static AzureStorage.Patterns.Common.DataAccess;

namespace AzureStorage.Patterns.InterPartition.Data
{
    class StoreData
    {
        private const string EmailIdIndexPrefix = "email_";
        private const string UserIdIndexPrefix = "id_";

        public async Task<CustomerDetails> GetCustomerById(string type, string id)
        {
            var table = await CreateTable("CustomersInterPartition");

            var result = await RetriveUsingRowAndPartitionKey<CustomerEntity>(table, UserIdIndexPrefix + type, id);

            if (result != null)
            {
                Console.WriteLine("\t{0}\t{1}\t{2}\t{3}\t{4}", result.PartitionKey, result.RowKey, result.Age, result.Email, result.Type);
            }

            var customerViewModel = MapViewModel(result);
            return customerViewModel;

        }

        public async Task<CustomerDetails> GetCustomerByEmail(string type, string email)
        {
            var table = await CreateTable("CustomersInterPartition");

            var result = await RetriveUsingRowAndPartitionKey<CustomerEntity>(table, EmailIdIndexPrefix + type, email);

            if (result != null)
            {
                Console.WriteLine("\t{0}\t{1}\t{2}\t{3}\t{4}", result.PartitionKey, result.RowKey, result.Age, result.Email, result.Type);
            }


            var customerViewModel = MapViewModel(result);
            return customerViewModel;

        }

        public async Task<CustomerDetails> AddCustomer(CustomerDetails cust)
        {
            var customerEntity1 = new CustomerEntity(EmailIdIndexPrefix + cust.Type, cust.Email)
            {
                Id = cust.Id,
                Name = cust.Name,
                Age = cust.Age,
                Email = cust.Email,
                Type = cust.Type
            };
            var customerEntity2 = new CustomerEntity(cust.Type + UserIdIndexPrefix, cust.Id)
            {
                Id = cust.Id,
                Name = cust.Name,
                Age = cust.Age,
                Email = cust.Email,
                Type = cust.Type
            };


            var table = await CreateTable("CustomersInterPartition");

            await InsertOrMerge(table, customerEntity1);
            await InsertOrMerge(table, customerEntity2);
            return cust;
        }

        public async Task<CustomerDetails> UpdateCustomer(CustomerDetails cust)
        {
            var customerEntity1 = new CustomerEntity(EmailIdIndexPrefix + cust.Type, cust.Id)
            {
                    Id = cust.Id,
                    Name = cust.Name,
                    Email = cust.Email,
                    Age = cust.Age,
                    Type = cust.Type
            };

            var customerEntity2 = new CustomerEntity(UserIdIndexPrefix + cust.Type, cust.Id)
            {
                Id = cust.Id,
                Name = cust.Name,
                Age = cust.Age,
                Email = cust.Email,
                Type = cust.Type
            };


            var table = await CreateTable("CustomersInterPartition");

            await InsertOrMerge(table, customerEntity1);
            await InsertOrMerge(table, customerEntity2);
            if (cust != null)
            {
                Console.WriteLine("\t{0}\t{1}\t{2}\t{3}\t{4}", cust.Id, cust.Name, cust.Age, cust.Email, cust.Type);
            }
            return cust;
        }

        public async Task DeleteCustomer(string type, string email)
        {

            var table = await CreateTable("CustomersInterPartition");
            var resultForEmail = await RetriveUsingRowAndPartitionKey<CustomerEntity>(table, EmailIdIndexPrefix + type, email);
            var resultForId = await RetriveUsingRowAndPartitionKey<CustomerEntity>(table, UserIdIndexPrefix + type, resultForEmail.Id);

            await ExecuteBatchDeleteAsync(table, new[] { resultForEmail, resultForId });
        }



        private CustomerDetails MapViewModel(CustomerEntity cust)
        {
            if (cust == null)
                return null;

            return new CustomerDetails
            {
                Id = cust?.RowKey,
                Name = cust?.Name,
                Age = cust.Age,
                Email = cust?.Email,
                Type = cust?.Type
            };
        }
    }
}
