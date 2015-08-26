using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lol.Api.Bilgewater
{
    [ProtoBuf.ProtoContract]
    public class MercStatistcs
    {
        [ProtoBuf.ProtoMember(1)]
        public MercTeam TeamRed { get; set; }
        [ProtoBuf.ProtoMember(2)]
        public MercTeam TeamBlue { get; set; }
        [ProtoBuf.ProtoMember(3)]
        public long MatchDuration { get; set; }
        [ProtoBuf.ProtoMember(4)]
        public long FirstBlood { get; set; }
        [ProtoBuf.ProtoMember(5)]
        public long? FirstTower { get; set; }
        [ProtoBuf.ProtoMember(6)]
        public long? FirstInhib { get; set; }
        [ProtoBuf.ProtoMember(7)]
        public long? FirstDragon { get; set; }
        [ProtoBuf.ProtoMember(8)]
        public long? FirstBaron { get; set; }
        [ProtoBuf.ProtoMember(9)]
        public long MatchId { get; set; }
    }
}
