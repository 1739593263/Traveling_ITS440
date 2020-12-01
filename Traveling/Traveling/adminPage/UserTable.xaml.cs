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

namespace Traveling.adminPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserTable : ContentPage
    {
        GetAllUsersViewModel allUsersViewModel;
        public string operation = "";
        Users user;
        List<Users> usersList;
        public UserTable()
        {
            InitializeComponent();
            allUsersViewModel = new GetAllUsersViewModel();
            allUsers.BindingContext = allUsersViewModel;
            /*pro.ItemsSource = ProList;*/
        }

        int tappedItem;
        async void Row_tapped(object sender, ItemTappedEventArgs e) {
            update.IsEnabled = true;
            delete.IsEnabled = true;
            tappedItem = e.ItemIndex;
            user = allUsersViewModel.UsersList[tappedItem];
        }

        /*async void toSearch(object sender, EventArgs e)
        {
            *//*var text = searchByName.Text;
            string[] name = text.Split(' ');
            DisplayAlert("", name[0], "OK");*//*
            await ExecuteSearchCommand();
            allUsers.BindingContext = "";
            allUsers.ItemsSource = usersList;
        }*/

        async void updateC(object sender, EventArgs e) {
            operation = "update";
            await ExecuteClickedCommand();
        }

        async void deleteC(object sender, EventArgs e)
        {
            operation = "delete";
            await ExecuteClickedCommand();

            /*2. */
            /*allUsers.BindingContext = "";
            allUsers.ItemsSource = usersList;*/
        }

        /*async Task ExecuteSearchCommand()
        {
            var text = searchByName.Text;
            string[] name = text.Split(' ');
            if (text != "")
            {
                usersList = await CosmosUsersDBService.GetSearchedUsers(name[0], name[1]);
            }
            else
            {
                usersList = await CosmosUsersDBService.GetAllUsers();
            }
        }*/

        async Task ExecuteClickedCommand()
        {

            if(operation == "update")
            {
                await CosmosUsersDBService.UpdateUser(user);
                await DisplayAlert("SUCCESS", "updated it!", "OK");
            }
            else if(operation == "delete")
            {
                await CosmosUsersDBService.DeleteUser(user);
                await Navigation.PushAsync(new loading());
                await Navigation.PopAsync();

                /*1. */
                /*usersList = await CosmosUsersDBService.GetAllUsers();*/
                await DisplayAlert("SUCCESS", "deleted it!", "OK");
            }
            else 
            {
                await DisplayAlert("","invalid operation","OK");
            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            allUsersViewModel.RefreshCommand.Execute(null);
        }
    }
}