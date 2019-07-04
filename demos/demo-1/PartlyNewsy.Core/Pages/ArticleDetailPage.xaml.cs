using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PartlyNewsy.Core
{
    [QueryProperty("ArticleUrl", "articleUrl")]
    public partial class ArticleDetailPage : ContentPage
    {
        public ArticleDetailPage()
        {
            InitializeComponent();
            BindingContext = this;
        }
        
        string articleUrl;
        public string ArticleUrl
        {
            get => articleUrl;
            set
            {
                articleUrl = Uri.UnescapeDataString(value);
                OnPropertyChanged(nameof(ArticleUrl));
            }
        }
    }
}
