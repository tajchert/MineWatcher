using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MineWatcher
{
    public class Worker
    {
        public string name { get; set; }
        public int shares { get; set; }
        public int stale { get; set; }
        public string last_share { get; set; }
        public float speed { get; set; }
        public string image { get; set; }
        public string speedText { get; set; }
        public string ago { get; set; }
        public string percent { get; set; }
        public DateTime last_online;
        //object
    }

    public class RootObject
    {
        public string balance { get; set; }
        public string payout { get; set; }
        public int block { get; set; }
        public int block_all { get; set; }
        public int last_round_start { get; set; }
        public string last_round_start_date { get; set; }
        public int hashrate { get; set; }
        public string workers_speed { get; set; }
        public List<Worker> workers { get; set; }
    }
}
