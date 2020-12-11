using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traveling.Models;
using Traveling.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Traveling
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class bookSchedule : ContentPage
    {
        List<grade> newList;
        int tapped_num = -1;
        double value = 0;
        string type = "";
        string Sort = "";
        string ID;

        Train _train = new Train();
        Schedule _schedule = new Schedule();
        Transaction transaction;

        int seats;

        public class grade
        {
            public string type { get; set; }
            public double price { get; set; }
        }
        public bookSchedule(string id, double price, double price2, double price3, string sort)
        {
            Sort = sort;
            ID = id;

            InitializeComponent();
            newList = new List<grade> {
                new grade{ type="Economy", price = price},
                new grade{ type="Business", price = price2},
                new grade{ type="First", price = price3},
            };
            scheduleitem.ItemsSource = newList;
        }

        async void tap(object sender, ItemTappedEventArgs e)
        {
            await ExecuteGetMaximum();

            type = newList[e.ItemIndex].type;

            // limit the maximum of stepper through the number of seats
            if (type == "Economy")
            {
                if(Sort == "Flight")   stepper.Maximum = _schedule.economy;
                else  stepper.Maximum = _train.economy;
            }
            else if (type == "Business") {
                if (Sort == "Flight")  stepper.Maximum = _schedule.business;
                else stepper.Maximum = _train.business;
            }
            else
            {
                if (Sort == "Flight")  stepper.Maximum = _schedule.first;
                else stepper.Maximum = _train.first;
            }
            

            if (e.ItemIndex != tapped_num)
            {
                sum.Text = "";
                tapped_num = e.ItemIndex;
                stepper.Value = 0;
            }
            else
            {
                tapped_num = e.ItemIndex;
            }
        }

        void OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            value = e.NewValue;
            if (tapped_num >= 0)
            {
                double price = newList[tapped_num].price;
                sum.Text = value * price + "";
            }
        }

        async Task ExecuteGetMaximum()
        {
            if(Sort == "Flight")
            {
                _schedule = await CosmosScheduleDBService.SearchScheduleById(ID);
            }
            else if(Sort == "Train")
            {
                _train = await CosmosTrainTableService.SearchTrainById(ID);
            }
            else
            {
                return;
            }
        }

        async Task ExecuteGetTransCommand()
        {
            if (CrossSecureStorage.Current.HasKey("transid"))
            {
                string tid = CrossSecureStorage.Current.GetValue("transid");
                transaction = await CosmosTransService.searchTransById(tid);

                CrossSecureStorage.Current.DeleteKey("transid");
            }
            else
            {
                string uid = CrossSecureStorage.Current.GetValue("id");
                transaction = await CosmosTransService.SearchTransByLastUid(uid);
            }
        }

        async Task ExecuteUpdateTrans()
        {
            await CosmosTransService.UpdateTransaction(transaction);
        }

        async Task ExecuteUpdatevehicle()
        {
            if (Sort == "Flight")
            {
                await CosmosScheduleDBService.UpdateSchedule(_schedule);
            }
            else if (Sort == "Train")
            {
                await CosmosTrainTableService.UpdateTrain(_train);
            }
            else return;
        }

        async void bookIt(object snder, EventArgs e)
        {
            await ExecuteGetTransCommand();
            transaction.price = value * newList[tapped_num].price;


            if (transaction.price > 0)
            {
                transaction.isPaid = 1;
                await ExecuteUpdateTrans();

                await DisplayAlert("SUCCESS", "operation success", "OK");
            }
            else
            {
                await DisplayAlert("ERROR", "Please complete the information", "OK");
            }

            // change seat quantity
            if (_schedule != null)
            {
                if (type == "Economy") _schedule.economy -= (int)value;
                else if (type == "Business") _schedule.business -= (int)value;
                else _schedule.first -= (int)value;
                await ExecuteUpdatevehicle();
            }
            else if (_train != null)
            {
                if (type == "Economy") _train.economy -= (int)value;
                else if (type == "Business") _train.business -= (int)value;
                else _train.first -= (int)value;
                await ExecuteUpdatevehicle();
            }
            else return;
        }
    }
}