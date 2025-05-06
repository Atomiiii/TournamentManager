
namespace Tournament_manager.Model
{
    public class Round
    {
        public int RoundNumber { get; set; }
        public List<Match> Matches { get; set; } = new List<Match>();
        public bool IsFinished => Matches.All(m => m.Player1Result != null);
    }
}
