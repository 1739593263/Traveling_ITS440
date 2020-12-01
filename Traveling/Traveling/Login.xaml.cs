using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traveling.adminPage;
using Traveling.Models;
using Traveling.Services;
using Traveling.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Traveling
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        CosmosUsersDBService server;

        List<Users> userlist;
        public Login()
        {
            InitializeComponent();
        }

        async Task ExecuteRefreshCommand()
        {
            var username = user.Text;
            var password = pwd.Text;

            userlist = await CosmosUsersDBService.CheckLogin(username, password);
        }

        async void log_in(object sender, EventArgs e) {

            server = new CosmosUsersDBService();

            var username = user.Text;
            var password = pwd.Text;
            await ExecuteRefreshCommand();

            /*viewModel = new GetItemsViewModel();
            Console.WriteLine("aas " + viewModel.ItemsList);*/
            if (userlist.Count != 0)
            {
                if (userlist[0].username == username && userlist[0].password == password)
                {
                    CrossSecureStorage.Current.SetValue("username", username);
                    CrossSecureStorage.Current.SetValue("pasword", password);
                    CrossSecureStorage.Current.SetValue("firstname", userlist[0].Firstname);
                    CrossSecureStorage.Current.SetValue("lastname", userlist[0].Lastname);
                    CrossSecureStorage.Current.SetValue("mail", userlist[0].mail);

                    if (userlist[0].profession == "customer")
                    {
                        await Navigation.PushAsync(new Navigation());
                    }
                    else if (userlist[0].profession == "admin")
                    {
                        await Navigation.PushAsync(new menu());
                    }
                    else if (userlist[0].profession == "service")
                    {

                    }
                    else 
                    {
                        await DisplayAlert("ERROR","login error","OK");
                    }
                }
                else
                {
                    await DisplayAlert("ERROR", "error account", "OK");
                }
            }
            else {
                await DisplayAlert("ERROR", "error account", "OK");
            }
            

        }

    }
}