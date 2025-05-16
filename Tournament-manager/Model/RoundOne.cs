
namespace Tournament_manager.Model
{
    public class RoundOne
    {
        // Round 1 pairing
        public static async Task<Round> MakeRoundOne(List<Player> players)
        {
            Round round = new Round
            {
                RoundNumber = 1
            };

            Random random = new Random();
            // Ordering players randomly
            players = players.OrderBy(x => random.Next()).ToList();

            // Issueing bye if odd player number
            int byeShift = 0;
            if (players.Count % 2 != 0)
            {
                Match byeMatch = new Match
                {
                    TableNumber = 0,
                    Player1 = players[players.Count - 1],
                    Player2 = null,
                    Bye = true
                };
                round.Matches.Add(byeMatch);
                round.Matches[0].Player1Result = Result.Win;
                round.Matches[0].Player1.HadBye = true;
                round.Matches[0].Player2Result = Result.Lose;

                byeShift = 1;
            }

            int table = 1;
            for (int i = 0; i < (players.Count - byeShift)/2; i++)
            {
                Match match = new Match
                {
                    TableNumber = table++,
                    Player1 = players[i],
                    Player2 = players[((players.Count - byeShift) / 2) + i]
                };
                match.Player1.Oponents.Add(match.Player2);
                match.Player2.Oponents.Add(match.Player1);
                round.Matches.Add(match);
            }
            return round;
        }
    }
}
