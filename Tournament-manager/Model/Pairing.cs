

namespace Tournament_manager.Model
{
    public class Pairing
    {
        public static async Task SortPlayers(List<Player> players)
        {
            players.Sort((x, y) =>
            {
                if (x.Points != y.Points)
                    return y.Points.CompareTo(x.Points);
                if (x.Score != y.Score)
                    return y.Score.CompareTo(x.Score);
                if ( x.Oponents.Count != 0 && y.Oponents.Count != 0)
                {
                    double xOponentWinrate = x.Oponents.Average(o => o.WinRate);
                    double yOponentWinrate = y.Oponents.Average(o => o.WinRate);
                    return xOponentWinrate.CompareTo(yOponentWinrate);
                }
                return x.WinRate.CompareTo(y.WinRate);
            });
        }
        public static async Task<Round> MakePairingAsynch(int roundNumber, List<Player> players)
        {
            Round round = new Round
            {
                RoundNumber = roundNumber
            };
            await SortPlayers(players);
            int byeIndex = players.Count - 1;
            if (players.Count % 2 != 0)
            {
                while (players[byeIndex].HadBye)
                {
                    byeIndex--;
                }
                players[players.Count - 1].HadBye = true;
                Match byeMatch = new Match
                {
                    Player1 = players[players.Count - 1],
                    Player2 = null,
                    Player1Result = Result.Win,
                    Bye = true
                };
                round.Matches.Add(byeMatch);
            }
            for (int i = 0; i < (players.Count - 1); i+=2)
            {
                Player player1;
                Player player2;
                if (players[i].HadBye)
                {
                    player1 = players[++i];
                    player2 = players[i + 1];
                }
                else if (players[i + 1].HadBye)
                {
                    player1 = players[i];
                    player2 = players[++i + 1];
                } else
                {
                    player1 = players[i];
                    player2 = players[i + 1];
                }
                Match match = new Match
                {
                    Player1 = player1,
                    Player2 = player2
                };
                match.Player1.Oponents.Add(player2);
                match.Player2.Oponents.Add(player1);
                round.Matches.Add(match);
            }
            return round;
        }
    }
}
