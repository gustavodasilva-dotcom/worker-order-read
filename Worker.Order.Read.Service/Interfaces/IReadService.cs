namespace Worker.Order.Read.Service.Interfaces
{
    public interface IReadService
    {
        void InsertRead(Entity.Order order, int logRead);
    }
}
