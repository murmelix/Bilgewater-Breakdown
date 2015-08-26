using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lol.Api.Bilgewater
{
    [ProtoBuf.ProtoContract]
    public class Participant
    {
        [ProtoBuf.ProtoMember(1)]
        public int Id { get; set; }
        [ProtoBuf.ProtoMember(2)]
        public int ChampionId { get; set; }
        [ProtoBuf.ProtoMember(3)]
        public bool FirstBlood { get; set; }
        [ProtoBuf.ProtoMember(4)]
        public int Spell1Id { get; set; }
        [ProtoBuf.ProtoMember(5)]
        public int Spell2Id { get; set; }
        [ProtoBuf.ProtoMember(6)]
        public long Deaths { get; set; }
        [ProtoBuf.ProtoMember(7)]
        public long Kills { get; set; }
        [ProtoBuf.ProtoMember(8)]
        public long PentaKills { get; set; }
        [ProtoBuf.ProtoMember(9)]
        public long MinionsKilled { get; set; }
        [ProtoBuf.ProtoMember(10)]
        public long Assists { get; set; }
        [ProtoBuf.ProtoMember(11)]
        public long Item0 { get; set; }
        [ProtoBuf.ProtoMember(12)]
        public long Item1 { get; set; }
        [ProtoBuf.ProtoMember(13)]
        public long Item2 { get; set; }
        [ProtoBuf.ProtoMember(14)]
        public long Item3 { get; set; }
        [ProtoBuf.ProtoMember(15)]
        public long Item4 { get; set; }
        [ProtoBuf.ProtoMember(16)]
        public long Item5 { get; set; }
        [ProtoBuf.ProtoMember(17)]
        public long Item6 { get; set; }
        [ProtoBuf.ProtoMember(18)]
        public long NeutralMinionsKilledEnemyJungle { get; set; }
        [ProtoBuf.ProtoMember(19)]
        public long NeutralMinionsKilledTeamJungle { get; set; }
        [ProtoBuf.ProtoMember(20)]
        public long NeutralMinionsKilled { get; set; }
    }
}
