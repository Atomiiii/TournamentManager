
namespace Tournament_manager.Model
{
    public class Tournament
    {
        public string Id { get; set; }
        public string Name { get; set; } = "Tournament";
        public int RoundDurations { get; set; }
        public List<Player> Players { get; set; } = new List<Player>();
        public List<Round> Rounds { get; set; } = new List<Round>();
    }
}
