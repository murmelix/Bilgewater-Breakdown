using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lol.Api.Static.Match
{
    [DataContract]
    public class ParticipantFrame
    {
        [DataMember(Name = "currentGold")] 
        public int CurrentGold { get; set; }
        [DataMember(Name = "dominionScore")]
        public int DominionScore { get; set; }
        [DataMember(Name = "jungleMinionsKilled")]
        public int JungleMinionsKilled { get; set; }
        [DataMember(Name = "level")]
        public int Level { get; set; }
        [DataMember(Name = "minionsKilled")]
        public int MinionsKilled { get; set; }
        [DataMember(Name = "participantId")]
        public int ParticipantId { get; set; }
        [DataMember(Name = "teamScore")]
        public int TeamScore { get; set; }
        [DataMember(Name = "totalGold")]
        public int TotalGold { get; set; }
        [DataMember(Name = "xp")]
        public int Xp { get; set; }
        [DataMember(Name = "position")]
        public Positions Position { get; set; }
    }
}
