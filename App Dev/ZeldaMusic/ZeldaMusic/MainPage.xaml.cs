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

namespace ZeldaMusic
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : ZeldaMusic.Common.LayoutAwarePage
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
            ZeldaSound.Source = new Uri(this.BaseUri, "ms-appx:///Assets/Musics/Ocarina_-_Zelda's_Lullaby.mp3");
        }

        private void SoundB2_Click(object sender, RoutedEventArgs e)
        {
            ZeldaSound.Source = new Uri(this.BaseUri, "ms-appx:///Assets/Musics/Ocarina_-_Epona's_Song.mp3");
        }

        private void SoundB3_Click(object sender, RoutedEventArgs e)
        {
            ZeldaSound.Source = new Uri(this.BaseUri, "ms-appx:///Assets/Musics/Ocarina_-_Saria's_Song.mp3");
        }

        private void SoundB4_Click(object sender, RoutedEventArgs e)
        {
            ZeldaSound.Source = new Uri(this.BaseUri, "ms-appx:///Assets/Musics/Ocarina_-_Sun's_Song.mp3");
        }

        private void SoundB5_Click(object sender, RoutedEventArgs e)
        {
            ZeldaSound.Source = new Uri(this.BaseUri, "ms-appx:///Assets/Musics/Ocarina_-_Song_of_Time.mp3");
        }

        private void SoundB6_Click(object sender, RoutedEventArgs e)
        {
            ZeldaSound.Source = new Uri(this.BaseUri, "ms-appx:///Assets/Musics/Ocarina_-_Song_of_Storms.mp3");
        }

        private void SoundB7_Click(object sender, RoutedEventArgs e)
        {
            ZeldaSound.Source = new Uri(this.BaseUri, "ms-appx:///Assets/Musics/Minuet_of_Forest.mp3");
        }

        private void SoundB8_Click(object sender, RoutedEventArgs e)
        {
            ZeldaSound.Source = new Uri(this.BaseUri, "ms-appx:///Assets/Musics/Bolero_of_Fire.mp3");
        }

        private void SoundB9_Click(object sender, RoutedEventArgs e)
        {
            ZeldaSound.Source = new Uri(this.BaseUri, "ms-appx:///Assets/Musics/Serenade_of_Water.mp3");
        }

        private void SoundB10_Click(object sender, RoutedEventArgs e)
        {
            ZeldaSound.Source = new Uri(this.BaseUri, "ms-appx:///Assets/Musics/Nocturne_of_Shadow.mp3");
        }

        private void SoundB11_Click(object sender, RoutedEventArgs e)
        {
            ZeldaSound.Source = new Uri(this.BaseUri, "ms-appx:///Assets/Musics/Requiem_of_Spirit.mp3");
        }

        private void SoundB12_Click(object sender, RoutedEventArgs e)
        {
            ZeldaSound.Source = new Uri(this.BaseUri, "ms-appx:///Assets/Musics/Prelude_of_Light.mp3");
        }

        private void SoundB13_Click(object sender, RoutedEventArgs e)
        {
            ZeldaSound.Source = new Uri(this.BaseUri, "ms-appx:///Assets/Musics/Zelda's_Theme.mp3");
        }

        private void SoundB14_Click(object sender, RoutedEventArgs e)
        {
            ZeldaSound.Source = new Uri(this.BaseUri, "ms-appx:///Assets/Musics/Lon_Lon_Ranch.mp3");
        }

        private void SoundB15_Click(object sender, RoutedEventArgs e)
        {
            ZeldaSound.Source = new Uri(this.BaseUri, "ms-appx:///Assets/Musics/Lost_Woods.mp3");
        }

        private void SoundB16_Click(object sender, RoutedEventArgs e)
        {
            ZeldaSound.Source = new Uri(this.BaseUri, "ms-appx:///Assets/Musics/Temple_of_Time.mp3");
        }

        private void SoundB17_Click(object sender, RoutedEventArgs e)
        {
            ZeldaSound.Source = new Uri(this.BaseUri, "ms-appx:///Assets/Musics/Windmill_Hut.mp3");
        }
    }
}
