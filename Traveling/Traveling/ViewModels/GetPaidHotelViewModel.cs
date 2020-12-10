using Plugin.SecureStorage;
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
    class GetPaidHotelViewModel : BaseViewModel
    {
        List<HotelTrans> hotellList;
        public List<HotelTrans> HotellList { get => hotellList; set => SetProperty(ref hotellList, value); }

        public ICommand RefreshCommand { get; }

        public GetPaidHotelViewModel()
        {
            RefreshCommand = new Command(async () => await ExecuteRefreshCommand());

        }

        async Task ExecuteRefreshCommand()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                string uid = "";
                uid = CrossSecureStorage.Current.GetValue("id");
                HotellList = await CosmosHotelTransService.searchPHotel(uid);
            }
            finally
            {
                IsBusy = false;
            }

        }
    }
}
