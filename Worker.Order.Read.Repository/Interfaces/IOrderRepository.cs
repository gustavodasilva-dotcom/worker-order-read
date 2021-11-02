using System.Collections.Generic;
using Worker.Order.Read.Models.ViewModels;

namespace Worker.Order.Read.Repository.Interfaces
{
    public interface IOrderRepository
    {
        bool CheckPendingOrders();

        int GetNextPendingOrder();

        OrderInfoViewModel GetNextPendingOrderInfo(int pendingOrderNumber);

        IEnumerable<ItemViewModel> GetNextPendingOrderItems(int pendingOrderNumber);

        bool OrderNumberExists(int orderNumber);
    }
}
