namespace Worker.Order.Read.Repository.Interfaces
{
    public interface IReadRepository
    {
        int InsertRead(int logRead, Entity.Order order);

        void InsertReadItems(int readId, Entity.Order order);
    }
}
