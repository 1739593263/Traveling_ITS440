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

namespace Traveling
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class bookHotel : ContentPage
    {
        CosmosHotelService hotelService;
        Hotel hotel;
        List<rooms> newList;

        string ID;
        int tapped_num = -1;

        public class rooms
        {
            public string picture { get; set; }
            public string type { get; set; }
            public double price { get; set; }
        }
        public bookHotel(string id, double p1, double p2, double p3)
        {
            InitializeComponent();
            ID = id;
            newList = new List<rooms> { 
                new rooms{ picture="dou.jpg", type = "Double", price= p1},
                new rooms{ picture="Qua.jpg", type = "Quadruple", price= p2},
                new rooms{ picture="Suite.jpg", type = "Suite", price= p3},
            };
            hotelitem.ItemsSource = newList;
        }

        async void tap(object sender, ItemTappedEventArgs e) 
        {
            if(e.ItemIndex != tapped_num)
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
            double value = e.NewValue;
            if (tapped_num >= 0)
            {
                double price = newList[tapped_num].price;
                sum.Text = value * price + "";
            }
        }

        async void bookIt(object sender, EventArgs e) {
            await DisplayAlert("",ID +" "+ newList[tapped_num].type+" "+ sum.Text, "OK");
        }
    }
}