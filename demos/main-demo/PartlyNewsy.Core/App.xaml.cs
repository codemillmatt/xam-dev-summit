using System;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AppCenter.Auth;

namespace PartlyNewsy.Core
{
    public partial class App : Xamarin.Forms.Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected async override void OnStart()
        {
            // Handle when your app starts
            AppCenter.Start("ios=601454a7-fa8e-4754-928a-7e7afae04148;" +
                  "android=62377643-89da-44cc-926c-1156186a6a22",
                  typeof(Analytics), typeof(Crashes), typeof(Auth));

            await Auth.SetEnabledAsync(true);

            await GetInitialTabLayout();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        async Task GetInitialTabLayout()
        {
            var dataService = DependencyService.Get<IDataService>();
            var displayNewsCategories = (await dataService.GetInterestCategories());

            var addMessage = new AddInterestTabMessage { InterestNames = new List<InterestSubMessage>() };
            foreach (var item in displayNewsCategories)
            {
                addMessage.InterestNames.Add(new InterestSubMessage { Title = item.DisplayName, Category = item.CategoryName });
            }

            MessagingCenter.Send(addMessage, AddInterestTabMessage.AddTabMessage);
        }

    }
}
