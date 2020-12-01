using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Traveling.Models;
using Traveling.Services;
using Xamarin.Forms;

namespace Traveling.ViewModels
{
    class UsersOperationViewModel:BaseViewModel
    {
        //Boolean logincheck = false;
        List<Users> usersList;
        //public Boolean loginCheck { get => logincheck; set => SetProperty(ref logincheck, value); }
        public List<Users> UsersList { get => usersList; set => SetProperty(ref usersList, value); }

        public ICommand RefreshCommand { get; }
        public string username { get; set; }
        public string password { get; set; }

        public UsersOperationViewModel(string user, string pwd) {
            username = user;
            password = pwd;
            RefreshCommand = new Command(async () => await ExecuteRefreshCommand());
            
        }
        async Task ExecuteRefreshCommand() {
           
            
                UsersList = await CosmosUsersDBService.CheckLogin(username, password);
                Console.WriteLine("Model...");
                //Console.WriteLine(UsersList[0].username);
            
        }
    }
}
