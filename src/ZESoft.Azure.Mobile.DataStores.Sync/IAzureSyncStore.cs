﻿using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using System.Threading.Tasks;
using ZESoft.Azure.Mobile.Models;

namespace ZESoft.Azure.Mobile.DataStores.Sync
{
    //public interface IBaseAzureSyncStore<T> : IBaseAzureStore<T> where T : class, IAzureDataObject, new()
    //{
    //    bool SyncEnabled { get; set; }
    //    bool SyncOnlyOverWiFi { get; set; }

    //    Task PullLatestAsync();
    //    Task SynchronizeAsync();

    //    Task<bool> PurgeAsync(bool force = true);
    //    Task<bool> DropTableAsync();
    //}

    public interface IAzureSyncStore
    {
        string Identifier { get; }

        void SetAzureMobileClient(MobileServiceClient newClient = null);
        void SetSQLiteStore(MobileServiceSQLiteStore newStore = null);

        bool SyncEnabled { get; set; }
        bool SyncOnlyOverWiFi { get; set; }

        Task PullLatestAsync();
        Task<bool> SynchronizeAsync();

        Task<bool> PurgeAsync(bool force = true);
        Task<bool> DropTableAsync();
    }
}
