using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Phone.Tasks;
using System.Runtime.Serialization;
namespace MineWatcher
{
    [DataContract]
    public class MinerGeneral
    {
        [DataMember]
        public double balance { get; set; }
        [DataMember]
        public double payout { get; set; }
        [DataMember]
        public int block { get; set; }
        [DataMember]
        public int block_all { get; set; }
        [DataMember]
        public string round_start_date { get; set; }
        [DataMember]
        public int speedSum { get; set; }
        [DataMember]
        public List<Worker> workers { get; set; }

    }
}
