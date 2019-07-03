using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PartlyNewsy.Core;
using Xamarin.Forms;

namespace PartlyNewsy
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        async void HandleLogin_Tapped(object sender, EventArgs e)
        {
            var authService = DependencyService.Get<IAuthenticationService>();

            var success = await authService.Login();

            if (!success)
            {
                await DisplayAlert("Login Error!", "Oh oh - something went wrong during the login!", "OK");
                return;
            }            
        }        
    }
}
