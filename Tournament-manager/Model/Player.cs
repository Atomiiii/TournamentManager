
namespace Tournament_manager.Model
{
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
        public int Score { get; set; } = 0;
        public int Points { get; set; } = 0;

        // This is the circular reference list that causes the need for ReferenceHandler
        public List<Player> Oponents { get; set; } = new();

        public bool HadBye { get; set; } = false;
        public List<string> Warnings { get; set; } = new();

        public double WinRate
        {
            get
            {
                if (Wins + Losses + Draws == 0)
                    return 0;
                return (Wins + Draws / 2.0) / (Wins + Losses + Draws);
            }
        }

        // Add this for convenience when manually creating players
        public Player() { }

        public Player(string name, PlayerDivision division)
        {
            Name = name;
            Division = division;
        }
    }
}
