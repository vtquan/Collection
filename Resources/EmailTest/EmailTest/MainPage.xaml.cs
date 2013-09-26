using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace EmailTest
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EmailComposeTask emailcomposer = new EmailComposeTask();
            emailcomposer.To = "chuon-chuon@hotmail.com";
            emailcomposer.Subject = "subject from test app";
            emailcomposer.Body = "This is a test mail from Email Composer";
            emailcomposer.Show();
        }
    }
}