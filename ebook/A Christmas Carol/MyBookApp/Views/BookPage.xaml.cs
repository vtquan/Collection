using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using MyBookApp.ViewModels;
using MyBookLibrary.BookReader;
using MyBookLibrary.Core;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace MyBookApp.Views
{
    public sealed partial class BookPage
    {
        public static readonly DependencyProperty ViewModelProperty;

        private static readonly double TextFontSize;
        private static readonly int MaxWheelRotations;

        private WebView _contentView;
        private EPubItem _item;
        private string _itemId;
        private Point? _pressedPosition;

        private bool _ready;

        static BookPage()
        {
            ViewModelProperty = DependencyProperty.Register("ViewModel", typeof (BookViewModel), typeof (BookPage), null);
            TextFontSize = 1.85;
            MaxWheelRotations = 10;
        }

        public BookPage()
        {
            InitializeComponent();

            _contentView = new WebView {Name = "contentView", Visibility = Visibility.Collapsed};

            WebView webView = _contentView;
            webView.ScriptNotify += contentView_ScriptNotify;

            Grid.SetColumn(_contentView, 1);
            ContentGrid.Children.Add(_contentView);

            Unloaded += ReadPage_Unloaded;

            CoreWindow forCurrentThread = CoreWindow.GetForCurrentThread();
            forCurrentThread.KeyDown += ReadPage_KeyDown;
            forCurrentThread.PointerWheelChanged += ReadPage_PointerWheelChanged;

            DisplayProperties.OrientationChanged += DisplayProperties_OrientationChanged;

            ViewModel = new BookViewModel {NumberOfColumns = ComputeNumberOfColumns()};
        }

        private BookViewModel ViewModel
        {
            get { return GetValue(ViewModelProperty) as BookViewModel; }
            set { SetValue(ViewModelProperty, value); }
        }

        private void DisplayProperties_OrientationChanged(Object sender)
        {
            int columns = ComputeNumberOfColumns();

            if (columns != ViewModel.NumberOfColumns)
            {
                ViewModel.NumberOfColumns = columns;

                var str = new string[1];
                str[0] = ViewModel.NumberOfColumns.ToString();
                _contentView.InvokeScript("changeOrientation", str);
            }
        }

        private void bottomAppBar_Closed(Object sender, Object e)
        {
            ShowWebViewDelayed();
        }

        private void bottomAppBar_Opened(Object sender, Object e)
        {
            if (_ready)
            {
                HideWebView();
                return;
            }

            bottomAppBar.IsOpen = false;
        }

        private async void HideWebView()
        {
            var webViewBrush = new WebViewBrush {SourceName = ("contentView")};
            webViewBrush.Redraw();
            WebViewRect.Fill = webViewBrush;

            await Task.Delay(75);

            _contentView.Visibility = Visibility.Collapsed;
        }

        private void btnFontSize_Click(Object sender, RoutedEventArgs e)
        {
            PositionPopup(BtnFontSize, FontSizePopup);

            FontSizeList.SelectionChanged -= fontSizeList_SelectionChanged;
            FontSizeList.SelectedIndex = (int) ApplicationData.Current.LocalSettings.Values["FontSize"];
            FontSizeList.SelectionChanged += fontSizeList_SelectionChanged;

            FontSizePopup.IsOpen = true;
        }

        private void fontSizeList_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["FontSize"] = FontSizeList.SelectedIndex;

            var str = new string[1];
            str[0] = ComputeFontSize(FontSizeList.SelectedIndex).ToString(CultureInfo.InvariantCulture);
            _contentView.InvokeScript("changeFontSize", str);

            FontSizePopup.IsOpen = false;
            bottomAppBar.IsOpen = false;
        }

        private void btnFontStyle_Click(Object sender, RoutedEventArgs e)
        {
            PositionPopup(BtnFontStyle, FontStylePopup);

            FontStylePopup.IsOpen = true;
        }

        private void fontStyleList_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            var listBoxItem = (ListBoxItem) FontStyleList.SelectedItem;

            if (listBoxItem != null)
            {
                if (listBoxItem.Content != null)
                {
                    string fontStyle = listBoxItem.Content.ToString();

                    ApplicationData.Current.LocalSettings.Values["FontStyle"] = fontStyle;

                    var strArrays = new[] {fontStyle};
                    _contentView.InvokeScript("changeFontStyle", strArrays);
                }
            }

            FontStylePopup.IsOpen = false;
            bottomAppBar.IsOpen = false;
        }

        private void btnTableOfContents_Click(Object sender, RoutedEventArgs e)
        {
            TableOfContentsPopup.Height = MainGrid.ActualHeight;
            GridToc.Height = MainGrid.ActualHeight;

            HideWebView();

            var itemsSource = (ObservableCollection<TableOfContensEntry>) TocList.ItemsSource;
            string toc = _contentView.InvokeScript("getTOCInfo", null);

            if (!string.IsNullOrEmpty(toc))
            {
                string[] strArrays = toc.Split('#');

                int num = int.Parse(strArrays[0]);
                int length = -1;

                for (int i = 1; i < strArrays.Length; i++)
                {
                    int num1 = Convert.ToInt32(strArrays[i]);
                    if (length == -1 && num < num1)
                    {
                        length = Math.Max(i - 2, 0);
                    }
                    if (itemsSource != null) itemsSource[i - 1].Section = num1.ToString();
                }

                if (length == -1)
                {
                    length = strArrays.Length - 2;
                }

                TocList.SelectionChanged -= tocList_SelectionChanged;
                TocList.SelectedIndex = length;
                TocList.SelectionChanged += tocList_SelectionChanged;
            }

            CloseBottomAppBar();

            TableOfContentsPopup.IsOpen = true;
        }

        private void btnTheme_Click(Object sender, RoutedEventArgs e)
        {
            ViewModel.CurrentThemeScheme = ViewModel.CurrentThemeScheme != ColorScheme.Light
                                               ? ColorScheme.Light
                                               : ColorScheme.Dark;

            ViewModel.NightMode = !ViewModel.NightMode;

            Color color = ViewModel.ThemeColors.BackgroundColor.Color;
            Color color1 = ViewModel.ThemeColors.ForegroundTextColor.Color;

            var strArrays = new string[2];
            strArrays[0] = string.Format("rgba({0}, {1}, {2}, {3:F2})", color.R, color.G, color.B,
                                         Convert.ToString((Double) color.A/255, CultureInfo.InvariantCulture));
            strArrays[1] = string.Format("rgba({0}, {1}, {2}, {3:F2})", color1.R, color1.G, color1.B,
                                         Convert.ToString((Double) color1.A/255, CultureInfo.InvariantCulture));

            _contentView.InvokeScript("changeColorTheme", strArrays);

            _contentView.Visibility = Visibility.Visible;
            HideWebView();
        }

        private void btnToggleNoOfColumns_Click(Object sender, RoutedEventArgs e)
        {
            ViewModel.NumberOfColumns = ViewModel.NumberOfColumns != 1 ? 1 : 2;
            bottomAppBar.IsOpen = false;

            var str = new string[1];
            str[0] = ViewModel.NumberOfColumns.ToString();
            _contentView.InvokeScript("changeNumberOfColumns", str);
        }

        private void btnToggleSlideMode_Click(Object sender, RoutedEventArgs e)
        {
            bool? isChecked = BtnToggleSlideMode.IsChecked;

            if (isChecked != null) ViewModel.SlidePageAnimation = isChecked.Value;

            var str = new string[1];
            str[0] = BtnToggleSlideMode.IsChecked.ToString();
            _contentView.InvokeScript("toggleSlideAnimation", str);
        }

        private void ChangeHtmlPage(Direction direction, int location)
        {
            Direction direction1 = direction;
            switch (direction1)
            {
                case Direction.Left:
                    {
                        _contentView.InvokeScript("goToNextPage", null);
                    }
                    break;

                case Direction.Right:
                    {
                        _contentView.InvokeScript("goToPrevPage", null);
                    }
                    break;

                case Direction.SkipToStart:
                    {
                        _contentView.InvokeScript("goToFirstPage", null);
                    }
                    break;

                case Direction.SkipToPage:
                    {
                        var str = new[] {location.ToString()};
                        _contentView.InvokeScript("goToPage", str);
                    }
                    break;

                case Direction.SkipToSection:
                    {
                        var strArrays = new[] {location.ToString()};
                        _contentView.InvokeScript("goToSection", strArrays);
                    }
                    break;

                case Direction.SkipToEnd:
                    {
                        _contentView.InvokeScript("goToLastPage", null);
                    }
                    break;
            }
        }

        private void ChangePage(Direction direction, int location = 0)
        {
            ChangeHtmlPage(direction, location);
        }

        private void CloseBottomAppBar()
        {
            bottomAppBar.Closed -= bottomAppBar_Closed;
            bottomAppBar.IsOpen = false;
            bottomAppBar.Closed += bottomAppBar_Closed;
        }

        private Double ComputeFontSize(int selectedFontSizeIndex)
        {
            return TextFontSize*(1 + (Double) (selectedFontSizeIndex - 2)/5);
        }

        private int ComputeNumberOfColumns()
        {
            if (DisplayProperties.CurrentOrientation != DisplayOrientations.Portrait &&
                DisplayProperties.CurrentOrientation != DisplayOrientations.PortraitFlipped)
            {
                if (ApplicationData.Current.LocalSettings.Values["LandscapeNumberOfColumns"] == null)
                {
                    return 2;
                }
                return (int) ApplicationData.Current.LocalSettings.Values["LandscapeNumberOfColumns"];
            }

            if (ApplicationData.Current.LocalSettings.Values["PortraitNumberOfColumns"] == null)
            {
                return 1;
            }

            return (int) ApplicationData.Current.LocalSettings.Values["PortraitNumberOfColumns"];
        }

        private void contentView_ScriptNotify(Object sender, NotifyEventArgs e)
        {
            ProcessHtmlEvent(e.Value);
        }

        private void ReadPage_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (bottomAppBar.IsOpen || !_ready || _contentView.Visibility == Visibility.Collapsed)
            {
                return;
            }

            ProcessKeyDown(args.VirtualKey);
        }

        private void ReadPage_PointerWheelChanged(CoreWindow sender, PointerEventArgs args)
        {
            if (bottomAppBar.IsOpen || !_ready || _contentView.Visibility == Visibility.Collapsed)
            {
                return;
            }

            ProcessMouseWheel(args.CurrentPoint.Properties.MouseWheelDelta);
        }

        private void ReadPage_Unloaded(Object sender, RoutedEventArgs e)
        {
            Unloaded -= ReadPage_Unloaded;
            CoreWindow.GetForCurrentThread().KeyDown -= ReadPage_KeyDown;
            CoreWindow.GetForCurrentThread().PointerWheelChanged -= ReadPage_PointerWheelChanged;

            DisplayProperties.OrientationChanged -= DisplayProperties_OrientationChanged;
            _contentView.ScriptNotify -= contentView_ScriptNotify;

            ContentGrid.Children.Remove(_contentView);

            _contentView = null;
        }

        protected override async void LoadState(Object navigationParameter, Dictionary<string, Object> pageState)
        {
            _itemId = (string) navigationParameter;

            if (ApplicationData.Current.LocalSettings.Values["FontSize"] == null)
            {
                ApplicationData.Current.LocalSettings.Values["FontSize"] = 2;
            }

            if (ApplicationData.Current.LocalSettings.Values["FontStyle"] == null)
            {
                ApplicationData.Current.LocalSettings.Values["FontStyle"] = "Cambria";
            }

            if (ApplicationData.Current.RoamingSettings.Values[_itemId] == null)
            {
                SetCurrentPage(0);
            }

            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(_itemId));

            var builder = new EPubItemBuilder();
            _item = await builder.GetItem(file);

            if (_item == null)
            {
                Frame.GoBack();
            }

            if (_item != null)
            {
                PageTitle.Text = _item.Title;

                string mycontent = PrepareTemplate(HtmlAssets.HtmlTemplate, _item.Title);

                _item.ItemId = _itemId;

                string content = await _item.GetContent(mycontent);

                _contentView.NavigateToString(content);
            }

            await Task.Delay(200);
            _contentView.Visibility = Visibility.Visible;

            bottomAppBar.Closed += bottomAppBar_Closed;

            if (_item != null)
            {
                ObservableCollection<TableOfContensEntry> tableOfContens = await _item.GetTableOfContens();

                if (tableOfContens.Count <= 1)
                {
                    TbNoToc.Visibility = Visibility.Visible;
                    TocList.Visibility = Visibility.Collapsed;
                }
                else
                {
                    TocList.ItemsSource = tableOfContens;
                    TbNoToc.Visibility = Visibility.Collapsed;
                    TocList.Visibility = Visibility.Visible;
                }
            }
        }

        private void PositionPopup(Button btn, Popup popup)
        {
            GeneralTransform visual = btn.TransformToVisual(MainGrid);
            Point point = visual.TransformPoint(new Point(0, 0));

            GeneralTransform generalTransform = bottomAppBar.TransformToVisual(MainGrid);
            Point point1 = generalTransform.TransformPoint(new Point(0, 0));

            popup.VerticalOffset = (point1.Y - popup.ActualHeight);
            popup.HorizontalOffset = (Math.Max(0, point.X - (popup.ActualWidth - btn.ActualWidth)/2));
        }

        private string PrepareTemplate(string template, string title)
        {
            Color backgroundColor = ViewModel.ThemeColors.BackgroundColor.Color;
            Color foregroundColor = ViewModel.ThemeColors.ForegroundTextColor.Color;

            template = template.Replace("[TITLE]", title);

            template = template.Replace("[BACKGROUND_COLOR]",
                                        string.Format("rgba({0}, {1}, {2}, {3:F2})", backgroundColor.R,
                                                      backgroundColor.G, backgroundColor.B,
                                                      Convert.ToString((Double) backgroundColor.A/255,
                                                                       CultureInfo.InvariantCulture)));

            template = template.Replace("[FONT_COLOR]",
                                        string.Format("rgba({0}, {1}, {2}, {3:F2})", foregroundColor.R,
                                                      foregroundColor.G, foregroundColor.B,
                                                      Convert.ToString((Double) foregroundColor.A/255,
                                                                       CultureInfo.InvariantCulture)));

            template = template.Replace("[COLUMN_COUNT]", ViewModel.NumberOfColumns.ToString());

            template = template.Replace("[FONT_SIZE]",
                                        Convert.ToString(
                                            ComputeFontSize(
                                                (int) ApplicationData.Current.LocalSettings.Values["FontSize"]),
                                            CultureInfo.InvariantCulture));

            template = template.Replace("[FONT_STYLE]",
                                        ApplicationData.Current.LocalSettings.Values["FontStyle"].ToString());

            if (!ViewModel.SlidePageAnimation)
            {
                template = template.Replace("[TRANSITION]", "initial");
                template = template.Replace("[TRANSITION_STATUS]", "false");
            }
            else
            {
                template = template.Replace("[TRANSITION]", "left 0.5s cubic-bezier(0, 0, 0, 1)");
                template = template.Replace("[TRANSITION_STATUS]", "true");
            }

            string page;

            if (ApplicationData.Current.RoamingSettings.Values[_itemId] != null)
            {
                ItemPersistedSettings itemPersistedSetting =
                    ItemPersistedSettings.DeserializeItemPersistedSettings(_itemId);

                page = string.Concat("{CurrentSection:", itemPersistedSetting.CurrentSection, ",CurrentPageInSection:",
                                     itemPersistedSetting.CurrentPageInSection, ",PagesInSection:",
                                     itemPersistedSetting.NumberOfPagesInSection, "}");

                if (itemPersistedSetting.CurrentSection == 0 && itemPersistedSetting.CurrentPageInSection == 0)
                {
                    SetCurrentPage(0);
                }
            }
            else
            {
                page = "null";
            }

            template = template.Replace("[START_PAGE]", page);

            template = template.Replace("[TRANSITION_CONFIG]", "left 0.5s cubic-bezier(0, 0, 0, 1)");

            return template;
        }

        private void ProcessBackgroundProcessing()
        {
            PagesPanel.Visibility = Visibility.Collapsed;
        }

        private void ProcessCurrentPageChanged(int page, int section, int currentPageInSection,
                                               int numberOfPagesInSection, int totalNumberOfPages)
        {
            var itemPersistedSetting = new ItemPersistedSettings
                {
                    CurrentPage = page,
                    CurrentSection = section,
                    CurrentPageInSection = currentPageInSection,
                    NumberOfPagesInSection = numberOfPagesInSection,
                    TotalNumberOfPages = totalNumberOfPages
                };

            ItemPersistedSettings.SerializeItemPersistedSettings(itemPersistedSetting, _itemId);

            SetCurrentPage(page);
        }

        private async void ProcessGetSectionContent(int section, int latestSectionContentRequestId)
        {
            var contentForSection = new[] {section.ToString(), null, null};

            contentForSection[1] = await _item.GetContentForSection(section);
            contentForSection[2] = latestSectionContentRequestId.ToString();

            _contentView.InvokeScript("sectionContentReceived", contentForSection);
        }

        private void ProcessGetSectionsInfo()
        {
            var strArrays = new[] {"false"};

            _contentView.InvokeScript("sectionsInfoReceived", strArrays);
        }

        private void ProcessHtmlEvent(string eventData)
        {
            string[] strArrays = eventData.Split('#');

            if (strArrays[0] != null)
            {
                switch (strArrays[0])
                {
                    case "pageReady":
                        {
                            ProcessPageReady();
                            return;
                        }
                    case "pointerdown":
                        {
                            ProcessPointerPressed(new Point(Double.Parse(strArrays[1]), Double.Parse(strArrays[2])));
                            return;
                        }
                    case "pointerup":
                        {
                            ProcessPointerReleased(new Point(Double.Parse(strArrays[1]), Double.Parse(strArrays[2])));
                            return;
                        }
                    case "mousewheel":
                        {
                            ProcessMouseWheel(int.Parse(strArrays[1]));
                            return;
                        }
                    case "keydown":
                        {
                            ProcessKeyDown((VirtualKey) int.Parse(strArrays[1]));
                            return;
                        }
                    case "numberOfPages":
                        {
                            ProcessNumberOfPagesChanged(strArrays[1]);
                            return;
                        }
                    case "currentPage":
                        {
                            ProcessCurrentPageChanged(int.Parse(strArrays[1]), int.Parse(strArrays[2]),
                                                      int.Parse(strArrays[3]), int.Parse(strArrays[4]),
                                                      int.Parse(strArrays[5]));
                            return;
                        }
                    case "numberOfColumnsChanged":
                        {
                            ProcessNumberOfColumnsChanged(int.Parse(strArrays[1]));
                            return;
                        }
                    case "userReadyStateChanged":
                        {
                            ProcessUserReadyStateChanged(bool.Parse(strArrays[1]));
                            return;
                        }
                    case "navigateToExternalPage":
                        {
                            ProcessNavigateToExternalPage(strArrays[1]);
                            return;
                        }
                    case "getSectionContent":
                        {
                            ProcessGetSectionContent(int.Parse(strArrays[1]), int.Parse(strArrays[2]));
                            return;
                        }
                    case "backgroundProcessing":
                        {
                            ProcessBackgroundProcessing();
                            return;
                        }
                    case "locationNotFound":
                        {
                            ProcessLocationNotFound();
                            return;
                        }
                    case "sectionsInfo":
                        {
                            return;
                        }
                    case "getSectionsInfo":
                        {
                            ProcessGetSectionsInfo();
                            break;
                        }
                }
            }
        }

        private void ProcessKeyDown(VirtualKey keyCode)
        {
            switch (keyCode)
            {
                case VirtualKey.Space:
                case VirtualKey.PageDown:
                case VirtualKey.Right:
                case VirtualKey.Down:
                    {
                        ChangePage(Direction.Left);
                        return;
                    }
                case VirtualKey.PageUp:
                case VirtualKey.Left:
                case VirtualKey.Up:
                    {
                        ChangePage(Direction.Right);
                        return;
                    }
                case VirtualKey.End:
                    {
                        ChangePage(Direction.SkipToEnd);
                        return;
                    }
                case VirtualKey.Home:
                    {
                        ChangePage(Direction.SkipToStart);
                        return;
                    }
            }
        }

        private async void ProcessLocationNotFound()
        {
            var messageDialog = new MessageDialog("Unable to find location.");

            messageDialog.Commands.Add(new UICommand("OK"));

            await messageDialog.ShowAsync();
        }

        private void ProcessMouseWheel(int wheelDelta)
        {
            int scroll = Math.Min((int) Math.Abs(Math.Round(Convert.ToDouble(wheelDelta)/120)), MaxWheelRotations);

            if (wheelDelta > 0)
            {
                for (int i = 0; i < scroll; i++)
                {
                    ChangePage(Direction.Right);
                }
            }
            else
            {
                for (int j = 0; j < scroll; j++)
                {
                    ChangePage(Direction.Left);
                }
            }
        }

        private async void ProcessNavigateToExternalPage(string href)
        {
            await Launcher.LaunchUriAsync(new Uri(href));
        }

        private void ProcessNumberOfColumnsChanged(int page)
        {
            SetCurrentPageLabel(page);
        }

        private void ProcessNumberOfPagesChanged(string numberOfPages)
        {
            PagesPanel.Visibility = Visibility.Visible;

            TotalNumberOfPages.Text = numberOfPages;

            CurrentPositionSlider.ValueChanged -= currentPositionSlider_ValueChanged;
            CurrentPositionSlider.Maximum = Double.Parse(numberOfPages) - 1;
            CurrentPositionSlider.ValueChanged += currentPositionSlider_ValueChanged;

            CurrentPositionSlider.StepFrequency = ViewModel.NumberOfColumns;
            CurrentPositionSlider.SmallChange = ViewModel.NumberOfColumns;
        }

        private void ProcessPageReady()
        {
            _contentView.Visibility = Visibility.Visible;

            if (bottomAppBar.IsOpen) bottomAppBar.IsOpen = false;
        }

        private void ProcessPointerPressed(Point position)
        {
            _pressedPosition = position;
        }

        private void ProcessPointerReleased(Point position)
        {
            if (_pressedPosition.HasValue)
            {
                Double x = position.X - _pressedPosition.Value.X;

                if (Math.Abs(x) <= 70)
                {
                    Double actualWidth = _contentView.ActualWidth;

                    if (position.X >= actualWidth*0.49)
                    {
                        if (position.X > actualWidth*0.51)
                        {
                            ChangePage(Direction.Left);
                        }
                    }
                    else
                    {
                        ChangePage(Direction.Right);
                    }
                }
                else
                {
                    ChangePage(x >= 0 ? Direction.Right : Direction.Left);
                }

                _pressedPosition = null;
            }
        }

        private void ProcessUserReadyStateChanged(bool userReady)
        {
            _ready = userReady;
        }

        private void SetCurrentPage(int currentPage)
        {
            Slider slider = CurrentPositionSlider;

            slider.ValueChanged -= currentPositionSlider_ValueChanged;

            if (ViewModel.NumberOfColumns <= 1)
            {
                CurrentPositionSlider.Value = currentPage;
            }
            else
            {
                CurrentPositionSlider.Value = currentPage <
                                              CurrentPositionSlider.Maximum - ViewModel.NumberOfColumns + 1
                                                  ? currentPage
                                                  : CurrentPositionSlider.Maximum;
            }

            slider.ValueChanged += currentPositionSlider_ValueChanged;
            SetCurrentPageLabel(currentPage);
        }

        private void currentPositionSlider_ValueChanged(Object sender, RangeBaseValueChangedEventArgs e)
        {
            ChangePage(Direction.SkipToPage, Convert.ToInt32(e.NewValue));

            ShowWebViewDelayed();
        }

        private void SetCurrentPageLabel(int currentPage)
        {
            int num1 = currentPage + 1;
            int num2 = currentPage + ViewModel.NumberOfColumns;

            if (ViewModel.NumberOfColumns <= 1)
            {
                CurrentPageNumber.Text = num1.ToString();

                return;
            }

            CurrentPageNumber.Text = (string.Concat(num1.ToString(), "-", num2.ToString()));
        }

        private void ShowWebViewDelayed()
        {
            if (_contentView.Visibility == Visibility.Collapsed)
            {
                Task.Run(async () =>
                    {
                        await Task.Delay(300);
                        await
                            Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                                                () => _contentView.Visibility = Visibility.Visible);
                    });
            }
        }

        private void tableOfContentsPopup_Closed(Object sender, Object e)
        {
            ShowWebViewDelayed();
        }

        private void tocList_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            var item = (TableOfContensEntry) e.AddedItems[0];
            var section = new[] {item.Section};

            _contentView.InvokeScript("goToSection", section);

            TableOfContentsPopup.IsOpen = false;
        }
    }
}