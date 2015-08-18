using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lol.Api.Static.Items
{
    [DataContract]
    public class MetaData
    {
        [DataMember(Name = "isRune")]
        public bool IsRune { get; set; }
        [DataMember(Name = "tier")]
        public string Tier { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; set; }
    }
}
