using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lol.Api.Bilgewater.Samples
{
    public class DurationStats
    {
        public long MatchDuration { get; set; }
        public float AvgMatchDuration { get; set; }
        public long Firstblood { get; set; }
        public float AvgFirstblood { get; set; }
        public long FirstTower { get; set; }
        public float AvgFirstTower { get; set; }
        public long FirstInhib { get; set; }
        public float AvgFirstInhib { get; set; }
        public long FirstDragon { get; set; }
        public long DragonKilledMatchCount { get; set; }
        public float AvgFirstDragon { get; set; }
        public long FirstBaron { get; set; }
        public float AvgFirstBaron { get; set; }
        public long BaronKilledMatchCount { get; set; }
    }
}
