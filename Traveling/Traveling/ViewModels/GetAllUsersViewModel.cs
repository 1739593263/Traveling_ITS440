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
    class GetAllUsersViewModel:BaseViewModel
    {
        //Boolean logincheck = false;
        List<Users> usersList;
        //public Boolean loginCheck { get => logincheck; set => SetProperty(ref logincheck, value); }
        public List<Users> UsersList { get => usersList; set => SetProperty(ref usersList, value); }

        public ICommand RefreshCommand { get; }
        public GetAllUsersViewModel()
        {
            RefreshCommand = new Command(async () => await ExecuteRefreshCommand());
        }

        async Task ExecuteRefreshCommand()
        {

            if (IsBusy) return;

            IsBusy = true;
            try
            {
                UsersList = await CosmosUsersDBService.GetAllUsers();
            }
            finally
            {
                IsBusy = false;
            }

        }
    }
}
