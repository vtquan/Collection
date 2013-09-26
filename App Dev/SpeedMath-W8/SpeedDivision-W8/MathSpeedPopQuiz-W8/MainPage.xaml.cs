using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

using Problem = MathSpeedPopQuiz_W8.Common.Problem;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace MathSpeedPopQuiz_W8
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : MathSpeedPopQuiz_W8.Common.LayoutAwarePage
    {
        int TIME = 90;

        int MinNum = 0;
        int MaxNum = 99;

        double num1;
        double num2;

        Problem problems;

        private int counter;
        DispatcherTimer timer;

        ObservableCollection<Problem> problemList;

        SolidColorBrush green = new SolidColorBrush(Windows.UI.Colors.Green);
        SolidColorBrush red = new SolidColorBrush(Windows.UI.Colors.Red);
        SolidColorBrush white = new SolidColorBrush(Windows.UI.Colors.White);

        public MainPage()
        {
            this.InitializeComponent();

            problemList = new ObservableCollection<Problem>();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

        async void timer_Tick(object sender, object e)
        {
            await textbox.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () => { textbox.Text = counter.ToString(); ; });
            counter--;
            if (counter == -1)
            {
                timer.Stop();

                ProbGrid.Visibility = Visibility.Collapsed;
                ResultGrid.Visibility = Visibility.Visible;

                this.ResultList.ItemsSource = problemList;
            }
        }

        private void StartB_Click(object sender, RoutedEventArgs e)
        {
            StartGrid.Visibility = Visibility.Collapsed;
            ProbGrid.Visibility = Visibility.Visible;

            Random random = new Random();
            num1 = random.Next(MinNum, MaxNum + 1);
            num2 = random.Next(MinNum, MaxNum + 1);

            problems = new Problem(num1, num2);
            ProbTextBlock.Text = problems.prob;

            counter = TIME;
            timer.Start();
        }

        private void EnterB_Click(object sender, RoutedEventArgs e)
        {
            problems.input = ProbTextBox.Text;
            
            double answer;
            double input;

            Double.TryParse(problems.ans, out answer);
            Double.TryParse(problems.input, out input);

            if (answer <= input+.01)
            {
                if (answer >= input-0.01)
                {
                    problems.correct = true;
                }
            }

            problemList.Add(problems);

            Random random = new Random();
            num1 = random.Next(MinNum, MaxNum + 1);
            num2 = random.Next(MinNum, MaxNum + 1);

            problems = new Problem(num1, num2);
            ProbTextBlock.Text = problems.prob;

            ProbTextBox.Text = "";

            ProbTextBox.Focus(FocusState.Programmatic);
        }

        private void ResetB_Click(object sender, RoutedEventArgs e)
        {
            StartGrid.Visibility = Visibility.Collapsed;
            ResultGrid.Visibility = Visibility.Collapsed;
            ProbGrid.Visibility = Visibility.Visible;

            Random random = new Random();
            num1 = random.Next(MinNum, MaxNum + 1);
            num2 = random.Next(MinNum, MaxNum + 1);

            problems = new Problem(num1, num2);
            ProbTextBlock.Text = problems.prob;

            problemList = new ObservableCollection<Problem>();

            ProbTextBox.Text = "";

            counter = TIME;
            timer.Start();
        }
    }
}
