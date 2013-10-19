using System.ComponentModel;
using MyBookLibrary.Core;
using Windows.Graphics.Display;
using Windows.Storage;

namespace MyBookApp.ViewModels
{
    public class BookViewModel : INotifyPropertyChanged
    {
        private readonly ColorThemeModel _themeColors = new ColorThemeModel();
        private bool _isShowingTwoColumns;
        private bool? _nightMode;
        private int _numberOfColumns = -1;
        private bool? _slidePageAnimation;

        public BookViewModel()
        {
            _themeColors.CurrentColorScheme = (NightMode ? ColorScheme.Dark : ColorScheme.Light);
        }

        public ColorScheme CurrentThemeScheme
        {
            get { return ThemeColors.CurrentColorScheme; }
            set
            {
                ThemeColors.CurrentColorScheme = value;
                RaisePropertyChanged("ThemeColors");
            }
        }

        public bool IsShowingTwoColumns
        {
            get { return _isShowingTwoColumns; }
            set
            {
                _isShowingTwoColumns = value;
                RaisePropertyChanged("IsShowingTwoColumns");
            }
        }

        public bool NightMode
        {
            get
            {
                if (!_nightMode.HasValue)
                {
                    if (ApplicationData.Current.RoamingSettings.Values["NightMode"] != null)
                    {
                        _nightMode = (bool) ApplicationData.Current.RoamingSettings.Values["NightMode"];
                    }
                    else
                    {
                        _nightMode = false;
                    }
                }
                return _nightMode.Value;
            }
            set
            {
                ApplicationData.Current.RoamingSettings.Values["NightMode"] = _nightMode;
                RaisePropertyChanged("NightMode");
                RaisePropertyChanged("NightModeBtnText");
            }
        }

        public string NightModeBtnText
        {
            get
            {
                if (NightMode)
                {
                    return "Night On";
                }
                return "Night Off";
            }
        }

        public int NumberOfColumns
        {
            get { return _numberOfColumns; }
            set
            {
                _numberOfColumns = value;
                if (DisplayProperties.CurrentOrientation == DisplayOrientations.Portrait ||
                    DisplayProperties.CurrentOrientation == DisplayOrientations.PortraitFlipped)
                {
                    ApplicationData.Current.LocalSettings.Values["PortraitNumberOfColumns"] = _numberOfColumns;
                }
                else
                {
                    ApplicationData.Current.LocalSettings.Values["LandscapeNumberOfColumns"] = _numberOfColumns;
                }
                IsShowingTwoColumns = _numberOfColumns == 2;
            }
        }

        public string SlideAnimationBtnText
        {
            get
            {
                if (SlidePageAnimation)
                {
                    return "Animation On";
                }
                return "Animation Off";
            }
        }

        public bool SlidePageAnimation
        {
            get
            {
                if (!_slidePageAnimation.HasValue)
                {
                    if (ApplicationData.Current.RoamingSettings.Values["SlideAnimation"] != null)
                    {
                        _slidePageAnimation = (bool) ApplicationData.Current.RoamingSettings.Values["SlideAnimation"];
                    }
                    else
                    {
                        _slidePageAnimation = true;
                    }
                }
                return _slidePageAnimation.Value;
            }
            set
            {
                _slidePageAnimation = value;
                ApplicationData.Current.RoamingSettings.Values["SlideAnimation"] = value;
                RaisePropertyChanged("SlidePageAnimation");
                RaisePropertyChanged("SlideAnimationBtnText");
            }
        }

        public ColorThemeModel ThemeColors
        {
            get { return _themeColors; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}