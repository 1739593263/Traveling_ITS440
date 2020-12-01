using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Traveling.Models;
using Traveling.ViewModels;
using Xamarin.Forms;

namespace Traveling.Services
{
    class FlightTableViewModel:BaseViewModel
    {
        List<Schedule> flightList;
        public List<Schedule> FlightList { get => flightList; set => SetProperty(ref flightList, value); }

        public ICommand RefreshCommand { get; }

        public FlightTableViewModel()
        {
            RefreshCommand = new Command(async () => await ExecuteRefreshCommand());
        }

        async Task ExecuteRefreshCommand()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                FlightList = await CosmosScheduleDBService.GetAllSchedule();
            }
            finally
            {
                IsBusy = false;
            }

        }
    }
}
