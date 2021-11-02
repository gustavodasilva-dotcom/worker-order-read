namespace Worker.Order.Read.Repository.Interfaces
{
    public interface IReadRepository
    {
        bool OrderNumberExists(int orderNumber);

        int InsertRead(int logRead, Entity.Order order);

        void InsertReadItems(int readId, Entity.Order order);
    }
}
