using Lol.Api.Static.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lol.Api.Static.Champion
{
    [DataContract]
    public class Champion
    {     
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "blurb")]
        public string Blurb { get; set; }
        [DataMember(Name = "key")]
        public string Key { get; set; }
        [DataMember(Name = "lore")]
        public string Lore { get; set; }
        [DataMember(Name = "partype")]
        public string Partype { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }   
        [DataMember(Name = "allytips")]
        public List<string> AllyTips { get; set; }
        [DataMember(Name = "enemytips")]
        public List<string> EnemyTips { get; set; }
        [DataMember(Name = "tags")]
        public List<string> Tags { get; set; }
        [DataMember(Name = "image")]
        public Image Image { get; set; }
        [DataMember(Name = "info")]
        public Info Info { get; set; }
        [DataMember(Name = "passive")]
        public Passive Passive { get; set; }
        [DataMember(Name = "stats")]
        public Stats Stats { get; set; }
        [DataMember(Name = "recommended")]
        public List<Recommended> Recommended { get; set; }
        [DataMember(Name = "custom")]
        public int CustomCount { get; set; }
        [DataMember(Name = "skins")]
        public List<Skin> Skins { get; set; }
        [DataMember(Name = "spells")]
        public List<ChampionSpell> Spells { get; set; } 
    }
}
