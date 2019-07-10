using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Documents;
using PartlyNewsy.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using PartlyNewsy.Core;


[assembly: Dependency(typeof(CosmosDataService))]
namespace PartlyNewsy.Core
{
    public class CosmosDataService : IDataService
    {
        IAuthenticationService authService;

        DocumentClient docClient;

        readonly string dbName = "user-preferences";
        readonly string collectionName = "Info";

        public CosmosDataService()
        {
            authService = DependencyService.Get<IAuthenticationService>();

            docClient = new DocumentClient(new Uri(Constants.CosmosEndpoint), Constants.CosmosApiKey);
        }

        public async Task<List<NewsCategory>> GetAllNewsCategoriesWithUserFavorites()
        {
            // This is meant to populate the grid (including the check marks)

            if (!authService.IsLoggedIn())
                return new List<NewsCategory>();

            var allCategories = new List<NewsCategory>(new AllNewsCategories());
            var savedInterests = await GetSavedUserInterests();

            foreach (var item in allCategories)
            {
                if (savedInterests.Any(si => si.CategoryName == item.CategoryName))
                    item.IsFavorite = true;
            }

            return allCategories;
        }

        public async Task<List<NewsCategory>> GetInterestCategories()
        {
            // This is meant to populate the top row of tabs
            if (!authService.IsLoggedIn())
            {
                return NewsCategory.GetDefaultNewsCategories();
            }
            else
            {
                var savedInterests = await GetSavedUserInterests();

                if (savedInterests.Count > 0)
                    return savedInterests;
                else
                    return NewsCategory.GetDefaultNewsCategories();
            }
        }

        async Task<List<NewsCategory>> GetSavedUserInterests()
        {
            List<NewsCategory> usersFavoriteNewsCategories = new List<NewsCategory>();

            var userInterests = await GetUserInterestsFromCosmos();

            // Now loop through all the user interests
            // finding the match inside of the AllNewsCategories mark it as a fave
            // then add that fave to the return list
            var allNewsCategories = new AllNewsCategories();
            foreach (var item in userInterests)
            {
                var newsCategory = allNewsCategories.FirstOrDefault(nc => nc.CategoryName == item.NewsCategoryName);

                newsCategory.IsFavorite = true;

                usersFavoriteNewsCategories.Add(newsCategory);
            }

            return usersFavoriteNewsCategories;
        }

        async Task<List<UserInterest>> GetUserInterestsFromCosmos()
        {
            // First get all the saved user interests from cosmos
            var accountId = Preferences.Get(Constants.PreferencesAccountKey, "");

            var categoryFaveQuery = docClient.CreateDocumentQuery<UserInterestDocumentDb>(
                                        UriFactory.CreateDocumentCollectionUri(dbName, collectionName))
                                    .Where(d => d.PartitionKey == $"user-{accountId}")
                                    .AsDocumentQuery();

            var userIntersts = new List<UserInterest>();

            while (categoryFaveQuery.HasMoreResults)
            {
                var interests = await categoryFaveQuery.ExecuteNextAsync<UserInterestDocumentDb>();

                userIntersts.AddRange(interests.Select(ui => ui.Document));
            }

            return userIntersts;
        }

        public async Task SaveFavoriteCategory(string categoryName)
        {
            var accountId = Preferences.Get(Constants.PreferencesAccountKey, string.Empty);

            if (string.IsNullOrEmpty(accountId))
                return;

            var docToInsert = new UserInterestDocumentDb
            {
                id = $"{categoryName}-{accountId}",
                PartitionKey = $"user-{accountId}",
                Document = new UserInterest { NewsCategoryName = categoryName }
            };

            var collectionUri = UriFactory.CreateDocumentCollectionUri(dbName, collectionName);

            var partitionKey = new PartitionKey($"user-{accountId}");
            var reqOptions = new RequestOptions { PartitionKey = partitionKey };

            await docClient.CreateDocumentAsync(collectionUri, docToInsert, reqOptions);
        }

        public async Task DeleteFavoriteCategory(string categoryName)
        {
            var accountId = Preferences.Get(Constants.PreferencesAccountKey, string.Empty);

            if (string.IsNullOrEmpty(accountId))
                return;

            var docId = $"{categoryName}-{accountId}";

            var docUri = UriFactory.CreateDocumentUri(dbName, collectionName, docId);

            var partitionKey = new PartitionKey($"user-{accountId}");

            var reqOptions = new RequestOptions { PartitionKey = partitionKey };

            await docClient.DeleteDocumentAsync(docUri, reqOptions);
        }
    }
}
