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
    class GetPlaceListViewModel : BaseViewModel
    {
        List<Places> placesList;
        public List<Places> PlacesList { get => placesList; set => SetProperty(ref placesList, value); }

        public ICommand RefreshCommand { get; }

        public GetPlaceListViewModel()
        {
            RefreshCommand = new Command(async () => await ExecuteRefreshCommand());
        }

        async Task ExecuteRefreshCommand()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                PlacesList = await CosmosPlacesService.GetPlacesList();
            }
            finally
            {
                IsBusy = false;
            }

        }
    }
}
