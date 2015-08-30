using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lol.Api.Static.Match
{
    [DataContract]
    public class Event
    { 
        [DataMember(Name = "ascendedType")]
        public string AscendedType { get; set; }
        [DataMember(Name = "assistingParticipantIds")]
        public List<int> AssistingParticipantIds { get; set; }
        [DataMember(Name = "buildingType")]
        public string BuildingType { get; set; }
        [DataMember(Name = "creatorId")]
        public int CreatorId { get; set; }
        [DataMember(Name = "eventType")]
        public string EventType { get; set; }
        [DataMember(Name = "itemAfter")]
        public int ItemAfter { get; set; }
        [DataMember(Name = "itemBefore")]
        public int ItemBefore { get; set; }
        [DataMember(Name = "itemId")]
        public int ItemId { get; set; }
        [DataMember(Name = "killerId")]
        public int KillerId { get; set; }
        [DataMember(Name = "laneType")]
        public string LaneType { get; set; }
        [DataMember(Name = "levelUpType")]
        public string LevelUpType { get; set; }
        [DataMember(Name = "monsterType")]
        public string MonsterType { get; set; }
        [DataMember(Name = "participantId")]
        public int ParticipantId { get; set; }
        [DataMember(Name = "pointCaptured")]
        public string PointCaptured { get; set; }
        //[DataMember(Name = "position")]
        //public Position Position { get; set; }
        [DataMember(Name = "skillSlot")]
        public int SkillSlot { get; set; }
        [DataMember(Name = "teamId")]
        public int TeamId { get; set; }
        [DataMember(Name = "towerType")]
        public string TowerType { get; set; }
        [DataMember(Name = "victimId")]
        public int VictimId { get; set; }
        [DataMember(Name = "wardType")]
        public string WardType { get; set; }
    }
}
