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
    public partial class register : ContentPage
    {
        public register()
        {
            Device.SetFlags(new string[] { "RadioButton_Experimental" });
            InitializeComponent();
        }

        async void sign_up(object sender, EventArgs e)
        {
            Users users = new Users();

            if(user.Text != null &&
                pwd.Text != null &&
                firstn.Text != null &&
                lastn.Text != null)
            {
                users.Firstname = firstn.Text;
                users.Lastname = lastn.Text;
                if (female.IsChecked)
                {
                    users.Gender = female.Text;
                }
                else if (male.IsChecked)
                {
                    users.Gender = male.Text;
                }
                else
                {
                    users.Gender = "";
                }
                users.username = user.Text;
                users.password = pwd.Text;
                users.birthday = birth.Date.ToString();
                users.profession = pro.SelectedItem.ToString();
                users.mail = mail.Text;
                users.location = location.Text;

                await ExecuteInsert(users);

                await DisplayAlert("", "success", "OK");
                firstn.Text = null;
                lastn.Text = null;
                female.IsChecked = false;
                male.IsChecked = false;
                user.Text = null;
                pwd.Text = null;
                pro.SelectedItem = "customer";
                mail.Text = null;
                location.Text = null;
            }
            else 
            {
                await DisplayAlert("Failure", "the basic information is not complete", "OK");
            }
        }

        async Task ExecuteInsert(Users user)
        {
            await CosmosUsersDBService.InsertUser(user);
        }
    }
}