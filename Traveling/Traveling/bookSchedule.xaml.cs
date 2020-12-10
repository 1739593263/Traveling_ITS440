using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public class grade
        {
            public string type { get; set; }
            public double price { get; set; }
        }
        public bookSchedule(string id, double price, double price2, double price3)
        {
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

        async void bookIt(object snder, EventArgs e)
        {
            await DisplayAlert("", "YES!", "OK");
        }
    }
}