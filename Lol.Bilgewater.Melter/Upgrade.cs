using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Lol.Bilgewater.Melter
{
    public class Upgrade
    {
        [XmlAttribute(AttributeName="Id")]
        public int Id { get; set; }
        [XmlAttribute(AttributeName = "Lvl")]
        public int Lvl { get; set; }
    }
}
