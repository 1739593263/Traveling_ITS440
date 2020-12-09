using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traveling.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Traveling.servicePage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class solvedPage : ContentPage
    {
        GetSolvedAdviseViewModel solvedAdviseViewModel;

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
    }
}