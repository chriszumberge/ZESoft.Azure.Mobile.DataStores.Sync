using System.Threading.Tasks;
using ZESoft.Azure.Mobile.Models;

namespace ZESoft.Azure.Mobile.DataStores.Sync
{
    public interface IBaseAzureSyncStore<T> : IBaseAzureStore<T> where T : class, IAzureDataObject, new()
    {
        bool SyncEnabled { get; set; }
        bool SyncOnlyOverWiFi { get; set; }

        Task PullLatestAsync();
        Task SynchronizeAsync();

        Task<bool> PurgeAsync(bool force = true);
        Task<bool> DropTableAsync();
    }
}
