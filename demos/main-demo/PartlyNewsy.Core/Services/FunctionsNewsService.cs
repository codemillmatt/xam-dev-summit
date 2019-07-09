using System;
using Xamarin.Forms;
using PartlyNewsy.Core;
using PartlyNewsy.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using Xamarin.Essentials;
using Microsoft.AppCenter.Crashes;

//[assembly: Dependency(typeof(FunctionsNewsService))]
namespace PartlyNewsy.Core
{
    public class FunctionsNewsService : INewsService
    {
        readonly INewsRestAPI newsRestAPI;

        public FunctionsNewsService()
        {
            newsRestAPI = RestService.For<INewsRestAPI>(Constants.NewsFunctionsUrl);
        }

        public async Task<List<Article>> GetLocalNews(string locality)
        {
            try
            {
                var current = Connectivity.NetworkAccess;

                if (current == NetworkAccess.Internet)
                {
                    var newsArticles = await newsRestAPI.GetLocalNews(locality);

                    return newsArticles;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

                return null;
            }

            return new List<Article>();
        }

        public async Task<List<Article>> GetMyNews(List<string> categories)
        {
            try
            {
                var current = Connectivity.NetworkAccess;

                if (current == NetworkAccess.Internet)
                {
                    var myNews = await newsRestAPI.GetMyNews(categories);

                    return myNews;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                return new List<Article>();
            }

            return new List<Article>();
        }

        public async Task<List<Article>> GetNewsByCategory(string category)
        {
            try
            {
                var current = Connectivity.NetworkAccess;

                if (current == NetworkAccess.Internet)
                {
                    var newsArticles = await newsRestAPI.GetNewsByCategory(category);

                    return newsArticles;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

                return null;
            }

            return new List<Article>();
        }

        public async Task<List<Article>> GetTopNews()
        {
            try
            {
                var current = Connectivity.NetworkAccess;

                if (current == NetworkAccess.Internet)
                {
                    var topNews = await newsRestAPI.GetTopNews();

                    return topNews;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                return new List<Article>();
            }

            return new List<Article>();
        }
    }

    interface INewsRestAPI
    {
        [Get("/GetNewsByCategory")]
        Task<List<Article>> GetNewsByCategory([AliasAs("category")]string categoryName);

        [Get("/GetTopNews")]
        Task<List<Article>> GetTopNews();

        [Post("/GetMyNews")]
        Task<List<Article>> GetMyNews([Body]List<string> categories);

        [Get("/GetLocalNews")]
        Task<List<Article>> GetLocalNews([AliasAs("locality")]string locality);
    }
}
