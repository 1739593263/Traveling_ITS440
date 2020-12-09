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

namespace Traveling.servicePage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class solvedPage : ContentPage
    {
        GetSolvedAdviseViewModel solvedAdviseViewModel;

        string cname;
        List<Advise> adviseList;
        public solvedPage()
        {
            InitializeComponent();
            solvedAdviseViewModel = new GetSolvedAdviseViewModel();
            solvedList.BindingContext = solvedAdviseViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            solvedAdviseViewModel.RefreshCommand.Execute(null);
        }

        async Task ExecuteSearch()
        {
            adviseList = await CosmosAdviceService.SearchSolvedAdvises(cname);
        }

        async void searchAdvise(object sender, EventArgs e)
        {
            cname = problme.Text;

            if (cname != null)
            {
                await ExecuteSearch();
                solvedList.BindingContext = "";
                solvedList.ItemsSource = adviseList;
            }
        }
    }
}