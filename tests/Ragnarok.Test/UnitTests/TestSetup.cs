using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ragnarok.AgentApi.Models;
using System.IO;

namespace Ragnarok.Test
{
    public class TestSetup
    {
        public ServiceProvider ServiceProvider { get; private set; }

        public TestSetup()
        {
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(
                     path: "appsettings.json",
                     optional: false,
                     reloadOnChange: true)
               .Build();

            services.AddOptions<RagnarokOptions>().Bind(configuration.GetSection(RagnarokOptions.Ragnarok));

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
