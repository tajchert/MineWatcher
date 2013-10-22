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
using Microsoft.Phone.Shell;
using System.Windows.Navigation;

namespace MineWatcher
{
    public partial class MinerDetails : PhoneApplicationPage
    {
        MinerGeneral general;
        public MinerDetails()
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
                panoramaRoot.Background = new SolidColorBrush(Colors.White);
            }

        }

        /*
         * LinearGradientBrush gradient = new LinearGradientBrush();
            gradient.StartPoint = new Point(0.5, 0);
            gradient.EndPoint = new Point(0.5, 1);

            GradientStop color2 = new GradientStop();
            color2.Color = Colors.Transparent;
            color2.Offset = 0.5;
            gradient.GradientStops.Add(color2);

            GradientStop color1 = new GradientStop();
            color1.Color = Colors.Yellow;
            color1.Offset = 0.7;
            gradient.GradientStops.Add(color1);

                
            panoramaRoot.Background = gradient;
         */
        private void Panorama_Loaded(object sender, RoutedEventArgs e)
        {
            //panoramaRoot.DefaultItem = (PanoramaItem)panoramaRoot.Items[1];
        }
        private static PhoneApplicationService appService = PhoneApplicationService.Current;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            general = default(MinerGeneral);

            System.Diagnostics.Debug.WriteLine("Trying to get general.");
            if (appService.State.ContainsKey("general"))
            {
                general = (MinerGeneral)appService.State["general"];
                System.Diagnostics.Debug.WriteLine("General: " + general.speedSum);
            }
            for (int i = 0; i < general.workers.Count; i++) // Loop through List with for
            {
                System.Diagnostics.Debug.WriteLine("Image patch: " + general.workers[i].image);

            }
            panoramaRoot.ItemsSource = general.workers;

            /*if (appService.State.ContainsKey("minerNumber"))
            {
                string tmp =(string) appService.State["minerNumber"];
                //minerNumber = (int) appService.State["minerNumber"];
                System.Diagnostics.Debug.WriteLine("minerNumber: " + tmp);
                //panoramaRoot.DefaultItem = (PanoramaItem)panoramaRoot.Items[minerNumber];
            }*/
            //Getting number of miner to which navigate to.
            string minerTmp;
            if (NavigationContext.QueryString.TryGetValue("minerNumber", out minerTmp))
            {
                int minerNumber = Convert.ToInt16(minerTmp);
                System.Diagnostics.Debug.WriteLine("Received miner number: " + minerNumber);
                panoramaRoot.DefaultItem = panoramaRoot.Items[minerNumber];
                //panoramaRoot.DefaultItem = (PanoramaItem)panoramaRoot.Items[2];
            }
            System.Diagnostics.Debug.WriteLine("Received miner number: " + panoramaRoot.HeaderTemplate);




        }
        /*void Panorama_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Panorama was slided");
        }*/

    }
}