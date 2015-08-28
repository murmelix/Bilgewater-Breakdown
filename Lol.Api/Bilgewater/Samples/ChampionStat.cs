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
    public class ChampionStat
    {
        [ProtoBuf.ProtoMember(1)]
        [DataMember]
        public int Id { get; set; }
        public long Pickcount { get; set; }
        public long Wincount { get; set; }
        [ProtoBuf.ProtoMember(2)]
        [DataMember]
        public float Pickrate { get; set; }
        [ProtoBuf.ProtoMember(3)]
        [DataMember]
        public float Winrate { get; set; }
        public long PentaKillCount { get; set; }
        [ProtoBuf.ProtoMember(4)]
        [DataMember]
        public float AvgPentaKillCount { get; set; }
        public long KillCount { get; set; }
        [ProtoBuf.ProtoMember(5)]
        [DataMember]
        public float AvgKillCount { get; set; }
        public long DeathCount { get; set; }
        [ProtoBuf.ProtoMember(6)]
        [DataMember]
        public float AvgDeathCount { get; set; }
        public long AssistCount { get; set; }
        [ProtoBuf.ProtoMember(7)]
        [DataMember]
        public float AvgAssistCount { get; set; }
        public long MinionCount { get; set; }
        [ProtoBuf.ProtoMember(8)]
        [DataMember]
        public float AvgMinionCount { get; set; }
        public long JungleCount { get; set; }
        [ProtoBuf.ProtoMember(9)]
        [DataMember]
        public float AvgJungleCount { get; set; }
        public long AlliedJungleCount { get; set; }
        [ProtoBuf.ProtoMember(10)]
        [DataMember]
        public float AvgAlliedJungleCount { get; set; }
        public long HostileJungleCount { get; set; }
        [ProtoBuf.ProtoMember(11)]
        [DataMember]
        public float AvgHostileJungleCount { get; set; }
        [ProtoBuf.ProtoMember(12)]
        [DataMember]
        public string Role { get; set; }
    }
}
