using System;
using System.Collections.Generic;
using PartlyNewsy.Core;
using PartlyNewsy.Models;
using Xamarin.Forms;
using System.Linq;
using MvvmHelpers;

namespace PartlyNewsy
{
    public partial class SearchPage : ContentPage
    {
        SearchPageViewModel vm;
        NewsCategorySearchHandler searchHandler;
        public SearchPage()
        {
            InitializeComponent();

            vm = new SearchPageViewModel();
            BindingContext = vm;

            searchHandler = new NewsCategorySearchHandler();

            searchHandler.SearchQueryEvent += (obj, input) => {
                if (!string.IsNullOrWhiteSpace(input))
                {
                    try
                    {
                        // filter down the binding context
                        var filteredCategories = new AllNewsCategories()
                            .Where(cat => cat.DisplayName.Contains(input.ToUpper())).ToList();

                        vm.NewsCategories.Clear();
                        vm.NewsCategories = new ObservableRangeCollection<NewsCategory>(filteredCategories);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"***** {ex} *****");
                    }
                }
                else
                {
                    vm.NewsCategories = new ObservableRangeCollection<NewsCategory>(new AllNewsCategories());
                }
            };

            Shell.SetSearchHandler(this, searchHandler);
        }
    }

    public class NewsCategorySearchHandler : SearchHandler
    {
        public event EventHandler<string> SearchQueryEvent;

        public NewsCategorySearchHandler()
        {
            SearchBoxVisibility = SearchBoxVisibility.Expanded;
            IsSearchEnabled = true;
            ShowsResults = false;
            Placeholder = "Search";
        }

        void OnSearchQueryChanged(string newInput)
        {
            SearchQueryEvent?.Invoke(this, newInput);
        }

        protected override void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);

            OnSearchQueryChanged(newValue);

//            if (string.IsNullOrEmpty(newValue))
//            {
//                ItemsSource = null;
//            }
//            else
//            {
//                var results = new List<string>();
//                var overall = new List<NewsCategory>(new AllNewsCategories());

//                results = overall.Where(x => x.DisplayName
//                    .IndexOf(newValue, StringComparison.InvariantCultureIgnoreCase) > -1)
//                    .Select(x => x.DisplayName)
//                    .ToList();

//u                ItemsSource = results;
//            }
        }
    }
}
