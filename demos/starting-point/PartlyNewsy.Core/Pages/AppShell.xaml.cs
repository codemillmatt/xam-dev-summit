using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AppCenter.Crashes;
using PartlyNewsy.Core;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PartlyNewsy
{
    public partial class AppShell : Shell
    {
        public static readonly string ArticleDetailsRoute = "articledetails";
        bool needToAddTabsAfterSignIn = true;

        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(AppShell.ArticleDetailsRoute, typeof(ArticleDetailPage));

            MessagingCenter.Subscribe<AddInterestTabMessage>(this, AddInterestTabMessage.AddTabMessage, (aitm) =>
            {                                  
                foreach (var item in aitm.InterestNames)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        newsTab.Items.Add(new ShellContent
                        {
                            Content = new NewsCollectionPage { CategoryName = item.Category, Title = item.Title },
                            Title = item.Title
                        });
                    });
                }              
            });

            MessagingCenter.Subscribe<RemoveInterestTabMessage>(this, RemoveInterestTabMessage.RemoveTabMessage, (ritm) =>
            {
                // this won't work :)
                var shellItem = newsTab.Items.First(nt => nt.Title == ritm.InterestName);

                MainThread.BeginInvokeOnMainThread(() => newsTab.Items.Remove(shellItem));                
            });

            MessagingCenter.Subscribe<AddTabsAfterSigninMessage>(this, AddTabsAfterSigninMessage.AfterSigninMessage, async (atas) =>
            {
                // removing tabs does not work - so we'll leave that for a bit

                // TODO: there's going to be a bug where if the user isn't a logged in when
                // the app starts, then they log in AND have some of the starter tabs added as
                // favorite news categories, they will appear twice

                if (needToAddTabsAfterSignIn)
                {
                    // now add all the tabs from user's data stash
                    var data = DependencyService.Get<IDataService>();
                    var newsCategories = await data.GetInterestCategories();

                    foreach (var item in newsCategories)
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            newsTab.Items.Add(new ShellContent
                            {
                                Content = new NewsCollectionPage { CategoryName = item.CategoryName, Title = item.DisplayName },
                                Title = item.DisplayName
                            });
                        });
                    }

                    needToAddTabsAfterSignIn = false;
                }
            });
        }        
    }
}
