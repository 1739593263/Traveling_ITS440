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
    public partial class TrainTable : ContentPage
    {
        TrainTableViewModel trainViewModel;
        PlacesViewModel placesViewModel;

        List<Train> trainLists;
        int avail = 0;
        int tap_num;
        Train trainline;
        Train add_trainline;

        string st;
        string et;
        public TrainTable()
        {
            InitializeComponent();

            trainViewModel = new TrainTableViewModel();
            TrainList.BindingContext = trainViewModel;

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
            trainline = trainViewModel.TrainList[tap_num];
            st = trainline.sourceTime;
            et = trainline.destinationTime;

            Console.WriteLine("asd " + trainline.destinationTime + " ");
        }

        async Task ExecuteSearchCommand()
        {
            string s = SDate.Date.ToString();
            string[] dates = s.Split(' ');
            string sou = (string)SPlace.SelectedItem;
            string des = (string)DPlace.SelectedItem;

            trainLists = await CosmosTrainTableService.SearchAllTrain(sou, des, dates[0]);
        }

        async Task ExecuteInsert()
        {
            await CosmosTrainTableService.InsertTrain(add_trainline);
        }

        async Task ExecuteAvailable()
        {
            await CosmosTrainTableService.UpdateTrain(trainline);
            if (trainline.isAvailable == 0)
            {
                await DisplayAlert("SUCCESS", "this line no longer available", "OK");
            }
            else if (trainline.isAvailable == 1)
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
            await CosmosTrainTableService.UpdateTrain(trainline);
            await DisplayAlert("SUCCESS", "operation success", "OK");
        }

        async Task ExecuteDelete()
        {
            await CosmosTrainTableService.DeleteTrain(trainline);
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
                TrainList.BindingContext = "";
                TrainList.ItemsSource = trainLists;
            }
        }

        async void insert(object sender, EventArgs e)
        {
            string s = SDate.Date.ToString();
            string[] dates = s.Split(' ');
            string sou = (string)SPlace.SelectedItem;
            string des = (string)DPlace.SelectedItem;

            add_trainline = new Train();
            add_trainline.source = sou;
            add_trainline.destination = des;
            add_trainline.date = dates[0];
            add_trainline.sourceTime = "00:00";
            add_trainline.destinationTime = "00:00";
            add_trainline.economy = 20;
            add_trainline.business = 10;
            add_trainline.first = 5;
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
                await DisplayAlert("SUCCESS", "train line is added", "OK");

                await Navigation.PushAsync(new loading());
                await Navigation.PopAsync();
            }
        }
        async void plusAvail(object sender, EventArgs e)
        {
            avail = 1;
            if (trainline.price <= 0 &&
                trainline.company == null &&
                trainline.sourceTime == trainline.destinationTime)
            {
                await DisplayAlert("ERROR", "please complete Info of tapped item", "OK");
            }
            else
            {
                trainline.isAvailable = 1;
                await ExecuteAvailable();
            }
        }

        async void dropAvail(object sender, EventArgs e)
        {
            avail = 0;
            trainline.isAvailable = 0;
            await ExecuteAvailable();
        }

        async void toupdate(object sender, EventArgs e)
        {
            trainline.price2 = trainline.price + 50;
            trainline.price3 = trainline.price + 100;

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

            trainViewModel.RefreshCommand.Execute(null);
            placesViewModel.RefreshCommand.Execute(null);
        }
    }
}