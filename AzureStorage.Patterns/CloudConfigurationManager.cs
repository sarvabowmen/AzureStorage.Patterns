using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureStorage.Patterns.One
{
    static class CloudConfigurationManager 
    {
        public static IConfiguration Config { get; set; }
    }
}
