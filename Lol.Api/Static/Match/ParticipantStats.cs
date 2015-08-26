using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lol.Api.Static.Match
{
    [DataContract]
    public class ParticipantStats
    {
        [DataMember(Name = "assists")]
        public long Assists { get; set; }
        [DataMember(Name = "champLevel")]
        public long ChampLevel { get; set; }
        [DataMember(Name = "combatPlayerScore")]
        public long CombatPlayerScore { get; set; }
        [DataMember(Name = "deaths")]
        public long Deaths { get; set; }
        [DataMember(Name = "doubleKills")]
        public long DoubleKills { get; set; }
        [DataMember(Name = "goldEarned")]
        public long GoldEarned { get; set; }
        [DataMember(Name = "goldSpent")]
        public long GoldSpent { get; set; }
        [DataMember(Name = "inhibitorKills")]
        public long InhibitorKills { get; set; }
        [DataMember(Name = "item0")]
        public long Item0 { get; set; }
        [DataMember(Name = "item1")]
        public long Item1 { get; set; }
        [DataMember(Name = "item2")]
        public long Item2 { get; set; }
        [DataMember(Name = "item3")]
        public long Item3 { get; set; }
        [DataMember(Name = "item4")]
        public long Item4 { get; set; }
        [DataMember(Name = "item5")]
        public long Item5 { get; set; }
        [DataMember(Name = "item6")]
        public long Item6 { get; set; }
        [DataMember(Name = "killingSprees")]
        public long KillingSprees { get; set; }
        [DataMember(Name = "kills")]
        public long Kills { get; set; }
        [DataMember(Name = "largestCriticalStrike")]
        public long LargestCriticalStrike { get; set; }
        [DataMember(Name = "largestKillingSpree")]
        public long LargestKillingSpree { get; set; }
        [DataMember(Name = "largestMultiKill")]
        public long LargestMultiKill { get; set; }
        [DataMember(Name = "magicDamageDealt")]
        public long MagicDamageDealt { get; set; }
        [DataMember(Name = "magicDamageDealtToChampions")]
        public long MagicDamageDealtToChampions { get; set; }
        [DataMember(Name = "magicDamageTaken")]
        public long MagicDamageTaken { get; set; }
        [DataMember(Name = "minionsKilled")]
        public long MinionsKilled { get; set; }
        [DataMember(Name = "neutralMinionsKilled")]
        public long NeutralMinionsKilled { get; set; }
        [DataMember(Name = "neutralMinionsKilledEnemyJungle")]
        public long NeutralMinionsKilledEnemyJungle { get; set; }
        [DataMember(Name = "neutralMinionsKilledTeamJungle")]
        public long NeutralMinionsKilledTeamJungle { get; set; }
        [DataMember(Name = "nodeCapture	")]
        public long NodeCapture { get; set; }
        [DataMember(Name = "nodeCaptureAssist")]
        public long NodeCaptureAssist { get; set; }
        [DataMember(Name = "nodeNeutralize")]
        public long NodeNeutralize { get; set; }
        [DataMember(Name = "nodeNeutralizeAssist")]
        public long NodeNeutralizeAssist { get; set; }
        [DataMember(Name = "objectivePlayerScore")]
        public long ObjectivePlayerScore { get; set; }
        [DataMember(Name = "pentaKills")]
        public long PentaKills { get; set; }
        [DataMember(Name = "physicalDamageDealt")]
        public long PhysicalDamageDealt { get; set; }
        [DataMember(Name = "physicalDamageDealtToChampions")]
        public long PhysicalDamageDealtToChampions { get; set; }
        [DataMember(Name = "physicalDamageTaken")]
        public long PhysicalDamageTaken { get; set; }
        [DataMember(Name = "quadraKills")]
        public long QuadraKills { get; set; }
        [DataMember(Name = "sightWardsBoughtInGame")]
        public long SightWardsBoughtInGame { get; set; }
        [DataMember(Name = "teamObjective")]
        public long TeamObjective { get; set; }
        [DataMember(Name = "totalDamageDealt")]
        public long TotalDamageDealt { get; set; }
        [DataMember(Name = "totalDamageDealtToChampions")]
        public long TotalDamageDealtToChampions { get; set; }
        [DataMember(Name = "totalDamageTaken")]
        public long TotalDamageTaken { get; set; }
        [DataMember(Name = "totalHeal")]
        public long TotalHeal { get; set; }
        [DataMember(Name = "totalPlayerScore")]
        public long TotalPlayerScore { get; set; }
        [DataMember(Name = "totalScoreRank")]
        public long TotalScoreRank { get; set; }
        [DataMember(Name = "totalTimeCrowdControlDealt")]
        public long TotalTimeCrowdControlDealt { get; set; }
        [DataMember(Name = "totalUnitsHealed")]
        public long TotalUnitsHealed { get; set; }
        [DataMember(Name = "towerKills")]
        public long TowerKills { get; set; }
        [DataMember(Name = "tripleKills")]
        public long TripleKills { get; set; }
        [DataMember(Name = "trueDamageDealt")]
        public long TrueDamageDealt { get; set; }
        [DataMember(Name = "trueDamageDealtToChampions")]
        public long TrueDamageDealtToChampions { get; set; }
        [DataMember(Name = "trueDamageTaken")]
        public long TrueDamageTaken { get; set; }
        [DataMember(Name = "unrealKills")]
        public long UnrealKills { get; set; }
        [DataMember(Name = "visionWardsBoughtInGame")]
        public long VisionWardsBoughtInGame { get; set; }
        [DataMember(Name = "wardsKilled")]
        public long WardsKilled { get; set; }
        [DataMember(Name = "wardsPlaced")]
        public long WardsPlaced { get; set; }
        [DataMember(Name = "winner")]
        public bool Winner { get; set; }
        [DataMember(Name = "firstBloodAssist")]
        public bool FirstBloodAssist { get; set; }
        [DataMember(Name = "firstBloodKill")]
        public bool FirstBloodKill { get; set; }
        [DataMember(Name = "firstInhibitorAssist")]
        public bool FirstInhibitorAssist { get; set; }
        [DataMember(Name = "firstInhibitorKill")]
        public bool FirstInhibitorKill { get; set; }
        [DataMember(Name = "firstTowerAssist")]
        public bool FirstTowerAssist { get; set; }
        [DataMember(Name = "firstTowerKill")]
        public bool FirstTowerKill { get; set; }
    }
}
