using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AzureSyncSample
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

            BindingContext = this;
		}

		ICommand _syncCommand;
		public ICommand SyncCommand => _syncCommand ?? (_syncCommand = new Command(async () => await App.SyncManager.SyncAllAsync()));
	}
}
