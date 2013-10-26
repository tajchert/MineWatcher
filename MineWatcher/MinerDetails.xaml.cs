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
using System.IO.IsolatedStorage;
using System.Globalization;
using Newtonsoft.Json;

namespace MineWatcher
{
    public partial class MinerDetails : PhoneApplicationPage
    {
        MinerGeneral general;
        private static string jsonContent;
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
            else
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains("json"))
                {
                    System.Diagnostics.Debug.WriteLine("Reading from a file");
                    jsonContent = IsolatedStorageSettings.ApplicationSettings["json"] as string;
                    if (jsonContent != null && jsonContent.Contains("last_round_start"))
                    {
                        parse_JSON(jsonContent);
                    }
                }
            }
            /*for (int i = 0; i < general.workers.Count; i++) // Loop through List with for
            {
                System.Diagnostics.Debug.WriteLine("Image patch: " + general.workers[i].image);

            }*/
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

        private void parse_JSON(string input)
        {
            RootObject root = JsonConvert.DeserializeObject<RootObject>(input);

            //Round up BTC and show
            general.payout = Convert.ToDouble(root.payout, CultureInfo.InvariantCulture);
            general.payout = Math.Round(general.payout, 5);

            general.balance = Convert.ToDouble(root.balance, CultureInfo.InvariantCulture);
            general.balance = Math.Round(general.balance, 5);
            //Calculate local speed sum
            general.speedSum = 0;
            for (int i = 0; i < root.workers.Count; i++) // Loop through List with for
            {
                System.Diagnostics.Debug.WriteLine("Speed of : " + i + ", is " + root.workers[i].speed);
                general.speedSum = Convert.ToInt32(root.workers[i].speed, CultureInfo.InvariantCulture) + general.speedSum;
            }
            //Set general object fields
            general.workers = root.workers;
            general.block = root.block;
            general.block_all = root.block_all;
            general.round_start_date = root.last_round_start_date;
            //general.round_start_date = DateTime.ParseExact(root.last_round_start_date, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

        }
        /*void Panorama_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Panorama was slided");
        }*/

    }
}