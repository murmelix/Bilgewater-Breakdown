using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lol.Api.Static.Champion
{
    [DataContract]
    public class SpellVars
    {
        [DataMember(Name = "ranksWith")]
        public string RanksWith { get; set; }
        [DataMember(Name = "link")]
        public string Link { get; set; }
        [DataMember(Name = "key")]
        public string Key { get; set; }
        [DataMember(Name = "dyn")]
        public string Dyn { get; set; }
        [DataMember(Name = "coeff")]
        public List<double> Coeff { get; set; }
    }
}
