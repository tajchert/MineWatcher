using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using System.Windows.Media;


namespace MineWatcher
{
    public partial class About : PhoneApplicationPage
    {
        public About()
        {
            InitializeComponent();
            var v = (Visibility)Resources["PhoneLightThemeVisibility"];
            //True for light theme
            if (v == Visibility.Visible)
            {
                /*var imageBrush = new ImageBrush
                 {
                   ImageSource = new BitmapImage(new Uri("/Assets/Servers_wallpapers_8_blackMode.png", UriKind.Relative))
                 };*/
                LayoutRoot.Background = new SolidColorBrush(Colors.White);
            }

        }

        private void MyWebPageButton_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri("http://tajchert.pl/");
            webBrowserTask.Show();
        }

        private void RateMyAppButton_Click(object sender, RoutedEventArgs e)
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
            marketplaceReviewTask.Show();
        }
    }
}