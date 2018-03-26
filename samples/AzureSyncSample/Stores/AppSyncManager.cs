using ZESoft.Azure.Mobile.DataStores.Sync;

namespace AzureSyncSample.Stores
{
    public class AppSyncManager : BaseSyncStoreManager
    {
        protected override string AzureServiceUrl => "https://azuresyncsample.azurewebsites.net";
        protected override string LocalDatabaseFile => "ToDoItems.db";
    }
}
