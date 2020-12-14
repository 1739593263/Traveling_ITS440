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
    public partial class CarPage : ContentPage
    {
        Transaction transaction;

        public CarPage()
        {
            InitializeComponent();
        }

        async void call(object sender, EventArgs e)
        {
            transaction = new Transaction();

            string sourceAddress = source.Text;
            string desAddress = destination.Text;
            string day = date.Date.ToString();
            string[] d = day.Split(' ');
            TimeSpan sourcetime = sourceTime.Time;
            string st = sourcetime + "";

            transaction.userId = CrossSecureStorage.Current.GetValue("id");
            transaction.source = sourceAddress;
            transaction.destination = desAddress;
            transaction.date = d[0];
            transaction.sourceTime = st;
            transaction.isPaid = 1;
            transaction.vehicleSort = "car";
            double p = new Random().Next(20, 120);
            transaction.price = p;
            if (sourceAddress!=null && desAddress != null)
            {
                amount.Text = p.ToString();
            }
            await InsertTransaction();
            /*string all = sourceAddress + "\n" + desAddress + "\n" + d[0] + "\n" + st;
            await DisplayAlert("", all, "OK");*/
        }

        async Task InsertTransaction()
        {
            await CosmosTransService.InsertTransaction(transaction);
            await DisplayAlert("", "You called a car", "OK");
        }
    }
}