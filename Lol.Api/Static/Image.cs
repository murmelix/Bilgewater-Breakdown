using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lol.Api.Static
{
    [DataContract]
    public class Image
    {
        [DataMember(Name = "full"), XmlElement(ElementName = "full")]
        public string Full { get; set; }
        [DataMember(Name = "group"), XmlElement(ElementName = "group")]
        public string Group { get; set; }
        [DataMember(Name = "h"), XmlElement(ElementName = "h")]
        public int Height { get; set; }
        [DataMember(Name = "sprite"), XmlElement(ElementName = "sprite")]
        public string Sprite { get; set; }
        [DataMember(Name = "w"), XmlElement(ElementName = "w")]
        public string Width { get; set; }
        [DataMember(Name = "x"), XmlElement(ElementName = "x")]
        public string X { get; set; }
        [DataMember(Name = "y"), XmlElement(ElementName = "y")]
        public string Y { get; set; }
    }
}
