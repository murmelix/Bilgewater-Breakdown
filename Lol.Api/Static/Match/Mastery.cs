using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lol.Api.Static.Match
{
    [DataContract]
    public class Mastery
    {
        [DataMember(Name = "masteryId")]
        public long MasteryId { get; set; }
        [DataMember(Name = "rank")]
        public long Rank { get; set; }
    }
}
