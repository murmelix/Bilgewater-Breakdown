using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lol.Api.Static.Champion
{
    [DataContract]
    public class Skin
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "num")]
        public int Num { get; set; }
    }
}
