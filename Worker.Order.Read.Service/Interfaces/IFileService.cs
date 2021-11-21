namespace Worker.Order.Read.Service.Interfaces
{
    public interface IFileService
    {
        int CheckFile();

        Entity.Order ReadFile(int logRead);

        void MoveFile(bool isError);
    }
}
