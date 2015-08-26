using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lol.Api.Static.Match
{
    [DataContract]
    public class Frame
    {
        [DataMember(Name = "events")]
        public List<Event> Events { get; set; }
        [DataMember(Name = "participantFrames")]
        public Dictionary<string,ParticipantFrame> ParticipantFrames { get; set; }
        [DataMember(Name = "timestamp")]
        public long Timestamp { get; set; }
    }
}
