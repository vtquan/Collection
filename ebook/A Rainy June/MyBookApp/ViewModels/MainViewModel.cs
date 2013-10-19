using System.Collections.Generic;
using MyBookApp.Models;
using Windows.UI.Xaml;

namespace MyBookApp.ViewModels
{
    internal class MainViewModel
    {
        public MainViewModel()
        {
            var book = Application.Current.Resources["AppName"] as string;
            var image = Application.Current.Resources["CoverImage"] as string;

            Groups = new List<MenuGroup>
                {
                    new MenuGroup
                        {
                            Title = "Read this book",
                            Items = new List<MenuItem>
                                {
                                    new Tile {Name = book, ImageUrl = image, RowSpan = 3},
                                }
                        },
                };
        }

        public List<MenuGroup> Groups { get; set; }
    }
}