using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament_manager.Model
{
    class Tournament
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RoundDurations { get; set; }
        public List<Player> Players { get; set; } = new List<Player>();
    }
}
