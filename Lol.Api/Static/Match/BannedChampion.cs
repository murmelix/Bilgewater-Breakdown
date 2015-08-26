using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lol.Api.Static.Match
{
    [DataContract]
    public class BannedChampion
    {
        [DataMember(Name = "championId")]
        public int ChampionId { get; set; }
        [DataMember(Name = "pickTurn")]
        public int PickTurn { get; set; }
    }
}
