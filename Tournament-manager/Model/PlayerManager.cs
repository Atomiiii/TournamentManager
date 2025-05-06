using System.Collections.ObjectModel;


namespace Tournament_manager.Model
{
    public static class PlayerManager
    {
        private static ObservableCollection<Player> Players = [
            new Player("Maxine", PlayerDivision.Master),
            new Player("Veru", PlayerDivision.Junior),
            ];
        public static ObservableCollection<Player> GetPlayers() => Players;

        public static void AddPlayer(Player player) => Players.Add(player);

    }
    
}
