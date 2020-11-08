using AzureStorage.Patterns.IntraPartition.Models;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using static AzureStorage.Patterns.Common.DataAccess;

namespace AzureStorage.Patterns.IntraPartition.Data
{
    class StoreData
    {
        private const string EmailIdIndexPrefix = "email_";
        private const string UserIdIndexPrefix = "id_";

        public async Task<CustomerDetails> GetCustomer(string id)
        {
            var table = await CreateTable("Customers");

            var result = await RetriveUsingRowAndPartitionKey(table, id.Substring(0, 1), id) as CustomerEntity;

            if (result != null)
            {
                Console.WriteLine("\t{0}\t{1}\t{2}\t{3}\t{4}", result.PartitionKey, result.RowKey, result.Age, result.Email, result.Type);
            }
           

            var customerViewModel = new CustomerDetails
            {
                Id = result.RowKey,
                Name = result.Name,
                Age =  result.Age,
                Email =  result.Email,
                Type =  result.Type    
            };
            return customerViewModel;

        }

        public async Task<CustomerDetails> AddCustomer(CustomerDetails cust)
        {
            var customerEntityLst = new List<CustomerEntity>();
            customerEntityLst.Add(new CustomerEntity(cust.Type, cust.Id, EmailIdIndexPrefix)
            {
                Id = cust.Id,
                Name = cust.Name,
                Age = cust.Age,
                Email = cust.Email,
                Type = cust.Type
            });

            customerEntityLst.Add(new CustomerEntity(cust.Type, cust.Id, UserIdIndexPrefix)
            {
                Id = cust.Id,
                Name = cust.Name,
                Age = cust.Age,
                Email = cust.Email,
                Type = cust.Type
            });


            var table = await CreateTable("Customers");

            await ExecuteBatchAsync(table, customerEntityLst);
            return cust;
        }

        public async Task<CustomerDetails> UpdateCustomer(CustomerDetails cust)
        {
            var customerEntityLst = new List<CustomerEntity>();
            customerEntityLst.Add(new CustomerEntity(cust.Type, cust.Id, EmailIdIndexPrefix)
            {
                Id = cust.Id,
                Name = cust.Name,
                Age = cust.Age,
                Email = cust.Email,
                Type = cust.Type
            });

            customerEntityLst.Add(new CustomerEntity(cust.Type, cust.Id, UserIdIndexPrefix)
            {
                Id = cust.Id,
                Name = cust.Name,
                Age = cust.Age,
                Email = cust.Email,
                Type = cust.Type
            });


            var table = await CreateTable("Customers");

            await ExecuteBatchAsync(table, customerEntityLst);
            if (cust != null)
            {
                Console.WriteLine("\t{0}\t{1}\t{2}\t{3}\t{4}", cust.Id, cust.Name, cust.Age, cust.Email, cust.Type);
            }
            return cust;
        }

        public async Task DeleteCustomer(string id)
        {

            var table = await CreateTable("Customers");
            var result = await RetriveUsingRowAndPartitionKey(table, id.Substring(0, 1), id);

            await Delete(table, result);
        }



        private CustomerDetails MapViewModel(CustomerEntity cust)
        {
            return new CustomerDetails
            {
                Id = cust.RowKey,
                Name = cust.Name,
                Age = cust.Age,
                Email = cust.Email,
                Type = cust.Type
            };
        }
    }
}
