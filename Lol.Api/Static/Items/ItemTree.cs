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
    public class ItemTree
    {
        [DataMember(Name = "header")]
        public string Header { get; set; }
        [DataMember(Name = "tags")]
        public List<string> Tags { get; set; }
    }
}
