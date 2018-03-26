using AzureSyncSample.Models;
using AzureSyncSample.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace AzureSyncSample
{
	public partial class App : Application
	{
        public static AppSyncManager SyncManager = new AppSyncManager();
        public static TodoItemSyncStore ToDoItemStore = new TodoItemSyncStore();

		public App ()
		{
			InitializeComponent();

			MainPage = new AzureSyncSample.MainPage();

            SyncManager.ManageStore(ToDoItemStore);
            SyncManager.EnableSync();

            ToDoItemStore.InsertAsync(new ToDoItem { Name = "Test Sync Store", IsCompleted = false });
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
