using Windows.UI;
using Windows.UI.Xaml.Media;

namespace MyBookLibrary.Core
{
    public class ColorThemeModel
    {
        public ColorThemeModel()
        {
            CurrentColorScheme = ColorScheme.Dark;
        }

        public SolidColorBrush BackgroundColor
        {
            get
            {
                if (CurrentColorScheme == ColorScheme.Light)
                {
                    return BackgroundColorLight;
                }

                return new SolidColorBrush(Color.FromArgb(255, 29, 29, 29));
            }
        }

        public SolidColorBrush BackgroundColorLight
        {
            get { return new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)); }
        }

        public SolidColorBrush BackgroundHoverColor
        {
            get
            {
                if (CurrentColorScheme == ColorScheme.Light)
                {
                    return new SolidColorBrush(Color.FromArgb(61, 0, 0, 0));
                }

                return BackgroundColorLight;
            }
        }

        public ColorScheme CurrentColorScheme { get; set; }

        public SolidColorBrush ForegroundColor
        {
            get
            {
                if (CurrentColorScheme == ColorScheme.Light)
                {
                    return ForegroundColorLight;
                }

                return BackgroundColorLight;
            }
        }

        public SolidColorBrush ForegroundColorLight
        {
            get { return new SolidColorBrush(Color.FromArgb(255, 0, 0, 0)); }
        }

        public SolidColorBrush ForegroundTextColor
        {
            get
            {
                if (CurrentColorScheme == ColorScheme.Light)
                {
                    return ForegroundColorLight;
                }

                return BackgroundColorLight;
            }
        }
    }
}