using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lol.Api.Static.Champion
{
    [DataContract]
    public class ChampionSpell
    {
        [DataMember(Name = "cooldownBurn")]
        public string CooldownBurn { get; set; }
        [DataMember(Name = "costBurn")]
        public string CostBurn { get; set; }
        [DataMember(Name = "costType")]
        public string CostType { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "key")]
        public string Key { get; set; }
        [DataMember(Name = "maxrank")]
        public int Maxrank { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "rangeBurn")]
        public string RangeBurn { get; set; }
        [DataMember(Name = "resource")]
        public string Resource { get; set; }
        [DataMember(Name = "sanitizedTooltip")]
        public string SanitizedTooltip { get; set; }
        [DataMember(Name = "tooltip")]
        public string Tooltip { get; set; }
        [DataMember(Name = "altimages")]
        public List<Image> Altimages { get; set; }
        [DataMember(Name = "cooldown")]
        public List<double> Cooldown { get; set; }
        [DataMember(Name = "cost")]
        public List<int> Cost { get; set; }
        [DataMember(Name = "effect")]
        public List<List<double>> Effect { get; set; }
        [DataMember(Name = "effectBurn")]
        public List<string> EffectBurn { get; set; }
        [DataMember(Name = "vars")]
        public List<SpellVars> Vars { get; set; }
        [DataMember(Name = "image")]
        public Image Image { get; set; }
        [DataMember(Name = "leveltip")]
        public LevelTip Leveltip { get; set; }
        [DataMember(Name = "range")]
        public object Range { get; set; }
    }
}
