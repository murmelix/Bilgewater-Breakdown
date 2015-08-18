using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lol.Api.Static.Match
{
    [DataContract]
    public class ParticipantIdentity
    {
        [DataMember(Name = "participantId")]
        public int ParticipantId { get; set; }
        [DataMember(Name = "player")]
        public Player Player { get; set; }
    }
}
