using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lol.Api.Bilgewater
{
    [ProtoBuf.ProtoContract]
    public class MercData
    {
        [ProtoBuf.ProtoMember(1)]
        public int Id { get; set; }
        [ProtoBuf.ProtoMember(2)]
        public int? UpgradeLevel { get; set; }
        [ProtoBuf.ProtoMember(3)]
        public int? OffenseLevel { get; set; }
        [ProtoBuf.ProtoMember(4)]
        public int? DefenseLevel { get; set; }
        [ProtoBuf.ProtoMember(5)]
        public int ParticipantId { get; set; }
    }
}
