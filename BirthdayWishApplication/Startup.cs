using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(BirthdayWishApplication.Startup))]
namespace BirthdayWishApplication
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {

            var configuration = BuildConfiguration(builder.GetContext().ApplicationRootPath);

            configuration.InititalizeConfigProperties();
            
        }
        private IConfiguration BuildConfiguration(string applicationRootPath)
        {
            var config =
                new ConfigurationBuilder()
                    .SetBasePath(applicationRootPath)
                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile("settings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

            return config;
        }
    }
}
