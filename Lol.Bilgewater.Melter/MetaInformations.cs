using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lol.Bilgewater.Melter
{
    public class MetaInformations
    {
        [XmlElement(ElementName="Item")]
        public List<int> Item { get; set; }
        [XmlElement(ElementName = "Merc")]
        public List<int> Merc { get; set; }
        [XmlElement(ElementName = "Upgrade")]
        public List<Upgrade> Upgrade { get; set; }
        [XmlElement(ElementName = "Offense")]
        public List<Upgrade> Offense { get; set; }
        [XmlElement(ElementName = "Defense")]
        public List<Upgrade> Defense { get; set; }
    }
}
