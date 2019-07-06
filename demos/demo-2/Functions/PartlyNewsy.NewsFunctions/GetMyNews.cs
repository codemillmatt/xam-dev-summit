using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Azure.CognitiveServices.Search.NewsSearch;
using Microsoft.Azure.CognitiveServices.Search.NewsSearch.Models;

namespace PartlyNewsy.Functions
{
    public static class GetMyNews
    {
        [FunctionName("GetMyNews")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get","post", Route = null)] HttpRequest req,
            ILogger log)
        {         
            var allTheNewsThatsFitToPrint = new List<PartlyNewsy.Models.Article>();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var favoriteCategories = JsonConvert.DeserializeObject<List<string>>(requestBody);

            var creds = new ApiKeyServiceClientCredentials(Environment.GetEnvironmentVariable("NewsSearchKey"));
            var newsClient = new NewsSearchClient(creds)
            {
                Endpoint = Environment.GetEnvironmentVariable("BingNewsSearchEndpointUrl")
            };

            List<News> theNews = new List<News>();

            try
            {
                foreach (var category in favoriteCategories)
                {
                    var newsForCategory = await newsClient.News.CategoryAsync(category: category);

                    theNews.Add(newsForCategory);
                }                
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            try
            {
                int articleCounter = 0;
                List<NewsArticle> newsToLoop = new List<NewsArticle>();

                foreach (var newsItem in theNews)
                {
                    newsToLoop.AddRange(newsItem.Value.Take(2));
                }

                var shuffled = ShuffleList(newsToLoop.ToList());

                foreach (var article in shuffled)
                {
                    try
                    {
                        allTheNewsThatsFitToPrint.Add(
                            new Models.Article
                            {
                                FeaturedImage = article.Image.Thumbnail.ContentUrl.Replace("&pid=News", string.Empty),
                                ImageUrl = article.Image.Thumbnail.ContentUrl.Replace("&pid=News", string.Empty),
                                NewsProviderName = article.Provider.FirstOrDefault()?.Name,
                                Headline = article.Name,
                                NewsProviderImageUrl = article.Provider.FirstOrDefault()?.Image?.Thumbnail?.ContentUrl.Replace("&pid=News", String.Empty),
                                Category = article.Category, 
                                DatePublished = DateTime.Parse(article.DatePublished),
                                CurrentArticleCount = articleCounter,
                                ArticleUrl = article.Url
                            }
                        );

                        articleCounter += 1;  
                    } catch (NullReferenceException) 
                    {
                        // just skip this item if we run into a null reference
                    }  
                }                
            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return new OkObjectResult(allTheNewsThatsFitToPrint);            
        }
    
        private static List<E> ShuffleList<E>(List<E> inputList)
        {
            List<E> randomList = new List<E>();

            Random r = new Random();
            int randomIndex = 0;
            while (inputList.Count > 0)
            {
                randomIndex = r.Next(0, inputList.Count); //Choose a random object in the list
                randomList.Add(inputList[randomIndex]); //add it to the new, random list
                inputList.RemoveAt(randomIndex); //remove to avoid duplicates
            }

            return randomList; //return the new random list
        }
    
    }
}
