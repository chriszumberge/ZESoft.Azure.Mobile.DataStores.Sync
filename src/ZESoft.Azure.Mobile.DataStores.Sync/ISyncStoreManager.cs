using System.Threading.Tasks;

namespace ZESoft.Azure.Mobile.DataStores.Sync
{
    public interface ISyncStoreManager
    { 
        bool IsSyncEnabled { get; }

        Task<bool> SyncAllAsync();

        bool EnableSync();
        bool DisableSync();

        void ManageStore(IAzureSyncStore store);
        void ForgetStore(IAzureSyncStore store);
        IAzureSyncStore GetStore(string storeIdentifier);

        //Task DropEverythingAsync();
        //Task InitializeAsync();
    }
}
