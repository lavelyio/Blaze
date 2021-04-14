using JASBlazor.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace JASBlazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var options = host.Services.GetService<IServiceScopeFactory>()
                .CreateScope()
                .ServiceProvider
                .GetRequiredService<DbContextOptions<JASDBContext>>();
            await options.EnsureDbCreatedAndSeedWithCountOfAsync(500);
            // back to your regularly scheduled program

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}