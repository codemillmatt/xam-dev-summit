using System;
using System.Collections.ObjectModel;
using MvvmHelpers;
using PartlyNewsy.Models;

namespace PartlyNewsy.Core
{
    public class SearchPageViewModel : BaseViewModel
    {
        public SearchPageViewModel()
        {
            NewsCategories = new ObservableRangeCollection<NewsCategory>(new AllNewsCategories());
        }

        ObservableRangeCollection<NewsCategory> newsCategories;
        public ObservableRangeCollection<NewsCategory> NewsCategories
        {
            get => newsCategories;
            set => SetProperty(ref newsCategories, value);
        }
    }
}
