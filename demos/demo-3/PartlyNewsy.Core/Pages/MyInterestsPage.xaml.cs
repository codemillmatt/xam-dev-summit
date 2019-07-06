using System;
using System.Collections.Generic;
using PartlyNewsy.Core;
using Xamarin.Forms;

namespace PartlyNewsy
{
    public partial class MyInterestsPage : ContentPage
    {
        MyInterestsViewModel vm;
        IAuthenticationService authService;

        bool isLoaded = false;

        public MyInterestsPage()
        {
            InitializeComponent();

            authService = DependencyService.Get<IAuthenticationService>();

            vm = new MyInterestsViewModel();
            BindingContext = vm;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (!isLoaded && authService.IsLoggedIn())
            {
                await vm.GetUserInterests();
                isLoaded = true;
            }
        }
    }
}
