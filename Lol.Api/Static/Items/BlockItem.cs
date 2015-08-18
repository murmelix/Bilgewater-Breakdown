using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lol.Api.Static.Items
{
    [DataContract]
    public class BlockItem
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "count")]
        public int Count { get; set; }
    }
}
