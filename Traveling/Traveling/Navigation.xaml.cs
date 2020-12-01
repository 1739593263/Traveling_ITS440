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
    public partial class Navigation : TabbedPage
    {
        public Navigation()
        {
            InitializeComponent();
        }

        async void toLogin(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Login());
        }

        async void toReserve(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BuyFlightPage());
        }

    }
}