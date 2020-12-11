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
    public partial class userPay : ContentPage
    {
        GetPTransactionViewModel GetPTransactionViewModel;
        int tap_num;

        public userPay()
        {
            InitializeComponent();
            GetPTransactionViewModel = new GetPTransactionViewModel();
            PaidList.BindingContext = GetPTransactionViewModel;
        }

        async void tap(object sender, ItemTappedEventArgs e)
        {
            tap_num = e.ItemIndex;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            GetPTransactionViewModel.RefreshCommand.Execute(null);
        }
    }
}