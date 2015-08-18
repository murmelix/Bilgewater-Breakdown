using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lol.Api.Static.Champion
{
    [DataContract]
    public class LevelTip
    {
        [DataMember(Name = "effect")]
        public List<string> Effect { get; set; }
        [DataMember(Name = "label")]
        public List<string> Label { get; set; }
    }
}
