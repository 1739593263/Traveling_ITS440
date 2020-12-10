using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traveling.Models;
using Traveling.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Traveling.Services
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class userHotel : ContentPage
    {
        GetUnPaidHotelViewModel unPaidHotelViewModel;
        GetPaidHotelViewModel paidHotelViewModel;

        List<HotelTrans> HotelTrainslist;

        bool paid = false;
        int tap_num;

        string hid;
        Hotel hotel;
        public userHotel()
        {
            InitializeComponent();
            if (CrossSecureStorage.Current.HasKey("tid"))
            {
                CrossSecureStorage.Current.DeleteKey("tid");
            }

            unPaidHotelViewModel = new GetUnPaidHotelViewModel();
            paidHotelViewModel = new GetPaidHotelViewModel();
            HotelList.BindingContext = unPaidHotelViewModel;

            HotelList.IsEnabled = true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            unPaidHotelViewModel.RefreshCommand.Execute(null);
            paidHotelViewModel.RefreshCommand.Execute(null);
        }

        async void tap(object sender, ItemTappedEventArgs e) {
            tap_num = e.ItemIndex;
            string tid = unPaidHotelViewModel.HotellList[tap_num].Id;
            hid = unPaidHotelViewModel.HotellList[tap_num].hotelId;
            CrossSecureStorage.Current.SetValue("tid", tid);

            await ExecuteSearchCommand();
            await Navigation.PushAsync(new bookHotel(hid, hotel.price1, hotel.price2, hotel.price3));
        }

        async void p(object sender, EventArgs e) 
        {
            HotelList.BindingContext = "";
            HotelList.ItemsSource = paidHotelViewModel.HotellList;
            HotelList.IsEnabled = false;
        }

        async void up(object sender, EventArgs e)
        {
            HotelList.BindingContext = "";
            HotelList.ItemsSource = unPaidHotelViewModel.HotellList;
            HotelList.IsEnabled = true;
        }

        async Task ExecuteSearchCommand()
        {
            hotel = await CosmosHotelService.GetHotelById(hid);
        }
    }
}