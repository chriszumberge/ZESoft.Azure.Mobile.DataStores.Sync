using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZESoft.Azure.Mobile.DataStores.Sync
{
    public abstract class BaseSyncStoreManager : ISyncStoreManager
    {
        MobileServiceClient MobileService { get; }
        MobileServiceSQLiteStore SQLiteStore { get; }

        protected abstract string AzureServiceUrl { get; }
        protected abstract string LocalDatabaseFile { get; }

        public BaseSyncStoreManager()
        {
            MobileService = new MobileServiceClient(AzureServiceUrl);
            SQLiteStore = new MobileServiceSQLiteStore(LocalDatabaseFile);
        }


        bool _isSyncEnabled = true;
        public bool IsSyncEnabled => _isSyncEnabled;

        public bool DisableSync()
        {
            UpdateStoreSyncAllowance(false);
            return true;
        }

        public bool EnableSync()
        {
            UpdateStoreSyncAllowance(true);
            return true;
        }

        void UpdateStoreSyncAllowance(bool syncEnabled)
        {
            _isSyncEnabled = syncEnabled;
            foreach (IAzureSyncStore store in _managedStores)
            {
                store.SyncEnabled = syncEnabled;
            }
        }

        List<IAzureSyncStore> _managedStores = new List<IAzureSyncStore>();

        public async Task<bool> SyncAllAsync()
        {
            var taskList = new List<Task<bool>>();
            foreach (IAzureSyncStore store in _managedStores)
            {
                taskList.Add(store.SynchronizeAsync());
            }

            var successes = await Task.WhenAll(taskList).ConfigureAwait(false);
            return successes.Any(x => !x);
        }

        public void ManageStore(IAzureSyncStore store)
        {
            store.SetAzureMobileClient(MobileService);
            store.SetSQLiteStore(SQLiteStore);
            _managedStores.Add(store);
        }

        public void ForgetStore(IAzureSyncStore store)
        {
            _managedStores.Remove(store);
            store.SetAzureMobileClient();
            store.SetSQLiteStore();
        }

        public IAzureSyncStore GetStore(string storeIdentifier) => _managedStores.FirstOrDefault(store => String.Equals(storeIdentifier, store.Identifier));

        public async Task<MobileServiceUser> LoginAsync()
        {
            return await MobileService.LoginAsync(MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory, JObject.FromObject(String.Empty));
        }

        public async Task LogoutAsync()
        {
            await MobileService.LogoutAsync();
        }
    }
}
