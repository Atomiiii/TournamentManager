
namespace Tournament_manager.Model
{
    public class Tournament
    {
        public string Id { get; set; }
        public string Name { get; set; } = "Tournament";
        public int RoundDurations { get; set; } = 50;
        public List<Player> Players { get; set; } = new List<Player>();
        public List<Round> Rounds { get; set; } = new List<Round>();
        public int currentRoundIndex { get; set; } = 1;
        public int TableCount { get { 
                    return (int)Math.Floor((double)Players.Count / 2); } }
        public int RoundCount { get { 
                if (Players.Count == 0)
                {
                    return 0;
                }   
                return (int)Math.Ceiling(Math.Log2((double)Players.Count)); } }
    }
}