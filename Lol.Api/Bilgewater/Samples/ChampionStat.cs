using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lol.Api.Bilgewater.Samples
{
    public class ChampionStat
    {
        public int Id { get; set; }
        public long Pickcount { get; set; }
        public long Wincount { get; set; }
        public float Pickrate { get; set; }
        public float Winrate { get; set; }
        public long PentaKillCount { get; set; }
        public float AvgPentaKillCount { get; set; }
        public long KillCount { get; set; }
        public float AvgKillCount { get; set; }
        public long DeathCount { get; set; }
        public float AvgDeathCount { get; set; }
        public long AssistCount { get; set; }
        public float AvgAssistCount { get; set; }
        public long MinionCount { get; set; }
        public float AvgMinionCount { get; set; }
        public long JungleCount { get; set; }
        public float AvgJungleCount { get; set; }
        public long AlliedJungleCount { get; set; }
        public float AvgAlliedJungleCount { get; set; }
        public long HostileJungleCount { get; set; }
        public float AvgHostileJungleCount { get; set; }
        public string Role { get; set; }
    }
}
