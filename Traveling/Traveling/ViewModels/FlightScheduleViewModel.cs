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
    class FlightScheduleViewModel:BaseViewModel
    {
        List<Schedule> flightList;
        public List<Schedule> FlightList { get => flightList; set => SetProperty(ref flightList, value); }

        public ICommand RefreshCommand { get; }

        public FlightScheduleViewModel()
        {
            RefreshCommand = new Command(async () => await ExecuteRefreshCommand());
        }

        async Task ExecuteRefreshCommand()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                FlightList = await CosmosScheduleDBService.GetSchedule();
            }
            finally
            {
                IsBusy = false;
            }

        }
    }
}
