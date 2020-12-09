using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Plugin.SecureStorage;
using Traveling.Models;
using Traveling.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Traveling
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServicePage : ContentPage
    {
        Advise advise;

        public ServicePage()
        {
            InitializeComponent();
        }

        async void OnButtonsubmitClicked(object sender, EventArgs e)
        {
            var t = title.Text;
            var c = content.Text;
            var uid = CrossSecureStorage.Current.GetValue("id");
            var uname = CrossSecureStorage.Current.GetValue("firstname") +" "+ CrossSecureStorage.Current.GetValue("lastname");
            if (t != null && c != null) {
                advise = new Advise();
                advise.caseName = t;
                advise.userTalk = c;
                advise.userId = uid;
                advise.userName = uname;
                advise.needReply = 1;

                if (uid != null) {
                    await ExecuteInsert();
                    await DisplayAlert("SUCCESS", "Thanks for your advice, and we will solving it in soon", "OK");
                }
                else
                {
                    await DisplayAlert("ERROR", "please login at first", "OK");
                }
                
            }
            else
            {
                await DisplayAlert("ERROR","uncompleted content","OK");
            }
        }

        async Task ExecuteInsert()
        {
            await CosmosAdviceService.InsertAdvise(advise);
        }
    }
}