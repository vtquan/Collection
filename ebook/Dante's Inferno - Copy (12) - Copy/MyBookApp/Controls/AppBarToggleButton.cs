using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace MyBookApp.Controls
{
    public class AppBarToggleButton : ToggleButton
    {
        public AppBarToggleButton()
        {
            AppBarToggleButton appBarToggleButton = this;

            appBarToggleButton.Click += appBarToggleButton_Click;
        }

        private void appBarToggleButton_Click(object sender, RoutedEventArgs e)
        {
            string str = (IsChecked == true ? "Checked" : "Unchecked");

            VisualStateManager.GoToState(this, str, false);
        }
    }
}