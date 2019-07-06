using System;
using System.Threading.Tasks;
using MvvmHelpers;
using PartlyNewsy.Models;
using System.Collections.Generic;
using Microsoft.AppCenter.Crashes;
using System.Windows.Input;
using Xamarin.Forms;


namespace PartlyNewsy.Core
{
    public class MyInterestsViewModel : BaseViewModel
    {        
        IDataService dataService;

        public ICommand SetFavoriteCommand { get; }
        public MyInterestsViewModel()
        {            
            SetFavoriteCommand = new Command(async () => await ExecuteSetFavoriteCommand());
            
            dataService = DependencyService.Get<IDataService>(DependencyFetchTarget.GlobalInstance);
        }

        ObservableRangeCollection<NewsCategory> newsCategories;
        public ObservableRangeCollection<NewsCategory> NewsCategories
        {
            get => newsCategories;
            set => SetProperty(ref newsCategories, value);
        }

        NewsCategory selectedCategory;
        public NewsCategory SelectedCategory
        {
            get => selectedCategory;
            set => SetProperty(ref selectedCategory, value);
        }

        async Task ExecuteSetFavoriteCommand()
        {
            if (SelectedCategory.IsFavorite)
                await dataService.DeleteFavoriteCategory(SelectedCategory.CategoryName);
            else
                await dataService.SaveFavoriteCategory(SelectedCategory.CategoryName);

            SelectedCategory.IsFavorite = !SelectedCategory.IsFavorite;

            if (SelectedCategory.IsFavorite)
            {
                var interestNames = new List<InterestSubMessage>();
                interestNames.Add(new InterestSubMessage { Category = SelectedCategory.CategoryName, Title = SelectedCategory.DisplayName });

                var aitm = new AddInterestTabMessage { InterestNames = interestNames };

                MessagingCenter.Send(aitm, AddInterestTabMessage.AddTabMessage);
            }
        }

        public async Task GetUserInterests()
        {
            try
            {
                var catsWithFaves = await dataService.GetAllNewsCategoriesWithUserFavorites();

                NewsCategories = new ObservableRangeCollection<NewsCategory>(catsWithFaves);

                //NewsCategories.AddRange(catsWithFaves);
                //OnPropertyChanged(nameof(NewsCategories));
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);                
            }
        }
    }
}
