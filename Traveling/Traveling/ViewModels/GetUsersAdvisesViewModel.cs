using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Traveling.Models;
using Traveling.Services;
using Xamarin.Forms;

using Plugin.SecureStorage;
namespace Traveling.ViewModels
{
    class GetUsersAdvisesViewModel : BaseViewModel
    {
        List<Advise> adviselList;
        public List<Advise> AdviselList { get => adviselList; set => SetProperty(ref adviselList, value); }

        public ICommand RefreshCommand { get; }

        public GetUsersAdvisesViewModel()
        {
            RefreshCommand = new Command(async () => await ExecuteRefreshCommand());

        }

        async Task ExecuteRefreshCommand()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                AdviselList = await CosmosAdviceService.SearchAdvisesById(CrossSecureStorage.Current.GetValue("id"));
            }
            finally
            {
                IsBusy = false;
            }

        }
    }
}
