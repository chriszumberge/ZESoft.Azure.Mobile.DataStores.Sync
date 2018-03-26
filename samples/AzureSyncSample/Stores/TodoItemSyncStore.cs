using AzureSyncSample.Models;
using ZESoft.Azure.Mobile.DataStores.Sync;

namespace AzureSyncSample.Stores
{
    public class TodoItemSyncStore : BaseAzureSyncStore<ToDoItem>
    {
        public override string Identifier => "ToDoItems";
        protected override string AzureServiceUrl => "https://azuresyncsample.azurewebsites.net";
        protected override string LocalDatabaseFile => "ToDoItems.db";
    }
}
