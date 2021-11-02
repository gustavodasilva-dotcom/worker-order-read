using System;
using Worker.Order.Read.Repository.Interfaces;
using Worker.Order.Read.Service.Interfaces;

namespace Worker.Order.Read.Service
{
    public class ReadService : IReadService
    {
        private readonly IReadRepository _readRepository;

        public ReadService(IReadRepository readRepository)
        {
            _readRepository = readRepository;
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
