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
    public partial class userAllorder : ContentPage
    {
        GetTransactionViewModel transactionViewModel;
        int tap_num;
        string ID;

        Transaction transaction;

        public userAllorder()
        {
            InitializeComponent();

            transactionViewModel = new GetTransactionViewModel();
            transList.BindingContext = transactionViewModel;
            
        }
        async void tap(object sender, ItemTappedEventArgs e)
        {
            tap_num = e.ItemIndex;
        }


        /*--------------------range of operation of drop-----------------------------------------------------------*/
        async Task ExecuteSearchById()
        {
            transaction = await CosmosTransService.searchTransById(ID);
        }

        async Task ExecuteDropOrder()
        {
            await CosmosTransService.DeleteTransaction(transaction);
        }

        async Task ExecuteSearchTrain()
        {
            Train train = await CosmosTrainTableService.SearchTrainById(transaction.vehicleId);
            train.business++;
            train.economy++;
            train.first++;
            await CosmosTrainTableService.UpdateTrain(train);
        }
        async Task ExecuteSearchFlight()
        {
            Schedule flight = await CosmosScheduleDBService.SearchScheduleById(transaction.vehicleId);
            flight.business++;
            flight.economy++;
            flight.first++;
            await CosmosScheduleDBService.UpdateSchedule(flight);
        }

        async void drop(object sender, EventArgs e) {
            var s = (MenuItem)sender;
            ID = s.CommandParameter+"";

            await ExecuteSearchById();
            await ExecuteDropOrder();

            // return the quantity
            if (transaction.vehicleSort == "flight") await ExecuteSearchFlight();
            if (transaction.vehicleSort == "train") await ExecuteSearchTrain();

            await Navigation.PushAsync(new loading());
            await Navigation.PopAsync();
        }
        /*-------------------------------------------------------------------------------------------------------------------*/

        protected override void OnAppearing()
        {
            base.OnAppearing();

            transactionViewModel.RefreshCommand.Execute(null);
        }
    }
}