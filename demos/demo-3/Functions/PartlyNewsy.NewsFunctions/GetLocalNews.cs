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
    public static class GetLocalNews
    {
        [FunctionName("GetLocalNews")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "news/GetLocalNews")] HttpRequest req,
            ILogger log)
        {            
            string locality = req.Query["locality"];            
            
            var allTheNewsThatsFitToPrint = new List<PartlyNewsy.Models.Article>();            

            var creds = new ApiKeyServiceClientCredentials(Environment.GetEnvironmentVariable("NewsSearchKey"));
            var newsClient = new NewsSearchClient(creds)
            {
                Endpoint = Environment.GetEnvironmentVariable("BingNewsSearchEndpointUrl")
            };
            
            News localNews = null;

            try
            {                                
                localNews = await newsClient.News.SearchAsync(query: locality);                              
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            
            try
            {
                int articleCounter = 0;
                foreach (var article in localNews.Value)
                {                    
                    try
                    {
                        allTheNewsThatsFitToPrint.Add(
                            new PartlyNewsy.Models.Article
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
                    } 
                    catch (NullReferenceException) 
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
