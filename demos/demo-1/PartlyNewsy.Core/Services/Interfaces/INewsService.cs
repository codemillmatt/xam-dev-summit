using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PartlyNewsy.Models;

namespace PartlyNewsy.Core
{
    public interface INewsService
    {
        Task<List<Article>> GetNewsByCategory(string category);
        Task<List<Article>> GetTopNews();
        Task<List<Article>> GetMyNews(List<string> categories);
        Task<List<Article>> GetLocalNews(string locality);
    }
}
