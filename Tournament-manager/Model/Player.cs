
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
    public class Player (string name, PlayerDivision division)
    {
        public string Name { get; set; } = name;
        public PlayerDivision Division { get; set; } = division;
        public List<Result> results { get; set; } = new List<Result>();
        public int score { get; set; } = 0;
        public List<Player> oponents { get; set; } = new List<Player>();
        public bool hadBye { get; set; } = false;
    }
}
