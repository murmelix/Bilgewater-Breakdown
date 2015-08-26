using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lol.Api.Bilgewater
{
    [ProtoBuf.ProtoContract]
    public class MercTeam
    {
        [ProtoBuf.ProtoMember(1)]
        public int TeamId { get; set; }
        [ProtoBuf.ProtoMember(2)]
        public List<Participant> Participants { get; set; }
        [ProtoBuf.ProtoMember(3)]
        public List<MercData> MercsBought { get; set; }
        [ProtoBuf.ProtoMember(4)]
        public bool Winner { get; set; }
        [ProtoBuf.ProtoMember(5)]
        public long? FirstInhibitorTaken { get; set; }
        [ProtoBuf.ProtoMember(6)]
        public long? FirstTowerTaken { get; set; }
        [ProtoBuf.ProtoMember(7)]
        public long? FirstDragon { get; set; }
        [ProtoBuf.ProtoMember(8)]
        public long? FirstBaron { get; set; }
        [ProtoBuf.ProtoMember(9)]
        public int BaronKills { get; set; }
        [ProtoBuf.ProtoMember(10)]
        public int DragonKills { get; set; }
        [ProtoBuf.ProtoMember(11)]
        public bool FirstBlood { get; set; }
    }
}
