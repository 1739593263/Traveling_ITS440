using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Plugin.SecureStorage;

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
            if (CrossSecureStorage.Current.HasKey("username")) {
                ToolbarItem item2 = new ToolbarItem
                {
                    Text = "Hi! "+ CrossSecureStorage.Current.GetValue("lastname"),
                    Order = ToolbarItemOrder.Secondary,
                };
                this.ToolbarItems.Add(item2);

                ToolbarItem item0 = new ToolbarItem
                {
                    Text = "sign out",
                    Order = ToolbarItemOrder.Secondary,
                };
                item0.Clicked += toOut;
                this.ToolbarItems.Add(item0);
            }
            else
            {
                ToolbarItem item0 = new ToolbarItem
                {
                    Text = "Login",
                    Order = ToolbarItemOrder.Secondary,
                };
                item0.Clicked += toLogin;
                this.ToolbarItems.Add(item0);
            }

            ToolbarItem item1 = new ToolbarItem
            {
                Text = "reserve",
                Order = ToolbarItemOrder.Secondary,
            };
            item1.Clicked += toReserve;
            this.ToolbarItems.Add(item1);

            ToolbarItem item3 = new ToolbarItem
            {
                Text = "hotel",
                Order = ToolbarItemOrder.Secondary,
            };
            item3.Clicked += toHotel;
            this.ToolbarItems.Add(item3);

            ToolbarItem item4 = new ToolbarItem
            {
                Text = "Train",
                Order = ToolbarItemOrder.Secondary,
            };
            item4.Clicked += toTrain;
            this.ToolbarItems.Add(item4);

        }

        async void toLogin(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
            await Navigation.PushAsync(new Login());
        }

        async void toOut(object sender, EventArgs e)
        {
            CrossSecureStorage.Current.DeleteKey("id");
            CrossSecureStorage.Current.DeleteKey("username");
            CrossSecureStorage.Current.DeleteKey("pasword");
            CrossSecureStorage.Current.DeleteKey("firstname");
            CrossSecureStorage.Current.DeleteKey("lastname");
            CrossSecureStorage.Current.DeleteKey("mail");
            await Navigation.PushAsync(new Login());
        }

        async void toReserve(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BuyFlightPage());
        }

        async void toHotel(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HotelPage());
        }

        async void toTrain(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TrainPage());
        }
    }
}