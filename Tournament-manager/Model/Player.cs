
namespace Tournament_manager.Model
{
    // Division naming from PokemonTCG = Junior youngest (0-12), senior older (13-16), master everyone older 
    public enum PlayerDivision
    {
        Master,
        Senior,
        Junior,
    }
    public enum Result
    {
        Win,
        Lose,
        Draw
    }

    public class Player
    {
        public int id { get; set; }
        public string Name { get; set; }
        public PlayerDivision Division { get; set; }

        public int Wins { get; set; } = 0;
        public int Losses { get; set; } = 0;
        public int Draws { get; set; } = 0;
        // score => win = 3 points, draw = 1 point, loss = 0 points
        public int Score { get; set; } = 0;

        public int Points { get; set; } = 0;

        public int Result { get; set; } = 0;
        // player can't play with the same oponent twice
        public List<Player> Oponents { get; set; } = new();
        // player can't have bye more then once
        public bool HadBye { get; set; } = false;
        public List<string> Warnings { get; set; } = new();
        public int WarningCount { get; set; } = 0;

        public double WinRate
        {
            get
            {
                if (Wins + Losses + Draws == 0)
                    return 0;
                return (Wins + Draws / 2.0) / (Wins + Losses + Draws);
            }
        }

        public Player() { }

        public Player(string name, PlayerDivision division)
        {
            Name = name;
            Division = division;
        }
    }
}
