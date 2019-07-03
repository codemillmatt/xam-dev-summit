using System;
using MvvmHelpers;
using PartlyNewsy.Models;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Microsoft.AppCenter.Crashes;

namespace PartlyNewsy.Core
{
    public class NewsCollectionViewModel : BaseViewModel
    {
        readonly INewsService newsService;
        readonly IDataService dataService;
        
        public ICommand SelectArticleCommand { get; }
        

        public NewsCollectionViewModel()
        {
            newsService = DependencyService.Get<INewsService>();
            dataService = DependencyService.Get<IDataService>();

            NewsCollection = new ObservableRangeCollection<Article>();

            SelectArticleCommand = new Command(async () => await ExecuteSelectArticleCommand());
        }

        string localityName;
        public string LocalityName
        {
            get => localityName;
            set => SetProperty(ref localityName, value);
        }

        bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set => SetProperty(ref isRefreshing, value);
        }

        async Task ExecuteSelectArticleCommand()
        {
            // put together the query string            
            var articleUrl = Uri.EscapeDataString(SelectedArticle.ArticleUrl);

            var queryString = $"articleUrl={articleUrl}";

            await Shell.Current.GoToAsync($"{AppShell.ArticleDetailsRoute}?{queryString}", true);
        }

        ObservableRangeCollection<Article> newsCollection;
        public ObservableRangeCollection<Article> NewsCollection
        {
            get => newsCollection;
            set
            {
                SetProperty(ref newsCollection, value);
            }
        }

        Article selectedArticle;
        public Article SelectedArticle
        {
            get => selectedArticle;
            set => SetProperty(ref selectedArticle, value);
        }

        public async Task LoadNewsByCategory(string category)
        {
            IsRefreshing = true;
            NewsCollection.Clear();

            List<Article> news = null;

            if (category == "Top-News")
            {
                news = await newsService.GetTopNews();
            }
            else if (category == "My-News")
            {
                var userInterests = (await dataService.GetInterestCategories()).Select(c => c.CategoryName).ToList();

                news = await newsService.GetMyNews(userInterests);

                // loop through everything and change over the category name
                var allCategories = new AllNewsCategories();
                foreach (var item in news)
                {
                    item.Category = allCategories.First(c => c.CategoryName == item.Category).DisplayName;
                }
            }
            else if (category == "Local")
            {
                var query = await GetLastLocation() ?? await GetFullLocation();

                LocalityName = $"{query} News";

                news = await newsService.GetLocalNews(query);
            }
            else
            {
                news = await newsService.GetNewsByCategory(category);
            }

            if (news != null)
                NewsCollection.AddRange(news);            

            IsRefreshing = false;
        }

        async Task<string> GetLastLocation()
        {
            Location location = null;

            try
            {
                location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    var placemarks = await Geocoding.GetPlacemarksAsync(location);

                    return placemarks.FirstOrDefault()?.Locality;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            return null;
        }

        async Task<string> GetFullLocation()
        {
            Location location = null;

            try
            {
                location = await Geolocation.GetLocationAsync();

                if (location != null)
                {
                    var placemarks = await Geocoding.GetPlacemarksAsync(location);

                    return placemarks.FirstOrDefault()?.Locality;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            return null;
        }

    }
}
