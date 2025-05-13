
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
                round.Matches[0].Player1Result = Result.Win;
                round.Matches[0].Player1Pts = 3;
                round.Matches[0].Player1.Wins++;
                round.Matches[0].Player1.Score += 3;
                round.Matches[0].Player1.HadBye = true;
                round.Matches[0].Player2Result = Result.Lose;

                byeShift = 1;
            }

            for (int i = 0; i < (players.Count - byeShift)/2; i++)
            {
                Match match = new Match
                {
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
