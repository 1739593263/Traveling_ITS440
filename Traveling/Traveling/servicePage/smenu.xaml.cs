using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Traveling.servicePage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class smenu : ContentPage
    {
        public smenu()
        {
            InitializeComponent();
        }
        async void S_Click(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new solvedPage());
        }


        async void US_Click(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new unsolvedPage());
        }
    }

}