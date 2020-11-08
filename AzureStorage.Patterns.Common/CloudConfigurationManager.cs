using Microsoft.Extensions.Configuration;

namespace AzureStorage.Patterns.Common
{
    public static class CloudConfigurationManager 
    {
        public static IConfiguration Config { get; set; }
    }
}
