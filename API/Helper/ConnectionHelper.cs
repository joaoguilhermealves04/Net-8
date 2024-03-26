using Microsoft.Extensions.Configuration;
using System.IO;

namespace API.Helper
{
    public class ConnectionHelper
    {
        public static string Connectiondatabase()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            return configuration.GetConnectionString("DefaultConnection");
        }
    }
}
