using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Ragnarok.HostedService.Extensions;

namespace WeatherAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseNgrok(options => options.DownloadNgrok = true); //use the extension method to configure the hostedservice
                });
    }
}
