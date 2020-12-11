using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traveling.Models;
using Traveling.Services;
using Traveling.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Traveling.adminPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LocationTable : ContentPage
    {
        GetPlaceListViewModel placesViewModel;
        string ID;


        string pname;
        Places p = new Places();

        List<Places> placeList;
        public LocationTable()
        {
            InitializeComponent();
            placesViewModel = new GetPlaceListViewModel();
            PlacesList.BindingContext = placesViewModel;
        }

        async Task ExecuteSearch()
        {
            placeList = await CosmosPlacesService.SearchPlaces(pname);
        }

        async Task ExecuteInsert()
        {
            await CosmosPlacesService.InsertPlace(p);
        }

        async Task ExecuteDelete()
        {
            p = await CosmosPlacesService.GetPlaceById(ID);

            await CosmosPlacesService.DeletePlace(p);
        }
        async void searchSchedule(object sender, EventArgs e) {
            pname = place.Text;
            if (pname != null) { 
                await ExecuteSearch();
                PlacesList.BindingContext = "";
                PlacesList.ItemsSource = placeList;
            }
            else await DisplayAlert("Error", "Please enter something", "OK");
        }

        async void insert(object sender, EventArgs e)
        {
            pname = place.Text;
            if (pname != null) {
                p.isflight = 1;
                p.place = pname;
                await ExecuteInsert();

                await Navigation.PushAsync(new loading());
                await Navigation.PopAsync();
            }
            else await DisplayAlert("Error", "Please enter something", "OK");
        }

        async void delete(object sender, EventArgs e)
        {
            var item = (MenuItem)sender;
            ID = item.CommandParameter+"";
            await ExecuteDelete();
            await DisplayAlert("success","You deleted a place","OK");

            await Navigation.PushAsync(new loading());
            await Navigation.PopAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            placesViewModel.RefreshCommand.Execute(null);
        }
    }
}