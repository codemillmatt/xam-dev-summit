//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using System.Linq;
//using Xamarin.Forms;
//using PartlyNewsy.Core;
//using Microsoft.Azure.CognitiveServices.Search.NewsSearch;
//using Microsoft.Azure.CognitiveServices.Search.NewsSearch.Models;


//[assembly: Dependency(typeof(BingNewsService))]
//namespace PartlyNewsy.Core
//{
//    public class BingNewsService : INewsService
//    {
//        readonly string bingUrl = "";
//        readonly string bingAPIKey = "";
//        readonly NewsSearchClient searchClient;

//        public BingNewsService()
//        {
//            ApiKeyServiceClientCredentials creds = new ApiKeyServiceClientCredentials(bingAPIKey);

//            searchClient = new NewsSearchClient(creds) { Endpoint = bingUrl };
//        }

//        public async Task<List<PartlyNewsy.Models.Article>> GetLocalNews(string locality)
//        {
//            var theNews = await searchClient.News.SearchAsync(query: locality);
            
//            return TransformNewsToArticles(theNews.Value);
//        }

//        public async Task<List<PartlyNewsy.Models.Article>> GetMyNews(List<string> categories)
//        {
//            var myArticles = new List<PartlyNewsy.Models.Article>();

//            foreach (var category in categories)
//            {
//                myArticles.AddRange(await GetNewsByCategory(category));
//            }

//            return myArticles;
//        }

//        public async Task<List<PartlyNewsy.Models.Article>> GetNewsByCategory(string category)
//        {
//            var categoryNews = await searchClient.News.CategoryAsync(category: category);

//            return TransformNewsToArticles(categoryNews.Value);
//        }

//        public async Task<List<PartlyNewsy.Models.Article>> GetTopNews()
//        {
//            var topNews = await searchClient.News.SearchAsync(query: string.Empty);

//            return TransformNewsToArticles(topNews.Value);
//        }

//        List<PartlyNewsy.Models.Article> TransformNewsToArticles(IList<NewsArticle> news)
//        {
//            var returnArticles = new List<PartlyNewsy.Models.Article>();

//            try
//            {            
//                int counter = 0;

//                foreach (var item in news)
//                {
//                    var article = new PartlyNewsy.Models.Article()
//                    {
//                        ArticleUrl = item.Url,
//                        Category = item.Category,
//                        CurrentArticleCount = counter,
//                        DatePublished = DateTime.Parse(item.DatePublished),
//                        FeaturedImage = item.Image.Thumbnail.ContentUrl.Replace("&pid=news", string.Empty),
//                        Headline = item.Name,
//                        ImageUrl = item.Image.Thumbnail.ContentUrl.Replace("&pid=news", string.Empty),
//                        NewsProviderImageUrl = item.Provider.First().Image.Thumbnail.ContentUrl.Replace("&pid=news", string.Empty),
//                        NewsProviderName = item.Provider.First().Name
//                    };

//                    returnArticles.Add(article);

//                    counter += 1;
//                }
//            }
//            catch (Exception ex)
//            {
//                // just skip any exceptions
//            }

//            return returnArticles;
//        }
//    }
//}
