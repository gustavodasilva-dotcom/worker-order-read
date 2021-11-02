namespace Worker.Order.Read.Repository.Interfaces
{
    public interface ILogsRepository
    {
        int LogRead(string fileName);

        void LogRead(string message, int logRead);
    }
}
