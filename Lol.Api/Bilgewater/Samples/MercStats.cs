using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lol.Api.Bilgewater.Samples
{
    public class MercStats
    {
        public int Id { get; set; }
        public int Pickcount { get; set; }
        public int Wincount { get; set; }
        public float Pickrate { get; set; }
        public float Winrate { get; set; }
    }
}
