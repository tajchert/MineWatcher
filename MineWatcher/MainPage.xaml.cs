using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections;
using System.Globalization;
using System.IO.IsolatedStorage;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using Microsoft.Phone.Tasks;
using System.Net.Http;
using Microsoft.Phone.Net.NetworkInformation;

namespace MineWatcher
{
    
    public partial class MainPage : PhoneApplicationPage
    {
        MinerGeneral general = new MinerGeneral();
        string token;
        public static string jsonContent;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine("Culture: " + CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
            //(ApplicationBar.MenuItems[3] as ApplicationBarMenuItem).Text = _SettingsTitle;
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

            //this.Loaded += new RoutedEventHandler(consumeJson);
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            progressBar1.Value = 0;
            if (IsolatedStorageSettings.ApplicationSettings.Contains("token") && IsolatedStorageSettings.ApplicationSettings.Contains("json"))
            {
                jsonContent = IsolatedStorageSettings.ApplicationSettings["json"] as string;
                if (jsonContent != null && jsonContent.Contains("last_round_start"))
                {

                    parse_JSON(jsonContent);
                }
                token = IsolatedStorageSettings.ApplicationSettings["token"] as string;
                progressBar1.Value = 20;
                consumeJson();
            }
            if (IsolatedStorageSettings.ApplicationSettings.Contains("token") && !IsolatedStorageSettings.ApplicationSettings.Contains("json"))
            {
                token = IsolatedStorageSettings.ApplicationSettings["token"] as string;
                progressBar1.Value = 20;
                consumeJson();
            }
            if (!IsolatedStorageSettings.ApplicationSettings.Contains("token"))
            {
                MessageBoxResult m = MessageBox.Show("No API Token", "Add API Token to connect to your mining pool (polmine.pl).", MessageBoxButton.OK);
                if (m == MessageBoxResult.Cancel)
                {
                    // Application.Current.Terminate();
                }
                else
                {
                    NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
                }
            }


        }
        async void consumeJson()
        {
            if (NetworkInterface.NetworkInterfaceType != NetworkInterfaceType.None)
            {
                progressBar1.Visibility = System.Windows.Visibility.Visible;
                progressBar1.Value = 22;
                WebClient webClient = new WebClient();
                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(OnReadCompleted);
                webClient.DownloadStringAsync(new Uri(Default.POLMINE_ADDRESS + token));
                progressBar1.Value = 33;
                //https://polmine.pl/?action=api&cmd=dc48a571da3d78b1e744d14d83355729
                //THEIR 2e58e39abfa62691501af17301128a98
                //MY dc48a571da3d78b1e744d14d83355729
            }
            else
            {
                MessageBoxResult m = MessageBox.Show("Conenction error", "No Internet connection.", MessageBoxButton.OK);
            }
            
        }
        void OnReadCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("OnReadCompleted");
            if (e.Result == null)
            {
                System.Diagnostics.Debug.WriteLine("Error on retreving.");
                return;
            }
            jsonContent = e.Result;
            progressBar1.Value = 50;

            //API repair
            jsonContent = jsonContent.Replace(",\"workers\":{", ",workers:[{\"name\":");
            jsonContent = jsonContent.Replace(":{\"shares\"", ", \"shares\"");
            jsonContent = jsonContent.Replace("}}}", "}]}");
            //In case of more than 1 miner
            jsonContent = jsonContent.Replace("},", "}, {\"name\":");
            jsonContent = jsonContent.Replace("\":{\"", "\", \"");
            progressBar1.Value = 60;
            System.Diagnostics.Debug.WriteLine("JSON Starts");
            System.Diagnostics.Debug.WriteLine("JSON: " + jsonContent);
            //Parse JSON
            parse_JSON(jsonContent);

        }
        private void parse_JSON(string input)
        {
            RootObject root = JsonConvert.DeserializeObject<RootObject>(input);

            //Round up BTC and show
            general.payout = Convert.ToDouble(root.payout, CultureInfo.InvariantCulture);
            general.payout = Math.Round(general.payout, 5);
            Payout.Text = general.payout + " BTC";

            general.balance = Convert.ToDouble(root.balance, CultureInfo.InvariantCulture);
            general.balance = Math.Round(general.balance, 5);
            Balance.Text = general.balance + " BTC";

            progressBar1.Value = 75;
            //Calculate local speed sum
            general.speedSum = 0;
            for (int i = 0; i < root.workers.Count; i++) // Loop through List with for
            {
                System.Diagnostics.Debug.WriteLine("Speed of : " + i + ", is " + root.workers[i].speed);
                general.speedSum = Convert.ToInt32(root.workers[i].speed, CultureInfo.InvariantCulture) + general.speedSum;
            }
            SumOfSpeed.Text = general.speedSum + " MH/s";
            System.Diagnostics.Debug.WriteLine("Server speed sum: " + root.hashrate + ", local is " + general.speedSum);

            //List<Worker> workerList = new List<Worker>();
            listBoxWorkers.ItemsSource = root.workers;

            //Set general object fields
            general.workers = root.workers;
            general.block = root.block;
            general.block_all = root.block_all;
            general.round_start_date = root.last_round_start_date;
            //general.round_start_date = DateTime.ParseExact(root.last_round_start_date, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            progressBar1.Value = 100;
            progressBar1.Visibility = System.Windows.Visibility.Collapsed;

        }


        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            // If selected index is -1 (no selection) do nothing
            if (listBoxWorkers.SelectedIndex == -1)
                return;

            // Navigate to the new page
            //NavigationService.Navigate(new Uri("MinerDetails.xaml?selectedItem=" + (listBoxWorkers.SelectedItem as MinerGeneral).ID, UriKind.Relative));
            PhoneApplicationService.Current.State["general"] = general;
            System.Diagnostics.Debug.WriteLine("Clicked: " + listBoxWorkers.SelectedIndex);
            general = Tools.Set_Color_Background(general);
            general = Tools.Set_SpeedText(general);
            //PhoneApplicationService.Current.State["minerNumber"] = listBoxWorkers.SelectedIndex;
            //NavigationService.Navigate(new Uri("/MinerDetails.xaml", UriKind.Relative));
            //PhoneApplicationService.Current.State["general"] = general;//paramNavigationService.Navigate(new Uri("/view/MinerDetails.xaml", UriKind.Relative));
            NavigationService.Navigate(new Uri("/MinerDetails.xaml?minerNumber=" + listBoxWorkers.SelectedIndex, UriKind.Relative));
            // Reset selected index to -1 (no selection)
            listBoxWorkers.SelectedIndex = -1;
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("button clicked - refresh");
            consumeJson();
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("button clicked - settings");
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));

        }

        private void About_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("button clicked - about");
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }
    }
}