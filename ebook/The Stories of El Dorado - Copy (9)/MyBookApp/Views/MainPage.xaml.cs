using System;
using MyBookApp.Common;
using MyBookApp.Models;
using MyBookApp.ViewModels;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyBookApp.Views
{
    public sealed partial class MainPage : LayoutAwarePage
    {
        public MainPage()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
        }

        private async void gridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var tile = e.ClickedItem as Tile;

            if (tile != null)
            {
                string url = tile.LinkUrl;

                if (!string.IsNullOrWhiteSpace(url))
                {
                    await Launcher.LaunchUriAsync(new Uri(url));
                }
                else
                {
                    Frame.Navigate(typeof (BookPage), Application.Current.Resources["EPubBook"] as string);
                }
            }
        }
    }
}