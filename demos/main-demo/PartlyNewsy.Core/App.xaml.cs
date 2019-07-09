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
            AppCenter.Start("ios=bd0701e6-b000-42e8-bf75-a390d2a2ee48;" +
                  "android=b7857000-1ebc-4252-b73b-39db6ab680c2",
                  typeof(Analytics), typeof(Crashes));

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
