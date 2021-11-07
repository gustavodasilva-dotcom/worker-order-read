namespace Worker.Order.Read.Service.Interfaces
{
    public interface IOrderService
    {
        bool CheckPendingOrders();

        Worker.Order.Read.Entity.Order GetNextPendingOrder();

        bool OrderNumberExists(Worker.Order.Read.Entity.Order order);

        void ProcessOrder(Worker.Order.Read.Entity.Order order);

        void DeactivatePreOrder(int preOrderID);
    }
}
