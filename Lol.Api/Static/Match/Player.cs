using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lol.Api.Static.Match
{
    [DataContract]
    public class Player
    {
        [DataMember(Name = "matchHistoryUri")]
        public string MatchHistoryUri { get; set; }
        [DataMember(Name = "profileIcon")]
        public int ProfileIcon { get; set; }
        [DataMember(Name = "summonerId")]
        public long SummonerId { get; set; }
        [DataMember(Name = "summonerName")]
        public string SummonerName { get; set; }
    }
}
