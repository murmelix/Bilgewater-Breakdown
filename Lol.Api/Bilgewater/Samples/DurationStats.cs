using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lol.Api.Bilgewater.Samples
{
    [ProtoBuf.ProtoContract]
    [DataContract]
    public class DurationStats
    {
        public long MatchDuration { get; set; }
        [ProtoBuf.ProtoMember(1)]
        [DataMember]
        public float AvgMatchDuration { get; set; }
        public long Firstblood { get; set; }
        [ProtoBuf.ProtoMember(2)]
        [DataMember]
        public float AvgFirstblood { get; set; }
        public long FirstTower { get; set; }
        [ProtoBuf.ProtoMember(3)]
        [DataMember]
        public float AvgFirstTower { get; set; }
        public long FirstInhib { get; set; }
        [ProtoBuf.ProtoMember(4)]
        [DataMember]
        public float AvgFirstInhib { get; set; }
        public long FirstDragon { get; set; }
        public long DragonKilledMatchCount { get; set; }
        [ProtoBuf.ProtoMember(5)]
        [DataMember]
        public float AvgFirstDragon { get; set; }
        public long FirstBaron { get; set; }
        [ProtoBuf.ProtoMember(6)]
        [DataMember]
        public float AvgFirstBaron { get; set; }
        public long BaronKilledMatchCount { get; set; }
    }
}
