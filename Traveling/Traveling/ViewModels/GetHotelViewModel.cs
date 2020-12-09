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
    class GetHotelViewModel : BaseViewModel
    {
        List<Hotel> hotelList;
        public List<Hotel> HotelList { get => hotelList; set => SetProperty(ref hotelList, value); }

        public ICommand RefreshCommand { get; }

        public GetHotelViewModel()
        {
            RefreshCommand = new Command(async () => await ExecuteRefreshCommand());

        }

        async Task ExecuteRefreshCommand()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                HotelList = await CosmosHotelService.GetHotel();
            }
            finally
            {
                IsBusy = false;
            }

        }
    }
}
