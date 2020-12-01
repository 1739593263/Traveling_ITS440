using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Traveling.adminPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class menu : ContentPage
    {
        public menu()
        {
            InitializeComponent();
        }

        async void UT_Click(object sender, EventArgs e) {
            await Navigation.PushAsync(new UserTable());
        }

        async void FT_Click(object sender, EventArgs e) {
            await Navigation.PushAsync(new FlightTable());
        }
    }
}