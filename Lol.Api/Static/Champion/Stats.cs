using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lol.Api.Static.Champion
{
    [DataContract]
    public class Stats
    {
        [DataMember(Name = "armor")]
        public double Armor { get; set; }
        [DataMember(Name = "armorperlevel")]
        public double ArmorPerLevel { get; set; }
        [DataMember(Name = "attackdamage")]
        public double AttackDamage { get; set; }
        [DataMember(Name = "attackdamageperlevel")]
        public double AttackDamagePerLevel { get; set; }
        [DataMember(Name = "attackrange")]
        public double AttackRange { get; set; }
        [DataMember(Name = "attackspeedoffset")]
        public double AttackspeedOffset { get; set; }
        [DataMember(Name = "attackspeedperlevel")]
        public double AttackspeedPerLevel { get; set; }
        [DataMember(Name = "crit")]
        public double Crit { get; set; }
        [DataMember(Name = "critperlevel")]
        public double CritPerLevel { get; set; }
        [DataMember(Name = "hp")]
        public double Hp { get; set; }
        [DataMember(Name = "hpperlevel")]
        public double HpPerLevel { get; set; }
        [DataMember(Name = "hpregen")]
        public double HpRegen { get; set; }
        [DataMember(Name = "hpregenperlevel")]
        public double HpRegenPerLevel { get; set; }
        [DataMember(Name = "movespeed")]
        public double Movespeed { get; set; }
        [DataMember(Name = "mp")]
        public double Mp { get; set; }
        [DataMember(Name = "mpperlevel")]
        public double MpPerLevel { get; set; }
        [DataMember(Name = "mpregen")]
        public double MpRegen { get; set; }
        [DataMember(Name = "mpregenperlevel")]
        public double MpRegenPerLevel { get; set; }
        [DataMember(Name = "spellblock")]
        public double Spellblock { get; set; }
        [DataMember(Name = "spellblockperlevel")]
        public double SpellblockPerLevel { get; set; }
    }
}
