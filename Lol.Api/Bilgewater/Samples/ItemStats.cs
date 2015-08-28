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
    public class ItemStats
    {
        [ProtoBuf.ProtoMember(1)]
        [DataMember]
        public int Id { get; set; }
        public int Pickcount { get; set; }
        public int Wincount { get; set; }
        [ProtoBuf.ProtoMember(2)]
        [DataMember]
        public float Pickrate { get; set; }
        [ProtoBuf.ProtoMember(3)]
        [DataMember]
        public float Winrate { get; set; }
        [DataMember]
        public bool IsBiglewater { get; set; }
    }
}
