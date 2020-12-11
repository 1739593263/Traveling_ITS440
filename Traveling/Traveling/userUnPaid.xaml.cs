using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traveling.Models;
using Traveling.Services;
using Traveling.ViewModels.threeTransaction;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Traveling
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class userUnPaid : ContentPage
    {

        GetUpTransactionViewModel upTransactionViewModel;
        int tap_num;
        string vehicleId;

        Train train;
        Schedule flight;

        public userUnPaid()
        {
            InitializeComponent();
            if (CrossSecureStorage.Current.HasKey("transid"))
            {
                CrossSecureStorage.Current.DeleteKey("transid");
            }


            upTransactionViewModel = new GetUpTransactionViewModel();
            UnPaidList.BindingContext = upTransactionViewModel;
        }

        async void tap(object sender, ItemTappedEventArgs e)
        {
            tap_num = e.ItemIndex;
            string tid = upTransactionViewModel.TransList[tap_num].Id;
            string sort = upTransactionViewModel.TransList[tap_num].vehicleSort;
            vehicleId = upTransactionViewModel.TransList[tap_num].vehicleId;


            CrossSecureStorage.Current.SetValue("transid", tid);
            if (sort == "train")
            {
                await ExecuteGetTranins();
                await Navigation.PushAsync(new bookSchedule(vehicleId, train.price, train.price2, train.price3, "Train"));
            }
            else if (sort == "flight")
            {
                await ExecuteGetFlight();
                await Navigation.PushAsync(new bookSchedule(vehicleId, flight.price, flight.price2, flight.price3, "Flight"));
            }
            else return;
        }

        async Task ExecuteGetTranins()
        {
            train = await CosmosTrainTableService.GetTrainById(vehicleId);
        }

        async Task ExecuteGetFlight()
        {
            flight = await CosmosScheduleDBService.GetFlightById(vehicleId);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            upTransactionViewModel.RefreshCommand.Execute(null);
        }
    }
}