using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lol.Api.Static.Match
{
    [DataContract]
    public class MatchDetail 
    {
        [DataMember(Name = "mapId")]
        public int MapId { get; set; }
        [DataMember(Name = "matchCreation")]
        public long MatchCreation { get; set; }
        [DataMember(Name = "matchDuration")]
        public long MatchDuration { get; set; }
        [DataMember(Name = "matchId")]
        public long MatchId { get; set; }
        [DataMember(Name = "matchMode")]
        public string MatchMode { get; set; }
        [DataMember(Name = "matchType")]
        public string MatchType { get; set; }
        [DataMember(Name = "matchVersion")]
        public string MatchVersion { get; set; }
        [DataMember(Name = "platformId")]
        public string PlatformId { get; set; }
        [DataMember(Name = "queueType")]
        public string QueueType { get; set; }
        [DataMember(Name = "region")]
        public string Region { get; set; }
        [DataMember(Name = "season")]
        public string Season { get; set; }
        [DataMember(Name = "timeline")]
        public Timeline Timeline { get; set; }
        [DataMember(Name = "teams")]
        public List<Team> Teams { get; set; }
        [DataMember(Name = "participantIdentities")]
        public List<ParticipantIdentity> ParticipantIdentities { get; set; }
        [DataMember(Name = "participants")]
        public List<Participant> Participants { get; set; }
    }
}
