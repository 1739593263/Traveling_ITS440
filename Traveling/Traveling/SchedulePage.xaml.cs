using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traveling.ViewModels;
using Traveling.ViewModels.threeTransaction;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Traveling
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SchedulePage : ContentPage
    {
        GetFlightTransactionViewModel flightTransactionViewModel;
        GetTrainTransViewModel trainTransViewModel;
        GetCarTransViewModel carTransViewModel;

        static int clickTotal;
        public SchedulePage()
        {
            InitializeComponent();

            flightTransactionViewModel = new GetFlightTransactionViewModel();
            trainTransViewModel = new GetTrainTransViewModel();
            carTransViewModel = new GetCarTransViewModel();

            ScheduleList.BindingContext = flightTransactionViewModel;
        }

        async void OnButtonairplaneClicked(object sender, EventArgs e)
        {
            title.Text = "Flight ";
            ScheduleList.BindingContext = "";
            ScheduleList.ItemsSource = flightTransactionViewModel.TransList;

            ScheduleList.BackgroundColor = Color.LightBlue;
        }

        async void OnButtontrainClicked(object sender, EventArgs e)
        {
            title.Text = "Train ";
            ScheduleList.BindingContext = "";
            ScheduleList.ItemsSource = trainTransViewModel.TransList;

            ScheduleList.BackgroundColor = Color.LightCyan;
        }

        async void OnButtoncarClicked(object sender, EventArgs e)
        {
            title.Text = "Car ";
            ScheduleList.BindingContext = "";
            ScheduleList.ItemsSource = carTransViewModel.TransList;

            ScheduleList.BackgroundColor = Color.WhiteSmoke;
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            flightTransactionViewModel.RefreshCommand.Execute(null);
            trainTransViewModel.RefreshCommand.Execute(null);
            carTransViewModel.RefreshCommand.Execute(null);
        }
    }
}