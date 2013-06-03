using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SoundTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Queue<Uri> uriQueue;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string input = InputBox.Text.ToUpper();    //to not worry cases
            string[] words = input.Split(' ');
            uriQueue = new Queue<Uri>(words.Length);

            int success = fillQueue(words);

            if (success == 1)
            {
                playQueue();
            }
        }

        private int fillQueue(string[] words) //fill uriQueue with the appropriate Uri 
        {
            foreach (string word in words)
            {
                if (File.Exists("Assets/" + word + ".wav"))
                {
                    uriQueue.Enqueue(new Uri("Assets/" + word + ".wav", UriKind.Relative));
                }
                else
                {
                    InputBox.Text = "Invalid Input";
                    return 0;
                }
            }
            return 1;
        }

        private void playQueue()
        {
            Console.WriteLine(uriQueue.Peek());
            mediaElement1.Source = uriQueue.Dequeue();
            mediaElement1.Play();
        }

        private void MediaEndedEventHandler(object sender, RoutedEventArgs e)
        {
            if (uriQueue.Count != 0)    //check if all sounds have been played
            {
                playQueue();
            }
            else
            {
                mediaElement1.Close();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string stn = "InputBox2";
            InputBox+"2".Text = "hello";
        }
    }
}
