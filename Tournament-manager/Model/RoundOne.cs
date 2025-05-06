
namespace Tournament_manager.Model
{
    public class RoundOne
    {
        public static Round MakeRoundOne(List<Player> players)
        {
            Round round = new Round
            {
                RoundNumber = 1
            };

            int byeShift = 0;
            if (players.Count % 2 != 0)
            {
                Match byeMatch = new Match
                {
                    Player1 = players[players.Count - 1],
                    Player2 = null,
                    Bye = true
                };
                round.Matches.Add(byeMatch);

                byeShift = 1;
            }

            for (int i = 0; i < (players.Count - byeShift)/2; i++)
            {
                Match match = new Match
                {
                    Player1 = players[i],
                    Player2 = players[i*2]
                };
                round.Matches.Add(match);
            }
            return round;
        }
    }
}
