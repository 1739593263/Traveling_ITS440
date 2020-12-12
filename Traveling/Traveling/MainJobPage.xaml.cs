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


        async void toReserve(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BuyFlightPage());
        }

        async void toHotel(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HotelPage());
        }

        async void toTrain(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TrainPage());
        }

        async void toCar(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CarPage());
        }
    }
}