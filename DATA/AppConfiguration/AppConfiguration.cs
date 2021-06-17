using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace DATA.AppConfiguration
{
    class AppConfiguration
    {
        //CONSTRUCTOR
        public AppConfiguration()
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();

            string path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

            configurationBuilder.AddJsonFile(path, false);

            IConfigurationRoot root = configurationBuilder.Build();

            IConfigurationSection appSettings = root.GetSection("ConnectionStrings:ArtExchangeAPI");

            SqlConnectionString = appSettings.Value;
        }

        public string SqlConnectionString { get; set; }
    }
}
