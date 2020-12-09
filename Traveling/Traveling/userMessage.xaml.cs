using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traveling.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Plugin.SecureStorage;
using Traveling.Models;
using Traveling.ViewModels;

namespace Traveling
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class userMessage : ContentPage
    {
        string user_id;
        GetUsersAdvisesViewModel usersAdvisesViewModel;

        string ID;

        public userMessage()
        {
            InitializeComponent();
            usersAdvisesViewModel = new GetUsersAdvisesViewModel();

            advises.BindingContext = usersAdvisesViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            usersAdvisesViewModel.RefreshCommand.Execute(null);
        }

        async void solve(object sender, EventArgs e)
        {
            var s = ((MenuItem)sender);
            ID = s.CommandParameter + "";

            await ExecuteUpdate();
        }

        async Task ExecuteUpdate()
        {
            await CosmosAdviceService.UpdateAdvise(ID);
            await DisplayAlert("SUCCESS", "Thanks for your reply", "OK");
        }
    }
}