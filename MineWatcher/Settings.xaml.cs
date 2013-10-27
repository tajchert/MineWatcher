using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using System.Globalization;
using System.Windows.Media;

namespace MineWatcher
{
    public partial class SettingData : PhoneApplicationPage
    {
        public SettingData()
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
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (IsolatedStorageSettings.ApplicationSettings.Contains("token"))
            {
                tokenInput.Text =
                IsolatedStorageSettings.ApplicationSettings["token"] as string;
            }
            if (IsolatedStorageSettings.ApplicationSettings.Contains("time"))
            {
                string tmpVal = IsolatedStorageSettings.ApplicationSettings["time"] as string;
                System.Diagnostics.Debug.WriteLine("Timer saved: " + tmpVal);
                sliderInput.Value = Convert.ToInt32(tmpVal, CultureInfo.InvariantCulture);
                System.Diagnostics.Debug.WriteLine("Timer saved: " + tmpVal);
                //sliderValue.Text = tmpVal;
            }
        }
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (!settings.Contains("token"))
            {
                settings.Add("token", tokenInput.Text);
            }
            else
            {
                settings["token"] = tokenInput.Text;
            }
            settings.Save();
            if (!settings.Contains("time"))
            {
                settings.Add("time", Math.Round(sliderInput.Value, 0) + "");
            }
            else
            {
                settings["time"] = Math.Round(sliderInput.Value, 0) + "";
            }
            settings.Save();
            base.OnBackKeyPress(e);
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (!settings.Contains("token"))
            {
                settings.Add("token", tokenInput.Text);
            }
            else
            {
                settings["token"] = tokenInput.Text;
            }
            settings.Save();
            if (!settings.Contains("time"))
            {
                settings.Add("time", Math.Round(sliderInput.Value, 0) + "");
            }
            else
            {
                settings["time"] = Math.Round(sliderInput.Value, 0) + "";
            }
            settings.Save();
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void sliderInput_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sliderInput != null && sliderInput.Value > 15 && sliderInput.Value < 120)
            {
                sliderValue.Text = Math.Round(sliderInput.Value, 0) + "";
            }
            
        }
    }
}