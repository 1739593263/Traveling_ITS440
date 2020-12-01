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

namespace Traveling.adminPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlightTable : ContentPage
    {
        FlightTableViewModel flightTableViewModel;
        PlacesViewModel placesViewModel;
        List<Schedule> flightLists;
        int avail = 0;
        int tap_num;
        Schedule flightline;
        Schedule add_flightline;

        public FlightTable()
        {
            InitializeComponent();
            flightTableViewModel = new FlightTableViewModel();
            FlightList.BindingContext = flightTableViewModel;

            placesViewModel = new PlacesViewModel();
            SPlace.BindingContext = placesViewModel;
            DPlace.BindingContext = placesViewModel;
        }

        async void tap(object sender, ItemTappedEventArgs e)
        {
            sa.IsEnabled = true;
            da.IsEnabled = true;
            update.IsEnabled = true;
            delete.IsEnabled = true;

            tap_num = e.ItemIndex;
            flightline = flightTableViewModel.FlightList[tap_num];
        }

        async Task ExecuteSearchCommand()
        {
            string s = SDate.Date.ToString();
            string[] dates = s.Split(' ');
            string sou = (string)SPlace.SelectedItem;
            string des = (string)DPlace.SelectedItem;

            flightLists = await CosmosScheduleDBService.SearchAllSchedule(sou, des, dates[0]);
        }

        async Task ExecuteInsert()
        {
            await CosmosScheduleDBService.InsertSchedule(add_flightline);
        }

        async Task ExecuteAvailable()
        {
            await CosmosScheduleDBService.UpdateSchedule(flightline);
            if (flightline.isAvailable == 0)
            {
                await DisplayAlert("SUCCESS", "this line no longer available", "OK");
            }
            else if(flightline.isAvailable == 1)
            {
                await DisplayAlert("SUCCESS", "this line is available", "OK");
            }
            else
            {
                await DisplayAlert("SUCCESS", "operation success", "OK");
            }
        }

        async Task ExecuteUpdate()
        {
            await CosmosScheduleDBService.UpdateSchedule(flightline);
            await DisplayAlert("SUCCESS", "operation success", "OK");
        }

        async Task ExecuteDelete()
        {
            await CosmosScheduleDBService.DeleteSchdule(flightline);
            await DisplayAlert("SUCCESS", "operation success", "OK");
        }

        async void searchSchedule(object sender, EventArgs e)
        {
            string s = SDate.Date.ToString();
            string[] dates = s.Split(' ');
            string sou = (string)SPlace.SelectedItem;
            string des = (string)DPlace.SelectedItem;
            Console.WriteLine(dates[0] + " " + sou + " " + des);
            if (sou == des)
            {
                await DisplayAlert("Error", "the same source and destination", "retry");
            }
            else if (sou == null || des == null) 
            {
                await DisplayAlert("Error", "please input source and destination", "retry");
            }
            else
            {
                await ExecuteSearchCommand();
                FlightList.BindingContext = "";
                FlightList.ItemsSource = flightLists;
            }
        }

        async void insert(object sender, EventArgs e)
        {
            string s = SDate.Date.ToString();
            string[] dates = s.Split(' ');
            string sou = (string)SPlace.SelectedItem;
            string des = (string)DPlace.SelectedItem;
            add_flightline = new Schedule();
            add_flightline.source = sou;
            add_flightline.destination = des;
            add_flightline.date = dates[0];
            if (sou == des)
            {
                await DisplayAlert("Error", "the same source and destination", "retry");
            }
            else if (sou == null || des == null)
            {
                await DisplayAlert("Error", "please input source and destination", "retry");
            }
            else
            {
                await ExecuteInsert();
                await DisplayAlert("SUCCESS", "flight line is added", "OK");
                await Navigation.PushAsync(new loading());
                await Navigation.PopAsync();
            }
        }
        async void plusAvail(object sender, EventArgs e) 
        {
            avail = 1;
            flightline.isAvailable = 1;
            if(flightline.price <= 0 && 
                flightline.sourceTime == flightline.destinationTime && 
                flightline.company == null)
            {
                await DisplayAlert("ERROR", "please complete Info of tapped item", "OK");
            }else{
                await ExecuteAvailable(); 
            }
        }

        async void dropAvail(object sender, EventArgs e)
        {
            avail = 0;
            flightline.isAvailable = 0;
            await ExecuteAvailable();
        }

        async void toupdate(object sender, EventArgs e) 
        {
            await ExecuteUpdate();
        }

        async void todelete(object sender, EventArgs e) 
        {
            await ExecuteDelete();
            await Navigation.PushAsync(new loading());
            await Navigation.PopAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            flightTableViewModel.RefreshCommand.Execute(null);
            placesViewModel.RefreshCommand.Execute(null);
        }
    }
}