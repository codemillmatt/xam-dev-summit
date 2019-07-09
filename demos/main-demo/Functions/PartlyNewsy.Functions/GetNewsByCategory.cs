using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using Microsoft.Azure.CognitiveServices.Search.NewsSearch;
using Microsoft.Azure.CognitiveServices.Search.NewsSearch.Models;
using Microsoft.Rest;

using PartlyNewsy.Models;
using System.Collections.Generic;
using System.Linq;

namespace PartlyNewsy.Functions
{
    public static class GetNewsByCategory
    {
        [FunctionName("GetNewsByCategory")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            var allTheNewsThatsFitToPrint = new List<PartlyNewsy.Models.Article>();

            string category = req.Query["category"];

            if (string.IsNullOrWhiteSpace(category))
                return new BadRequestResult();

            var creds = new ApiKeyServiceClientCredentials(Environment.GetEnvironmentVariable("NewsSearchKey"));
            var newsClient = new NewsSearchClient(creds)
            {
                Endpoint = Environment.GetEnvironmentVariable("BingNewsSearchEndpointUrl")
            };

            News theNews = null;

            try
            {
                theNews = await newsClient.News.CategoryAsync(category: category);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            try
            {
                int articleCounter = 0;
                foreach (var article in theNews.Value)
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
    }
}
