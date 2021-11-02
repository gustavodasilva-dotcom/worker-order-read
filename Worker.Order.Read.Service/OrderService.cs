using System;
using Worker.Order.Read.Repository.Interfaces;
using Worker.Order.Read.Service.Interfaces;

namespace Worker.Order.Read.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public bool CheckPendingOrders()
        {
            try
            {
                return _orderRepository.CheckPendingOrders();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void GetNextPendingOrder()
        {
            try
            {
                var pendingOrderId = _orderRepository.GetNextPendingOrder();

                var pendingOrderInfo = _orderRepository.GetNextPendingOrderInfo(pendingOrderId);

                var pendingOrderItems = _orderRepository.GetNextPendingOrderItems(pendingOrderId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
