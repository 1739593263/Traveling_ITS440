using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Traveling.Models;
using Traveling.Services;
using Xamarin.Forms;

namespace Traveling.ViewModels
{
    class GetItemsViewModel:BaseViewModel
    {
        List<Items> itemsList;
        public List<Items> ItemsList { get => itemsList; set => SetProperty(ref itemsList, value); }
        
        public ICommand RefreshCommand { get; }

        public GetItemsViewModel() {
            RefreshCommand = new Command(async () => await ExecuteRefreshCommand());

        }

        async Task ExecuteRefreshCommand()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                ItemsList = await CosmosDBService.GetItems();
            }
            finally
            {
                IsBusy = false;
            }

        }
    }
}
