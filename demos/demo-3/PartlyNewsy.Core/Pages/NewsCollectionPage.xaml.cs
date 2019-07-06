using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;
using PartlyNewsy.Core;
using System.Linq;

namespace PartlyNewsy
{
    public partial class NewsCollectionPage : ContentPage
    {
        readonly NewsCollectionViewModel viewModel;
        bool isLoaded = false;

        string categoryName;
        public string CategoryName
        {
            get => categoryName;
            set
            {
                categoryName = value;
                OnPropertyChanged(nameof(CategoryName));
            }
        }        

        public NewsCollectionPage()
        {
            InitializeComponent();

            viewModel = new NewsCollectionViewModel();
            BindingContext = viewModel;

            CategoryName = "";

            //newsList.BeginRefresh();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (!isLoaded)
            {
                if (!string.IsNullOrEmpty(CategoryName))
                    await viewModel.LoadNewsByCategory(CategoryName);
                
                isLoaded = true;
            }
        }

        
    }
}
