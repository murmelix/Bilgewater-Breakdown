using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lol.Api.Static.Match
{
    [DataContract]
    public class Positions
    {
        [DataMember(Name = "x")]
        public string X { get; set; }
        [DataMember(Name = "y")]
        public string Y { get; set; }
    }
}
 