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
                        var order = _orderService.GetNextPendingOrder();

                        if (order != null)
                        {
                            _logger.LogInformation("Processing order no {0}.", order.OrderNumber);

                            if (!_orderService.OrderNumberExists(order))
                            {
                                _orderService.ProcessOrder(order);
                            }
                            else
                            {
                                _logger.LogInformation("There is already a processed order {0}.", order.OrderNumber);

                                _orderService.DeactivatePreOrder(order.PreOrderID);
                            }
                        }
                        else
                        {
                            _logger.LogInformation("There isn't pending orders.");
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError("An error occurred: {0}", e.Message);
                }
                finally
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    await Task.Delay(10000, stoppingToken);
                }
            }
        }
    }
}
