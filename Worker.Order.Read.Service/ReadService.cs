using System;
using Worker.Order.Read.Repository.Interfaces;
using Worker.Order.Read.Service.Interfaces;

namespace Worker.Order.Read.Service
{
    public class ReadService : IReadService
    {
        private readonly IReadRepository _readRepository;

        private readonly ILogsRepository _logsRepository;

        public ReadService(IReadRepository readRepository, ILogsRepository logsRepository)
        {
            _readRepository = readRepository;

            _logsRepository = logsRepository;
        }

        public bool CheckRead(Entity.Order order, int logRead)
        {
            if (_readRepository.OrderNumberExists(order.OrderNumber))
            {
                _logsRepository.LogRead("oi", logRead);

                return false;
            }

            return true;
        }

        public void InsertRead(Entity.Order order, int logRead)
        {
            try
            {
                var readId = _readRepository.InsertRead(logRead, order);

                _readRepository.InsertReadItems(readId, order);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
