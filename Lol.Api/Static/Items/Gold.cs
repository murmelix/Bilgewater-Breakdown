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
    public class Gold
    {
        [DataMember(Name = "base")]
        public int Base { get; set; }
        [DataMember(Name = "purchasable")]
        public bool Purchasable { get; set; }
        [DataMember(Name = "sell")]
        public int Sell { get; set; }
        [DataMember(Name = "total")]
        public int Total { get; set; }
    }
}
