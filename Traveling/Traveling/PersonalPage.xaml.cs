using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Plugin.SecureStorage;
using Traveling.Services;

namespace Traveling
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PersonalPage : ContentPage
    {

        public PersonalPage()
        {
            InitializeComponent();

            if (CrossSecureStorage.Current.HasKey("id"))
            {
                grey.IsVisible = false;
                head.IsVisible = !grey.IsVisible;
                intro.IsVisible = !grey.IsVisible;
            }
            else
            {
                grey.IsVisible = true;
                head.IsVisible = !grey.IsVisible;
                intro.IsVisible = grey.IsVisible;
            }
            string uname = CrossSecureStorage.Current.GetValue("firstname")+" "+CrossSecureStorage.Current.GetValue("lastname");
            name.Text = uname;
            mail.Text = CrossSecureStorage.Current.GetValue("mail");
            id.Text = CrossSecureStorage.Current.GetValue("id");
        }


        async void figure(object sender, EventArgs e) {
            if (grey.IsVisible) {
                await DisplayAlert("!", "You have not log in yet", "OK");
            }
            else
            {
            }
        }

        async void toUp(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new userUnPaid());
        }

        async void toP(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new userPay());
        }

        async void toAp(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new userAllorder());
        }

        async void toAd(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new userPay());
        }

        async void toHt(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new userAllorder());
        }
    }
}