using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PartlyNewsy
{
    public partial class ExploreCategoryTile : Frame
    {
        public static readonly BindableProperty HeadlineProperty = BindableProperty.Create(nameof(Headline), typeof(string), typeof(ExploreCategoryTile));

        public string Headline
        {
            get
            {
                return (string)GetValue(HeadlineProperty);
            }
            set
            {
                SetValue(HeadlineProperty, value);
            }
        }

        public static readonly BindableProperty FeaturedImageProperty = BindableProperty.Create(nameof(FeaturedImage), typeof(string), typeof(ExploreCategoryTile));

        public string FeaturedImage
        {
            get
            {
                return (string)GetValue(FeaturedImageProperty);
            }
            set
            {
                SetValue(FeaturedImageProperty, value);
            }
        }

        public ExploreCategoryTile()
        {
            InitializeComponent();
        }
    }
}
