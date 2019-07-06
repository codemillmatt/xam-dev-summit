using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PartlyNewsy.Models;

namespace PartlyNewsy.Core
{
    public interface IDataService
    {
        Task<List<NewsCategory>> GetInterestCategories();
        Task<List<NewsCategory>> GetAllNewsCategoriesWithUserFavorites();
        Task SaveFavoriteCategory(string categoryName);
        Task DeleteFavoriteCategory(string categoryName);
    }
}
