using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Traveling
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainJobPage : ContentPage
    {
        public MainJobPage()
        {
            InitializeComponent();
        }

        async void toLogin(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Login());
        }

        async void toReserve(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BuyFlightPage());
        }

        async void OnButtonAirportClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AirportPage());
        }

        async void OnButtonTrainClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TrainPage());
        }

        async void OnButtonCarClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CarPage());
        }
        async void OnButtonHotelClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HotelPage());
        }
        async void OnButtonScenicClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ScenicPage());
        }
    }
}