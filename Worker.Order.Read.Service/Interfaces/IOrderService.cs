using System.Data;

namespace Worker.Order.Read.Service.Interfaces
{
    public interface IOrderService
    {
        bool CheckPendingOrders();

        void GetNextPendingOrder();
    }
}
