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

namespace Subtraction_Pop_Quiz
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : Subtraction_Pop_Quiz.Common.LayoutAwarePage
    {
        int TIME = 60;
        private int counter;
        DispatcherTimer timer;

        TextBlock[,] text;
        TextBox[,] box;
        KeyValuePair<int, int>[,] prob;
        int[,] ans;
        int numCorrect;

        SolidColorBrush green = new SolidColorBrush(Windows.UI.Colors.Green);
        SolidColorBrush red = new SolidColorBrush(Windows.UI.Colors.Red);
        SolidColorBrush white = new SolidColorBrush(Windows.UI.Colors.White);

        public MainPage()
        {
            this.InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;

            setLink();  //link textblock and textbox control to the array

            setup();    //create problem show them on page
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

        void setLink()
        {
            text = new TextBlock[4, 5];
            text[0, 0] = t00;
            text[0, 1] = t01;
            text[0, 2] = t02;
            text[0, 3] = t03;
            text[0, 4] = t04;

            text[1, 0] = t10;
            text[1, 1] = t11;
            text[1, 2] = t12;
            text[1, 3] = t13;
            text[1, 4] = t14;

            text[2, 0] = t20;
            text[2, 1] = t21;
            text[2, 2] = t22;
            text[2, 3] = t23;
            text[2, 4] = t24;

            text[3, 0] = t30;
            text[3, 1] = t31;
            text[3, 2] = t32;
            text[3, 3] = t33;
            text[3, 4] = t34;

            box = new TextBox[4, 5];
            box[0, 0] = b00;
            box[0, 1] = b01;
            box[0, 2] = b02;
            box[0, 3] = b03;
            box[0, 4] = b04;

            box[1, 0] = b10;
            box[1, 1] = b11;
            box[1, 2] = b12;
            box[1, 3] = b13;
            box[1, 4] = b14;

            box[2, 0] = b20;
            box[2, 1] = b21;
            box[2, 2] = b22;
            box[2, 3] = b23;
            box[2, 4] = b24;

            box[3, 0] = b30;
            box[3, 1] = b31;
            box[3, 2] = b32;
            box[3, 3] = b33;
            box[3, 4] = b34;

            prob = new KeyValuePair<int, int>[4, 5];

            ans = new int[4, 5];
        }

        void setup()
        {
            Random random = new Random();

            int MAX = 12;
            int MIN = -12;

            numCorrect = 0;

            int num1;
            int num2;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    num1 = random.Next(MIN, MAX + 1);
                    num2 = random.Next(MIN, MAX + 1);
                    prob[i, j] = new KeyValuePair<int, int>(num1, num2);
                    ans[i, j] = num1 - num2;
                    text[i, j].Text = "(" + prob[i, j].Key.ToString() + ")" + " - " + "(" + prob[i, j].Value.ToString() + ")";
                }
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    box[i, j].Background = white;
                    box[i, j].Text = "";
                }
            }

            counter = TIME;
            timer.Start();
        }

        async void timer_Tick(object sender, object e)
        {
            await textbox.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () => { textbox.Text = counter.ToString(); ; });
            counter--;
            if (counter == -1)
            {
                timer.Stop();
                checkAns();
            }
        }

        async void checkAns()
        {
            var messageDialog = new MessageDialog("");
            int result;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    try
                    {
                        result = Convert.ToInt32(box[i, j].Text);
                        if (result == ans[i, j])
                        {
                            box[i, j].Background = green;
                            numCorrect++;
                        }
                        else
                        {
                            box[i, j].Background = red;
                            box[i, j].Text = "Ans is: " + ans[i, j];
                        }
                    }
                    catch (OverflowException)
                    {
                        box[i, j].Background = red;
                        box[i, j].Text = "Ans is: " + ans[i, j];
                    }
                    catch (FormatException)
                    {
                        box[i, j].Background = red;
                        box[i, j].Text = "Ans is: " + ans[i, j];
                    }
                }
            }

            messageDialog = new MessageDialog("You got " + numCorrect + " questions right out of 20");
            messageDialog.Title = "Result";

            messageDialog.Commands.Add(new UICommand(
            "Retry",
            new UICommandInvokedHandler(this.CommandInvokedHandler)));

            messageDialog.Commands.Add(new UICommand(
            "Close"));

            // Set the command that will be invoked by default
            messageDialog.DefaultCommandIndex = 1;

            // Set the command to be invoked when escape is pressed
            messageDialog.CancelCommandIndex = 1;

            // Show the message dialog and wait
            await messageDialog.ShowAsync();
        }

        private void CommandInvokedHandler(IUICommand command)  //clear action log and reset hp, mp and music
        {
            setup();
        }

        private void ResetB_Click(object sender, RoutedEventArgs e)
        {
            setup();
        }
    }
}
