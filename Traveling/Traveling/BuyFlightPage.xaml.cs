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
    public partial class BuyFlightPage : ContentPage
    {
        FlightScheduleViewModel flightViewModel;
        PlacesViewModel placesViewModel;
        List<Schedule> flightLists;

        Schedule flight;
        Transaction transaction;

        public BuyFlightPage()
        {
            InitializeComponent();
            flightViewModel = new FlightScheduleViewModel();
            FlightList.BindingContext = flightViewModel;

            placesViewModel = new PlacesViewModel();
            SPlace.BindingContext = placesViewModel;
            DPlace.BindingContext = placesViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            flightViewModel.RefreshCommand.Execute(null);
            placesViewModel.RefreshCommand.Execute(null);
        }

        async Task ExecuteSearchCommand()
        {
            string s = SDate.Date.ToString();
            string[] dates = s.Split(' ');
            string sou = (string)SPlace.SelectedItem;
            string des = (string)DPlace.SelectedItem;
            flightLists = await CosmosScheduleDBService.SearchSchedule(sou, des, dates[0]);
        }

        async void searchSchedule(object sender, EventArgs e) {
            string s = SDate.Date.ToString();
            string[] dates = s.Split(' ');
            string sou = (string)SPlace.SelectedItem;
            string des = (string)DPlace.SelectedItem;
            if (sou == des)
            {
                await DisplayAlert("Error", "the same source and destination", "retry");
            }
            else if ((sou == null) || (des == null))
            {
                await DisplayAlert("Error", "please input source and destination", "retry");
            }
            else {
                await ExecuteSearchCommand();
                FlightList.BindingContext = "";
                FlightList.ItemsSource = flightLists;
            }
        }



        async void book(object sender, EventArgs e)
        {
            var Flightl = ((MenuItem)sender);
            string ID = Flightl.CommandParameter + "";
            await ExecuteSearchCommand(ID);

            transaction = new Transaction();
            transaction.source = flight.source;
            transaction.sourceTime = flight.sourceTime;
            transaction.destination = flight.destination;
            transaction.destinationTime = flight.destinationTime;
            transaction.company = flight.company;
            transaction.userId = CrossSecureStorage.Current.GetValue("id");
            transaction.vehicleId = flight.Id;
            transaction.vehicleNum = flight.airplane;
            transaction.vehicleSort = "flight";
            transaction.date = flight.date;
            transaction.days = flight.days;
            await ExecuteInsrtTransCommand();

            /*CrossSecureStorage.Current.SetValue("hotelName", hotel.Name);*/
            await Navigation.PushAsync(new bookSchedule(ID, flight.price, flight.price2, flight.price3));
        }

        async Task ExecuteSearchCommand(string _id)
        {
            flight = await CosmosScheduleDBService.GetFlightById(_id);
        }

        async Task ExecuteInsrtTransCommand()
        {
            await CosmosTransService.InsertTransaction(transaction);
        }
    }
}