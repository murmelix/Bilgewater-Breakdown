using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lol.Api.Static.Match
{
    [DataContract]
    public class Participant
    {
        [DataMember(Name = "championId")]
        public int ChampionId { get; set; }
        [DataMember(Name = "highestAchievedSeasonTier")]
        public string HighestAchievedSeasonTier { get; set; }
        [DataMember(Name = "masteries")]
        public List<Mastery> Masteries { get; set; }
        [DataMember(Name = "participantId")]
        public int ParticipantId { get; set; }
        [DataMember(Name = "runes")]
        public List<Rune> Runes { get; set; }
        [DataMember(Name = "spell1Id")]
        public int Spell1Id { get; set; }
        [DataMember(Name = "spell2Id")]
        public int Spell2Id { get; set; }
        [DataMember(Name = "stats")]
        public ParticipantStats Stats { get; set; }
        [DataMember(Name = "teamId")]
        public int TeamId { get; set; }
        [DataMember(Name = "timeline")]
        public ParticipantTimeline Timeline { get; set; }
    }
}
