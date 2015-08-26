using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lol.Api.Static.Match
{
    [DataContract]
    public class Rune
    {
        [DataMember(Name = "rank")]
        public long Rank { get; set; }
        [DataMember(Name = "runeId")]
        public long RuneId { get; set; }
    }
}
