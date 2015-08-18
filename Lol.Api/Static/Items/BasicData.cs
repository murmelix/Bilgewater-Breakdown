using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lol.Api.Static.Items
{
    [DataContract]
    public class BasicData
    {
        [DataMember(Name = "colloq")]
        public string Colloq { get; set; }
        [DataMember(Name = "consumeOnFull")]
        public bool ConsumeOnFull { get; set; }
        [DataMember(Name = "consumed")]
        public bool Consumed { get; set; }
        [DataMember(Name = "depth")]
        public int Depth { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "from")]
        public List<string> From { get; set; }
        [DataMember(Name = "gold")]
        public Gold Gold { get; set; }
        [DataMember(Name = "group")]
        public string Group { get; set; }
        [DataMember(Name = "hideFromAll")]
        public bool HideFromAll { get; set; }
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "image")]
        public Image Image { get; set; }
        [DataMember(Name = "inStore")]
        public bool InStore { get; set; }
        [DataMember(Name = "into")]
        public List<string> Into { get; set; }
        [DataMember(Name = "maps")]
        public Dictionary<string, bool> Maps { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "plaintext")]
        public string Plaintext { get; set; }
        [DataMember(Name = "requiredChampion")]
        public string RequiredChampion { get; set; }
        [DataMember(Name = "rune")]
        public MetaData Rune { get; set; }
        [DataMember(Name = "sanitizedDescription")]
        public string SanitizedDescription { get; set; }
        [DataMember(Name = "specialRecipe")]
        public int SpecialRecipe { get; set; }
        [DataMember(Name = "stacks")]
        public int Stacks { get; set; }
        [DataMember(Name = "stats")]
        public BasicDataStats Stats { get; set; }
        [DataMember(Name = "tags")]
        public List<string> Tags { get; set; }
    }
}
