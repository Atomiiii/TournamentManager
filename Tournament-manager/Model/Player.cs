using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament_manager.Model
{
    public enum PlayerDivision
    {
        Junior,
        Senior,
        Master
    }
    public enum Result
    {
        Win,
        Lose,
        Draw
    }
    class Player
    {
        public string Name { get; set; }
        public PlayerDivision Division { get; set; }
        public List<Result> results { get; set; } = new List<Result>();
        public List<Player> oponents { get; set; } = new List<Player>();
    }
}
