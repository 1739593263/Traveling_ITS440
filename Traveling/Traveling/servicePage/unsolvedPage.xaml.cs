using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traveling.Models;
using Traveling.Services;
using Traveling.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Plugin.SecureStorage;
namespace Traveling.servicePage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class unsolvedPage : ContentPage
    {
        GetunSolvedAdviseViewModel getunSolvedAdviseViewModel;
        Advise advise;
        int tap_num = -1;

        string cname;
        List<Advise> adviseList;
        public unsolvedPage()
        {
            InitializeComponent();
            re.IsEnabled = false;

            getunSolvedAdviseViewModel = new GetunSolvedAdviseViewModel();
            unsolvedList.BindingContext = getunSolvedAdviseViewModel;
        }

        async void tap(object sender, ItemTappedEventArgs e)
        {
            tap_num = e.ItemIndex;
            re.IsEnabled = true;
            advise = getunSolvedAdviseViewModel.AdviselList[tap_num];
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            re.IsEnabled = true;
            getunSolvedAdviseViewModel.RefreshCommand.Execute(null);
        }

        async Task ExecuteResponse()
        {
            await CosmosAdviceService.UpdateAdvise(advise);
            await DisplayAlert("SUCCESS", "operation success", "OK");
        }

        async Task ExecuteSearch()
        {
            adviseList = await CosmosAdviceService.SearchunSolvedAdvises(cname);
        }

        async void searchAdvise(object sender, EventArgs e) {
            cname = problme.Text;

            if (cname != null)
            {
                await ExecuteSearch();
                unsolvedList.BindingContext = "";
                unsolvedList.ItemsSource = adviseList;
            }
        }

        async void response(object sender, EventArgs e) {
            string result = await DisplayPromptAsync("Response", "Enter your response", placeholder: "limited in 50 words", maxLength: 50);
            /*Console.WriteLine("result: "+result);*/
            var sid = CrossSecureStorage.Current.GetValue("id");
            var sname = CrossSecureStorage.Current.GetValue("firstname") + " " + CrossSecureStorage.Current.GetValue("lastname");



            if (tap_num >=0 && result != "")
            {
                advise.isReply = 1;
                advise.needReply = 0;
                advise.serviceId = sid;
                advise.serviceName = sname;
                advise.serviceTalk = result;

                await ExecuteResponse();
            }
            else
            {
                await DisplayAlert("Hey!", "operation failure", "OK");
            } 
        }

    }
}