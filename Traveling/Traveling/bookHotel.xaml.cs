using Plugin.SecureStorage;
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
        HotelTrans hotelt;

        string ID;
        int tapped_num = -1;
        double value = 0;

        string type = "";
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
            type = newList[e.ItemIndex].type;
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

        async Task ExecuteGetTransCommand()
        {
            if (CrossSecureStorage.Current.HasKey("tid"))
            {
                string tid = CrossSecureStorage.Current.GetValue("tid");
                hotelt = await CosmosTransactionService.searchHotelById(tid);
                
                CrossSecureStorage.Current.DeleteKey("tid");
            }
            else
            {
                string uid = CrossSecureStorage.Current.GetValue("id");
                hotelt = await CosmosTransactionService.SearchHotelByLastUid(uid);
            }
        }

        async Task ExecuteSearchById() {
            hotel = await CosmosHotelService.GetHotelById(ID);
        }

        async Task ExecuteUpdateQuantity()
        {
            await CosmosHotelService.UpdateHotelInQuantity(ID,type,"-");
        }

        async Task ExecuteUpdateTrans()
        {
            await CosmosTransactionService.UpdateHotelTrans(hotelt);
            await DisplayAlert("SUCCESS", "operation success", "OK");
        }

        async void bookIt(object sender, EventArgs e) {
            await ExecuteGetTransCommand();

            string s = date.Date.ToString();
            string[] dates = s.Split(' ');
            hotelt.date = dates[0];
            hotelt.days = (int)stepper.Value;
            hotelt.roomType = newList[tapped_num].type;
            hotelt.price = value * newList[tapped_num].price;

            await ExecuteSearchById();
            // check the vacant 
            if (type == "Double")
            {
                if (hotel.doubleroom == 0)
                {
                    await DisplayAlert("ERROR", "NO ROOM", "OK");
                    return;
                }
            }
            else if (type == "Quadruple")
            {
                if (hotel.Quaroom == 0)
                {
                    await DisplayAlert("ERROR", "NO ROOM", "OK");
                    return;
                }
            }
            else
            {
                if (hotel.suit == 0)
                {
                    await DisplayAlert("ERROR", "NO ROOM", "OK");
                    return;
                }
            }

            if (hotelt.price > 0)
            {
                hotelt.isPaid = 1;
                await ExecuteUpdateTrans();
                await ExecuteUpdateQuantity();
            }
            else {
                await DisplayAlert("ERROR", "Please complete the information", "OK");
            }
        }
    }
}