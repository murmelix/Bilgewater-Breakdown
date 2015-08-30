using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lol.Api.Bilgewater
{
    public enum MerictFormat { Win, Float, Int}
    public class Merit
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public float ValueBilgewater { get; set; }
        public float ValueRanked { get; set; }
    }
}
