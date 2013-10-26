using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Phone.Tasks;
using System.IO.IsolatedStorage;

namespace MineWatcher
{
    class Tools
    {
        public static string mh = "MH/s";
        public static MinerGeneral Set_Color_Background(MinerGeneral general)
        {
            System.Diagnostics.Debug.WriteLine("Started updating image patch.");


            DateTime CurrentDate = System.DateTime.Now;
            DateTime then;
            for (int i = 0; i < general.workers.Count; i++) // Loop through List with for
            {

                //general.workers[i].last_online = DateTime.ParseExact(general.workers[i].last_share, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                then = DateTime.ParseExact(general.workers[i].last_share, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                //System.Diagnostics.Debug.WriteLine("Time: " + general.workers[i].last_online);
                System.Diagnostics.Debug.WriteLine("Time: " + then);
                //TimeSpan TimeDifference = CurrentDate - general.workers[i].last_online;
                TimeSpan TimeDifference = CurrentDate - then;
                System.Diagnostics.Debug.WriteLine("Miner number: " + i + ", last online: " + TimeDifference.TotalMinutes + "minutes ago.");
                int Minutes_To_Offline = Default.MINUTES_TO_OFFLINE;
                if (IsolatedStorageSettings.ApplicationSettings.Contains("time"))
                {
                    Minutes_To_Offline = Convert.ToInt16(IsolatedStorageSettings.ApplicationSettings["time"] as string, CultureInfo.InvariantCulture);
                }
                if (TimeDifference.TotalMinutes > Minutes_To_Offline)
                {
                    general.workers[i].image = "/Assets/offline.png";
                    System.Diagnostics.Debug.WriteLine("Is offline.");
                }
                else
                {
                    general.workers[i].image = "/Assets/online.png";
                    System.Diagnostics.Debug.WriteLine("Is online.");
                }
                if(TimeDifference.TotalDays>1){
                    general.workers[i].ago = Math.Round(TimeDifference.TotalDays, 2) + "  Days";
                }
                if (TimeDifference.TotalHours > 1 && TimeDifference.TotalHours < 24)
                {
                    general.workers[i].ago = Math.Round(TimeDifference.TotalHours, 2) + "  Hours";
                }
                if (TimeDifference.TotalMinutes < 60 )
                {
                    general.workers[i].ago = Math.Round(TimeDifference.TotalMinutes, 2) +"  Min.";
                }
                if (general.workers[i].shares > 0)
                {
                    double tmpPerc = (general.workers[i].stale * 100);
                    tmpPerc = tmpPerc / general.workers[i].shares;
                    string perc = tmpPerc + "";
                    perc = perc.Substring(0, Math.Min(perc.Length, 7));
                    general.workers[i].percent = perc + " %";
                }
                else
                {
                    general.workers[i].percent = "0 %";
                }
                
            }

            return general;
        }
        public static MinerGeneral Set_SpeedText(MinerGeneral general)
        {
            System.Diagnostics.Debug.WriteLine("Started updating miner speedText.");

            DateTime CurrentDate = System.DateTime.Now;
            for (int i = 0; i < general.workers.Count; i++) // Loop through List with for
            {
                general.workers[i].speedText = general.workers[i].speed + " " + Tools.mh;
            }

            return general;
        }
    }
}
