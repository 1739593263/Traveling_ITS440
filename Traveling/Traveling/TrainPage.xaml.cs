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
    public partial class TrainPage : ContentPage
    {
        GetTrainViewModel trainViewModel;
        PlacesViewModel placesViewModel;
        List<Train> trainlist;

        Train train;
        Transaction transaction;
        public TrainPage()
        {
            InitializeComponent();
            trainViewModel = new GetTrainViewModel();
            TrainList.BindingContext = trainViewModel;

            placesViewModel = new PlacesViewModel();
            SPlace.BindingContext = placesViewModel;
            DPlace.BindingContext = placesViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            trainViewModel.RefreshCommand.Execute(null);
            placesViewModel.RefreshCommand.Execute(null);
        }

        async Task ExecuteSearchCommand()
        {
            string s = SDate.Date.ToString();
            string[] dates = s.Split(' ');
            string sou = (string)SPlace.SelectedItem;
            string des = (string)DPlace.SelectedItem;
            trainlist = await CosmosTrainTableService.SearchTrain(sou, des, dates[0]);
        }

        async void searchSchedule(object sender, EventArgs e)
        {
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
            else
            {
                await ExecuteSearchCommand();
                TrainList.BindingContext = "";
                TrainList.ItemsSource = trainlist;
            }
        }

        async void book(object sender, EventArgs e)
        {
            var Trainl = ((MenuItem)sender);
            string ID = Trainl.CommandParameter + "";
            await ExecuteSearchCommand(ID);

            transaction = new Transaction();
            transaction.source = train.source;
            transaction.sourceTime = train.sourceTime;
            transaction.destination = train.destination;
            transaction.destinationTime = train.destinationTime;
            transaction.company = train.company;
            transaction.userId = CrossSecureStorage.Current.GetValue("id");
            transaction.vehicleId = train.Id;
            transaction.vehicleNum = train.line;
            transaction.vehicleSort = "train";
            transaction.date = train.date;
            transaction.days = train.days;
            await ExecuteInsrtTransCommand();

            /*CrossSecureStorage.Current.SetValue("hotelName", hotel.Name);*/
            await Navigation.PushAsync(new bookSchedule(ID, train.price, train.price2, train.price3, "Train"));
        }

        async Task ExecuteInsrtTransCommand()
        {
            await CosmosTransService.InsertTransaction(transaction);
        }

        async Task ExecuteSearchCommand(string _id)
        {
            train = await CosmosTrainTableService.GetTrainById(_id);
        }
    }
}