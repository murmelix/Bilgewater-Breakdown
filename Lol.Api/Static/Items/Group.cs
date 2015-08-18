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
    public class Group
    {
        [DataMember(Name = "MaxGroupOwnable")]
        public string MaxGroupOwnable { get; set; }
        [DataMember(Name = "key")]
        public string Key { get; set; }
    }
}
