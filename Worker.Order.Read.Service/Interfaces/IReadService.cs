namespace Worker.Order.Read.Service.Interfaces
{
    public interface IReadService
    {
        bool CheckRead(Entity.Order order, int logRead);

        void InsertRead(Entity.Order order, int logRead);
    }
}
