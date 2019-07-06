using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PartlyNewsy.Models
{
    public class NewsCategory : INotifyPropertyChanged
    {
        public string DisplayName { get; set; }
        public string CategoryName { get; set; }
        public string BackgroundImageUrl { get; set; }

        //static readonly string mainUrl = "https://xdsstoragescus.blob.core.windows.net/";
        static readonly string mainUrl = "https://xam-dev-summit-cdn.azureedge.net/";

        bool isFavorite;
        public bool IsFavorite
        {
            get => isFavorite;
            set
            {
                isFavorite = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsFavorite)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static List<NewsCategory> GetDefaultNewsCategories()
        {
            return new List<NewsCategory>
            {
                new NewsCategory { CategoryName = "US", DisplayName = "US News".ToUpper(), BackgroundImageUrl = $"{mainUrl}thumbnails/news.jpg" },
                new NewsCategory { CategoryName = "World", DisplayName = "World".ToUpper(), BackgroundImageUrl = $"{mainUrl}thumbnails/europe.jpg" }
                //new NewsCategory { CategoryName = "Politics", DisplayName = "Politics".ToUpper(), BackgroundImageUrl = $"{mainUrl}thumbnails/politics.jpg" },
                //new NewsCategory { CategoryName = "Business", DisplayName = "Business".ToUpper(), BackgroundImageUrl = $"{mainUrl}thumbnails/business.jpg" },
                //new NewsCategory { CategoryName = "Sports", DisplayName = "Sports".ToUpper(), BackgroundImageUrl = $"{mainUrl}thumbnails/sports.jpg" },
                //new NewsCategory { CategoryName = "Technology", DisplayName = "Technology".ToUpper(), BackgroundImageUrl = $"{mainUrl}thumbnails/technology.jpg" },
                //new NewsCategory { CategoryName = "Entertainment", DisplayName = "Entertainment".ToUpper(), BackgroundImageUrl = $"{mainUrl}thumbnails/entertainment.jpg" },
            };
        }        
    }

    public class AllNewsCategories : List<NewsCategory>
    {
        public static AllNewsCategories GetDefaultCategories { get; }
        //static readonly string mainUrl = "https://xdsstoragescus.blob.core.windows.net/";
        static readonly string mainUrl = "https://xam-dev-summit-cdn.azureedge.net/";

        public AllNewsCategories()
        {
            AddRange(new NewsCategory[] {
                new NewsCategory { CategoryName = "Business", DisplayName = "Business".ToUpper(), BackgroundImageUrl = $"{mainUrl}thumbnails/business.jpg" },
                new NewsCategory { CategoryName = "Entertainment", DisplayName = "Entertainment".ToUpper(), BackgroundImageUrl = $"{mainUrl}thumbnails/entertainment.jpg" },
                new NewsCategory { CategoryName = "Entertainment_MovieAndTV", DisplayName = "Movies and TV".ToUpper(), BackgroundImageUrl = $"{mainUrl}thumbnails/movie.jpg" },
                new NewsCategory { CategoryName = "Entertainment_Music", DisplayName = "Music".ToUpper(), BackgroundImageUrl = $"{mainUrl}thumbnails/music.jpg" },
                new NewsCategory { CategoryName = "Health", DisplayName = "Health".ToUpper(), BackgroundImageUrl = $"{mainUrl}thumbnails/health.jpg" },
                new NewsCategory { CategoryName = "Politics", DisplayName = "Politics".ToUpper(), BackgroundImageUrl = $"{mainUrl}thumbnails/politics.jpg" },
                new NewsCategory { CategoryName = "Products", DisplayName = "Products".ToUpper(), BackgroundImageUrl = $"{mainUrl}thumbnails/toy.jpg" },
                new NewsCategory { CategoryName = "ScienceAndTechnology", DisplayName = "Science and Tech".ToUpper(), BackgroundImageUrl = $"{mainUrl}thumbnails/astronomy.jpg" },
                new NewsCategory { CategoryName = "Technology", DisplayName = "Technology".ToUpper(), BackgroundImageUrl = $"{mainUrl}thumbnails/technology.jpg" },
                new NewsCategory { CategoryName = "Science", DisplayName = "Science".ToUpper(), BackgroundImageUrl = $"{mainUrl}thumbnails/science.jpg" },
                new NewsCategory { CategoryName = "Sports", DisplayName = "Sports".ToUpper(), BackgroundImageUrl = $"{mainUrl}thumbnails/sports.jpg" },
                new NewsCategory { CategoryName = "Sports_Golf", DisplayName = "Golf".ToUpper(), BackgroundImageUrl = $"{mainUrl}thumbnails/golf.jpg" },
                new NewsCategory { CategoryName = "Sports_MLB", DisplayName = "MLB", BackgroundImageUrl = $"{mainUrl}thumbnails/baseball.jpg" },
                new NewsCategory { CategoryName = "Sports_NBA", DisplayName = "NBA", BackgroundImageUrl = $"{mainUrl}thumbnails/basketball.jpg" },
                new NewsCategory { CategoryName = "Sports_NFL", DisplayName = "NFL", BackgroundImageUrl = $"{mainUrl}thumbnails/football.jpg" },
                new NewsCategory { CategoryName = "Sports_NHL", DisplayName = "NHL", BackgroundImageUrl = $"{mainUrl}thumbnails/hockey.jpg" },
                new NewsCategory { CategoryName = "Sports_Soccer", DisplayName = "Soccer".ToUpper(), BackgroundImageUrl = $"{mainUrl}thumbnails/soccer.jpg" },
                new NewsCategory { CategoryName = "Sports_Tennis", DisplayName = "Tennis".ToUpper(), BackgroundImageUrl = $"{mainUrl}thumbnails/tennis.jpg" },
                new NewsCategory { CategoryName = "Sports_CFB", DisplayName = "College Football".ToUpper(), BackgroundImageUrl = $"{mainUrl}thumbnails/cfb.jpg" },
                new NewsCategory { CategoryName = "Sports_CBB", DisplayName = "College Baseball".ToUpper(), BackgroundImageUrl = $"{mainUrl}thumbnails/cbb.jpg" },
                new NewsCategory { CategoryName = "US", DisplayName = "US News".ToUpper(), BackgroundImageUrl = $"{mainUrl}thumbnails/news.jpg" },
                new NewsCategory { CategoryName = "US_Northeast", DisplayName = "US Northeast News".ToUpper(), BackgroundImageUrl = $"{mainUrl}thumbnails/vermont.jpg" },
                new NewsCategory { CategoryName = "US_South", DisplayName = "US South News".ToUpper(), BackgroundImageUrl = $"{mainUrl}thumbnails/georgia.jpg" },
                new NewsCategory { CategoryName = "US_Midwest", DisplayName = "US Midwest News".ToUpper(), BackgroundImageUrl = $"{mainUrl}thumbnails/wisconsin.jpg" },
                new NewsCategory { CategoryName = "US_West", DisplayName = "US West News".ToUpper(), BackgroundImageUrl = $"{mainUrl}thumbnails/oregon.jpg" },
                new NewsCategory { CategoryName = "World", DisplayName = "World".ToUpper(), BackgroundImageUrl = $"{mainUrl}thumbnails/europe.jpg" },
                new NewsCategory { CategoryName = "World_Africa", DisplayName = "Africa".ToUpper(), BackgroundImageUrl = $"{mainUrl}thumbnails/africa.jpg" },
                new NewsCategory { CategoryName = "World_Americas", DisplayName = "Americas".ToUpper(), BackgroundImageUrl = $"{mainUrl}thumbnails/mexico.jpg" },
                new NewsCategory { CategoryName = "World_Asia", DisplayName = "Asia".ToUpper(), BackgroundImageUrl = $"{mainUrl}thumbnails/asia.jpg" },
                new NewsCategory { CategoryName = "World_Europe", DisplayName = "Europe".ToUpper(), BackgroundImageUrl = $"{mainUrl}thumbnails/norway.jpg" },
                new NewsCategory { CategoryName = "World_MiddleEast", DisplayName = "Middle East".ToUpper(), BackgroundImageUrl = $"{mainUrl}thumbnails/middleeast.jpg" }
            });           
        }
    }
}
