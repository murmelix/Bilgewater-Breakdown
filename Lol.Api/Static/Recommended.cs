using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lol.Api.Static
{
    [DataContract]
    public class Recommended
    {
        [DataMember(Name = "champion")]
        public string Champion { get; set; }
        [DataMember(Name = "map")]
        public string Map { get; set; }
        [DataMember(Name = "mode")]
        public string Mode { get; set; }
        [DataMember(Name = "priority")]
        public bool Priority { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "blocks")]
        public List<Block> Blocks { get; set; }
    }
}
