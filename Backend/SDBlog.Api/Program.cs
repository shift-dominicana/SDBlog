using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SDBlog.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .CaptureStartupErrors(true);

        public static T EntityRepository<T>() =>
            ((IServiceScopeFactory)CreateWebHostBuilder(new string[] { }).Build().Services.GetService(typeof(IServiceScopeFactory)))
                .CreateScope()
                .ServiceProvider
                .GetRequiredService<T>();
    }
}
