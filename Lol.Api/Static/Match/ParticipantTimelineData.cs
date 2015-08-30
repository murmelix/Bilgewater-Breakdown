using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lol.Api.Static.Match
{
    [DataContract]
    public class ParticipantTimelineData
    {
        [DataMember(Name = "tenToTwenty")]
        public double TenToTwenty { get; set; }
        [DataMember(Name = "thirtyToEnd")]
        public double ThirtyToEnd { get; set; }
        [DataMember(Name = "twentyToThirty")]
        public double TwentyToThirty { get; set; }
        [DataMember(Name = "zeroToTen")]
        public double ZeroToTen { get; set; }
    }
}
 