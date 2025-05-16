
using System.Collections.Generic;

namespace Tournament_manager.Model
{
    public class Pairing
    {
        public static async Task SortPlayers(List<Player> players)
        {
            players.Sort((x, y) =>
            {
                
                if (x.Score != y.Score)
                {
                    return y.Score.CompareTo(x.Score);
                }
                else if (x.Points != y.Points)
                {
                    return y.Points.CompareTo(x.Points);
                }
                else if ( x.Oponents.Count != 0 && y.Oponents.Count != 0)
                {
                    double xOponentWinrate = x.Oponents.Average(o => o.WinRate);
                    double yOponentWinrate = y.Oponents.Average(o => o.WinRate);
                    return xOponentWinrate.CompareTo(yOponentWinrate);
                }
                else
                {
                    return x.WinRate.CompareTo(y.WinRate);
                }
            });
        }

        public static bool FindOponent(Round round, Player wantsOponent, List<Player> players, int index, HashSet<Player> inGame, Player currentBye)
        {
            while (index < players.Count)
            {
                if (inGame.Contains(players[index]) || currentBye == players[index] || wantsOponent.Oponents.Contains(players[index]))
                {
                    index++;
                    continue;
                }
                Match match = new Match
                {
                    Player1 = wantsOponent,
                    Player2 = players[index],
                };
                inGame.Add(wantsOponent);
                inGame.Add(players[index]);
                match.Player1.Oponents.Add(match.Player2);
                match.Player2.Oponents.Add(match.Player1);
                round.Matches.Add(match);
                return true;
            }

            return false;
        }
        public static bool MakePairingRec(Round round, List<Player> players, int index, HashSet<Player> inGame, Player? bye)
        {
            if (index >= players.Count)
            {
                return true;
            }

            Player p1 = players[index];
            if (inGame.Contains(p1) || p1 == bye)
            {
                return MakePairingRec(round, players, index + 1, inGame, bye);
            }

            for (int i = index + 1; i < players.Count; i++)
            {
                Player p2 = players[i];
                if (inGame.Contains(p2) || p2 == bye || p1.Oponents.Contains(p2))
                {
                    continue;
                }

                Match match = new Match
                {
                    Player1 = p1,
                    Player2 = p2
                };

                round.Matches.Add(match);
                inGame.Add(p1);
                inGame.Add(p2);
                p1.Oponents.Add(p2);
                p2.Oponents.Add(p1);

                if (MakePairingRec(round, players, index + 1, inGame, bye))
                {
                    return true;
                }
                    
                round.Matches.RemoveAt(round.Matches.Count - 1);
                inGame.Remove(p1);
                inGame.Remove(p2);
                p1.Oponents.Remove(p2);
                p2.Oponents.Remove(p1);
            }
            return false;
        }
        public static async Task<Round> MakePairingAsynch(Tournament tournament, int roundNumber, List<Player> players)
        {
            Round round = new Round { RoundNumber = roundNumber };
            await SortPlayers(players);

            Player? byePlayer = null;

            int byeIndex = players.Count - 1;
            if (players.Count % 2 != 0)
            {
                IssueBye(players, ref byeIndex, false);
                Match byeMatch = new Match
                {
                    TableNumber = 0,
                    Player1 = players[byeIndex],
                    Player2 = null,
                    Player1Result = Result.Win,
                    Bye = true
                };
                round.Matches.Add(byeMatch);
                byePlayer = players[byeIndex];
            }
            int[] tables = new int[tournament.TableCount];
            for (int i = 0; i < tables.Length; i++)
            {
                tables[i] = i + 1;
            }
            int tableIndex = 0;
            System.Random.Shared.Shuffle(tables);
            bool success = MakePairingRec(round, players, 0, new HashSet<Player>(), byePlayer);
            foreach (Match match in round.Matches)
            {
                if (match.Bye)
                {
                    continue;
                }
                match.TableNumber = tables[tableIndex];
                tableIndex++;
            }
            if (!success)
                throw new Exception("Could not create a valid Swiss pairing.");

            return round;
        }

        public static void IssueBye(List<Player> players, ref int byeIndex, bool takeYounglins)
        {
            while (players[byeIndex].HadBye || (!takeYounglins && (players[byeIndex].Division == PlayerDivision.Senior || players[byeIndex].Division == PlayerDivision.Junior)))
            {
                byeIndex--;
            }
            if (byeIndex < 0)
            {
                IssueBye(players, ref byeIndex, true);
                return;
            }
            players[byeIndex].HadBye = true;
        }

    }
}
