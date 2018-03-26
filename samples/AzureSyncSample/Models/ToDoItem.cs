using ZESoft.Azure.Mobile.Models;

namespace AzureSyncSample.Models
{
    public class ToDoItem : BaseAzureDataObject
    {
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
    }
}
