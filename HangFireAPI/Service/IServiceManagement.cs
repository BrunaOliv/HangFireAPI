namespace HangFireAPI.Service
{
    public interface IServiceManagement
    {
        void SendEmail();
        void UpdateDatabase();
        void GenerateMerchandise();
        void SyncRecords();
    }
}
