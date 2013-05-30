using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Find_Dead_Pixels
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : Find_Dead_Pixels.Common.LayoutAwarePage
    {
        SolidColorBrush Red = new SolidColorBrush(Windows.UI.Colors.Red);
        SolidColorBrush Yellow = new SolidColorBrush(Windows.UI.Colors.Yellow);
        SolidColorBrush Green = new SolidColorBrush(Windows.UI.Colors.Green);
        SolidColorBrush Blue = new SolidColorBrush(Windows.UI.Colors.Blue);
        SolidColorBrush Purple = new SolidColorBrush(Windows.UI.Colors.Purple);

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

        private void redB_Click(object sender, RoutedEventArgs e)
        {
            colorCanvas.Background = Red;
            colorCanvas.Visibility = Visibility.Visible;
        }

        private void yellowB_Click(object sender, RoutedEventArgs e)
        {
            colorCanvas.Background = Yellow;
            colorCanvas.Visibility = Visibility.Visible;
        }

        private void greenB_Click(object sender, RoutedEventArgs e)
        {
            colorCanvas.Background = Green;
            colorCanvas.Visibility = Visibility.Visible;
        }

        private void blueB_Click(object sender, RoutedEventArgs e)
        {
            colorCanvas.Background = Blue;
            colorCanvas.Visibility = Visibility.Visible;
        }

        private void purpleB_Click(object sender, RoutedEventArgs e)
        {
            colorCanvas.Background = Purple;
            colorCanvas.Visibility = Visibility.Visible;
        }

        private void Canvas_Tapped(object sender, TappedRoutedEventArgs e)
        {
            colorCanvas.Visibility = Visibility.Collapsed;
        }
    }
}
