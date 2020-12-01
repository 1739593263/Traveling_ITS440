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
    class PlacesViewModel:BaseViewModel
    {
        String[] placesList;
        public String[] PlacesList { get => placesList; set => SetProperty(ref placesList, value); }

        public ICommand RefreshCommand { get; }

        public PlacesViewModel()
        {
            RefreshCommand = new Command(async () => await ExecuteRefreshCommand());
        }

        async Task ExecuteRefreshCommand()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                PlacesList = await CosmosPlacesService.GetPlaces();
            }
            finally
            {
                IsBusy = false;
            }

        }
    }
}
