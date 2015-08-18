using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lol.Api.Static.Match
{
    [DataContract]
    public class Team
    {
        [DataMember(Name = "bans")]
        public List<BannedChampion> Bans { get; set; }
        [DataMember(Name = "baronKills")]
        public int BaronKills { get; set; }
        [DataMember(Name = "dominionVictoryScore")]
        public long DominionVictoryScore { get; set; }
        [DataMember(Name = "dragonKills")]
        public int DragonKills { get; set; }
        [DataMember(Name = "firstBaron")]
        public bool FirstBaron { get; set; }
        [DataMember(Name = "firstBlood")]
        public bool FirstBlood { get; set; }
        [DataMember(Name = "firstDragon")]
        public bool FirstDragon { get; set; }
        [DataMember(Name = "firstInhibitor")]
        public bool FirstInhibitor { get; set; }
        [DataMember(Name = "firstTower")]
        public bool FirstTower { get; set; }
        [DataMember(Name = "inhibitorKills")]
        public int InhibitorKills { get; set; }
        [DataMember(Name = "teamId")]
        public int TeamId { get; set; }
        [DataMember(Name = "towerKills")]
        public int TowerKills { get; set; }
        [DataMember(Name = "vilemawKills")]
        public int VilemawKills { get; set; }
        [DataMember(Name = "winner")]
        public bool Winner { get; set; }
    }
}
