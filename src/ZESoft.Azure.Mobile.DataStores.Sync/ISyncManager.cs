using System;
using System.Collections.Generic;
using System.Text;

namespace ZESoft.Azure.Mobile.DataStores.Sync
{
    public interface ISyncManager
    {
        bool SyncEnabled { get; set; }

        bool SyncAll();

        bool EnableSync();
        bool DisableSync();
    }
}
