using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Worker.Order.Read.Service.Interfaces;

namespace Worker.Order.Read.SendToProduction
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private readonly IOrderService _orderService;

        public Worker(ILogger<Worker> logger, IOrderService orderService)
        {
            _logger = logger;

            _orderService = orderService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (_orderService.CheckPendingOrders())
                    {
                        _orderService.GetNextPendingOrder();
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError("An error occurred: {0}", e.Message);
                }
                finally
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    await Task.Delay(1000, stoppingToken);
                }
            }
        }
    }
}
