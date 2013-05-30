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

namespace ZeldaSoundBoard
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : ZeldaSoundBoard.Common.LayoutAwarePage
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

        private void SoundB1_Click(object sender, RoutedEventArgs e)
        {
            ZeldaSound.Source = new Uri(this.BaseUri, "ms-appx:///Assets/Sound Effects/bigfall.wav");
        }

        private void SoundB2_Click(object sender, RoutedEventArgs e)
        {
            ZeldaSound.Source = new Uri(this.BaseUri, "ms-appx:///Assets/Sound Effects/bigslash.wav");
        }

        private void SoundB3_Click(object sender, RoutedEventArgs e)
        {
            ZeldaSound.Source = new Uri(this.BaseUri, "ms-appx:///Assets/Sound Effects/heylisten.wav");
        }

        private void SoundB4_Click(object sender, RoutedEventArgs e)
        {
            ZeldaSound.Source = new Uri(this.BaseUri, "ms-appx:///Assets/Sound Effects/hurt.wav");
        }

        private void SoundB5_Click(object sender, RoutedEventArgs e)
        {
            ZeldaSound.Source = new Uri(this.BaseUri, "ms-appx:///Assets/Sound Effects/jump.wav");
        }

        private void SoundB6_Click(object sender, RoutedEventArgs e)
        {
            ZeldaSound.Source = new Uri(this.BaseUri, "ms-appx:///Assets/Sound Effects/longshot.wav");
        }

        private void SoundB7_Click(object sender, RoutedEventArgs e)
        {
            ZeldaSound.Source = new Uri(this.BaseUri, "ms-appx:///Assets/Sound Effects/look!.wav");
        }

        private void SoundB8_Click(object sender, RoutedEventArgs e)
        {
            ZeldaSound.Source = new Uri(this.BaseUri, "ms-appx:///Assets/Sound Effects/mastersword.wav");
        }

        private void SoundB9_Click(object sender, RoutedEventArgs e)
        {
            ZeldaSound.Source = new Uri(this.BaseUri, "ms-appx:///Assets/Sound Effects/navihello.wav");
        }

        private void SoundB10_Click(object sender, RoutedEventArgs e)
        {
            ZeldaSound.Source = new Uri(this.BaseUri, "ms-appx:///Assets/Sound Effects/pullup.wav");
        }

        private void SoundB11_Click(object sender, RoutedEventArgs e)
        {
            ZeldaSound.Source = new Uri(this.BaseUri, "ms-appx:///Assets/Sound Effects/roll.wav");
        }

        private void SoundB12_Click(object sender, RoutedEventArgs e)
        {
            ZeldaSound.Source = new Uri(this.BaseUri, "ms-appx:///Assets/Sound Effects/scream.wav");
        }

        private void SoundB13_Click(object sender, RoutedEventArgs e)
        {
            ZeldaSound.Source = new Uri(this.BaseUri, "ms-appx:///Assets/Sound Effects/spinslash.wav");
        }

        private void SoundB14_Click(object sender, RoutedEventArgs e)
        {
            ZeldaSound.Source = new Uri(this.BaseUri, "ms-appx:///Assets/Sound Effects/triplehit.wav");
        }

        private void SoundB15_Click(object sender, RoutedEventArgs e)
        {
            ZeldaSound.Source = new Uri(this.BaseUri, "ms-appx:///Assets/Sound Effects/wallgrab.wav");
        }

        private void SoundB16_Click(object sender, RoutedEventArgs e)
        {
            ZeldaSound.Source = new Uri(this.BaseUri, "ms-appx:///Assets/Sound Effects/watchout!.wav");
        }
    }
}
