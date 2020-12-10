using Plugin.SecureStorage;
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

namespace Traveling
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HotelPage : ContentPage
    {
        GetHotelViewModel hotelViewModel;
        PlacesViewModel placesViewModel;

        CosmosHotelService hotelService;
        List<Hotel> hotellist;
        Hotel hotel;

        HotelTrans hotelTrans;
        public HotelPage()
        {
            InitializeComponent();
            hotelViewModel = new GetHotelViewModel();
            hotelList.BindingContext = hotelViewModel;

            placesViewModel = new PlacesViewModel();
            city.BindingContext = placesViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            hotelViewModel.RefreshCommand.Execute(null);
            placesViewModel.RefreshCommand.Execute(null);
        }

        async Task ExecuteSearchCommand()
        {
            string c = (string)city.SelectedItem;
            if (c != null)
            {
                hotellist = await CosmosHotelService.SearchHotel(c);
            }
            else return;
        }

        async void searchHotel(object sender, EventArgs e) {
            await ExecuteSearchCommand();
            hotelList.BindingContext = "";
            hotelList.ItemsSource = hotellist;
        }
         
        async void book(object sender, EventArgs e)
        {
            var hotell = ((MenuItem)sender);
            string ID = hotell.CommandParameter + "";
            await ExecuteSearchCommand(ID);

            hotelTrans = new HotelTrans();
            hotelTrans.hotelId = ID;
            hotelTrans.Name = hotel.Name;
            hotelTrans.District = hotel.District;
            hotelTrans.description = hotel.description;
            hotelTrans.picture = hotel.picture;
            hotelTrans.city = hotel.city;
            hotelTrans.userId = CrossSecureStorage.Current.GetValue("id");
            hotelTrans.userName = CrossSecureStorage.Current.GetValue("firstname")+" "+ CrossSecureStorage.Current.GetValue("lastname");
            await ExecuteInsrtTransCommand();

            CrossSecureStorage.Current.SetValue("hotelName", hotel.Name);
            await Navigation.PushAsync(new bookHotel(ID, hotel.price1, hotel.price2, hotel.price3));
        }

        async Task ExecuteInsrtTransCommand()
        {
            await CosmosTransactionService.InsertHotelTrans(hotelTrans);
        }

        async Task ExecuteSearchCommand(string _id)
        {
            hotel = await CosmosHotelService.GetHotelById(_id);
        }
    }
}