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
        List<Schedule> flightLists = new List<Schedule>();
        int avail = 0;
        int tap_num;
        Schedule flightline;
        Schedule add_flightline;

        string ID;
        string st;
        string et;
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
            st = flightline.sourceTime;
            et = flightline.destinationTime;

            Console.WriteLine("asd " + flightline.destinationTime + " ");
        }

        async void select(object sender, SelectedItemChangedEventArgs e)
        {
            var si = e.SelectedItem as Schedule;
            ID = si.Id;

            sa.IsEnabled = true;
            da.IsEnabled = true;
            update.IsEnabled = true;
            delete.IsEnabled = true;

            tap_num = e.SelectedItemIndex;
            flightline = new Schedule();
            flightline = await CosmosScheduleDBService.GetFlightById(ID);

            // For update new created item
            if (flightLists.Count != 0)
            {
                flightline.sourceTime = flightLists[tap_num].sourceTime;
                flightline.destinationTime = flightLists[tap_num].destinationTime;
                flightline.price = flightLists[tap_num].price;
                flightline.company = flightLists[tap_num].company;
                flightline.airplane = flightLists[tap_num].airplane;
                flightline.days = flightLists[tap_num].days;
            }
            else
            {
                flightline.sourceTime = flightTableViewModel.FlightList[tap_num].sourceTime;
                flightline.destinationTime = flightTableViewModel.FlightList[tap_num].destinationTime;
                flightline.price = flightTableViewModel.FlightList[tap_num].price;
                flightline.company = flightTableViewModel.FlightList[tap_num].company;
                flightline.airplane = flightTableViewModel.FlightList[tap_num].airplane;
                flightline.days = flightTableViewModel.FlightList[tap_num].days;
            }

            //flightline = flightTableViewModel.FlightList[tap_num];
            st = flightline.sourceTime;
            et = flightline.destinationTime;

            Console.WriteLine(tap_num+" "+flightline.airplane);
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
                flightTableViewModel.FlightList[tap_num].isAvailable = 0;
                await DisplayAlert("SUCCESS", "this line no longer available", "OK");
            }
            else if (flightline.isAvailable == 1)
            {
                flightTableViewModel.FlightList[tap_num].isAvailable = 1;
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
            add_flightline.sourceTime = "00:00";
            add_flightline.destinationTime = "00:00";
            add_flightline.economy = 20;
            add_flightline.business = 10;
            add_flightline.first = 5;
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
            if (flightline.price <= 0 &&
                flightline.company == null &&
                flightline.sourceTime == flightline.destinationTime)
            {
                await DisplayAlert("ERROR", "please complete Info of tapped item", "OK");
            }
            else
            {
                flightline.isAvailable = 1;
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
            flightline.price2 = flightline.price + 50;
            flightline.price3 = flightline.price + 100;

            int i = 0;
            string[] ss = st.Split(':');
            string[] ee = et.Split(':');
            if (ss.Length != 2 && ee.Length != 2)
            {
                await DisplayAlert("ERROR", "invalid time format", "OK");
                return;
            }
            else
            {
                bool s1 = int.TryParse(ss[0], out i);
                bool s2 = int.TryParse(ss[1], out i);
                bool e1 = int.TryParse(ee[0], out i);
                bool e2 = int.TryParse(ee[1], out i);
                if (s1 && s2 && e1 && e2)
                {
                    await ExecuteUpdate();
                }
                else
                {
                    await DisplayAlert("ERROR", "invalid time format", "OK");
                    return;
                }
            }
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