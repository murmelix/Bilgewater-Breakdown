using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lol.Api.Static.Items
{
    [DataContract]
    public class Item : BasicData
    {
        public bool IsBilgewater { get { return Tags != null && Tags.Any(x => x == "Bilgewater"); } }
        [DataMember(Name = "effect")]
        public Dictionary<string, string> Effect { get; set; }
    }
}
