using System.Collections.Generic;
using Xamarin.Forms;
using MySqlConnector;
using QC = Microsoft.Data.SqlClient;
using DT = System.Data;
using System;
using Traveling.ViewModels;
using Traveling.Services;
using Plugin.SecureStorage;

namespace Traveling
{

    public partial class MainPage : ContentPage
    {
		GetItemsViewModel ViewModel;
        public MainPage()
        {
            InitializeComponent();
            ViewModel = new GetItemsViewModel();
			BindingContext = ViewModel;

            session.Text = CrossSecureStorage.Current.GetValue("firstname") + " " + CrossSecureStorage.Current.GetValue("lastname");
        }

        async void toLogin(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Login());
        }

        async void toReserve(object sender, EventArgs e) 
        {
            await Navigation.PushAsync(new BuyFlightPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.RefreshCommand.Execute(null);
        }

    }
}
