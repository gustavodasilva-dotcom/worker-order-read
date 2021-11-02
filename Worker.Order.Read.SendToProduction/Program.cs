using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Worker.Order.Read.Repository;
using Worker.Order.Read.Repository.Interfaces;
using Worker.Order.Read.Service;
using Worker.Order.Read.Service.Interfaces;

namespace Worker.Order.Read.SendToProduction
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IOrderRepository, OrderRepository>();
                    services.AddSingleton<IOrderService, OrderService>();

                    services.AddHostedService<Worker>();
                });
    }
}
