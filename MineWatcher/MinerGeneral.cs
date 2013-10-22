using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Phone.Tasks;

namespace MineWatcher
{
    class MinerGeneral
    {
        public double balance { get; set; }
        public double payout { get; set; }
        public int block { get; set; }
        public int block_all { get; set; }
        public DateTime round_start_date { get; set; }
        public int speedSum { get; set; }
        public List<Worker> workers { get; set; }

        /*
        public List<Worker> Workers
        {
            get { return workers; }
            set { workers = value; }
        }
        public int SpeedSum
        {
            get { return speedSum; }
            set { speedSum = value; }
        }
        public DateTime Round_Start_Date
        {
            get { return round_start_date; }
            set { round_start_date = value; }
        }
        public double Balance
        {
            get { return balance; }
            set { balance = value; }
        }
        public double Payout
        {
            get { return payout; }
            set { payout = value; }
        }
        public string Payout
        {
            get { return payout+""; }
            set { payout = Convert.ToDouble(value); }
        }

        public int Block
        {
            get { return block; }
            set { block = value; }
        }
        public int Block_All
        {
            get { return block_all; }
            set { block_all = value; }
        }*/
    }
}
