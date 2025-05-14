
namespace Tournament_manager.Model
{
    public class Match
    {
        public Player Player1 { get; set; }
        public Player? Player2 { get; set; }
        public Result? Player1Result { get; set; }
        public Result? Player2Result { get; set; }

        public int Player1Pts { get; set; }
        public int Player2Pts { get; set; }

        public bool Bye { get; set; } = false;
        public int TableNumber { get; set;}
    }
}
