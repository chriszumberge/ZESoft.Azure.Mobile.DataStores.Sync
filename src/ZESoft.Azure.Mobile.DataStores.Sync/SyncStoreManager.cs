using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZESoft.Azure.Mobile.DataStores.Sync
{
    public class SyncStoreManager : ISyncStoreManager
    {

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
            _managedStores.Add(store);
        }

        public void ForgetStore(IAzureSyncStore store)
        {
            _managedStores.Remove(store);
        }

        public IAzureSyncStore GetStore(string storeIdentifier) => _managedStores.FirstOrDefault(store => String.Equals(storeIdentifier, store.Identifier));
    }
}
