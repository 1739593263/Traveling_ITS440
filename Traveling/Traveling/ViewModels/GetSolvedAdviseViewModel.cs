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
    class GetSolvedAdviseViewModel : BaseViewModel
    {
        List<Advise> adviselList;
        public List<Advise> AdviselList { get => adviselList; set => SetProperty(ref adviselList, value); }

        public ICommand RefreshCommand { get; }

        public GetSolvedAdviseViewModel()
        {
            RefreshCommand = new Command(async () => await ExecuteRefreshCommand());

        }

        async Task ExecuteRefreshCommand()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                AdviselList = await CosmosAdviceService.GetSolvedAdvises();
            }
            finally
            {
                IsBusy = false;
            }

        }
    }
}
