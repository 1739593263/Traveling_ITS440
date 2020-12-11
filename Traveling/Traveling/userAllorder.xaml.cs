using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traveling.ViewModels.threeTransaction;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Traveling
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class userAllorder : ContentPage
    {
        GetTransactionViewModel transactionViewModel;
        int tap_num;

        public userAllorder()
        {
            InitializeComponent();

            transactionViewModel = new GetTransactionViewModel();
            transList.BindingContext = transactionViewModel;
            
        }
        async void tap(object sender, ItemTappedEventArgs e)
        {
            tap_num = e.ItemIndex;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            transactionViewModel.RefreshCommand.Execute(null);
        }
    }
}