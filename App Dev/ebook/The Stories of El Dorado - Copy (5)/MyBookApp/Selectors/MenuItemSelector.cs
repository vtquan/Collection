using MyBookApp.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyBookApp.Selectors
{
    internal class MenuItemSelector : DataTemplateSelector
    {
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var tile = item as Tile;

            if (tile != null) VariableSizedWrapGrid.SetRowSpan(container as UIElement, tile.RowSpan);

            return Application.Current.Resources["TileDataTemplate"] as DataTemplate;
        }
    }
}