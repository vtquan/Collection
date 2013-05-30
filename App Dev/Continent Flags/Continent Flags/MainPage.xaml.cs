using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Continent_Flags
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : Continent_Flags.Common.LayoutAwarePage
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private async void Canvas_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            var messageDialog = new MessageDialog("");
            Canvas a = sender as Canvas;

            messageDialog = new MessageDialog("Antigua & Barbuda");
            messageDialog.Title = "Country";
            await messageDialog.ShowAsync();
        }

        private async void Canvas_Tapped_2(object sender, TappedRoutedEventArgs e)
        {
            var messageDialog = new MessageDialog("");
            Canvas a = sender as Canvas;

            messageDialog = new MessageDialog("Bahamas");
            messageDialog.Title = "Country";
            await messageDialog.ShowAsync();
        }

        private async void Canvas_Tapped_3(object sender, TappedRoutedEventArgs e)
        {
            var messageDialog = new MessageDialog("");
            Canvas a = sender as Canvas;

            messageDialog = new MessageDialog("Barbados");
            messageDialog.Title = "Country";
            await messageDialog.ShowAsync();
        }

        private async void Canvas_Tapped_4(object sender, TappedRoutedEventArgs e)
        {
            var messageDialog = new MessageDialog("");
            Canvas a = sender as Canvas;

            messageDialog = new MessageDialog("Belize");
            messageDialog.Title = "Country";
            await messageDialog.ShowAsync();
        }

        private async void Canvas_Tapped_5(object sender, TappedRoutedEventArgs e)
        {
            var messageDialog = new MessageDialog("");
            Canvas a = sender as Canvas;

            messageDialog = new MessageDialog("Canada");
            messageDialog.Title = "Country";
            await messageDialog.ShowAsync();
        }

        private async void Canvas_Tapped_6(object sender, TappedRoutedEventArgs e)
        {
            var messageDialog = new MessageDialog("");
            Canvas a = sender as Canvas;

            messageDialog = new MessageDialog("Costa Rica");
            messageDialog.Title = "Country";
            await messageDialog.ShowAsync();
        }

        private async void Canvas_Tapped_7(object sender, TappedRoutedEventArgs e)
        {
            var messageDialog = new MessageDialog("");
            Canvas a = sender as Canvas;

            messageDialog = new MessageDialog("Cuba");
            messageDialog.Title = "Country";
            await messageDialog.ShowAsync();
        }

        private async void Canvas_Tapped_8(object sender, TappedRoutedEventArgs e)
        {
            var messageDialog = new MessageDialog("");
            Canvas a = sender as Canvas;

            messageDialog = new MessageDialog("Dominica");
            messageDialog.Title = "Country";
            await messageDialog.ShowAsync();
        }

        private async void Canvas_Tapped_9(object sender, TappedRoutedEventArgs e)
        {
            var messageDialog = new MessageDialog("");
            Canvas a = sender as Canvas;

            messageDialog = new MessageDialog("Dominican Republic");
            messageDialog.Title = "Country";
            await messageDialog.ShowAsync();
        }

        private async void Canvas_Tapped_10(object sender, TappedRoutedEventArgs e)
        {
            var messageDialog = new MessageDialog("");
            Canvas a = sender as Canvas;

            messageDialog = new MessageDialog("El Salvador");
            messageDialog.Title = "Country";
            await messageDialog.ShowAsync();
        }

        private async void Canvas_Tapped_11(object sender, TappedRoutedEventArgs e)
        {
            var messageDialog = new MessageDialog("");
            Canvas a = sender as Canvas;

            messageDialog = new MessageDialog("Grenada");
            messageDialog.Title = "Country";
            await messageDialog.ShowAsync();
        }

        private async void Canvas_Tapped_12(object sender, TappedRoutedEventArgs e)
        {
            var messageDialog = new MessageDialog("");
            Canvas a = sender as Canvas;

            messageDialog = new MessageDialog("Guatemala");
            messageDialog.Title = "Country";
            await messageDialog.ShowAsync();
        }

        private async void Canvas_Tapped_13(object sender, TappedRoutedEventArgs e)
        {
            var messageDialog = new MessageDialog("");
            Canvas a = sender as Canvas;

            messageDialog = new MessageDialog("Haiti");
            messageDialog.Title = "Country";
            await messageDialog.ShowAsync();
        }

        private async void Canvas_Tapped_14(object sender, TappedRoutedEventArgs e)
        {
            var messageDialog = new MessageDialog("");
            Canvas a = sender as Canvas;

            messageDialog = new MessageDialog("Honduras");
            messageDialog.Title = "Country";
            await messageDialog.ShowAsync();
        }

        private async void Canvas_Tapped_15(object sender, TappedRoutedEventArgs e)
        {
            var messageDialog = new MessageDialog("");
            Canvas a = sender as Canvas;

            messageDialog = new MessageDialog("Jamaica");
            messageDialog.Title = "Country";
            await messageDialog.ShowAsync();
        }

        private async void Canvas_Tapped_16(object sender, TappedRoutedEventArgs e)
        {
            var messageDialog = new MessageDialog("");
            Canvas a = sender as Canvas;

            messageDialog = new MessageDialog("Mexico");
            messageDialog.Title = "Country";
            await messageDialog.ShowAsync();
        }

        private async void Canvas_Tapped_17(object sender, TappedRoutedEventArgs e)
        {
            var messageDialog = new MessageDialog("");
            Canvas a = sender as Canvas;

            messageDialog = new MessageDialog("Nicaragua");
            messageDialog.Title = "Country";
            await messageDialog.ShowAsync();
        }

        private async void Canvas_Tapped_18(object sender, TappedRoutedEventArgs e)
        {
            var messageDialog = new MessageDialog("");
            Canvas a = sender as Canvas;

            messageDialog = new MessageDialog("Panama");
            messageDialog.Title = "Country";
            await messageDialog.ShowAsync();
        }

        private async void Canvas_Tapped_19(object sender, TappedRoutedEventArgs e)
        {
            var messageDialog = new MessageDialog("");
            Canvas a = sender as Canvas;

            messageDialog = new MessageDialog("St. Kitts & Nevis");
            messageDialog.Title = "Country";
            await messageDialog.ShowAsync();
        }

        private async void Canvas_Tapped_20(object sender, TappedRoutedEventArgs e)
        {
            var messageDialog = new MessageDialog("");
            Canvas a = sender as Canvas;

            messageDialog = new MessageDialog("St. Lucia");
            messageDialog.Title = "Country";
            await messageDialog.ShowAsync();
        }

        private async void Canvas_Tapped_21(object sender, TappedRoutedEventArgs e)
        {
            var messageDialog = new MessageDialog("");
            Canvas a = sender as Canvas;

            messageDialog = new MessageDialog("St. Vincent & the Grenadines");
            messageDialog.Title = "Country";
            await messageDialog.ShowAsync();
        }

        private async void Canvas_Tapped_22(object sender, TappedRoutedEventArgs e)
        {
            var messageDialog = new MessageDialog("");
            Canvas a = sender as Canvas;

            messageDialog = new MessageDialog("Trinidad & Tobago");
            messageDialog.Title = "Country";
            await messageDialog.ShowAsync();
        }

        private async void Canvas_Tapped_23(object sender, TappedRoutedEventArgs e)
        {
            var messageDialog = new MessageDialog("");
            Canvas a = sender as Canvas;

            messageDialog = new MessageDialog("United States");
            messageDialog.Title = "Country";
            await messageDialog.ShowAsync();
        }
    }
}
