using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lol.Api.Static.Champion
{
    [DataContract]
    public class Info
    {
        [DataMember(Name="attack")]
        public int Attack { get; set; }
        [DataMember(Name = "defense")]
        public int Defense { get; set; }
        [DataMember(Name = "difficulty")]
        public int Difficulty { get; set; }
        [DataMember(Name = "magic")]
        public int Magic { get; set; }
    }
}
