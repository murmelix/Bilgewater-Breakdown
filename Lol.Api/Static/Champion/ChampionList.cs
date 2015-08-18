using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lol.Api.Static.Champion
{
    [DataContract]
    public class ChampionList
    {
        [DataMember(Name = "data")]
        public Dictionary<string, Champion> Data { get; set; }
        [DataMember(Name = "keys")]
        public Dictionary<string, string> Keys { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "version")]
        public string Version { get; set; }
        [DataMember(Name = "format")]
        public string Format { get; set; }
    }
}
