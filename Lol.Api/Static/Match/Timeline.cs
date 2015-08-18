using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lol.Api.Static.Match
{
    [DataContract]
    public class Timeline
    {
        [DataMember(Name = "frameInterval")]
        public long FrameInterval { get; set; }
        [DataMember(Name = "frames")]
        public List<Frame> Frames { get; set; }
    }
}
