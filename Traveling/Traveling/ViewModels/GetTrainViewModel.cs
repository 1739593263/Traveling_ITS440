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
    class GetTrainViewModel : BaseViewModel
    {
        List<Train> trainList;
        public List<Train> TrainList { get => trainList; set => SetProperty(ref trainList, value); }

        public ICommand RefreshCommand { get; }

        public GetTrainViewModel()
        {
            RefreshCommand = new Command(async () => await ExecuteRefreshCommand());
        }

        async Task ExecuteRefreshCommand()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                TrainList = await CosmosTrainTableService.GetTrain();
            }
            finally
            {
                IsBusy = false;
            }

        }
    }
}
