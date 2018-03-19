using System;
using System.Collections.Generic;
using System.Text;
using ZESoft.Azure.Mobile.Models;

namespace ZESoft.Azure.Mobile.DataStores.Sync
{
    public interface ISyncManager
    {
        bool SyncEnabled { get; set; }

        bool SyncAll();

        bool EnableSync();
        bool DisableSync();

        void ManageStore<T>(IBaseAzureSyncStore<T> store) where T : class, IAzureDataObject, new();

        void ForgetStore<T>(IBaseAzureSyncStore<T> store) where T : class, IAzureDataObject, new();
    }
}
