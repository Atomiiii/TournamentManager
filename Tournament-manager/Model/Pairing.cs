
using System.Collections.Generic;

namespace Tournament_manager.Model
{
    public class Pairing
    {
        // sorting players by wins-draws-losses, points, oponents winrate
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
                else if (x.Oponents.Count != 0 && y.Oponents.Count != 0)
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

        public static bool MakePairingRec(Round round, List<Player> players, int index, HashSet<Player> inGame, Player? bye)
        {
            // finished pairing
            if (index >= players.Count)
            {
                return true;
            }

            Player candidate = players[index];
            // candidate in game or has bye
            if (inGame.Contains(candidate) || candidate == bye)
            {
                return MakePairingRec(round, players, index + 1, inGame, bye);
            }

            // find oponent
            for (int i = index + 1; i < players.Count; i++)
            {
                Player oponent = players[i];
                if (inGame.Contains(oponent) || oponent == bye || candidate.Oponents.Contains(oponent))
                {
                    continue;
                }

                Match match = new Match
                {
                    Player1 = candidate,
                    Player2 = oponent
                };

                round.Matches.Add(match);
                inGame.Add(candidate);
                inGame.Add(oponent);
                candidate.Oponents.Add(oponent);
                oponent.Oponents.Add(candidate);

                if (MakePairingRec(round, players, index + 1, inGame, bye))
                {
                    return true;
                }
                // if pairing failed, try next oponent 
                round.Matches.RemoveAt(round.Matches.Count - 1);
                inGame.Remove(candidate);
                inGame.Remove(oponent);
                candidate.Oponents.Remove(oponent);
                oponent.Oponents.Remove(candidate);
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
            // Randomize tables => better for Warhammer Tournaments
            int[] tables = new int[tournament.TableCount];
            for (int i = 0; i < tables.Length; i++)
            {
                tables[i] = i + 1;
            }
            int tableIndex = 0;
            System.Random.Shared.Shuffle(tables);

            bool success = MakePairingRec(round, players, 0, new HashSet<Player>(), byePlayer);
            // pairing failed (shouldn't happen)
            if (!success)
                throw new Exception("Could not create a valid Swiss pairing.");
            // assign tables
            foreach (Match match in round.Matches)
            {
                if (match.Bye)
                {
                    continue;
                }
                match.TableNumber = tables[tableIndex];
                tableIndex++;
            }
            

            return round;
        }

        // Issue bye => give bye to older players first so younger can play
        public static void IssueBye(List<Player> players, ref int byeIndex, bool takeYounglins)
        {
            while (byeIndex >= 0 &&  (players[byeIndex].HadBye || (!takeYounglins && (players[byeIndex].Division == PlayerDivision.Senior || players[byeIndex].Division == PlayerDivision.Junior))))
            {
                byeIndex--;
            }
            if (byeIndex < 0)
            {
                if (takeYounglins == false)
                {
                    byeIndex = players.Count - 1;
                    IssueBye(players, ref byeIndex, true);
                    return;
                }
                throw new Exception("No player available for bye."); // should never happen, bcs there are much less rounds then players
            }
            players[byeIndex].HadBye = true;
        }

    }
}
