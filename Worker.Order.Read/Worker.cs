using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Worker.Order.Read.Service.Interfaces;

namespace Worker.Order.Read
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private readonly IFileService _fileService;

        private readonly IReadService _readService;

        public Worker(ILogger<Worker> logger, IFileService fileService, IReadService readService)
        {
            _logger = logger;

            _fileService = fileService;

            _readService = readService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var logRead = _fileService.CheckFile();

                    if (logRead != 0)
                    {
                        var order = _fileService.ReadFile(logRead);

                        if (order != null)
                        {
                            _readService.InsertRead(order, logRead);

                            _fileService.MoveFile(false);
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
