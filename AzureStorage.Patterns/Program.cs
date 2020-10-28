using Microsoft.Extensions.Configuration;
using System;


namespace AzureStorage.Patterns
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json").AddEnvironmentVariables().Build();

            CloudConfigurationManager.Config = config;




            Console.WriteLine("Hello World!");
        }
    }
}
