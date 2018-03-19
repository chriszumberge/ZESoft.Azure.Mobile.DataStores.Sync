using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZESoft.Azure.Mobile.Models;

namespace ZESoft.Azure.Mobile.DataStores.Sync
{
    public abstract class BaseAzureSyncStore<T> : IBaseAzureSyncStore<T> where T : class, IAzureDataObject, new()
    {
        public virtual string Identifier => "Items";

        public bool SyncEnabled { get; set; }
        public bool SyncOnlyOverWiFi { get; set; }

        protected abstract string AzureServiceUrl { get; }
        protected abstract string LocalDatabaseFile { get; }

        MobileServiceClient _azureClient;

        IMobileServiceSyncTable<T> _table;
        protected IMobileServiceSyncTable<T> Table => _table ?? (_table = _azureClient.GetSyncTable<T>());

        IMobileServiceTable<T> _endpointTable;
        protected IMobileServiceTable<T> EndpointTable => _endpointTable ?? (_endpointTable = _azureClient.GetTable<T>());

        public BaseAzureSyncStore()
        {
            try
            {
                _azureClient = new MobileServiceClient(AzureServiceUrl);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
        }

        protected async Task InitializeAsync()
        {
            try
            {
                if (_table != null)
                    return;

                var store = new MobileServiceSQLiteStore(LocalDatabaseFile);
                store.DefineTable<T>();

                await _azureClient.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());
                _table = _azureClient.GetSyncTable<T>();

                await _table.PurgeAsync(true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
        }

        public async Task<IEnumerable<T>> GetItemsAsync()
        {
            try
            {
                await InitializeAsync();

                await SynchronizeAsync();

                return await _table.ToEnumerableAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
        }

        public async Task<T> GetItemAsync(string id)
        {
            try
            {
                await InitializeAsync();

                await SynchronizeAsync();

                var items = await _table.Where(s => s.Id == id).ToListAsync();

                if (items == null || items.Count == 0)
                    return null;

                return items.First();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
        }

        public async Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>> queryPredicate)
        {
            try
            {
                await InitializeAsync();

                await SynchronizeAsync();

                return await _table.Where(queryPredicate).ToListAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
        }

        public async Task<T> InsertAsync(T item)
        {
            try
            {
                await InitializeAsync();

                await _table.InsertAsync(item);

                await SynchronizeAsync();

                return item;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
        }

        public async Task<bool> UpdateAsync(T item)
        {
            try
            {
                await InitializeAsync();

                await _table.UpdateAsync(item);

                await SynchronizeAsync();

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                // Get will do initialize and sync
                var item = await GetItemAsync(id);

                item.Deleted = true;

                await UpdateAsync(item);

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
        }

        public async Task<bool> PurgeAsync(bool force = true)
        {
            try
            {
                await _table?.PurgeAsync(force);

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
        }

        public async Task<bool> DropTableAsync()
        {
            throw new NotImplementedException();
        }

        public async Task PullLatestAsync()
        {
            try
            {
                await _table.PullAsync($"all{typeof(T).Name}", _table.CreateQuery());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw;
            }
        }

        public async Task SynchronizeAsync()
        {
            // If the suer isn't connected to any networks at all, return
            if (!CrossConnectivity.Current.IsConnected)
                return;

            // If syncing is turned off for the entire table, return
            if (!SyncEnabled)
                return;

            // If the user only wants to sync over wifi and the device isn't currently connected, return
            if (this.SyncOnlyOverWiFi && !CrossConnectivity.Current.ConnectionTypes.Contains(Plugin.Connectivity.Abstractions.ConnectionType.WiFi))
                return;

            await PushContext();

            await PullLatestAsync();
        }

        async Task PushContext()
        {
            try
            {
                await _azureClient.SyncContext.PushAsync();
            }
            catch (MobileServicePushFailedException PushFailEx)
            {
                if (PushFailEx.PushResult.Status == MobileServicePushStatus.CancelledByAuthenticationError)
                {
                    // login, resync, return
                }

                if (PushFailEx.PushResult != null)
                    foreach (var result in PushFailEx.PushResult.Errors)
                        await ResolveErrorAsync(result);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw;
            }
        }

        /// <summary>
        /// This method is used to resolve any conflicts that occur. This can happen 
        /// if two clients update the same record and then try to push their changes.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>0
        private async Task ResolveErrorAsync(MobileServiceTableOperationError result)
        {
            // Ignore if we can't see both sides.
            if ((result.Result == null) || (result.Item == null))
                return;

            var serverItem = result.Result.ToObject<T>();
            var localItem = result.Item.ToObject<T>();

            // always take the client
            localItem.Version = serverItem.Version;

            await result.UpdateOperationAsync(JObject.FromObject(localItem));
        }
    }
}
