﻿using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Traveling.Models;
using Traveling.Services;
using Xamarin.Forms;

namespace Traveling.ViewModels.threeTransaction
{
    class GetPTransactionViewModel : BaseViewModel
    {
        List<Transaction> transList;
        public List<Transaction> TransList { get => transList; set => SetProperty(ref transList, value); }

        public ICommand RefreshCommand { get; }

        public GetPTransactionViewModel()
        {
            RefreshCommand = new Command(async () => await ExecuteRefreshCommand());
        }

        async Task ExecuteRefreshCommand()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                TransList = await CosmosTransService.GetPTransaction(CrossSecureStorage.Current.GetValue("id"));
            }
            finally
            {
                IsBusy = false;
            }

        }
    }
}
